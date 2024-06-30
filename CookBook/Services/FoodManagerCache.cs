using DataAccessLayer.Contracts;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Services
{
    public class FoodManagerCache
    {
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly IRecipesRepository _recipesRepository;
        private readonly IRecipeIngredientsRepository _recipeIngredientsRepository;

        private List<Ingredient> _ingredients;
        private List<Recipe> _recipes;
        private List<RecipeIngredient> _recipeIngredients;

        public List<Recipe> AvailableRecipes;
        public List<Recipe> UnavailableRecipes;

        public FoodManagerCache(IIngredientsRepository ingredientsRepository, IRecipesRepository recipesRepository, IRecipeIngredientsRepository recipeIngredientsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
            _recipesRepository = recipesRepository;
            _recipeIngredientsRepository = recipeIngredientsRepository;
        }

        public async Task RefreshData()
        {
            _ingredients = await _ingredientsRepository.GetIngredients();
            _recipeIngredients = await _recipeIngredientsRepository.GetAllRecipeIngredients();
            _recipes = await _recipesRepository.GetAllRecipes();

            ClassifyRecipes();
        }

        public void ClassifyRecipes()
        {
            AvailableRecipes = new List<Recipe>();
            UnavailableRecipes = new List<Recipe>();

            var groupedRecipesAndIngredients = _recipeIngredients.GroupBy(ri=>ri.RecipeId).ToList();

            foreach(var recipeGroup in groupedRecipesAndIngredients)
            {
                int recipeId= recipeGroup.Key;
                bool isRecipeAvailable = true;

                foreach(var ri in recipeGroup)
                {
                    Ingredient fi = _ingredients.FirstOrDefault(i => i.Id == ri.IngredientId);

                    if(fi==null || fi.Weight < ri.Amount) 
                    { 
                        isRecipeAvailable = false;
                        break;
                    }
                }

                Recipe recipeToAdd = _recipes.FirstOrDefault(r => r.Id == recipeId);

                if (isRecipeAvailable)
                    AvailableRecipes.Add(recipeToAdd);
                else
                    UnavailableRecipes.Add(recipeToAdd);

            }
        }

    }
}
