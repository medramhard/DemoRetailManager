using Caliburn.Micro;
using DRMDesktopUI.EventModels;
using DRMDesktopUILibrary.Api;
using System;
using System.Threading.Tasks;

namespace DRMDesktopUI.ViewModels
{
	public class LoginViewModel : Screen
    {
		private string _userName;
		private string _password;
		private string _errorMessage;
		private readonly IApiHelper _api;
		private readonly IEventAggregator _events;

		public LoginViewModel(IApiHelper api, IEventAggregator events)
		{
			_api = api;
			_events = events;
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}

		public bool CanLogIn
		{
			get
			{
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
		}

		public bool IsErrorVisible
		{
			get 
			{
				if (string.IsNullOrWhiteSpace(ErrorMessage))
				{
					return false;
				}

				return true; 
			}
		}

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set 
			{
				_errorMessage = value;
				NotifyOfPropertyChange(() => IsErrorVisible);
				NotifyOfPropertyChange(() => ErrorMessage);
            }
		}


		public async Task LogIn()
		{
			try
			{
                ErrorMessage = string.Empty;
				var result = await _api.Authenticate(UserName, Password);

				await _api.GetUser(result.Access_Token);
				await _events.PublishOnUIThreadAsync(new LogOnEvent());
            }
            catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}
	}
}
