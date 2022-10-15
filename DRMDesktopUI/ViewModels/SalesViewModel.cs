using AutoMapper;
using Caliburn.Micro;
using DRMDesktopUI.Models;
using DRMDesktopUILibrary.Api;
using DRMDesktopUILibrary.Helpers;
using DRMDesktopUILibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private int _productQuantity = 1;
        private CartItemDisplayModel _selectedInCart;
        private ProductDisplayModel _selectedProduct;
        private readonly IProductEndpoint _productEndpoint;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly IMapper _mapper;
        private readonly IWindowManager _window;
        private readonly StatusInfoViewModel _status;

        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, IConfigHelper configHelper, IMapper mapper, IWindowManager window, StatusInfoViewModel status)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
            _window = window;
            _status = status;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message.ToLower() == "unauthorized")
                {
                    _status.UpdateMessage("Unathorized Access", "You do not have permission to interact with Sale Page");
                    _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialogAsync(_status, null, settings);
                }

                await TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var results = new BindingList<ProductModel>(await _productEndpoint.GetAll());
            Products = new BindingList<ProductDisplayModel>(_mapper.Map<List<ProductDisplayModel>>(results));
        }
        private async Task ResetSaleViewmodel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanBuy);
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


        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public CartItemDisplayModel SelectedInCart
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

                if (SelectedProduct?.QuantityInStock > 0)
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

                if (SelectedInCart?.QuantityInCart > 0)
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ProductQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
            NotifyOfPropertyChange(() => CanBuy);
        }

        public void Remove()
        {
            ProductDisplayModel product = Products.FirstOrDefault(x => x == SelectedInCart.Product);

            product.QuantityInStock += ProductQuantity;
            SelectedInCart.QuantityInCart -= ProductQuantity;

            if (SelectedInCart.QuantityInCart < 1)
            {
                Cart.Remove(SelectedInCart);
            }

            ProductQuantity = 1;

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

            await _saleEndpoint.Post(sale);
            ProductQuantity = 1;
            await ResetSaleViewmodel();
        }
    }
}
