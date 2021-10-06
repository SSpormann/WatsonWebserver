using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WatsonWebserver
{
    /// <summary>
    /// Attribute route attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AttributeRouteAttribute : Attribute
    {
        #region Public-Members

        /// <summary>
        /// Globally-unique identifier.
        /// </summary>
        [JsonProperty(Order = -1)]
        public string GUID { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// HTTP method.
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.GET;

        /// <summary>
        /// Method name.
        /// </summary>
        public string Name { get; set; } = null;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public AttributeRouteAttribute()
        {
        }

        /// <summary>
        /// Instantiate.
        /// </summary>
        /// <param name="method">Method.</param>
        /// <param name="name">Method name.</param>
        public AttributeRouteAttribute(HttpMethod method, string name = null)
        {
            Method = method;
            Name = null;
        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}
