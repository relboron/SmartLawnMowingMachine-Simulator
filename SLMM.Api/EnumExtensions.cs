using System;
using System.Linq;
using System.Reflection;

namespace SLMM.Api
{
    public static class Extensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            return value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }
}
