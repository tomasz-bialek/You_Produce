using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Production_Facility.Models;
using System.Windows.Input;
using Production_Facility.ViewModels.Commands;
using System.Windows;
using System.Collections.ObjectModel;

namespace Production_Facility.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {


        private Order order;

        public Order Order
        {
            get
            { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        private List<Order> orders;

        public List<Order> Orders
        {
            get

            {
                using (FacilityDBContext dbContext = new FacilityDBContext())
                {
                    return orders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
                }

            }
            set
            {
                orders = value;
                OnPropertyChanged("Orders");
            }
        }
        

        private Item item;
        public Item Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }

        private string name = "Zlecenie Produkcyjne";
        

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public void SetComboBox(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var s = (string)obj;

                var entities = (from q in dbContext.Items
                                       from x in dbContext.Recipes
                                       where q.Name.Contains(s)
                                       where x.RecipeOwner == q.Number
                                       select q).ToList();

                UserChoice = entities;
            }
        }

        public void SetOrdersParams(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                Item = dbContext.Items.FirstOrDefault(q => q.Number == (string)obj);
            }
                
        }

        public void SetDataGrid(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                if (Order != null)
                {
                    Components.Clear();
                }
                else
                {
                    Components = new ObservableCollection<OrderComponent>();
                }


                var values = (object[])obj;

                var key = (string)values[0];

                var quantity_temp = (string)values[1];

                var quantity = int.Parse(quantity_temp);

                var recipe = dbContext.RecipeComponents.Where(q => q.OwnerKey == key).ToList();

                foreach (var line in recipe)
                {
                    var tempQuantity = line.Quantity * quantity;

                    var oc = new OrderComponent(line.Line, line.OwnerKey, line.OwnerName, line.ComponentKey, line.ComponentName, tempQuantity);

                    Components.Add(oc);
                }
            }

        }

        public void SaveOrder(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var cutObj = (object[])obj;

                DateTime date;

                if (cutObj[2].ToString() == "")
                    date = DateTime.Now;

                else
                    date = ((DateTime)cutObj[2]);

                if (Int32.TryParse((string)cutObj[3], out int orderID))
                {
                    var order = dbContext.Orders.Where(xx => xx.OrderID == orderID).SingleOrDefault<Order>();

                    order.Quantity = Order.Quantity;
                    order.PlannedDate = Order.PlannedDate;

                    var components = dbContext.OrderComponents.Where(q => q.OrderId == orderID).ToList();

                    MessageBox.Show("components.Count = " + components.Count.ToString() + '\n' + "Components.Count = " + Components.Count.ToString());

                    if(components.Count == Components.Count)
                    {
                        for(int i=0;i<components.Count;i++)
                        {
                            components[i].ComponentKey = Components[i].ComponentKey;
                            components[i].ComponentName = Components[i].ComponentName;
                            components[i].Line = Components[i].Line;
                            components[i].OwnerKey = Components[i].OwnerKey;
                            components[i].OwnerName = Components[i].OwnerName;
                            components[i].Quantity = Components[i].Quantity;
                        }
                        
                    }

                    else if (components.Count > Components.Count)
                    {
                        for (int i = 0; i < Components.Count; i++)
                        {
                            components[i].ComponentKey = Components[i].ComponentKey;
                            components[i].ComponentName = Components[i].ComponentName;
                            components[i].Line = Components[i].Line;
                            components[i].OwnerKey = Components[i].OwnerKey;
                            components[i].OwnerName = Components[i].OwnerName;
                            components[i].Quantity = Components[i].Quantity;
                        }

                        for (int i = Components.Count; i < components.Count; i++)
                        {
                            dbContext.OrderComponents.Remove(components[i]);
                        }
                    }

                    else
                    {
                        for (int i = 0; i < components.Count; i++)
                        {
                            components[i].ComponentKey = Components[i].ComponentKey;
                            components[i].ComponentName = Components[i].ComponentName;
                            components[i].Line = Components[i].Line;
                            components[i].OwnerKey = Components[i].OwnerKey;
                            components[i].OwnerName = Components[i].OwnerName;
                            components[i].Quantity = Components[i].Quantity;
                        }

                        for (int i = components.Count; i < Components.Count; i++)
                        {
                            //components.Add(Components[i]);

                            dbContext.OrderComponents.Add(Components[i]);
                        }
                    }

                    dbContext.SaveChanges();
                }
                else
                {
                    var x = dbContext.Orders.Add(new Order(cutObj[0].ToString(),  Convert.ToInt32(cutObj[1]), date));

                    foreach (OrderComponent rc in Components)
                    {
                        var order = new OrderComponent(rc.Line, rc.OwnerKey, rc.OwnerName, x.OrderID, rc.ComponentKey, rc.ComponentName, rc.Quantity);

                        dbContext.OrderComponents.Add(order);
                    }
                    dbContext.SaveChanges();

                    Order = x;

                }
                
            }

        }

        public void ProduceOrder(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var cutObj = (object[])obj;

                var key = (string)cutObj[0];
                var name = (string)cutObj[1];
                var quantity = Convert.ToDecimal((string)cutObj[2]);
                var unit = (string)cutObj[3];
                var orderID = (string)cutObj[4];

                var item = dbContext.Items.SingleOrDefault(i => i.Number == key);

                var tCost = new decimal();

                var isAllStockItemsAvailable = true;

                foreach (OrderComponent line in Components)
                {
                    var qAvailable = Convert.ToDecimal(line.Quantity);
                    var xxx = dbContext.StockItems.Where(q => q.Number == line.ComponentKey).OrderBy(xx => xx.ExpirationDate).FirstOrDefault<StockItem>();

                    if (xxx == null)
                    {
                        isAllStockItemsAvailable = false;
                        MessageBox.Show(String.Format("Brak w bazie pozycji | {0} |\n| {1} | \nPRODUKCJA WSTRZYMANA", line.ComponentKey, line.ComponentName));
                    }
                }

                if (isAllStockItemsAvailable)
                {
                    foreach (OrderComponent line in Components)
                    {
                        var qAvailable = Convert.ToDecimal(line.Quantity);
                        var xxx = dbContext.StockItems.Where(xx => xx.Number == line.ComponentKey).OrderBy(xx => xx.ExpirationDate).FirstOrDefault<StockItem>();

                        xxx.QTotal = xxx.QTotal - line.Quantity;
                        xxx.QAvailable = xxx.QTotal;
                        xxx.LastActionDate = DateTime.Now;
                        tCost += xxx.UnitCost * Convert.ToDecimal(line.Quantity);
                        dbContext.SaveChanges();
                    }
                    var uCost = tCost / quantity;
                    var temp_orderID = Convert.ToInt32(orderID);
                    var prOrder = dbContext.Orders.FirstOrDefault(q => q.OrderID == temp_orderID);
                    prOrder.OrderStatus = "CLOSED";
                    prOrder.ClosingDate = DateTime.Now;
                    var newStockItem = new StockItem(key, item.Name, quantity.ToString(), "WR-PR-WG", uCost.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.AddYears(2).ToString(), item.Unit.ToString(), item.Section.ToString(),orderID);
                    dbContext.StockItems.Add(newStockItem);
                    dbContext.SaveChanges();
                    MessageBox.Show(String.Format("WYPRODUKOWANO : \n\n | {0} | \n\n | {1} | \n\n w ilości | {2} {3} |)", newStockItem.Number,newStockItem.Name, newStockItem.QTotal, newStockItem.Unit));
                }


            }

        }
        public void ProdOrderChosen(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {


                if (Components != null)
                    Components.Clear();

                if (Int32.TryParse((string)obj, out int orderID))
                {
                    var orderComponents = dbContext.OrderComponents.Where(q => q.OrderId == orderID).ToList();

                    Components = new ObservableCollection<OrderComponent>(orderComponents);


                    //Order = recipe.GetRecipe(productionOrder.OrderComposition);

                    Order = dbContext.Orders.SingleOrDefault(q => q.OrderID == orderID);
                    Item = dbContext.Items.FirstOrDefault(i => i.Number == Order.OwnerKey);
                }


                //int orderID = Convert.ToInt32((string)obj);


            }

        }


        private ICommand _ProdOrderChosenCommand;
        public ICommand ProdOrderChosenCommand
        {
            get
            {
                if (_ProdOrderChosenCommand == null)
                {
                    _ProdOrderChosenCommand = new RelayCommand(ProdOrderChosen, Can_ProdOrderChosen_Execute);
                }
                return _ProdOrderChosenCommand;
            }
        }

        private ICommand _RefreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null)
                {
                    _RefreshCommand = new RelayCommand(RefreshComboBox);
                }
                return _RefreshCommand;
            }
        }

        private ICommand _LoadOrderCommand;
        public ICommand LoadOrderCommand
        {
            get
            {
                if (_LoadOrderCommand == null)
                {
                    _LoadOrderCommand = new RelayCommand(SetDataGrid, Can_SetDataGrid_Execute);
                }
                return _LoadOrderCommand;
            }
        }

        private ICommand _ComboBoxLoader;
        public ICommand ComboBoxLoader
        {
            get
            {
                if (_ComboBoxLoader == null)
                {
                    _ComboBoxLoader = new RelayCommand(SetComboBox);
                }
                return _ComboBoxLoader;
            }
        }

        private ICommand _OrdersParamsLoader;
        public ICommand OrdersParamsLoader
        {
            get
            {
                if (_OrdersParamsLoader == null)
                {
                    _OrdersParamsLoader = new RelayCommand(SetOrdersParams);
                }
                return _OrdersParamsLoader;
            }
        }

        private ICommand _SaveOrderCommand;
        public ICommand SaveOrderCommand
        {
            get
            {
                if (_SaveOrderCommand == null)
                {
                    _SaveOrderCommand = new RelayCommand(SaveOrder);
                }
                return _SaveOrderCommand;
            }
        }



        private ICommand _ProduceOrderCommand;
        public ICommand ProduceOrderCommand
        {
            get
            {
                if (_ProduceOrderCommand == null)
                {
                    _ProduceOrderCommand = new RelayCommand(ProduceOrder);
                }
                return _ProduceOrderCommand;
            }
        }



        private void RefreshComboBox(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                Orders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
            }
        }

        private bool Can_ProdOrderChosen_Execute(object obj)
        {
            if (Int32.TryParse((string)obj, out int orderID))
                return true;
            return false;

        }

        private bool Can_SetDataGrid_Execute(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var view = (object[])obj;
                if (view == null || view[0] == null || view[1] == null)
                    return false;

                string key = (string)view[0];

                bool isInteger = int.TryParse((string)view[1], out int quantity);

                if (isInteger && dbContext.Recipes.Where(xx => xx.RecipeOwner == key).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
                
        }

        private List<Item> userChoice = new List<Item>();
        public List<Item> UserChoice
        {
            get { return userChoice; }
            set
            {
                userChoice = value;
                OnPropertyChanged("UserChoice");
            }
        }

        private ObservableCollection<OrderComponent> components;
        public ObservableCollection<OrderComponent> Components
        {
            get { return components; }
            set
            {
                components = value;
                OnPropertyChanged("Components");
            }
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
