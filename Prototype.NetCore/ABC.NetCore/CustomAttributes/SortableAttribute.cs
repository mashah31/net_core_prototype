using System;

namespace ABC.NetCore.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SortableAttribute : Attribute
    {
        public bool Default { get; set; }

        public string EntityProperty { get; set; }
    }
}
