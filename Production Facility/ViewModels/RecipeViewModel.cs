using Production_Facility.Models;
using Production_Facility.ViewModels.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace Production_Facility.ViewModels
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        public RecipeViewModel()
        {

            ComboBoxLoader = new RelayCommand(SetComboBox);
            DataGridLoader = new RelayCommand(SetRecipe);
        }

        public ICommand DataGridLoader { get; set; }
        public ICommand ComboBoxLoader { get; set; }

        private string name = "Receptury";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        private Recipe recipe;
        public Recipe Recipe
        {
            get { return recipe; }
            set
            {
                recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        public void SetComboBox (object obj)
        {
            using(FacilityDBContext dbContext = new FacilityDBContext())
            {
                var entities = (from q in dbContext.Items
                                       from x in dbContext.Recipes
                                       where q.Name.Contains((string)obj)
                                       where x.RecipeOwner == q.Number
                                       select q).ToList();

                UserChoice = entities;
            }
            
        }

        public void SetRecipe(object obj)
        {
            using (FacilityDBContext dbContext = new FacilityDBContext())
            {
                Recipe = dbContext.Recipes.SingleOrDefault(q => q.RecipeOwner == (string)obj);

                var comps = dbContext.RecipeComponents.Where(q => q.OwnerKey == Recipe.RecipeOwner).ToList();

                RecipeComponents = new ObservableCollection<RecipeComponent>(comps);
            }
                
        }

        private ObservableCollection<RecipeComponent> recipeComponents = new ObservableCollection<RecipeComponent>();

        public ObservableCollection<RecipeComponent> RecipeComponents
        {
            get
            {
                return recipeComponents;
            }
            set
            {
                recipeComponents = value;
                OnPropertyChanged("RecipeComponents");
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
