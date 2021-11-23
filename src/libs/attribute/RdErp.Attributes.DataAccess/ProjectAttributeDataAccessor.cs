using System;
using System.Linq;
using System.Linq.Expressions;

using LinqToDB.Data;
using LinqToDB.Mapping;

namespace RdErp.Attributes
{
    public class ProjectAttributeDataAccessor : ICustomAttributeDataAccessor
    {
        private readonly DataConnection db;

        public ProjectAttributeDataAccessor(DataConnection db) => this.db = db
            ??
            throw new ArgumentNullException(nameof(db));

        public string AttributeKey => KnownAttributeNames.Project;

        public string ParentAttributeKey => KnownAttributeNames.Customer;

        public IQueryable<CustomAttributeInfo> GetOneValue(int refId) => GetAttributeList(c => c.Id == refId);

        public IQueryable<CustomAttributeInfo> GetValueList() => GetAttributeList(c => true);

        private IQueryable<CustomAttributeInfo> GetAttributeList(Expression<Func<ProjectAttributeInfo, bool>> filter) =>
            db.GetTable<ProjectAttributeInfo>()
            .Where(filter)
            .Select(a => new CustomAttributeInfo
            {
                AttributeKey = KnownAttributeNames.Project,
                    ParentAttributeKey = KnownAttributeNames.Customer,
                    ParentId = a.CustomerId,
                    RefId = a.Id,
                    Title = a.Name,
                    IsActive = a.IsActive
            });
    }

    [Table(Name = "projects", Schema = "swd")]
    public class ProjectAttributeInfo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}