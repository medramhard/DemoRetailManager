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

        public ShellViewModel(SalesViewModel salesVM, IEventAggregator events)
        {
            _salesVM = salesVM;
            _events = events;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(_salesVM);
        }
    }
}
