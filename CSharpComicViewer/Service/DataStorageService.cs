using CSharpComicViewerLib.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComicViewer.Service
{
    /// <summary>
    /// Service that provides saving and loading of data.
    /// </summary>
    /// <seealso cref="CSharpComicViewerLib.Service.IDataStorageService" />
    public class DataStorageService : IDataStorageService
    {
        #if DEBUG
            private static readonly string localDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "C# Comicviewer","Debug");
        #else
            private static readonly string localDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "C# Comicviewer");
        #endif

        /// <summary>
        /// Saves the specified data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public void Save(string key, object data)
        {
            if (!Directory.Exists(localDataPath))
            {
                Directory.CreateDirectory(localDataPath);
            }

            var path = Path.Combine(localDataPath, key + ".json");

            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// An instance of <c>T</c>.
        /// </returns>
        public T Load<T>(string key)
        {
            var path = Path.Combine(localDataPath, key + ".json");

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }
    }
}