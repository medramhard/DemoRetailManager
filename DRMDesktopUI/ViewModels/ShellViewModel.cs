using Caliburn.Micro;
using DRMDesktopUI.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesVM;
        private readonly IEventAggregator _events;
        private readonly SimpleContainer _container;

        public ShellViewModel(SalesViewModel salesVM, IEventAggregator events, SimpleContainer container)
        {
            _salesVM = salesVM;
            _events = events;
            _container = container;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(_salesVM);
        }
    }
}
