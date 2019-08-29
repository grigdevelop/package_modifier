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
    public class ProjectFilesViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;


        public ObservableCollection<ProjectDocument> ProjectDocuments { get; }
            = new ObservableCollection<ProjectDocument>();

        public ProjectFilesViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FolderSelectedEvent>().Subscribe(OnFolderSelected);
        }

        private void OnFolderSelected(string solutionPath)
        {
            var projectFiles = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories);

            ProjectDocuments.Clear();

            foreach(var projectFile in projectFiles)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(projectFile);
                var projectDocument = new ProjectDocument(xmlDocument, projectFile);
                ProjectDocuments.Add(projectDocument);
            }

            RaisePropertyChanged(nameof(ProjectDocuments));
        }
    }
}
