using System.Linq;

namespace SeekYouRS.Storing
{
    /// <summary>
    /// Defined the behaviour of a ModelStore. 
    /// This store saves any kind of objects. 
    /// The implementation should use to store the Models eg.ViewModels.
    /// </summary>
    public interface IStoreModels
    {
        /// <summary>
        /// Gets a single instance of model by query parameters.
        /// </summary>
        /// <typeparam name="T">Type of Model</typeparam>
        /// <param name="query">Query parameters</param>
        /// <returns>Single instance</returns>
        T Retrieve<T>(dynamic query);
        /// <summary>
        /// Adds a single Model to the store. 
        /// </summary>
        /// <param name="model">The Model without any special type</param>
        void Add(dynamic model);
        /// <summary>
        /// Changes a Model from store
        /// </summary>
        /// <param name="model">The Model to change. Use your own identifier to localize the Model</param>
        void Change(dynamic model);
        /// <summary>
        /// Removes a Model form store.
        /// </summary>
        /// <param name="model">The Model to remove. Use your own identifier to localize the Models</param>
        void Remove(dynamic model);
    }
}