using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PromoWeb.Common
{
    public static class AssemblyExtensions //рефлексии на версию и на атрибуты
    {
        public static string GetAssemblyDescription(this Assembly assembly)
        {
            return assembly.GetAssemblyAttribute<AssemblyDescriptionAttribute>()?.Description;
        }

        public static string GetAssemblyVersion(this Assembly assembly)
        {
            return assembly.GetName().Version?.ToString();
        }

        public static T GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute
        {
            var attributes = assembly.GetCustomAttributes(typeof(T), true);

            if (attributes.Length == 0)
                return null;

            return (T)attributes[0];
        }
    }
}
