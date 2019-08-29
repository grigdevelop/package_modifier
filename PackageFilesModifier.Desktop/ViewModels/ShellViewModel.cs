using PackageFilesModifier.Desktop.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PackageFilesModifier.Desktop.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;


        public ICommand RaiseSelectFolderCommand { get; private set; }

        private string _selectedFolder;

        public string SelectedFolder
        {
            get => _selectedFolder;
            set => base.SetProperty(ref _selectedFolder, value);
        }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            this.RaiseSelectFolderCommand = new DelegateCommand(OnSelectFolder);
            this._eventAggregator = eventAggregator;
        }

        private void OnSelectFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SelectedFolder = dialog.SelectedPath;
                    _eventAggregator.GetEvent<FolderSelectedEvent>().Publish(SelectedFolder);
                }
            }
        }
    }
}
