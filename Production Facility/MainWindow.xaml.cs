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
        SortedDictionary<string, Item> bazaDanych = new SortedDictionary<string, Item>();
        Dictionary<string, Recipe> bazaReceptur = new Dictionary<string, Recipe>();
        //List<StockItem> bazaStockItem = new List<StockItem>();
        //List<StockItem> listaSI = new List<StockItem>();
        FunctionTemp ft = new FunctionTemp();

        public enum Wylicz { raz, dwa, trzy };

        FacilityDBContext context = new FacilityDBContext();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelNavigator();

            //var sr = new StreamReader("C:/Temp/Recipes.csv", Encoding.GetEncoding("UTF-8"));

            //string line;
            //string[] cut;

            //while ((line = sr.ReadLine()) != null)
            //{
            //    cut = line.Split('\t');

            //    var si = new Recipe(cut[0], cut[1]);
            //    context.Recipes.Add(si);
            //}

            //sr.Close();
            //context.SaveChanges();

            //foreach(var si in bazaStockItem)
            //{
            //    context.StockItems.Add(si);
            //}
            //context.SaveChanges();

            //foreach (StockItem si in context.StockItems.ToList())
            //{
            //    si.QAvailable = si.QTotal;
            //    si.QReserved = 0;
            //}
            //context.SaveChanges();

        }
    }
}
