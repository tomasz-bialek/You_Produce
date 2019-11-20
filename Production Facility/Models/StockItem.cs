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

        public string Number { get; set; }

        public string Name { get; set; }

        public UnitType Unit { get; set; }

        public SectionType Section { get; set; }

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

        //public StockItem(string number, string qTotal,)
        //{

        //}

        public StockItem(string number, string name, string qTotal, string location, string uCost,
            string laDate, string inDate, string exDate, string unit,string section)
        {
            this.Number = number;
            this.Name = name;
            this.QTotal = double.Parse(qTotal);
            this.QAvailable = this.QTotal;
            this.Location = location;
            this.UnitCost = decimal.Parse(uCost);
            this.TotalCost = UnitCost * (Convert.ToDecimal(QTotal));
            this.LastActionDate = DateTime.Parse(laDate);
            this.IncomingDate = DateTime.Parse(inDate);

            try
            {
                this.ExpirationDate = DateTime.Parse(exDate);
            }
            catch (System.FormatException e)
            {
                //MessageBox.Show(name + '\n' + "=>" + exDate + "<=");
                this.ExpirationDate = null;
            }

            //if (exDate == "")
            //{
            //    this.ExpirationDate = null;
            //}
            //else if (exDate != "")
            //{
            //    this.ExpirationDate = DateTime.Parse(exDate);
            //}

            //else
            //{
            //    MessageBox.Show("=>"+exDate+"<=");
            //}

            if (unit == "szt")
            {
                this.Unit = UnitType.szt;
            }
            else if (unit == "kg")
            {
                this.Unit = UnitType.kg;
            }
            else if (unit == "m")
            {
                this.Unit = UnitType.m;
            }
            else
            {
                MessageBox.Show(unit);
            }

            if (section == "Article")
                this.Section = SectionType.Article;
            else if (section == "Intermediate")
                this.Section = SectionType.Intermediate;
            else if (section == "Substance")
                this.Section = SectionType.Substance;
            else if (section == "Product")
                this.Section = SectionType.Product;
            //var batch_temp = String.Concat(Enumerable.Repeat("0", 6 - batch.Length));
            //this.BatchNumber = "PO/" + batch_temp + batch;


        }

        public StockItem()
        {

        }

    }
}
