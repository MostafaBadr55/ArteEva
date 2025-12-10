using ArtEva.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArteEva.Models
{
 

    public class Order : BaseModel
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public int ShippingAddressId { get; set; }

        [Required]
        [MaxLength(40)]
        public string OrderNumber { get; set; }

        //[MaxLength(50)]
        //public string BuyerCodename { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ShippingFee { get; set; }

        public decimal Discount { get; set; }

        public decimal TaxTotal { get; set; }

        public decimal GrandTotal { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Shop Shop { get; set; }
        public Address ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<Refund> Refunds { get; set; }
        public ICollection<Dispute> Disputes { get; set; }
    }
}
