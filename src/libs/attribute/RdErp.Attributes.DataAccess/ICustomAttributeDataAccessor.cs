using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Attributes
{
    public interface ICustomAttributeDataAccessor
    {
        string AttributeKey { get; }

        string ParentAttributeKey { get; }

        /// <summary>
        /// Gets a single value of specific attribute with given Ref ID
        /// or empty result if value doesn't exists.
        /// Method must return even a disabled value.
        /// </summary>
        IQueryable<CustomAttributeInfo> GetOneValue(int refId);

        /// <summary>
        /// Gets a queryable of all active attribute values.
        /// </summary>
        IQueryable<CustomAttributeInfo> GetValueList();
    }
}