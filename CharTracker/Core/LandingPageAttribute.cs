using System;

namespace RetiraTracker.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class LandingPageAttribute : Attribute
    {
        public string Page { get; private set; }
        public string Display { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsEnabled { get; private set; }

        public LandingPageAttribute(string pageName, string display, bool isSelected = false, bool isEnabled = true)
        {
            Page = pageName;
            Display = display;
            IsSelected = isSelected;
            IsEnabled = isEnabled;
        }

        public LandingPageAttribute(string pageName, bool isEnabled = true) : this(pageName, string.Empty, isEnabled) { }
    }
}
