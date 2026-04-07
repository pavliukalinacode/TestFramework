using System;

namespace Tests.Data.Attributes
{
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