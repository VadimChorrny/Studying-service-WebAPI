using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace DAL.Queries
{
    public  class ApplySortService
    {
        public static IQueryable<T> ApplySort<T>(IQueryable<T> users, string orderByQueryString)
        {
            const string sortSequebceDesc = "descending";
            const string sortSequebceAsce = "ascending";
            const string checkSequenceSort = " descend";

            if (string.IsNullOrWhiteSpace(orderByQueryString) || users == null || !users.Any())
            {
                return users;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ").FirstOrDefault();
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name
                    .Equals(propertyFromQueryName,
                        StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var sortingOrder = param.EndsWith(checkSequenceSort) ? sortSequebceDesc : sortSequebceAsce;
                orderQueryBuilder.Append($"{ objectProperty.Name } { sortingOrder }, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return users;
            }

            return users.OrderBy(orderQuery);
        }
    }
}