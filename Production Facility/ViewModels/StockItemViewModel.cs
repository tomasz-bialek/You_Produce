using Production_Facility.Models;
using Production_Facility.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Production_Facility.ViewModels
{
    public class StockItemViewModel : INotifyPropertyChanged
    {
        private ICommand _dataGridLoader;
        public ICommand DataGridLoader
        {
            get
            {
                if (_dataGridLoader == null)
                {
                    _dataGridLoader = new RelayCommand(SetStockItems);
                }
                return _dataGridLoader;
            }
        }


        private List<StockItem> _stockItems;
        public List<StockItem> StockItems
        {
            get { return _stockItems; }
            set
            {
                _stockItems = value;
                OnPropertyChanged("StockItems");
            }
        }

        /// <summary>
        /// Merges single queries to one and finds desired stocks.
        /// </summary>
        public void SetStockItems(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var time1 = DateTime.Now;

                var objects = (object[])obj;

                IQueryable<StockItem> query = dbContext.StockItems.Include("Item");

                for (int i = 0; i < objects.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(objects[i] != null ? objects[i].ToString() : ""))
                        switch (i)
                        {
                            case (0):
                                query = QueryBuilder(query, objects[0], "Section");
                                break;
                            case (1):
                                query = QueryBuilder(query, objects[1], "Unit");
                                break;
                            case (2):
                                query = QueryBuilder(query, objects[2], "Number");
                                break;
                            case (3):
                                query = QueryBuilder(query, objects[3], "Name");
                                break;
                            case (4):
                                query = QueryBuilder(query, objects[4], "Location");
                                break;
                            case (5):
                                query = QueryBuilder(query, objects[5], "Batch");
                                break;
                            case (6):
                                query = QueryBuilder(query, objects[6], "Quantity Total");
                                break;
                            case (7):
                                query = QueryBuilder(query, objects[7], "Quantity Reserved");
                                break;
                            case (8):
                                query = QueryBuilder(query, objects[8], "Quantity Available");
                                break;
                            case (9):
                                query = QueryBuilder(query, objects[9], "Unit Cost");
                                break;
                            case (10):
                                query = QueryBuilder(query, objects[10], "Total Cost");
                                break;
                            case (11):
                                query = QueryBuilder(query, objects[11], "Incoming Date");
                                break;
                            case (12):
                                query = QueryBuilder(query, objects[12], "Expiration Date");
                                break;
                            case (13):
                                query = QueryBuilder(query, objects[13], "Last Action Date");
                                break;
                            default:
                                break;
                        }
                }
                StockItems = query.ToList();
            }

        }


        /// <summary>
        /// This method creates a single database query based by a user typed values.
        /// </summary>
        private IQueryable<StockItem> QueryBuilder(IQueryable<StockItem> query, object obj, string property)
        {
            if (property == "Section")
            {
                switch ((string)obj)
                {
                    case ("Product"):
                        return query = query.Where(q => q.Item.Section == SectionType.Product);
                    case ("Intermediate"):
                        return query = query.Where(q => q.Item.Section == SectionType.Intermediate);
                    case ("Substance"):
                        return query = query.Where(q => q.Item.Section == SectionType.Substance);
                    case ("Article"):
                        return query = query.Where(q => q.Item.Section == SectionType.Article);
                    default:
                        return query;
                }
            }
            else if (property == "Unit")
            {
                switch ((string)obj)
                {
                    case ("szt"):
                        return query = query.Where(q => q.Item.Unit == UnitType.szt);
                    case ("kg"):
                        return query = query.Where(q => q.Item.Unit == UnitType.kg);
                    case ("m"):
                        return query = query.Where(q => q.Item.Unit == UnitType.m);
                    default:
                        return query;
                }
            }
            else if (property == "Number" || property == "Name" || property == "Location" || property == "Batch")
            {
                var value = (string)obj;

                switch (property)
                {
                    case ("Number"):
                        return query = query.Where(q => q.Item.Number.Contains(value));
                    case ("Name"):
                        return query = query.Where(q => q.Item.Name.Contains(value));
                    case ("Location"):
                        return query = query.Where(q => q.Location.Contains(value));
                    case ("Batch"):
                        return query = query.Where(q => q.BatchNumber.Contains(value));
                    default:
                        return query;
                }
            }
            else if (property == "Quantity Total" || property == "Quantity Reserved" || property == "Quantity Available" || property == "Unit Cost" || property == "Total Cost")
            {
                var values = GetDoubles(obj);

                if (values.Length == 1)
                {
                    var value = values[0];

                    switch (property)
                    {
                        case ("Quantity Total"):
                            return query = query.Where(q => q.QTotal == value);
                        case ("Quantity Reserved"):
                            return query = query.Where(q => q.QReserved == value);
                        case ("Quantity Available"):
                            return query = query.Where(q => q.QAvailable == value);
                        case ("Unit Cost"):
                            var unitCostDecimal = Convert.ToDecimal(value);
                            return query = query.Where(q => q.UnitCost == unitCostDecimal);
                        case ("Total Cost"):
                            var totalCostDecimal = Convert.ToDecimal(value);
                            return query = query.Where(q => q.TotalCost == totalCostDecimal);
                        default:
                            return query;
                    }
                }
                else if (values.Length == 2)
                {
                    var value1 = values[0];
                    var value2 = values[1];

                    switch (property)
                    {
                        case ("Quantity Total"):
                            return query = query.Where(q => q.QTotal >= value1 && q.QTotal <= value2);
                        case ("Quantity Reserved"):
                            return query = query.Where(q => q.QReserved >= value1 && q.QReserved <= value2);
                        case ("Quantity Available"):
                            return query = query.Where(q => q.QAvailable >= value1 && q.QAvailable <= value2);
                        case ("Unit Cost"):
                            var unitCostDecimal_1 = Convert.ToDecimal(value1);
                            var unitCostDecimal_2 = Convert.ToDecimal(value2);
                            return query = query.Where(q => q.UnitCost >= unitCostDecimal_1 && q.UnitCost <= unitCostDecimal_2);
                        case ("Total Cost"):
                            var totalCostDecimal_1 = Convert.ToDecimal(value1);
                            var totalCostDecimal_2 = Convert.ToDecimal(value2);
                            return query = query.Where(q => q.TotalCost >= totalCostDecimal_1 && q.TotalCost <= totalCostDecimal_2);
                        default:
                            return query;
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Incorrect value(s) in property '{0}'.", property));
                    return query;
                }
            }
            else if (property == "Incoming Date" || property == "Expiration Date" || property == "Last Action Date")
            {
                var dates = GetDates(obj);

                if (dates.Length == 2)
                {
                    var date1 = dates[0];
                    var date2 = dates[1];

                    switch (property)
                    {
                        case ("Incoming Date"):
                            return query = query.Where(q => q.IncomingDate >= date1 && q.IncomingDate <= date2);
                        case ("Expiration Date"):
                            return query = query.Where(q => q.ExpirationDate >= date1 && q.ExpirationDate <= date2);
                        case ("Last Action Date"):
                            return query = query.Where(q => q.LastActionDate >= date1 && q.LastActionDate <= date2);
                        default:
                            return query;
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("Incorrect value(s) in property '{0}'.", property));
                    return query;
                }
            }
            else
                return query;
        }


        /// <summary>
        /// QueryBuilder use this method to validate user typed floating point number and to get proper double values.
        /// </summary>
        /// <param name="obj">Method attempt to split object to find out if user typed a range of floating point numbers.</param>
        private double[] GetDoubles (object obj)
        {
            var values = obj.ToString().Replace('.',',').Split('-');

            switch (values.Length)
            {
                case (1):
                    if (Double.TryParse(values[0], out double val))
                        return new double[] { val };
                    else
                        return new double[] { };
                case (2):
                    if (Double.TryParse(values[0], out double val1) && Double.TryParse(values[1], out double val2))
                        return new double[] { val1, val2 };
                    else
                        return new double[] { };
                default:
                    return new double[] { };
            }
        }

        /// <summary>
        /// QueryBuilder use this method to validate user typed dates and to get proper values.
        /// </summary>
        /// <param name="obj">Method attempt to split object to find out if user typed a range of dates.</param>
        private DateTime[] GetDates (object obj)
        {
            var values = obj.ToString().Split('-');

            switch (values.Length)
            {
                case (1):

                    if (DateTime.TryParse(values[0], out DateTime date))
                        return new DateTime[] { date, date.AddDays(1) };
                        
                    else
                        return new DateTime[] { };
                case (2):
                    if (DateTime.TryParse(values[0], out DateTime date1) && DateTime.TryParse(values[1], out DateTime date2))
                        return new DateTime[] { date1, date2 };
                    else
                    {
                        var str_date_1 = String.Format("{0}.{1}.{2}", values[0], DateTime.Now.Month, DateTime.Now.Year);
                        var str_date_2 = String.Format("{0}.{1}.{2}", values[1], DateTime.Now.Month, DateTime.Now.Year);

                        if (DateTime.TryParse(str_date_1, out DateTime date_1) && DateTime.TryParse(str_date_2, out DateTime date_2))
                        {
                            return new DateTime[] { date_1, date_2 };
                        }
                        else
                            return new DateTime[] { };
                    }
                        
                default:
                    return new DateTime[] { };
            }
            
        }


        public StockItemViewModel()
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

}
