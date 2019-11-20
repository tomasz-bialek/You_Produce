using Production_Facility.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Production_Facility.ViewModels
{
    public class ViewModelNavigator : INotifyPropertyChanged
    {
        public ICommand ItemCommand { get; set; }
        public ICommand StockItemCommand { get; set; }
        public ICommand RecipeCommand { get; set; }
        public ICommand ProductionOrderCommand { get; set; }


        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

        public ViewModelNavigator()
        {
            ItemCommand = new RelayCommand(SetItemVM);
            StockItemCommand = new RelayCommand(SetStockItemVM);
            RecipeCommand = new RelayCommand(SetRecipeVM);
            ProductionOrderCommand = new RelayCommand(SetProductionOrderVM);
        }

        private void SetProductionOrderVM(object obj)
        {
            SelectedViewModel = new OrderViewModel();
        }

        public void SetStockItemVM(object obj)
        {
            SelectedViewModel = new StockItemViewModel();
        }

        public void SetItemVM(object obj)
        {

            SelectedViewModel = new ItemViewModel();
        }

        public void SetRecipeVM (object obj)
        {
            SelectedViewModel = new RecipeViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
