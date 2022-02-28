using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleExplorer.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MimeTypeAttribute : Attribute
    {
        public string[] MimeTypes { get; set; }

        public MimeTypeAttribute(params string[] typeNames)
        {
            MimeTypes = typeNames;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DisplayAttribute:Attribute
    {
        public string Display { get; set; }

        public DisplayAttribute(string display)
        {
            Display = display;
        }
    }

    public static class MimeExtensions
    {
        public static string GetMimeType(this MimeTypes mime)
        {
            MimeTypeAttribute mimeAttribute = (MimeTypeAttribute)mime.GetType()
                .GetMember(mime.ToString())
                .First()
                .GetCustomAttributes(typeof(MimeTypeAttribute), false)
                .First();
            return mimeAttribute.MimeTypes.First();
        }

        public static string[] GetMimeTypes(this MimeTypes mime)
        {
            MimeTypeAttribute mimeAttribute = (MimeTypeAttribute)mime.GetType()
                .GetMember(mime.ToString())
                .First()
                .GetCustomAttributes(typeof(MimeTypeAttribute), false)
                .First();
            return mimeAttribute.MimeTypes;
        }

        public static string[] GetMimeTypes(this MimeTypes[] mime)
        {
            List<string> mimeTypes = new List<string>();
            foreach(MimeTypes e in mime)
                mimeTypes.AddRange(e.GetMimeTypes());

            return mimeTypes.ToArray();
        }

        public static MimeTypes GetMimeTypes(this string mime)
        {
            foreach (MimeTypes m in Enum.GetValues(typeof(MimeTypes)))
                foreach (string mi in m.GetMimeTypes())
                    if (mi == mime)
                        return m;

            return MimeTypes.Null;
        }
    }

    public static class PermissionExtension
    {
        public static string Display(this Enum @enum)
        {
            DisplayAttribute displayAttribute = (DisplayAttribute)@enum.GetType()
                .GetMember(@enum.ToString())
                .First()
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .First();
            return displayAttribute.Display;
        }

    }
}
