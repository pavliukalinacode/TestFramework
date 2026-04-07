using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tests.Data.Attributes
{
    public static class BddFieldApplier
    {
        private static readonly Dictionary<Type, Dictionary<string, MethodInfo>> Cache = new();

        public static TBuilder Apply<TBuilder>(TBuilder builder, string field, string value)
        {
            ArgumentNullException.ThrowIfNull(builder);

            if (!GetMethodMap(typeof(TBuilder)).TryGetValue(field, out var method))
            {
                throw new NotSupportedException(
                    $"Field '{field}' is not supported by builder '{typeof(TBuilder).Name}'.");
            }

            method.Invoke(builder, [value]);

            return builder;
        }

        private static Dictionary<string, MethodInfo> GetMethodMap(Type builderType)
        {
            if (!Cache.TryGetValue(builderType, out var methodMap))
            {
                methodMap = builderType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(method => method.GetCustomAttribute<BddFieldAttribute>() != null)
                    .ToDictionary(
                        method => method.GetCustomAttribute<BddFieldAttribute>()!.Name,
                        method => method,
                        StringComparer.OrdinalIgnoreCase);

                Cache[builderType] = methodMap;
            }

            return methodMap;
        }
    }
}