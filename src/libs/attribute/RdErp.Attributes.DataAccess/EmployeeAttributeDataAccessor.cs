using System;
using System.Linq;
using System.Linq.Expressions;

using LinqToDB.Data;
using LinqToDB.Mapping;

namespace RdErp.Attributes
{
    public class EmployeeAttributeDataAccessor : ICustomAttributeDataAccessor
    {
        private readonly DataConnection db;

        public EmployeeAttributeDataAccessor(DataConnection db) => this.db = db
            ??
            throw new ArgumentNullException(nameof(db));

        public string AttributeKey => KnownAttributeNames.Employee;

        public string ParentAttributeKey => String.Empty;

        public IQueryable<CustomAttributeInfo> GetOneValue(int refId) => GetAttributeList(c => c.Id == refId);

        public IQueryable<CustomAttributeInfo> GetValueList() => GetAttributeList(c => true);

        private IQueryable<CustomAttributeInfo> GetAttributeList(Expression<Func<EmployeeAttributeInfo, bool>> filter) =>
            db.GetTable<EmployeeAttributeInfo>()
            .Where(filter)
            .Select(a => new CustomAttributeInfo
            {
                AttributeKey = KnownAttributeNames.Employee,
                    ParentAttributeKey = null,
                    ParentId = 0,
                    RefId = a.Id,
                    Title = a.Name,
                    IsActive = a.IsActive
            });
    }

    [Table(Name = "employees", Schema = "empl")]
    public class EmployeeAttributeInfo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}