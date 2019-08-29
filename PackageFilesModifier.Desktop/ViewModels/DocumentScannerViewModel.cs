using PackageFilesModifier.Desktop.Events;
using PackageFilesModifier.Desktop.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageFilesModifier.Desktop.ViewModels
{
    public class DocumentScannerViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<PackagesDocument> PackagesDocuments { get; }
         = new ObservableCollection<PackagesDocument>();

        private PackagesDocument _selectedPackages;
        public PackagesDocument SelectedPackages
        {
            get => _selectedPackages;
            set => SetProperty(ref _selectedPackages, value);
        }

        public DocumentScannerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(OnFolderSelected);
        }

        private void OnFolderSelected(string selectedPath)
        {
            // get all package file paths
            var packageFiles = Directory.GetFiles(selectedPath, "packages.config", SearchOption.AllDirectories);

            PackagesDocuments.Clear();

            // convert all to xml documents
            foreach (var packageFilePath in packageFiles)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(packageFilePath);
                var packagesDocument = new PackagesDocument(xmlDocument, packageFilePath);
                PackagesDocuments.Add(packagesDocument);

                _eventAggregator.GetEvent<PackagesReceivedEvent>().Publish(packagesDocument.PackagesList);
            }
           
            RaisePropertyChanged(nameof(PackagesDocuments));            
        }
    }
}
