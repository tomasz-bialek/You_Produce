using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Production_Facility.Models
{
    public class FunctionTemp
    {
        StreamReader zBazy = new StreamReader("C:/Temp/Z4.csv", Encoding.GetEncoding("UTF-8"));
        StreamReader zReceptur = new StreamReader("C:/Temp/SSP.csv", Encoding.GetEncoding("UTF-8"));
        StreamReader zTotena = new StreamReader("C:/Temp/TOTEN.csv", Encoding.GetEncoding("UTF-8"));
        //StreamWriter doNowejBazyItem = new StreamWriter("C:/Temp/NowaBazaItem.csv");

        //public Dictionary<string, List<string[]>> bazaReceptur = new Dictionary<string, List<string[]>>();
        public Dictionary<string, Recipe> bazaReceptur = new Dictionary<string, Recipe>();
        FacilityDBContext context = new FacilityDBContext();
        //SortedDictionary<string, Item> baza;

        string line;
        string[] cutLine;

        public void BaseReader(SortedDictionary<string, Item> bazaDanych)
        {
            while ((line = zBazy.ReadLine()) != null)
            {
                cutLine = line.Split('\t');

                if (!bazaDanych.ContainsKey(cutLine[3]))
                {
                    if (cutLine[3].Contains("W-") && cutLine[3].Length > 13)
                    {
                        Item item = new Item(cutLine[3], cutLine[2], SectionType.Product);
                        bazaDanych.Add(cutLine[3], item);
                    }
                    else if (cutLine[3].Contains("W-") && cutLine[3].Length <= 13 && cutLine[3] != "W-PD002-A0000")
                    {
                        Item item = new Item(cutLine[3], cutLine[2], SectionType.Intermediate);
                        bazaDanych.Add(cutLine[3], item);
                    }
                    else if (cutLine[3].Contains("SU") || cutLine[3] == "W-PD002-A0000")
                    {
                        Item item = new Item(cutLine[3], cutLine[2], SectionType.Substance);
                        bazaDanych.Add(cutLine[3], item);
                    }
                    else
                    {
                        Item item = new Item(cutLine[3], cutLine[2], SectionType.Article);
                        bazaDanych.Add(cutLine[3], item);
                    }
                }
            }
        }

        //public void TotenReader(List<StockItem> bazaStockItem, FacilityDBContext context)
        //{
        //    while ((line = zTotena.ReadLine()) != null)
        //    {
        //        cutLine = line.Split('\t');
        //        string number = cutLine[0];

        //        if (context.Items.Any(xx => xx.Number == number))
        //        {
        //            StockItem sItem = new StockItem(cutLine[0], cutLine[1], cutLine[2], cutLine[3],
        //                    cutLine[4], cutLine[5], cutLine[6], cutLine[8], cutLine[7]);

        //            bazaStockItem.Add(sItem);
        //        }
        //        else
        //        {
        //            StreamWriter brakWbazie = new StreamWriter("C:/Temp/BrakiWBazie3.csv", true);
        //            brakWbazie.WriteLine(cutLine[0] + '\t' + cutLine[1] + '\t' + cutLine[2]);
        //            brakWbazie.Close();
        //        }
        //    }
        //}

        public void RecepturReader(SortedDictionary<string, Item> bazaDanych, FacilityDBContext context)
        {
            while ((line = zReceptur.ReadLine()) != null)
            {
                cutLine = line.Split('\t');
                string number = cutLine[0];

                if (!bazaDanych.ContainsKey(cutLine[0]) && !context.Items.Any(xx => xx.Number == number))
                {
                    if (cutLine[0].Contains("W-") && cutLine[0].Length > 13)
                    {
                        Item item = new Item(cutLine[0], cutLine[5], SectionType.Product);
                        bazaDanych.Add(cutLine[0], item);
                    }
                    else if (cutLine[0].Contains("W-") && cutLine[0].Length <= 13 && cutLine[0] != "W-PD002-A0000")
                    {
                        Item item = new Item(cutLine[0], cutLine[5], SectionType.Intermediate);
                        bazaDanych.Add(cutLine[0], item);
                    }
                    else if (cutLine[0].Contains("SU") || cutLine[0] == "W-PD002-A0000")
                    {
                        Item item = new Item(cutLine[0], cutLine[5], SectionType.Substance);
                        bazaDanych.Add(cutLine[0], item);
                    }
                    else
                    {
                        Item item = new Item(cutLine[0], cutLine[5], SectionType.Article);
                        bazaDanych.Add(cutLine[0], item);
                    }
                }
            }
        }





        //public void ItemWriter(SortedDictionary<string, Item> bazaDanych)
        //{
        //    foreach (KeyValuePair<string, Item> p in bazaDanych)
        //    {
        //        doNowejBazyItem.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", p.Value.Number, p.Value.Name, p.Value.Unit, p.Value.Section));
        //    }
        //    doNowejBazyItem.Close();
        //}

        //public void RecipeReader(Dictionary<string, Recipe> bazaReceptur, FacilityDBContext context)
        //{

        //    while ((line = zReceptur.ReadLine()) != null)
        //    {
        //        cutLine = line.Split('\t');
        //        string number = cutLine[0];

        //        if (context.Recipes.Any(xx => xx.RecipeOwner == number))
        //        {
        //            if (!bazaReceptur.ContainsKey(cutLine[0]))
        //            {
        //                Recipe nowaReceptura = new Recipe(cutLine[0]);

        //                if (cutLine[1] == "Struktura recepturowa")
        //                {

        //                    nowaReceptura.recipeLine = new Recipe.RecipeLine(int.Parse(cutLine[2]), cutLine[3], cutLine[6], double.Parse(cutLine[7]));
        //                    nowaReceptura.ItemRecipe.Add(nowaReceptura.recipeLine);

        //                    bazaReceptur.Add(cutLine[0], nowaReceptura);
        //                }

        //                else
        //                {
        //                    nowaReceptura.recipeLine = new Recipe.RecipeLine(int.Parse(cutLine[2]), cutLine[3], cutLine[6], double.Parse(cutLine[4]));
        //                    nowaReceptura.ItemRecipe.Add(nowaReceptura.recipeLine);

        //                    bazaReceptur.Add(cutLine[0], nowaReceptura);
        //                }
        //            }
        //            else
        //            {
        //                if (cutLine[1] == "Struktura recepturowa")
        //                {
        //                    bazaReceptur[cutLine[0]].ItemRecipe.Add(new Recipe.RecipeLine(int.Parse(cutLine[2]), cutLine[3], cutLine[6], double.Parse(cutLine[7])));
        //                }
        //                else
        //                {
        //                    bazaReceptur[cutLine[0]].ItemRecipe.Add(new Recipe.RecipeLine(int.Parse(cutLine[2]), cutLine[3], cutLine[6], double.Parse(cutLine[4])));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("wtf");
        //        }

        //    }

        //    foreach (KeyValuePair<string, Recipe> pozycja in bazaReceptur)
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        foreach (Recipe.RecipeLine line in pozycja.Value.ItemRecipe)
        //        {
        //            sb.Append(line.RecipeLine_Nr.ToString() + '=' + line.RecipeLine_Key + '=' + line.RecipeLine_Name + '=' + line.RecipeLine_Amount.ToString() + '|');
        //        }
        //        sb.Remove(sb.Length - 1, 1);
        //        pozycja.Value.RecipeComposition = sb.ToString();
    //}

    //WYCIĘTE Z MAIN WINDOW (OPERACJE NA TABELI RECIPE


    //// REMOVE from Recipes table
    //foreach(Recipe pozycja in context.Recipes)
    //{
    //    context.Recipes.Remove(pozycja);
    //}

    //context.SaveChanges();

    // ADD to Recipes table

    //foreach (KeyValuePair<string, Recipe> pozycja in bazaReceptur)
    //{
    //    context.Recipes.Add(pozycja.Value);

    //}
    //context.SaveChanges();
    //}

//}
    }
}
