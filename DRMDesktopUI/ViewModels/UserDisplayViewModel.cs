using Caliburn.Micro;
using DRMDesktopUI.Models;
using DRMDesktopUILibrary.Api;
using DRMDesktopUILibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IWindowManager _window;
        private readonly StatusInfoViewModel _status;

        public UserDisplayViewModel(IUserEndpoint userEndpoint, IWindowManager window, StatusInfoViewModel status)
        {
            _userEndpoint = userEndpoint;
            _window = window;
            _status = status;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
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

        private async Task LoadUsers()
        {
            Users = new BindingList<UserModel>(await _userEndpoint.GetAll());
        }

        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

    }
}
