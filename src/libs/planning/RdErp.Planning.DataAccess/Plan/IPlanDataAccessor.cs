using System;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Planning
{
    public interface IPlanDataAccessor
    {
        IQueryable<Plan> All();

        Task<Plan> GetById(int id);

        Task<int> Insert(Plan plan);

        Task Update(Plan plan);

        Task Delete(int planId);
    }
}