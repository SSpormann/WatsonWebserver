using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonWebserver
{
    /// <summary>
    /// Defines properties and methods for API controller.
    /// </summary>
    public class ApiControllerBase
    {
        #region Public-Members

        /// <summary>
        /// HTTP context.
        /// </summary>
        public HttpContext Context { get; set; } = null;

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public ApiControllerBase()
        {

        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        #endregion
    }
}
