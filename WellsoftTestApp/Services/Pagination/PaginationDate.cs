using System.Collections.Generic;

namespace WellsoftTestApp.Services
{
    public class PaginationData<T> where  T: class
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
