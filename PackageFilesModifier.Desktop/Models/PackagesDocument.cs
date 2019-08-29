using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageFilesModifier.Desktop.Models
{
    public class PackagesDocument : BindableBase
    {
        private readonly XmlDocument _root;
        private readonly string _originalPath;

        public string OriginalPath => _originalPath;

        public List<PackageItemDocument> PackagesList { get; }
            = new List<PackageItemDocument>();

        public ProjectDocument Project { get; private set; }

        public PackagesDocument(XmlDocument root, string originalPath)
        {
            this._root = root;
            this._originalPath = originalPath;

            // first load project items, because package file should
            // have access to project one
            LoadProjectFile();

            LoadPackages();
            
        }

        private void LoadPackages()
        {
            PackagesList.Clear();

            foreach (XmlNode packageNode in _root.GetElementsByTagName("package"))
            {                               
                var item = new PackageItemDocument(packageNode);

                var projectPackageItem = Project.Packages.FirstOrDefault(x => x.Name == item.Name);
                if (projectPackageItem != null) item.AttachProjectPackage(projectPackageItem);

                if (item.HasVersion)
                {
                    PackagesList.Add(item);
                }

            }

            RaisePropertyChanged(nameof(PackagesList));
        }

        private void LoadProjectFile()
        {
            FileInfo fi = new FileInfo(_originalPath);
            var projFiles = fi.Directory.GetFiles("*.csproj");

            if (projFiles.Length == 0) return;
            if (projFiles.Length > 1) throw new Exception("More then one project file found in one project");
            var projFile = projFiles[0];
            var projXmlDoc = new XmlDocument();
            projXmlDoc.Load(projFile.FullName);
            this.Project = new ProjectDocument(projXmlDoc);
        }
    }
}
