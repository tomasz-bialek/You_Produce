﻿using System;
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
        private Item _item;
        public Item MasterItem
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged("MasterItem");
            }
        }

        private Item this [string key]
        {
            get
            {
                using (var dbContext = new FacilityDBContext())
                {
                    return dbContext.Items.SingleOrDefault(q => q.Number == key);
                }
            }
        }



        private List<Item> _itemsFound = new List<Item>();
        public List<Item> ItemsFound
        {
            get { return _itemsFound; }
            set
            {
                _itemsFound = value;
                OnPropertyChanged("ItemsFound");
            }
        }


        private Order _order;
        public Order Order
        {
            get
            { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("Order");
            }
        }


        private List<Order> _existingOrders;
        public List<Order> ExistingOrders
        {
            get
            {
                using (FacilityDBContext dbContext = new FacilityDBContext())
                {
                    return _existingOrders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
                }
            }
            set
            {
                _existingOrders = value;
                OnPropertyChanged("ExistingOrders");
            }
        }


        private ObservableCollection<OrderComponent> _components;
        public ObservableCollection<OrderComponent> Components
        {
            get { return _components; }
            set
            {
                _components = value;
                OnPropertyChanged("Components");
            }
        }

        private ObservableCollection<OrderComponent> _componentsInDB;
        public ObservableCollection<OrderComponent> ComponentsInDB
        {
            get { return _componentsInDB; }
            set
            {
                _componentsInDB = value;
                OnPropertyChanged("ComponentsInDB");
            }
        }



        /// <summary>
        /// Find items with existing recipes by a name in a database.
        /// </summary>
        /// <param name="obj">Object will be used as a string to find existing items with recipes.</param>
        public void FindItems(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var name = (string)obj;

                var entities = (from q in dbContext.Items
                                       from x in dbContext.Recipes
                                       where q.Name.Contains(name)
                                       where x.RecipeOwner == q.Number
                                       select q).ToList();

                ItemsFound = entities;
            }
        }


        /// <summary>
        /// Choose master item from founded results.
        /// </summary>
        /// <param name="obj">Object will be used as a string to set proper master item.</param>
        public void ItemChosen(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                MasterItem = this[(string)obj];
            }  
        }


        /// <summary>
        /// Generate order components from existed recipe with required quantity.
        /// </summary>
        /// <param name="obj">Object will be used as an array[2], then [0] as a string and [1] as an int.</param>
        public void SetComponents(object obj)
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


                var objects = (object[])obj;

                var key = (string)objects[0];

                var quantity_temp = (string)objects[1];

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


        /// <summary>
        /// Save new order to database or save existing order with new parameters.
        /// </summary>
        /// <param name="obj">Object will be used as an array[4], [0] as a String (key), [1] as an Int (quantity), [2] as a DateTime, [3] as an Int (orderID - if exist)</param>
        public void SaveOrder(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var objects = (object[])obj;

                if (!DateTime.TryParse(objects[2].ToString(), out DateTime date))
                    date = DateTime.Now;



                if (Int32.TryParse((string)objects[3], out int orderID))
                {
                    var order = dbContext.Orders.SingleOrDefault(xx => xx.OrderID == orderID);

                    order.Quantity = Convert.ToInt32(objects[1]);
                    order.PlannedDate = date; 

                    var components = dbContext.OrderComponents.Where(q => q.OrderId == orderID).ToList();

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
                            var key = (string)objects[0];

                            var item = this[key];

                            Components[i].OwnerKey = key;
                            Components[i].OwnerName = item.Name;
                            Components[i].OrderId = orderID;

                            if (this[Components[i].ComponentKey] != null)
                            {
                                Components[i].ComponentName = this[Components[i].ComponentKey].Name;
                                dbContext.OrderComponents.Add(Components[i]);
                            }
                            else
                                MessageBox.Show("Item " + Components[i].ComponentKey + " don't exist !");


                        }
                    }

                    dbContext.SaveChanges();
                    ExistingOrders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
                    Order = dbContext.Orders.SingleOrDefault(xx => xx.OrderID == orderID);

                    Components = SetComponents(orderID);

                    //try
                    //{
                    //    dbContext.SaveChanges();
                    //    ExistingOrders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
                    //    Order = dbContext.Orders.SingleOrDefault(xx => xx.OrderID == orderID);

                    //    Components = SetComponents(orderID);
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show("exception");
                    //}

                }
                else
                {
                    var newOrder = dbContext.Orders.Add(new Order(objects[0].ToString(),  Convert.ToInt32(objects[1]), date));

                    foreach (OrderComponent oc in Components)
                    {
                        var order = new OrderComponent(oc.Line, oc.OwnerKey, oc.OwnerName, newOrder.OrderID, oc.ComponentKey, oc.ComponentName, oc.Quantity);

                        dbContext.OrderComponents.Add(order);
                    }
                    dbContext.SaveChanges();
                    ExistingOrders = dbContext.Orders.Where(q => q.OrderStatus == "PLANNED").ToList();
                    Order = newOrder;

                    Components = SetComponents(Order.OrderID);
                }
            }
        }

        private ObservableCollection <OrderComponent> SetComponents (int id)
        {
            using (var dbContext = new FacilityDBContext())
            {
                var orderComponents = dbContext.OrderComponents.Where(q => q.OrderId == id).ToList();

                ComponentsInDB = new ObservableCollection<OrderComponent>(orderComponents);

                var components = new ObservableCollection<OrderComponent>();

                foreach (OrderComponent oc in orderComponents)
                {
                    components.Add(new OrderComponent()
                    {
                        OrderId = oc.OrderId,
                        Line = oc.Line,
                        ComponentKey = oc.ComponentKey,
                        ComponentName = oc.ComponentName,
                        OwnerKey = oc.OwnerKey,
                        OwnerName = oc.OwnerName,
                        Quantity = oc.Quantity
                    });
                }

                return components;
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


        public bool Can_SaveOrder_Execute (object obj)
        {
            using (var dbContext = new FacilityDBContext())
            {
                var objects = (object[])obj;



                if (objects == null)
                    return false;
                else
                {
                    if(objects[0] != null)
                    {
                        var key = (string)objects[0];

                        //New Order
                        if (Int32.TryParse(objects[1].ToString(), out int quantity) && DateTime.TryParse(objects[2].ToString(), out DateTime date))
                        {
                            if (quantity != Order.Quantity || date.ToShortDateString() != Order.PlannedDate.ToShortDateString())
                                return true;
                            else if (!Int32.TryParse(objects[3].ToString(), out int id) && Int32.TryParse(objects[1].ToString(), out int quant) && dbContext.Recipes.Any(xx => xx.RecipeOwner == key) && Components != null)
                                return true;
                            else if (ComponentsInDB != null)
                            {
                                if (Components.Count == ComponentsInDB.Count)
                                {
                                    bool isComponentsCollectionsEqual = false;

                                    for (int i = 0; i < ComponentsInDB.Count; i++)
                                    {
                                        if (ComponentsInDB[i].Line != Components[i].Line || ComponentsInDB[i].ComponentKey != Components[i].ComponentKey
                                            || ComponentsInDB[i].Quantity != Components[i].Quantity)
                                            isComponentsCollectionsEqual = true;
                                    }

                                    return isComponentsCollectionsEqual;
                                }
                                else
                                    return true;

                                
                            }
                            return false;
                        }
                        else if (!Int32.TryParse(objects[3].ToString(), out int id) && Int32.TryParse(objects[1].ToString(), out int quant) && dbContext.Recipes.Any(xx => xx.RecipeOwner == key) && Components != null)
                            return true;
                        else if (Order == null)//|| OrderInDB == null
                            return false;
                        else
                        {
                            if (ComponentsInDB != null)
                            {
                                bool isComponentsCollectionsEqual = false;

                                for (int i = 0; i < ComponentsInDB.Count; i++)
                                {
                                    if (ComponentsInDB[i].Line != Components[i].Line || ComponentsInDB[i].ComponentKey != Components[i].ComponentKey
                                        || ComponentsInDB[i].Quantity != Components[i].Quantity)
                                        isComponentsCollectionsEqual = true;
                                }

                                return isComponentsCollectionsEqual;
                            }

                            else
                                return false;
                        }
                    }
                    else
                        return false;
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

                    ComponentsInDB = new ObservableCollection<OrderComponent>(orderComponents);

                    Components = new ObservableCollection<OrderComponent>();

                    foreach (OrderComponent oc in ComponentsInDB)
                    {
                        Components.Add(new OrderComponent()
                        {
                            OrderId = oc.OrderId,
                            Line = oc.Line,
                            ComponentKey = oc.ComponentKey,
                            ComponentName = oc.ComponentName,
                            OwnerKey = oc.OwnerKey,
                            OwnerName = oc.OwnerName,
                            Quantity = oc.Quantity
                        });
                    }

                    Order = dbContext.Orders.SingleOrDefault(q => q.OrderID == orderID);

                    MasterItem = this[Order.OwnerKey];

                    //MasterItem = dbContext.Items.FirstOrDefault(i => i.Number == Order.OwnerKey);
                }
            }

        }

        private bool Can_ProdOrderChosen_Execute(object obj)
        {
            if (Int32.TryParse((string)obj, out int orderID))
                return true;
            return false;

        }

        private bool Can_SetComponents_Execute(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                var objects = (object[])obj;
                if (objects == null || objects[0] == null || objects[1] == null)
                    return false;

                string key = (string)objects[0];

                bool isInteger = int.TryParse((string)objects[1], out int quantity);

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


        private ICommand _existingOrderChosenCommand;
        public ICommand ExistingOrderChosenCommand
        {
            get
            {
                if (_existingOrderChosenCommand == null)
                {
                    _existingOrderChosenCommand = new RelayCommand(ProdOrderChosen, Can_ProdOrderChosen_Execute);
                }
                return _existingOrderChosenCommand;
            }
        }


        private ICommand _findItems;
        public ICommand FindItemsCommand
        {
            get
            {
                if (_findItems == null)
                {
                    _findItems = new RelayCommand(FindItems);
                }
                return _findItems;
            }
        }


        private ICommand _itemChosenCommand;
        public ICommand ItemChosenCommand
        {
            get
            {
                if (_itemChosenCommand == null)
                {
                    _itemChosenCommand = new RelayCommand(ItemChosen);
                }
                return _itemChosenCommand;
            }
        }


        private ICommand _setComponentsCommand;
        public ICommand SetComponentsCommand
        {
            get
            {
                if (_setComponentsCommand == null)
                {
                    _setComponentsCommand = new RelayCommand(SetComponents, Can_SetComponents_Execute);
                }
                return _setComponentsCommand;
            }
        }


        private ICommand _SaveOrderCommand;
        public ICommand SaveOrderCommand
        {
            get
            {
                if (_SaveOrderCommand == null)
                {
                    _SaveOrderCommand = new RelayCommand(SaveOrder, Can_SaveOrder_Execute);
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
