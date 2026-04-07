using System;
using System.Linq;
using System.Reflection;

namespace Tests.Data.Attributes
{
    /// <summary>
    /// Applies BDD table field values to builder methods marked with BddFieldAttribute.
    /// </summary>
    public static class BddFieldApplier
    {
        public static TBuilder Apply<TBuilder>(TBuilder builder, string field, string value)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var method = typeof(TBuilder)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(method =>
                {
                    var attribute = method.GetCustomAttribute<BddFieldAttribute>();

                    return attribute != null &&
                           string.Equals(attribute.Name, field, StringComparison.OrdinalIgnoreCase);
                });

            if (method == null)
            {
                throw new NotSupportedException(
                    $"Field '{field}' is not supported by builder '{typeof(TBuilder).Name}'.");
            }

            method.Invoke(builder, [value]);

            return builder;
        }
    }
}