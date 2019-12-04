using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Production_Facility.Models;

namespace Production_Facility.Models
{
    [Table("Items")]
    public class Item
    {
        [Key]
        public string Number { get; set; }

        public string Name { get; set; }

        public UnitType Unit { get; set; }

        public SectionType Section { get; set; }

        //public virtual ICollection<StockItem> StockItems { get; set; }

        public Item(string number, string name, string unit, string section)
        {
            this.Number = number;
            this.Name = name;
            switch (unit)
            {
                case "szt":
                    this.Unit = UnitType.szt;
                    break;
                case "kg":
                    this.Unit = UnitType.kg;
                    break;
                case "m":
                    this.Unit = UnitType.m;
                    break;
                default:
                    MessageBox.Show("Nieznana jednostka !!!");
                    break;
            }
            switch (section)
            {
                case "Product":
                    this.Section = SectionType.Product;
                    break;
                case "Intermediate":
                    this.Section = SectionType.Intermediate;
                    break;
                case "Substance":
                    this.Section = SectionType.Substance;
                    break;
                case "Article":
                    this.Section = SectionType.Article;
                    break;
                default:
                    MessageBox.Show("Uwaga, nieznana Sekcja lub błąd");
                    break;
            }
        }

        public Item(string number, string name, SectionType section)
        {
            this.Number = number;
            this.Name = name;
            this.Section = section;
        }

        public Item() { }
    }
}
