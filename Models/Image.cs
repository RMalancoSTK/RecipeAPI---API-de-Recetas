using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public string? Alt { get; set; }
        public Guid RecipeId { get; set; }
        public virtual Recipe? recipe { get; set; }
    }
}