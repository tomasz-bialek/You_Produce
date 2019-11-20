using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    [Table("Recipe")]
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }
        public string RecipeOwner { get; set; }

        public string RecipeComposition { get; set; }

        public List<RecipeLine> ItemRecipe = new List<RecipeLine>();


        public Recipe(string owner,string composition)
        {
            this.RecipeOwner = owner;
            this.RecipeComposition = composition;
        }
        public Recipe(string owner)
        {
            this.RecipeOwner = owner;
        }
        public Recipe()
        {

        }

        public RecipeLine recipeLine;

        public List<RecipeLine> GetRecipe (FacilityDBContext context, string key)
        {
            var list = new List<RecipeLine>();

            var recipe = (from q in context.Recipes where q.RecipeOwner == key select q).FirstOrDefault<Recipe>();

            var line = recipe.RecipeComposition.Split('|');

            foreach(string pozycja in line)
            {
                var cut = pozycja.Split('=');
                var newRecipeline = new RecipeLine(int.Parse(cut[0]), cut[1], cut[2], double.Parse(cut[3]));
                list.Add(newRecipeline);

            }
            return list;
        }

        public ObservableCollection<RecipeLine> GetRecipe(string recipeComposition)
        {
            var list = new ObservableCollection<RecipeLine>();

            var line = recipeComposition.Split('|');

            foreach (string pozycja in line)
            {
                var cut = pozycja.Split('=');
                var newRecipeline = new RecipeLine(int.Parse(cut[0]), cut[1], cut[2], double.Parse(cut[3]));
                list.Add(newRecipeline);

            }
            return list;
        }

        public string GetRecipeComposition(ObservableCollection<Recipe.RecipeLine> recipeLines )
        {
            var sb = new StringBuilder();
            foreach(Recipe.RecipeLine line in recipeLines)
            {
                sb.Append(line.RecipeLine_Nr.ToString() + '=' + line.RecipeLine_Key + '=' + line.RecipeLine_Name + '=' + line.RecipeLine_Amount +'|');
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public class RecipeLine
        {
            public int RecipeLine_Nr { get; set; }
            public string RecipeLine_Key { get; set; }
            public string RecipeLine_Name { get; set; }
            public double RecipeLine_Amount { get; set; }

            public RecipeLine(int line_nr, string key, string name, double amount)
            {
                this.RecipeLine_Nr = line_nr;
                this.RecipeLine_Key = key;
                this.RecipeLine_Name = name;
                this.RecipeLine_Amount = amount;
            }
        }
    }
}
