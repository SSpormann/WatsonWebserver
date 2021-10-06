using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WatsonWebserver
{
    /// <summary>
    /// API controller method information.
    /// </summary>
    public class ApiControllerMethodInfo
    {
        #region Internal-Members

        /// <summary>
        /// Context.
        /// </summary>
        public FieldInfo ContextField
        {
            get
            {
                return _ContextField;
            }
        }

        /// <summary>
        /// Route prefix.
        /// </summary>
        public string RoutePrefix
        {
            get
            {
                return _RoutePrefix;
            }
        }

        /// <summary>
        /// Route name.
        /// </summary>
        public string RouteName
        {
            get
            {
                return _RouteName;
            }
        }

        /// <summary>
        /// Qualified route name, i.e. '[method] prefix/route'.
        /// </summary>
        public string QualifiedRouteName
        {
            get
            {
                string ret = _HttpMethod.ToString() + " " + _RoutePrefix.Trim('/') + "/" + _RouteName.Trim('/');
                return ret;
            }
        }

        /// <summary>
        /// Class type.
        /// </summary>
        public Type ClassType
        {
            get
            {
                return _ClassType;
            }
        }

        /// <summary>
        /// Method info.
        /// </summary>
        public MethodInfo MethodInfo
        {
            get
            {
                return _MethodInfo;
            }
        }

        /// <summary>
        /// HTTP method.
        /// </summary>
        public HttpMethod HttpMethod
        {
            get
            {
                return _HttpMethod;
            }
        }

        #endregion

        #region Private-Members

        private FieldInfo _ContextField = null;
        private string _RoutePrefix = null;
        private string _RouteName = null;
        private Type _ClassType = null;
        private MethodInfo _MethodInfo = null;
        private bool _IsStatic = false;
        private ApiControllerParameterInfo[] _Params;
        private HttpMethod _HttpMethod = HttpMethod.GET;

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate.
        /// </summary>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="routePrefix">Route prefix.</param>
        /// <param name="routeName">Route name.</param>
        /// <param name="classType">Class type.</param>
        /// <param name="method">Method.</param>
        public ApiControllerMethodInfo(HttpMethod httpMethod, string routePrefix, string routeName, Type classType, MethodInfo method)
        {
            if (String.IsNullOrEmpty(routePrefix)) throw new ArgumentNullException(nameof(routePrefix));
            if (String.IsNullOrEmpty(routeName)) throw new ArgumentNullException(nameof(routeName));
            if (classType == null) throw new ArgumentNullException(nameof(classType));
            if (method == null) throw new ArgumentNullException(nameof(method));

            _ContextField = classType.GetField("Context");
            _RoutePrefix = routePrefix;
            _RouteName = routeName;
            _ClassType = classType;
            _MethodInfo = method;
            _IsStatic = method.IsStatic;
            _HttpMethod = httpMethod;

            if (!_IsStatic)
            {
                if (_ContextField == null)
                {
                    // throw new InvalidOperationException("The API controller at prefix '" + routePrefix + "' and name '" + routeName + "' must either be static or implement ApiControllerBase.");
                }
            }

            var parameters = method.GetParameters();
            _Params = new ApiControllerParameterInfo[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var methodParam = parameters[i];

                _Params[i] = new ApiControllerParameterInfo()
                {
                    Name = methodParam.Name,
                    IsValueType = methodParam.ParameterType.IsValueType,
                    ParameterType = methodParam.ParameterType,
                    IsHttpContext = methodParam.ParameterType == typeof(HttpContext)
                };
            }
        }

        #endregion

        #region Internal-Methods

        internal async Task Invoke(HttpContext ctx, CancellationToken token = default)
        {
            object instance; // Static or instantiated API controller class

            if (_IsStatic)
            {
                instance = _ClassType;
            }
            else
            {
                ConstructorInfo constructor = _ClassType.GetConstructor(Type.EmptyTypes);
                instance = constructor.Invoke(new object[] { });
                this._ContextField.SetValue(instance, ctx);
            }

            var invokeParameters = new object[this._Params.Length];

            var queryParameters = ctx.Request.Query.Elements;
            for (int i = 0; i < invokeParameters.Length; i++)
            {
                var methodParam = this._Params[i];
                if (methodParam.IsHttpContext)
                {
                    // Pass HttpContext as parameter
                    invokeParameters[i] = ctx;
                }
                else
                {
                    if (queryParameters.TryGetValue(methodParam.Name, out var p))
                    {
                        // Query parameter found, try to instantiate
                        invokeParameters[i] = JsonConvert.DeserializeObject(p, methodParam.ParameterType);
                    }
                    else
                    {
                        if (_HttpMethod == HttpMethod.GET)
                        {
                            // Missing parameter, default values should be passed
                            invokeParameters[i] = methodParam.GetDefaultValue();
                        }
                        else if (_HttpMethod == HttpMethod.POST)
                        {
                            // Instantiate parameter by post data
                            invokeParameters[i] = JsonConvert.DeserializeObject(ctx.Request.DataAsString, methodParam.ParameterType);
                        }
                    }
                }
            }

            // Invoke controller method
            Task task = (Task)this._MethodInfo.Invoke(instance, invokeParameters);
            await task.ConfigureAwait(false);

            // Get result
            var resultProperty = task.GetType().GetProperty("Result");
            object result = resultProperty.GetValue(task);

            // Serialize to JSON and send 
            await ctx.Response.Send(JsonConvert.SerializeObject(result), token).ConfigureAwait(false);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
