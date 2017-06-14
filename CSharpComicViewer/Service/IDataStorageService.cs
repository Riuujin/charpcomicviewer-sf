namespace CSharpComicViewer.Service
{
    public interface IDataStorageService
    {
        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>An instance of <c>T</c>.</returns>
        T Load<T>(string key);

        /// <summary>
        /// Saves the specified data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        void Save(string key, object data);
    }
}