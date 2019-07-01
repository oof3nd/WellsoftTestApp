using System;
using System.Collections.Generic;
using System.Linq;

namespace WellsoftTestApp.Services
{
    public static class Pagination
    {
        public static PaginationData<T> PagedResult<T>(this List<T> list, int PageNumber, int PageSize) where T : class
        {
            var result = new PaginationData<T>();
            result.Data = list.Skip(PageSize * (PageNumber - 1)).Take(PageSize).ToList();
            result.TotalPages = (int)(Math.Ceiling((double)list.Count() / PageSize));
            result.CurrentPage = PageNumber;
            return result;
        }
    }
}
