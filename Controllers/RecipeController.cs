using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Data;
using RecipeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeDbContext _context;

        public RecipeController(RecipeDbContext context)
        {
            _context = context;
        }

        [HttpGet] // GET /recipe este es para obtener todos los elementos
        public ActionResult<List<Recipe>> GetAll()
        {
            return _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Images)
                .Include(r => r.Tags)
                .ToList();
        }

        [HttpGet("{id}", Name = "GetRecipe")] // GET /recipe/{id} este es para obtener un solo elemento
        public ActionResult<Recipe> GetById(Guid id)
        {
            var recipe = _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Images)
                .Include(r => r.Tags)
                .FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        [HttpPost] // POST /recipe este es para crear
        public IActionResult Create(Recipe item)
        {
            var recipeId = Guid.NewGuid();

            // agregamos la receta pero sin los ingredientes, pasos, imágenes y tags
            _context.Recipes.Add(new Recipe
            {
                Id = recipeId,
                Title = item.Title,
                Description = item.Description,
                Ingredients = item.Ingredients.Select(i => new Ingredient
                {
                    Id = Guid.NewGuid(),
                    Name = i.Name,
                    Quantity = i.Quantity,
                    RecipeId = recipeId
                }).ToList(),
                Steps = item.Steps.Select(s => new Step
                {
                    Id = Guid.NewGuid(),
                    Description = s.Description,
                    RecipeId = recipeId
                }).ToList(),
                Images = item.Images.Select(i => new Image
                {
                    Id = Guid.NewGuid(),
                    Url = i.Url,
                    Alt = i.Alt,
                    RecipeId = recipeId
                }).ToList(),
                Tags = item.Tags.Select(t => new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = t.Name,
                    RecipeId = recipeId
                }).ToList()
            });

            _context.SaveChanges();
            return Ok("Receta agregada con éxito: " + recipeId);
        }

        [HttpPut("{id}")] // PUT /recipe/{id} este es para actualizar
        public IActionResult Update(Guid id, Recipe item)
        {
            var recipe = _context.Recipes
                          .Include(r => r.Ingredients)
                          .Include(r => r.Steps)
                          .Include(r => r.Images)
                          .Include(r => r.Tags)
                          .FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Title = item.Title;
            recipe.Description = item.Description;
            recipe.Rating = item.Rating;

            // Actualizamos los ingredientes
            UpdateCollection(recipe.Ingredients, item.Ingredients, (i, j) => i.Name == j.Name && i.Quantity == j.Quantity, (i, j) => i.Name = j.Name);

            // Actualizamos los pasos
            UpdateCollection(recipe.Steps, item.Steps, (i, j) => i.Description == j.Description, (i, j) => i.Description = j.Description);

            // Actualizamos las imágenes
            UpdateCollection(recipe.Images, item.Images, (i, j) => i.Url == j.Url && i.Alt == j.Alt, (i, j) => i.Url = j.Url);

            // Actualizamos los tags
            UpdateCollection(recipe.Tags, item.Tags, (i, j) => i.Name == j.Name, (i, j) => i.Name = j.Name);

            _context.SaveChanges();
            return Ok("Receta actualizada con éxito: " + id);
        }

        private void UpdateCollection<T>(ICollection<T> original, ICollection<T> updated, Func<T, T, bool> compare, Action<T, T> update)
        {
            // Eliminamos los elementos que ya no están en la colección actualizada
            var toRemove = original.Where(i => !updated.Any(j => compare(i, j))).ToList();
            foreach (var item in toRemove)
            {
                original.Remove(item);
            }

            // Actualizamos los elementos que ya existen
            foreach (var item in original)
            {
                var newItem = updated.FirstOrDefault(i => compare(i, item));
                if (newItem != null)
                {
                    update(item, newItem);
                }
            }

            // Agregamos los elementos que no existen
            var toAdd = updated.Where(i => !original.Any(j => compare(i, j))).ToList();
            foreach (var item in toAdd)
            {
                original.Add(item);
            }
        }

        [HttpDelete("{id}")] // DELETE /recipe/{id} este es para borrar
        public IActionResult Delete(Guid id)
        {
            var recipeToDelete = _context.Recipes.FirstOrDefault(r => r.Id == id);

            if (recipeToDelete == null)
            {
                return NotFound($"No se encontró una receta con el Id {id}");
            }

            _context.Recipes.Remove(recipeToDelete);
            _context.SaveChanges();

            return Ok($"Receta con el Id {id} fue eliminada exitosamente");
        }

    }
}