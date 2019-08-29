using PackageFilesModifier.Desktop.Events;
using PackageFilesModifier.Desktop.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PackageFilesModifier.Desktop.ViewModels
{
    public class AllPackagesViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private VersionedPackage _selectedVersionedPackage;


        private IDictionary<string, VersionedPackage> _versionedPackages =
            new Dictionary<string, VersionedPackage>();

        public ObservableCollection<PackageItemDocument> AllPackages { get; }
            = new ObservableCollection<PackageItemDocument>();


        public ObservableCollection<VersionedPackage> VersionedPackages { get; } =
            new ObservableCollection<VersionedPackage>();

        #region Commands

        public ICommand RaiseUpdateSelectedToLatestVersion { get; }
        public ICommand RaiseUpdateAllToLatestVersion { get; }
        public ICommand RaiseSaveChanges { get; }

        #endregion

        public VersionedPackage SelectedVersionedPackage
        {
            get => _selectedVersionedPackage;
            set => SetProperty(ref _selectedVersionedPackage, value);
        }

        public AllPackagesViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PackagesReceivedEvent>().Subscribe(OnPackagesReceived);

            // commands
            this.RaiseUpdateSelectedToLatestVersion = new DelegateCommand(OnUpdateSelectedToLatestVersion);
            this.RaiseUpdateAllToLatestVersion = new DelegateCommand(OnUpdateAllToLatestVersion);
            this.RaiseSaveChanges = new DelegateCommand(OnSaveChanges);
        }

        private void OnPackagesReceived(IEnumerable<PackageItemDocument> packages)
        {
            AllPackages.AddRange(packages);

            foreach (var package in packages)
            {
                if (!_versionedPackages.ContainsKey(package.Name))
                {
                    _versionedPackages.Add(package.Name, new VersionedPackage(package));
                }
                else
                {
                    _versionedPackages[package.Name].AddVersion(package);
                }
            }

            VersionedPackages.Clear();
            VersionedPackages.AddRange(_versionedPackages.Values);
            //RaisePropertyChanged(nameof(VersionedPackages));
        }

        #region Execution

        private void OnUpdateSelectedToLatestVersion()
        {
            if (SelectedVersionedPackage == null) return;

            foreach(var package in SelectedVersionedPackage.Versions)
            {
                package.Version = SelectedVersionedPackage.LatestVersion;
            }
        }

        private void OnUpdateAllToLatestVersion()
        {
            foreach(var versionedPackage in VersionedPackages)
            {
                foreach (var package in versionedPackage.Versions)
                {
                    package.Version = SelectedVersionedPackage.LatestVersion;
                }
            }
        }

        private void OnSaveChanges()
        {

        }

        #endregion
    }
}
