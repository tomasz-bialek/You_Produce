using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [ForeignKey("Item")]
        public string ItemNumber { get; set; }

        public virtual Item Item { get; set; }

        public double Quantity { get; set; }

        public Stock (string itemNumber, double quantity)
        {
            ItemNumber = itemNumber;
            Quantity = quantity;
        }

        public Stock()
        {

        }
    }
}
