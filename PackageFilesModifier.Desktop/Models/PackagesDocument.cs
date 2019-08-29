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
            //LoadProjectFile();

            LoadPackages();
            
        }

        private void LoadPackages()
        {
            PackagesList.Clear();

            foreach (XmlNode packageNode in _root.GetElementsByTagName("package"))
            {                               
                var item = new PackageItemDocument(packageNode);
               
                if (item.HasVersion)
                {
                    PackagesList.Add(item);
                }

            }

            RaisePropertyChanged(nameof(PackagesList));
        }       
    }
}
