﻿namespace ApplicationCore.Helpers.Datatables.Model
{
    public class DatatablesParameter
    {
        public DatatablesParameter() { }
        public int Draw { get; set; }
        public DatatablesColumn[] Columns { get; set; }
        public DatatablesOrder[] Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DatatablesSearch Search { get; set; }
        public string SortOrder { get; }
    }
}
