using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Remontinka.Client.Core.Interception
{
    public static class UnityHelper
    {
        public static bool HasAttribute<T>(this PropertyInfo property)
        {
            return HasAttribute(property, typeof(T));
        }

        public static bool HasAttribute(this PropertyInfo property, Type attributeType)
        {
            return property.GetCustomAttributes(attributeType, true).Length > 0;
        }
    }
}
