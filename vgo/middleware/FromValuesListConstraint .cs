using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.middleware
{
    public class FromValuesListConstraint : IRouteConstraint
    {
        private string[] _values;

        public FromValuesListConstraint(params string[] values)
        {
            this._values = values;
        }
        
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[routeKey].ToString();

            // Return true is the list of allowed values contains
            // this value.

            for (int i = 0; i < _values.Length; i++)
                if (SContains(_values[i], value, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        public bool SContains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
