using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;

namespace RecipeAPI.Data
{
    public class RecipeDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.recipe)
                .HasForeignKey(i => i.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.recipe)
                .HasForeignKey(s => s.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Images)
                .WithOne(i => i.recipe)
                .HasForeignKey(i => i.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Tags)
                .WithOne(t => t.recipe)
                .HasForeignKey(t => t.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasData(
                    new Recipe
                    {
                        Id = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b5b"),
                        Title = "Pancakes",
                        Description = "Delicious pancakes",
                        Rating = 5.0
                    }
                );

            modelBuilder.Entity<Ingredient>()
                .HasData(
                    new Ingredient
                    {
                        Id = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b6b"),
                        Name = "Flour",
                        Quantity = 1.0,
                        RecipeId = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b5b")
                    }
                );

            modelBuilder.Entity<Step>()
                .HasData(
                    new Step
                    {
                        Id = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b7b"),
                        Number = 1,
                        Description = "Mix flour and water",
                        RecipeId = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b5b")
                    }
                );

            modelBuilder.Entity<Image>()
                .HasData(
                    new Image
                    {
                        Id = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b8b"),
                        Url = "https://www.google.com",
                        Alt = "Pancakes",
                        RecipeId = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b5b")
                    }
                );

            modelBuilder.Entity<Tag>()
                .HasData(
                    new Tag
                    {
                        Id = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b9b"),
                        Name = "Breakfast",
                        RecipeId = Guid.Parse("f5b5b5b5-5b5b-5b5b-5b5b-5b5b5b5b5b5b")
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}