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
        private BindingList<UserModel> _users;
        private BindingList<UserRoleModel> _selectedUserRoles;
        private BindingList<UserRoleModel> _availableRoles;
        private UserModel _selectedUser;
        private UserRoleModel _selectedUserRole;
        private UserRoleModel _selectedAvailableRole;

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
                await LoadRoles();
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

                await _window.ShowDialogAsync(_status, null, settings);
                await TryCloseAsync();
            }
        }

        private async Task LoadUsers()
        {
            Users = new BindingList<UserModel>(await _userEndpoint.GetAll());
        }

        private async Task LoadRoles()
        {
            AvailableRoles = new BindingList<UserRoleModel>(await _userEndpoint.GetAllRoles());
        }

        private void RefreshPage()
        {
            SelectedUserRoles.ResetBindings();
            Users.ResetBindings();

            NotifyOfPropertyChange(() => Users);
            NotifyOfPropertyChange(() => SelectedUser);
            NotifyOfPropertyChange(() => SelectedUserRoles);
            NotifyOfPropertyChange(() => SelectedAvailableRole);
            NotifyOfPropertyChange(() => CanAdd);
            NotifyOfPropertyChange(() => CanRemove);
        }

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

        public BindingList<UserRoleModel> SelectedUserRoles
        {
            get { return _selectedUserRoles; }
            private set
            {
                _selectedUserRoles = value;

                NotifyOfPropertyChange(() => Users);
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => SelectedUserRoles);
            }
        }

        public BindingList<UserRoleModel> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                _selectedUserRoles = new BindingList<UserRoleModel>(SelectedUser.Roles);

                NotifyOfPropertyChange(() => Users);
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => SelectedUserRoles);
            }
        }

        public UserRoleModel SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;

                NotifyOfPropertyChange(() => SelectedUserRole);
                NotifyOfPropertyChange(() => CanRemove);
            }
        }

        public UserRoleModel SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public bool CanRemove
        {
            get
            {
                bool output = false;

                if (SelectedUserRole != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanAdd
        {
            get
            {
                bool output = false;

                if (SelectedUserRoles?.Any(x => x.Id == SelectedAvailableRole?.Id) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task Add()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole.Name);
            Users.FirstOrDefault(x => x.Id == SelectedUser.Id).Roles.Add(SelectedAvailableRole);
            RefreshPage();
        }

        public async Task Remove()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole.Name);
            Users.FirstOrDefault(x => x.Id == SelectedUser.Id).Roles.RemoveAll(x => x.Id == SelectedUserRole.Id);
            RefreshPage();
        }
    }
}
