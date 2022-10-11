using Caliburn.Micro;
using DRMDesktopUILibrary.Api;
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
        private decimal _subTotal;
        private decimal _tax;
        private decimal _total;
        private ProductModel _selectedProduct;
        private readonly IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
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
                foreach (var item in Cart)
                {
                    _subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }

                return _subTotal.ToString("C");
            }
        }

        public string Tax
        {
            get { return _tax.ToString("C"); }
        }

        public string Total
        {
            get { return _total.ToString("C"); }
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
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void Buy()
        {
            throw new NotImplementedException();
        }
    }
}
