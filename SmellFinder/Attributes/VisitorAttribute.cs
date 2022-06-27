using System;

namespace SmellFinder.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VisitorAttribute : Attribute
    {
        public string Name;
        public string Description;

        public VisitorAttribute(string name)
        {
            Name = name;
        }
    }
}
