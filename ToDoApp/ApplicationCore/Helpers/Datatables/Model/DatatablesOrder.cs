using ApplicationCore.Constants;

namespace ApplicationCore.Helpers.Datatables.Model
{
    public class DatatablesOrder
    {
        public DatatablesOrder() { }
        public int Column { get; set; }
        public OrderType Dir { get; set; }
    }
}
