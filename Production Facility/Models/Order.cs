using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public string OwnerKey { get; set; }

        public int Quantity { get; set; }

        public Nullable <DateTime> EntryDate { get; set; }

        public DateTime PlannedDate { get; set; }

        public Nullable <DateTime> ClosingDate { get; set; }

        public string OrderStatus { get; set; }

        public virtual ICollection<OrderComponent> OrderComponents { get; set; }

        //public ObservableCollection<Recipe.RecipeLine> Order { get; set; }

        public Order(string key, int quantity, DateTime plannedDate)
        {
            this.OwnerKey = key;
            this.Quantity = quantity;
            this.PlannedDate = plannedDate;
            this.OrderStatus = "PLANNED";
            this.EntryDate = DateTime.Now;
            //this.OrderComposition = orderComposition;
        }
        public Order()
        {

        }

    }
}
