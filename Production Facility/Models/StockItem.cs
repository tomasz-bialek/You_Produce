using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Production_Facility.Models
{
    [Table("StockItems")]
    public class StockItem

    {
        [Key]
        public int StockItem_ID { get; set; }

        public virtual Item Item { get; set; }

        [ForeignKey("Item")]
        public string Number { get; set; }

        public double QTotal { get; set; }

        public double QReserved { get; set; }

        public double QAvailable { get; set; }


        public decimal UnitCost { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime IncomingDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime LastActionDate { get; set; }

        public string Location { get; set; }

        public string BatchNumber { get; set; }

        public StockItem(string number, string name, string unit, string section, string qTotal, string qReserved,string qAvailable, string uCost, string tCost,
            string inDate, string exDate, string laDate,   string location)
        {
            this.Number = number;
            this.Item.Number = number;
            this.Item.Name = name;
            this.QTotal = double.Parse(qTotal);
            this.QAvailable = this.QTotal;
            this.Location = location;
            this.UnitCost = decimal.Parse(uCost);
            this.TotalCost = decimal.Parse(tCost);
            this.TotalCost = UnitCost * (Convert.ToDecimal(QTotal));
            this.LastActionDate = DateTime.Parse(laDate);
            this.IncomingDate = DateTime.Parse(inDate);
            this.QReserved = double.Parse(qReserved);
            this.QAvailable = double.Parse(qAvailable);

            try
            {
                this.ExpirationDate = DateTime.Parse(exDate);
            }
            catch (System.FormatException e)
            {
                this.ExpirationDate = null;
            }


            if (unit == "szt")
            {
                this.Item.Unit = UnitType.szt;
            }
            else if (unit == "kg")
            {
                this.Item.Unit = UnitType.kg;
            }
            else if (unit == "m")
            {
                this.Item.Unit = UnitType.m;
            }
            else
            {
                MessageBox.Show(unit);
            }

            if (section == "Article")
                this.Item.Section = SectionType.Article;
            else if (section == "Intermediate")
                this.Item.Section = SectionType.Intermediate;
            else if (section == "Substance")
                this.Item.Section = SectionType.Substance;
            else if (section == "Product")
                this.Item.Section = SectionType.Product;
        }

        public StockItem()
        {

        }

        public StockItem(string key, double quantity, decimal unitCost, string orderID,string location)
        {
            Number = key;
            QTotal = quantity;
            QReserved = 0;
            QAvailable = quantity;
            UnitCost = unitCost;
            TotalCost = unitCost * Convert.ToDecimal(quantity);
            IncomingDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(2);
            LastActionDate = DateTime.Now;
            Location = location;

            var batch_temp = String.Concat(Enumerable.Repeat("0", 7 - orderID.Length));
            BatchNumber = "ORD/" + batch_temp + orderID;
        }

    }
}
