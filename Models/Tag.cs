using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid RecipeId { get; set; }
        public virtual Recipe? recipe { get; set; }
    }
}