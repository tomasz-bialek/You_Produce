using Production_Facility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Production_Facility.ViewModels.Commands
{
    public class LoadOrderCommand : ICommand
    {
        FacilityDBContext context = new FacilityDBContext();

        public ProductionOrderViewModel ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public LoadOrderCommand(ProductionOrderViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //var view = (object[])parameter;
            //if (view==null ||view[0] == null || view[1] == null)
            //    return false;

            //string key = (string)view[0];

            //bool isInteger = int.TryParse((string)view[1],out int quantity);

            //if(isInteger && context.Recipes.Where(xx => xx.RecipeOwner == key).Any())
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public void Execute(object parameter)
        {
            this.ViewModel.SetDataGrid(parameter);
        }
    }
}
