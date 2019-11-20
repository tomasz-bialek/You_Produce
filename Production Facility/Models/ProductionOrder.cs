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
    [Table("ProductionOrders")]
    public class ProductionOrder
    {
        [Key]
        public int OrderID { get; set; }

        public string ItemKey { get; set; }

        public int Quantity { get; set; }

        public Nullable <DateTime> OrderDate { get; set; }

        public DateTime PlannedDate { get; set; }

        public Nullable <DateTime> ProductionDate { get; set; }

        public string OrderStatus { get; set; }

        public string OrderComposition { get; set; }

        //public ObservableCollection<Recipe.RecipeLine> Order { get; set; }

        public ProductionOrder(string key, int quantity, DateTime plannedDate, string orderComposition)
        {
            this.ItemKey = key;
            this.Quantity = quantity;
            this.PlannedDate = plannedDate;
            this.OrderStatus = "PLANNED";
            this.OrderDate = DateTime.Now;
            this.OrderComposition = orderComposition;
        }
        public ProductionOrder()
        {

        }

    }
}
