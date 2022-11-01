using DRMDesktopUI.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics;
using DRMDesktopUILibrary.Api.Interfaces;
using AutoMapper;
using DRMDesktopUILibrary.Models;
using System.Dynamic;
using System.Windows;

namespace DRMDesktopUI.ViewModels;

public class InventoryViewModel : Screen
{
    private readonly IProductEndpoint _productEndpoint;
    private readonly IMapper _mapper;
    private readonly IWindowManager _window;
    private readonly StatusInfoViewModel _status;
    private BindingList<ProductDisplayModel> _items;
    private ProductDisplayModel _selectedItem;
    private int _quantity = 1;

    public InventoryViewModel(IProductEndpoint productEndpoint, IMapper mapper, IWindowManager window, StatusInfoViewModel status)
    {
        _productEndpoint = productEndpoint;
        _mapper = mapper;
        _window = window;
        _status = status;
    }

    protected override async void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        try
        {
            await LoadItems();
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
            }
            else
            {
                _status.UpdateMessage("Fatal Exception", ex.Message);
            }

            _window.ShowDialogAsync(_status, null, settings);
            await TryCloseAsync();
        }
    }

    private async Task LoadItems()
    {
        var results = new BindingList<ProductModel>(await _productEndpoint.GetAll());
        Items = new BindingList<ProductDisplayModel>(_mapper.Map<List<ProductDisplayModel>>(results));
    }

    private async Task ResetInventoryViewModel()
    {
        Items = new();
        _quantity = 1;
        await LoadItems();
        NotifyOfPropertyChange(() => Quantity);
        NotifyOfPropertyChange(() => CanRestock);
    }

    public BindingList<ProductDisplayModel> Items
    {
        get { return _items; }
        set
        {
            _items = value;
            NotifyOfPropertyChange(() => Items);
        }
    }

    public ProductDisplayModel SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            _selectedItem = value;
            NotifyOfPropertyChange(() => SelectedItem);
            NotifyOfPropertyChange(() => CanRestock);
        }
    }

    public int Quantity
    {
        get { return _quantity; }
        set
        {
            _quantity = value;
            NotifyOfPropertyChange(() => Quantity);
            NotifyOfPropertyChange(() => CanRestock);
        }
    }

    public bool CanRestock
    {
        get
        {
            bool output = false;

            if(SelectedItem != null && Quantity > 0)
            {
                output = true;
            }

            return output;
        }
    }

    public async Task Restock()
    {
        ProductModel item = new()
        {
            Id = SelectedItem.Id,
            QuantityInStock = SelectedItem.QuantityInStock + _quantity
        };

        await _productEndpoint.UpdateQuantity(item);
        await ResetInventoryViewModel();
    }
}
