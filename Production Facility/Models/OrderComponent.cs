using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    public class OrderComponent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }

        public byte Line { get; set; }

        [StringLength(32)]
        [Required]
        public string OwnerKey { get; set; }

        [StringLength(128)]
        [Required]
        public string OwnerName { get; set; }

        [StringLength(32)]
        [Required]
        public string ComponentKey { get; set; }

        [StringLength(128)]
        [Required]
        public string ComponentName { get; set; }

        [Required]
        public double Quantity { get; set; }

        public virtual Order Order { get; set; }

        public OrderComponent(byte line, string ownerKey, string ownerName, int orderId, string ComponentKey, string ComponentName, double quantity)
        {
            this.Line = line;
            this.OwnerKey = ownerKey;
            this.OwnerName = ownerName;
            this.OrderId = orderId;
            this.ComponentKey = ComponentKey;
            this.ComponentName = ComponentName;
            this.Quantity = quantity;
        }

        public OrderComponent(byte line, string ownerKey, string ownerName, string ComponentKey, string ComponentName, double quantity)
        {
            this.Line = line;
            this.OwnerKey = ownerKey;
            this.OwnerName = ownerName;
            this.ComponentKey = ComponentKey;
            this.ComponentName = ComponentName;
            this.Quantity = quantity;
        }

        public OrderComponent()
        {

        }

    }
}
