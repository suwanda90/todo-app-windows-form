﻿namespace ApplicationCore.Helpers.Datatables.Model
{
    public class DatatablesColumn
    {
        public DatatablesColumn() { }
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
    }
}
