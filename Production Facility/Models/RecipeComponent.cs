using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    public class RecipeComponent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RecipeId { get; set; }

        public byte Line { get; set; }

        [StringLength(32)]
        [Required]
        public string OwnerKey { get; set; }

        [StringLength(128)]
        [Required]
        public string OwnerName { get; set; }

        [StringLength(32)]
        [Required]
        public string ComponentKey { get; set; }

        [StringLength(128)]
        [Required]
        public string ComponentName { get; set; }

        [Required]
        public double Quantity { get; set; }

        public Recipe Recipe { get; set; }

        public RecipeComponent (byte line, string ownerKey,string ownerName, int recipeId, string ComponentKey, string ComponentName, double quantity)
        {
            this.Line = line;
            this.OwnerKey = ownerKey;
            this.OwnerName = ownerName;
            this.RecipeId = recipeId;
            this.ComponentKey = ComponentKey;
            this.ComponentName = ComponentName;
            this.Quantity = quantity;
        }

        public RecipeComponent()
        {

        }

    }
}
