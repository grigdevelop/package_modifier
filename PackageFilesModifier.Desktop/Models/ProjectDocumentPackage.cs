using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageFilesModifier.Desktop.Models
{
    public class ProjectDocumentPackage
    {
        private readonly XmlNode _root;

        public ProjectDocumentPackage(XmlNode root)
        {
            this._root = root;
        }

        public bool HasVersion
        {
            get
            {
                var include = _root.Attributes["Include"];
                if (include == null) return false;

                var includeParamsList = include.Value.Split(',').Skip(1).ToArray();
                if (includeParamsList.Length == 0) return false;

                return includeParamsList.Any(x => x.Contains("Version="));
            }
        }

        public string Name
        {
            get
            {
                return _root.Attributes["Include"].Value.Split(',')
                    .First();
            }
        }

        public string Version
        {
            get
            {
                return _root.Attributes["Include"]
                    .Value.Split(',').Skip(1)
                    .ToArray().Select(x => x.Split('='))
                    .ToArray().First(x => x[0].Contains("Version"))[1];
            }

            set
            {
                // replace first inner path to dll file
                //foreach (XmlNode chn in _root.ChildNodes[0])
                //{
                //    chn.Value = chn.Value.Replace($"{Name}.{Version}", $"{Name}.{value}");
                //}

                var attr = _root.Attributes["Include"];
                var val = attr.Value;
                val.Replace("Version=" + Version, "Version=" + value);
                attr.Value = val;
            }
        }

        public string Reference
        {
            get
            {
                string reference = null;

                if (_root.ChildNodes.Count == 0) throw new Exception($"No reference child node for '{Name}'");
                if (_root.ChildNodes.Count > 1) throw new Exception($"Reference child nodes for '{Name}' more then one (??!!)");

                foreach (XmlNode bodyNode in _root.ChildNodes)
                {
                    reference = bodyNode.Value;
                }


                return reference;
            }
        }
    }
}
