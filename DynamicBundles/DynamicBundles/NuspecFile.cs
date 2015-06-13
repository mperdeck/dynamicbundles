using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DynamicBundles
{
    public class NuspecFile
    {
#region classes

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class package
        {

            private packageMetadata metadataField;

            private packageFile[] filesField;

            /// <remarks/>
            public packageMetadata metadata
            {
                get
                {
                    return this.metadataField;
                }
                set
                {
                    this.metadataField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("file", IsNullable = false)]
            public packageFile[] files
            {
                get
                {
                    return this.filesField;
                }
                set
                {
                    this.filesField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class packageMetadata
        {

            private string idField;

            private string versionField;

            private string titleField;

            private string authorsField;

            private string ownersField;

            private string projectUrlField;

            private string iconUrlField;

            private bool requireLicenseAcceptanceField;

            private string descriptionField;

            private string copyrightField;

            private string tagsField;

            private packageMetadataDependency[] dependenciesField;

            /// <remarks/>
            public string id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }

            /// <remarks/>
            public string title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string authors
            {
                get
                {
                    return this.authorsField;
                }
                set
                {
                    this.authorsField = value;
                }
            }

            /// <remarks/>
            public string owners
            {
                get
                {
                    return this.ownersField;
                }
                set
                {
                    this.ownersField = value;
                }
            }

            /// <remarks/>
            public string projectUrl
            {
                get
                {
                    return this.projectUrlField;
                }
                set
                {
                    this.projectUrlField = value;
                }
            }

            /// <remarks/>
            public string iconUrl
            {
                get
                {
                    return this.iconUrlField;
                }
                set
                {
                    this.iconUrlField = value;
                }
            }

            /// <remarks/>
            public bool requireLicenseAcceptance
            {
                get
                {
                    return this.requireLicenseAcceptanceField;
                }
                set
                {
                    this.requireLicenseAcceptanceField = value;
                }
            }

            /// <remarks/>
            public string description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string copyright
            {
                get
                {
                    return this.copyrightField;
                }
                set
                {
                    this.copyrightField = value;
                }
            }

            /// <remarks/>
            public string tags
            {
                get
                {
                    return this.tagsField;
                }
                set
                {
                    this.tagsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("dependency", IsNullable = false)]
            public packageMetadataDependency[] dependencies
            {
                get
                {
                    return this.dependenciesField;
                }
                set
                {
                    this.dependenciesField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class packageMetadataDependency
        {

            private string idField;

            private string versionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class packageFile
        {

            private string srcField;

            private string targetField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string src
            {
                get
                {
                    return this.srcField;
                }
                set
                {
                    this.srcField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string target
            {
                get
                {
                    return this.targetField;
                }
                set
                {
                    this.targetField = value;
                }
            }
        }
#endregion

        private package _package;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="absolutePath">
        /// Absolute path of the Nuspec file.
        /// </param>
        public NuspecFile(string absolutePath)
        {
            using (TextReader reader = new StreamReader(absolutePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(package));
                _package = (package)serializer.Deserialize(reader);
            }
        }

        public static bool IsNuspecFile(string absolutePath)
        {
            string extension = Path.GetExtension(absolutePath);
            return (String.CompareOrdinal(extension, ".nuspec") == 0);
        }

        /// <summary>
        /// Returns the ids of the dependencies in the nuspec file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> DependencyIds
        {
            get
            {
                if ((_package.metadata == null) || (_package.metadata.dependencies == null)) { return new List<string>(); }

                return _package.metadata.dependencies.Select(d => d.id);
            }
        }
    }
}

