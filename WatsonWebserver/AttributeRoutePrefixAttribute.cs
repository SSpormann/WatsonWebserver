using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonWebserver
{
    /// <summary>
    /// Annotates a controller with a route prefix that applies to all actions within the controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AttributeRoutePrefixAttribute : Attribute
    {
        #region Public-Members

        /// <summary>
        /// Route prefix.
        /// </summary>
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                _Prefix = value;
            }
        }

        #endregion

        #region Private-Members

        private string _Prefix = null;

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public AttributeRoutePrefixAttribute()
        {

        }

        /// <summary>
        /// Instantiate.
        /// </summary>
        /// <param name="prefix">Route prefix.</param>
        public AttributeRoutePrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}
