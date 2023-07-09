namespace Shipping_System.ViewModels
{
    public class OrderStatusViewModel
    {
        public int NewCount { get; set; }
        public int pendingCount { get; set; }
        public int The_order_has_been_deliveredCount { get; set; }
        public int sent_delivered_handedCount { get; set; }
        public int Can_not_reachCount { get; set; }
        public int postponedCount { get; set; }
        public int Partially_deliveredCount { get; set; }
        public int Canceled_by_ClientCount { get; set; }
        public int Refused_with_paymentCount { get; set; }
        public int Refused_with_part_paymentCount { get; set; }
        public int Rejected_and_not_paidCount { get; set; }

    }
}
