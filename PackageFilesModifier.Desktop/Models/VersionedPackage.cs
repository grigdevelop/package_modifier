using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageFilesModifier.Desktop.Models
{
    public class VersionedPackage : BindableBase
    {
        public PackageItemDocument Package { get; }

        public ObservableCollection<PackageItemDocument> Versions { get; }
            = new ObservableCollection<PackageItemDocument>();

        public string LatestVersion { get; private set; }

        public VersionedPackage(PackageItemDocument package)
        {
            this.Package = package;
            this.Versions.Add(package);
            this.LatestVersion = package.Version;
        }

        public void AddVersion(PackageItemDocument version)
        {
            if (!Versions.Contains(version))
            {
                Versions.Add(version);
                if(string.Compare(LatestVersion, version.Version) < 0)
                {
                    LatestVersion = version.Version;
                    RaisePropertyChanged(nameof(LatestVersion));
                }
            }
        }
    }
}
