using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Shipping_System.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }

        [Display(Name = "Order Type ")]
        [ForeignKey("OrderType")]
        public int OrderTypeId { get; set; }
        public OrderType? OrderType { get; set; }

        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "FullName must be more than 2 char")]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Please Enter Valid Phone Number")]
        [Display(Name = "Client Phone 1")]
        public string ClientPhone1 { get; set; }

        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Please Enter Valid Phone Number")]
        [Display(Name = "Client Phone 2")]
        public string ClientPhone2 { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Client Email Address")]
        public string ClientEmailAddress { get; set; }

        [Display(Name = "Client Governorate")]
        [ForeignKey("ClientGovernorate")]
        public int ClientGovernorateId { get; set; }
        public Governorate? ClientGovernorate { get; set; }

        [Display(Name = "Client City")]
        [ForeignKey("ClientCity")]
        public int ClientCityId { get; set; }
        public City? ClientCity { get; set; }

        [Display(Name = "Deliver To Village ?")]
        public bool DeliverToVillage { get; set; }

        public string? Village_Street { get; set; }

        [Display(Name = "Delivery Type ")]
        [ForeignKey("DeliverType")]
        public int DeliveryTypeId { get; set; }
        public DeliverType? DeliverType { get; set; }

        [Display(Name = "Payment Method ")]
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        [Display(Name = "Branch ")]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public DateTime creationDate { get; set; } = DateTime.Now.Date;

        [NotMapped]
        public virtual List<Product>? Products { get; set; } = new List<Product>();

        [Display(Name = "Order State ")]
        [ForeignKey("OrderState")]
        public int OrderStateId { get; set; } = 1;
        public OrderState? OrderState { get; set; }

        [ForeignKey("Trader")]
        public string TraderId { get; set; }
        public Trader? Trader { get; set; }


        [ForeignKey("Representative")]
        public string? RepresentativeId { get; set; }
        public Representative? Representative { get; set; }


        public bool IsDeleted { get; set; }


        [Display(Name = "Order Price")]
        public decimal OrderPrice { get; set; }

        [Display(Name = "Order Price Recieved")]
        public decimal OrderPriceRecieved { get; set; } = 0;

        [Display(Name = "Shipping Price")]
        public decimal ShippingPrice { get; set; }

        [Display(Name = "Shipping Price Recieved ")]
        public decimal ShippingPriceRecived { get; set; } = 0;

        public decimal TotalWeight { get; set; }

    }
}
