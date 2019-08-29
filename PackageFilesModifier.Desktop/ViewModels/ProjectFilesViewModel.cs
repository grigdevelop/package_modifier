using PackageFilesModifier.Desktop.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageFilesModifier.Desktop.ViewModels
{
    public class ProjectFilesViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ProjectFilesViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(OnFolderSelected);
        }

        private void OnFolderSelected(string solutionPath)
        {
            
        }
    }
}
