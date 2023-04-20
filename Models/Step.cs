using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Step
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string? Description { get; set; }
        public Guid RecipeId { get; set; }
        public virtual Recipe? recipe { get; set; }
    }
}