using CookBook.Services;
using CookBook.ViewModels;
using DomainModel.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookBook.UI
{
    public partial class FoodManagerForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        FoodManagerCache _foodManagerCache;
        public FoodManagerForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;

            _foodManagerCache = _serviceProvider.GetRequiredService<FoodManagerCache>();

        }

        private async void FoodManagerForm_Load(object sender, EventArgs e)
        {
            await _foodManagerCache.RefreshData();
        }

        private void AvailableBtn_Click(object sender, EventArgs e)
        {
            List<ListBoxItemVM> dataSource = new List<ListBoxItemVM>();
            foreach (Recipe recipe in _foodManagerCache.AvailableRecipes)
            {
                ListBoxItemVM item = new ListBoxItemVM(recipe, recipe.Name);
                dataSource.Add(item);
            }
            RecipesLbx.SetDataSource(dataSource);
        }

        private void UnavailableBtn_Click(object sender, EventArgs e)
        {
            List<ListBoxItemVM> dataSource = new List<ListBoxItemVM>();
            foreach (Recipe recipe in _foodManagerCache.UnavailableRecipes)
            {
                ListBoxItemVM item = new ListBoxItemVM(recipe, recipe.Name);
                dataSource.Add(item);
            }
            RecipesLbx.SetDataSource(dataSource);
        }
    }
}
