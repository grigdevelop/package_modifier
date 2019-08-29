using PackageFilesModifier.Desktop.Models;
using Prism.Events;
using System.Collections.Generic;

namespace PackageFilesModifier.Desktop.Events
{
    public class PackagesReceivedEvent : PubSubEvent<IEnumerable<PackageItemDocument>>
    {
    }
}
