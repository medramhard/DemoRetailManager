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
        private CartItemModel _selectedInCart;
        private ProductModel _selectedProduct;
        private readonly IProductEndpoint _productEndpoint;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IConfigHelper _configHelper;

        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
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

        public CartItemModel SelectedInCart
        {
            get { return _selectedInCart; }
            set
            {
                _selectedInCart = value;
                NotifyOfPropertyChange(() => SelectedInCart);
                NotifyOfPropertyChange(() => CanRemove);
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
                NotifyOfPropertyChange(() => CanRemove);
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

                if (ProductQuantity > 0 && SelectedInCart?.QuantityInCart >= ProductQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanBuy
        {
            get
            {
                bool output = false;

                if (Cart?.Count > 0)
                {
                    output = true;
                }

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
            Products.ResetBindings();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanBuy);
        }

        public void Remove()
        {
            ProductModel product = Products.FirstOrDefault(x => x == SelectedInCart.Product);

            product.QuantityInStock += ProductQuantity;
            SelectedInCart.QuantityInCart -= ProductQuantity;

            if (SelectedInCart.QuantityInCart < 1)
            {
                Cart.Remove(SelectedInCart);
            }

            ProductQuantity = 1;
            Products.ResetBindings();
            Cart.ResetBindings();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanBuy);
        }

        public async Task Buy()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            // TODO: Make an API call
            await _saleEndpoint.Post(sale);

            ProductQuantity = 1;
            Cart.Clear();
            Cart.ResetBindings();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanBuy);
        }
    }
}
