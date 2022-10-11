using Caliburn.Micro;
using DRMDesktopUILibrary.Api;
using DRMDesktopUILibrary.Helpers;
using DRMDesktopUILibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _productQuantity = 1;
        private ProductModel _selectedProduct;
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            Products = new BindingList<ProductModel>(await _productEndpoint.GetAll());
        }

        private decimal CalculateSubTotal()
        {
            decimal output = 0;

            foreach (var item in Cart)
            {
                output += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return output;
        }

        private decimal CalculateTax()
        {
            decimal output = 0;
            decimal taxRate = _configHelper.GetTaxRate();

            output = Cart.
                Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate / 100);

            return output;
        }

        private decimal CalculateTotal()
        {
            decimal output = CalculateSubTotal() + CalculateTax();

            return output;
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public int ProductQuantity
        {
            get { return _productQuantity; }
            set
            {
                _productQuantity = value;
                NotifyOfPropertyChange(() => ProductQuantity);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                return CalculateTotal().ToString("C");
            }
        }

        public bool CanAdd
        {
            get
            {
                bool output = false;

                if (ProductQuantity > 0 && SelectedProduct?.QuantityInStock >= ProductQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanRemove
        {
            get
            {
                bool output = false;
                return output;
            }
        }

        public bool CanBuy
        {
            get
            {
                bool output = false;
                return output;
            }
        }

        public void Add()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ProductQuantity;
                Cart.ResetBindings();
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ProductQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ProductQuantity;
            ProductQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            Products.ResetBindings();
        }

        public void Remove()
        {

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public void Buy()
        {
            throw new NotImplementedException();
        }
    }
}
