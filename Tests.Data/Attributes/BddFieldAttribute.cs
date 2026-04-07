using System;

namespace Tests.Data.Attributes
{
    /// <summary>
    /// Marks a builder method as supporting a specific BDD table field name.
    /// Used by BddFieldApplier to map scenario table fields to builder methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BddFieldAttribute : Attribute
    {
        public string Name { get; }

        public BddFieldAttribute(string name)
        {
            Name = name;
        }
    }
}