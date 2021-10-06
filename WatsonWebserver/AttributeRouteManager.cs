using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WatsonWebserver
{
    /// <summary>
    /// Attribute route manager.  Attribute routes are used to invoke class or static methods for requests using specific HTTP methods to a matching path.
    /// </summary>
    public class AttributeRouteManager
    {
        #region Internal-Members

        #endregion

        #region Private-Members

        private readonly object _Lock = new object();
        private Dictionary<string, ApiControllerMethodInfo> _Routes = new Dictionary<string, ApiControllerMethodInfo>();

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        public AttributeRouteManager()
        {

        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Add a route.
        /// </summary>
        /// <param name="routePrefix">Route prefix.</param>
        /// <param name="routeName">Method name.</param>
        /// <param name="classType">Class type.</param>
        /// <param name="classMethod">Class method.</param>
        /// <param name="httpMethod">HTTP method.</param>
        public void Add(string routePrefix, string routeName, Type classType, MethodInfo classMethod, HttpMethod httpMethod)
        {
            ApiControllerMethodInfo api = new ApiControllerMethodInfo(httpMethod, routePrefix, routeName, classType, classMethod);
            Add(api);
        }

        /// <summary>
        /// Add a route.
        /// </summary>
        /// <param name="api">API controller method information.</param>
        public void Add(ApiControllerMethodInfo api)
        {
            if (api == null) throw new ArgumentNullException(nameof(api));

            lock (_Lock)
            {
                _Routes.Add(api.QualifiedRouteName, api);
            }
        }

        /// <summary>
        /// Match a request to a handler.
        /// </summary>
        /// <param name="req">HTTP request.</param>
        /// <returns>API controller method info.</returns>
        public ApiControllerMethodInfo Match(HttpRequest req)
        {
            ApiControllerMethodInfo invokeInformation = null;
            string key = req.Method.ToString() + " " + req.Url.RawWithoutQuery.Trim('/');
            Console.WriteLine("Matching route " + key);

            lock (_Lock)
            {
                if (_Routes.TryGetValue(key, out invokeInformation))
                {
                    return invokeInformation;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
