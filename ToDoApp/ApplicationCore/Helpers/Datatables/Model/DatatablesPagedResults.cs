using System.Collections.Generic;

namespace ApplicationCore.Helpers.Datatables.Model
{
    public class DatatablesPagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalSize { get; set; }
    }
}
