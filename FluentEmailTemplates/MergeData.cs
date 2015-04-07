using System.Collections.Generic;

namespace VisualProduct.FluentEmailTemplates
{
    /// <summary>
    /// The merge data. Really a wrapper for dictionary of key value pairs.
    /// </summary>
    public class MergeData
    {
        private readonly IDictionary<string, string> _dictionary = new Dictionary<string, string>();

        /// <summary>
        /// Add a simple key value pair.
        /// </summary>
        /// <param name="key">The key of the merge field. e.g. "FirstName".</param>
        /// <param name="value">The value for the merge field. e.g. "Donald".</param>
        public MergeData Add(string key, string value)
        {
            _dictionary.Add(key, value);
            return this;
        }

        public string GetValue(string key)
        {
            string value;
            _dictionary.TryGetValue(key, out value);
            return value;
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }
    }
}