using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDesktopUI.Models;

public class CartItemDisplayModel : INotifyPropertyChanged
{
    private int _quantityInCart;

    public ProductDisplayModel Product { get; set; }
    public int QuantityInCart
    {
        get
        {
            return _quantityInCart;
        }
        set
        {
            _quantityInCart = value;
            CallPropertyChanged(nameof(QuantityInCart));
        }
    }

    private void CallPropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
