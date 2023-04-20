using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; }
        public virtual ICollection<Ingredient>? Ingredients { get; set; }
        public virtual ICollection<Step>? Steps { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
    }
}