using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;

namespace RdErp.Attributes
{
    public class CustomAttributeService : ICustomAttributeService
    {
        private readonly IEnumerable<ICustomAttributeDataAccessor> customAttributeDataAccessors;

        public CustomAttributeService(IEnumerable<ICustomAttributeDataAccessor> customAttributeDataAccessors)
        {
            this.customAttributeDataAccessors = customAttributeDataAccessors
                ??
                throw new ArgumentNullException(nameof(customAttributeDataAccessors));
        }

        public async Task<Dictionary<string, CustomAttributeInfo[]>> GetAttributeValueList(CustomAttributeValue[] values)
        {
            values = values ?? new CustomAttributeValue[] { };
            var valuesInfo = await customAttributeDataAccessors
                .Select(accessor =>(
                    key: accessor.AttributeKey,
                    parentKey: accessor.ParentAttributeKey,
                    accessor: accessor,
                    values: accessor.GetValueList()))
                .Select(rec =>
                {
                    var value = values.FirstOrDefault(i => StringComparer
                        .OrdinalIgnoreCase
                        .Equals(i.AttributeKey, rec.key));
                    var parentValue = values.FirstOrDefault(i => StringComparer
                        .OrdinalIgnoreCase
                        .Equals(i.AttributeKey, rec.parentKey));

                    var result = rec.values;

                    if (parentValue != null)
                    {
                        result = result.Where(v => v.ParentId == parentValue.RefId);
                    }

                    if (value != null)
                    {
                        result = result.Union(rec.accessor.GetOneValue(value.RefId));
                    }

                    return result;

                })
                .Aggregate(
                    default(IQueryable<CustomAttributeInfo>),
                    (result, item) => result == null ? item : result.Union(item))
                .ToArrayAsync();

            var valueDictionary = new Dictionary<string, CustomAttributeInfo[]>();

            foreach (var group in valuesInfo.GroupBy(v => v.AttributeKey))
            {
                valueDictionary[group.Key] = group.ToArray();
            }

            return valueDictionary;
        }

        public async Task<CustomAttributeInfo[]> LookupValues(CustomAttributeValue[] values) =>
            await (values ?? new CustomAttributeValue[] { })
            .Select(v =>(value: v, accessor: customAttributeDataAccessors
                .FirstOrDefault(accessor => StringComparer
                    .OrdinalIgnoreCase
                    .Equals(accessor.AttributeKey, v.AttributeKey)
                )))
            .Where(rec => rec.accessor != null)
            .Select(record => record.accessor.GetOneValue(record.value.RefId))
            .Aggregate(
                default(IQueryable<CustomAttributeInfo>),
                (result, item) => result == null ? item : result.Union(item))
            .ToArrayAsync();
    }
}