using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageFilesModifier.Desktop.Models
{
    public class ProjectDocument
    {
        private readonly XmlDocument _root;

        public List<ProjectDocumentPackage> Packages { get; }
            = new List<ProjectDocumentPackage>();

        public ProjectDocument(XmlDocument root)
        {
            this._root = root;
            LoadProjectDocumentPackages();
        }

        private void LoadProjectDocumentPackages()
        {
            foreach(XmlNode refer in _root.GetElementsByTagName("Reference"))
            {
                var item = new ProjectDocumentPackage(refer);
                if (item.HasVersion)
                {
                    Packages.Add(item);
                }
            }
        }
    }
}
