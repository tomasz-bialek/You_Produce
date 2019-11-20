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
    public class ItemViewModel : INotifyPropertyChanged
    {
        

        private string name = "Indeksy";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        private Item model;
        public Item Model
        {
            get { return model; }
            set
            {
                model = value;
            }
        }

        public ItemViewModel()
        {
            DataGridLoader = new RelayCommand(SetItems);
        }
        public ItemViewModel(Item model)
        {
            Model = model;
        }

        public ICommand DataGridLoader { get; set; }

        public void SetItems(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var values = (object[])obj;

                var section = (string)values[0];
                switch (section)
                {
                    case ("Product"):
                        section = "0";
                        break;
                    case ("Intermediate"):
                        section = "1";
                        break;
                    case ("Substance"):
                        section = "2";
                        break;
                    case ("Article"):
                        section = "3";
                        break;
                    default:
                        section = "";
                        break;
                }

                var unit = (string)values[1];
                switch (unit)
                {
                    case ("szt"):
                        unit = "0";
                        break;
                    case ("kg"):
                        unit = "1";
                        break;
                    case ("m"):
                        unit = "2";
                        break;
                    default:
                        unit = "";
                        break;
                }
                var key = (string)values[2];
                var name = (string)values[3];

                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM Items ");

                bool isBuildingStarted = false;

                for (int i = 0; i < values.Count(); i++)
                {

                    if (!string.IsNullOrEmpty(values[i].ToString()))
                        switch (i)
                        {
                            case (0):
                                queryBuilder.Append("WHERE Section LIKE '%" + section + "%'");
                                isBuildingStarted = true;
                                break;
                            case (1):
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE Unit LIKE '%" + unit + "%'");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND Unit LIKE '%" + unit + "%'");
                                }
                                break;
                            case (2):
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE Number LIKE '%" + key + "%'");
                                    isBuildingStarted = true;
                                }
                                else
                                {
                                    queryBuilder.Append(" AND Number LIKE '%" + key + "%'");
                                }
                                break;
                            case (3):
                                if (!isBuildingStarted)
                                {
                                    queryBuilder.Append("WHERE Name LIKE '%" + name + "%'");
                                }
                                else
                                {
                                    queryBuilder.Append(" AND Name LIKE '%" + name + "%'");
                                }
                                break;
                            default:
                                break;
                        }
                }

                string s = queryBuilder.ToString();

                var items = dbContext.Items.SqlQuery(s).ToList();

                Items = items;
            }
            
        }

        private List<Item> items;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }

        }
    }

}
