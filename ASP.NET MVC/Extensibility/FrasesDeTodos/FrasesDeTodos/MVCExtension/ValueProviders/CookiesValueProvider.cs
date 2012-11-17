using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Specialized;

namespace FrasesDeTodos.MVCExtension.ValueProviders
{
    public class CookieValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new CookieValueProvider(controllerContext.HttpContext.Request.Cookies);
        }

        private class CookieValueProvider : IValueProvider
        {
            private Dictionary<string, string> _cookiesKeys = new Dictionary<string, string>();

            public CookieValueProvider(HttpCookieCollection cookieCollection)
            {
                foreach (string cookieName in cookieCollection)
                {
                    var cookie = cookieCollection.Get(cookieName);

                    foreach (var key in cookie.Values.AllKeys)
                    {
                        if (key != null)
                            _cookiesKeys.Add(key, cookie.Values.Get(key));
                    }
                }
            }

            public bool ContainsPrefix(string prefix)
            {
                var containsPrefix = _cookiesKeys.Keys.Any(x => x == prefix);

                return containsPrefix;
            }

            public ValueProviderResult GetValue(string key)
            {
                var value = _cookiesKeys[key];

                return value != null ?
                    new ValueProviderResult(value, value, CultureInfo.CurrentUICulture) : null;
            }
        }
    }
}