using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private List<Product> _allProducts = new List<Product>();

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        [ObservableProperty]
        private string? searchText;

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Clear();
            _allProducts.Clear();

            var all = _productService.GetAll() ?? Enumerable.Empty<Product>();
            foreach (var p in all)
            {
                Products.Add(p);
                _allProducts.Add(p);
            }
        }

        [RelayCommand]
        private void Search(string query)
        {
            Products.Clear();

            if (string.IsNullOrWhiteSpace(query))
            {
                foreach (var p in _allProducts)
                    Products.Add(p);
                return;
            }

            var filtered = _allProducts
                .Where(p => p.Name?.Contains(query, System.StringComparison.OrdinalIgnoreCase) == true)
                .ToList();

            foreach (var p in filtered)
                Products.Add(p);
        }
    }
}
