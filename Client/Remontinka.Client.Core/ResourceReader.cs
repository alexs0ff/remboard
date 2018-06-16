using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Общий считыватель встроенных Ресурсов.
    /// </summary>
    public class EmbeddedResourceReader
    {
        /// <summary>
        /// Содержит имя ресурса.
        /// </summary>
        private readonly string _resourceName;

        /// <summary>
        /// Содержит путь к ресурсу.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Имя сборки с внедренными ресурсами.
        /// </summary>
        private readonly Type _typeOfAssemblyWithResources;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceReader" /> class.
        /// </summary>
        /// <param name="resourceName">Имя внедренного русерса.</param>
        /// <param name="typeOfAssemblyWithResources">Тип в сборке содержащий ресурс.</param>
        /// <param name="pathItems">Пункты в пути к ресурсам.</param>
        public EmbeddedResourceReader(string resourceName, Type typeOfAssemblyWithResources, params string[] pathItems)
        {
            _resourceName = resourceName;
            var builder = new StringBuilder();

            foreach (var pathItem in pathItems)
            {
                builder.AppendFormat("{0}.", pathItem);
            } //foreach

            _path = builder.ToString().TrimEnd(new[] { '.' });
            _typeOfAssemblyWithResources = typeOfAssemblyWithResources;
        }

        /// <summary>
        ///   Reades specific resource.
        /// </summary>
        /// <returns> The resource stream. </returns>
        public Stream ReadStream()
        {
            string assemblyName = _typeOfAssemblyWithResources.Assembly.FullName;
            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(','));

            return _typeOfAssemblyWithResources.Assembly
                .GetManifestResourceStream(string.Format("{0}.{1}.{2}", assemblyName, _path, _resourceName));
        }

        /// <summary>
        ///   Reades specific resource.
        /// </summary>
        /// <returns> The resource string. </returns>
        public string Read()
        {
            var result = string.Empty;

            using (var stream = ReadStream())
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}
