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
        private BindingList<ProductModel> _items;
        private BindingList<ProductModel> _cart;
        private int _itemQuantity;
        private string _subTotal;
        private string _tax;
        private string _total;
        private readonly IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadItems();
        }

        private async Task LoadItems()
        {
            Items = new BindingList<ProductModel>(await _productEndpoint.GetAll());
        }

        public BindingList<ProductModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            get { return _subTotal; }
            set
            {
                _subTotal = value;
                NotifyOfPropertyChange(() => SubTotal);
            }
        }

        public string Tax
        {
            get { return _tax; }
            set
            {
                _tax = value;
                NotifyOfPropertyChange(() => Tax);
            }
        }

        public string Total
        {
            get { return _total; }
            set
            {
                _total = value;
                NotifyOfPropertyChange(() => Total);
            }
        }

        public bool CanAdd
        {
            get
            {
                bool output = false;
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

        public async Task Add()
        {
            throw new NotImplementedException();
        }

        public async Task Remove()
        {
            throw new NotImplementedException();
        }

        public async Task Buy()
        {
            throw new NotImplementedException();
        }
    }
}
