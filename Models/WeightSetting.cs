namespace Shipping_System.Models
{
    public class WeightSetting
    {
        public int Id { get; set; }

        public decimal DefaultSize { get; set; }

        public int PriceForEachExtraKilo { get; set; }
    }
}
