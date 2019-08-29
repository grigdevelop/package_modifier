using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageFilesModifier.Desktop.Models
{
    public class PackageItemDocument : BindableBase, IEquatable<PackageItemDocument>
    {
        private readonly XmlNode _root;
     
        public string Name
        {
            get
            {
                return _root.Attributes["id"].Value;
            }
        }

        public string Version
        {
            get
            {
                return _root.Attributes["version"].Value;
            }

            set
            {
                _root.Attributes["version"].Value = value;              
                RaisePropertyChanged();
            }
        }

        public PackageItemDocument(XmlNode root)
        {
            this._root = root;
        }      

        public bool HasVersion => _root.Attributes["version"] != null;

        public bool Equals(PackageItemDocument other)
        {
            return Version == other.Version;
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }
    }
}
