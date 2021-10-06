using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonWebserver
{
    internal class ApiControllerParameterInfo
    {
        #region Internal-Members

        internal string Name;
        internal Type ParameterType;
        internal bool IsValueType;
        internal bool IsHttpContext;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        #endregion

        #region Public-Methods

        /// <summary>
        /// Get default value.
        /// </summary>
        /// <returns>Object.</returns>
        public object GetDefaultValue()
        {
            return IsValueType ? Activator.CreateInstance(ParameterType) : null;
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
