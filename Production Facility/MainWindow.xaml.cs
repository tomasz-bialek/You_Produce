using Production_Facility.Models;
using Production_Facility.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Production_Facility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelNavigator();

            using (FacilityDBContext context = new FacilityDBContext())
            {
                foreach (Recipe r in context.Recipes)
                {
                    foreach(var line in r.GetRecipe(r.RecipeComposition))
                    {
                        var item = context.Items.SingleOrDefault(i => i.Number == r.RecipeOwner);

                        var com = new Component((byte)line.RecipeLine_Nr,r.RecipeOwner,item.Name,r.RecipeID,line.RecipeLine_Key,line.RecipeLine_Name,line.RecipeLine_Amount);
                        context.Components.Add(com);
                    }
                }

                context.SaveChanges();
            }


        }
    }
}
