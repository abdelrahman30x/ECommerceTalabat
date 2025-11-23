using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence
{
    public static class SpecificationEvlautor
    {
        public static IQueryable<Tentity> CreateQuery<Tentity, Tkey>(
            IQueryable<Tentity> BaseQuery,
            ISpecifications<Tentity, Tkey> Specification)
            where Tentity : BaseEntity<Tkey>
        {
            var Query = BaseQuery;

            // APPLY CRITERIA (WHERE)
            if (Specification.Criteria != null)
                Query = Query.Where(Specification.Criteria);

            // APPLY INCLUDES
            if (Specification.Includes != null && Specification.Includes.Any())
            {
                Query = Specification.Includes.Aggregate(Query,
                            (currentQuery, include) => currentQuery.Include(include));
            }

            // APPLY ORDERING
            if (Specification.OrderBy != null)
                Query = Query.OrderBy(Specification.OrderBy);
            else if (Specification.OrderByDesc != null)
                Query = Query.OrderByDescending(Specification.OrderByDesc);

            // APPLY PAGINATION
            if (Specification.IsPaginated)
                Query = Query.Skip(Specification.Skip).Take(Specification.Take);

            return Query;
        }
    }
}
