using System;
using System.Linq;

namespace GoogleExplorer.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MimeTypeAttribute: Attribute
    {
        public string[] MimeTypes { get; set; }

        public MimeTypeAttribute(params string[] typeNames) 
        {
            MimeTypes = typeNames;
        }
    }

    public static class MimeExtensions
    {
        public static string GetMimeType(this Enum mime)
        {
            MimeTypeAttribute mimeAttribute = (MimeTypeAttribute)mime.GetType()
                .GetMember(mime.ToString())
                .First()
                .GetCustomAttributes(typeof(MimeTypeAttribute), false)
                .First();
            return mimeAttribute.MimeTypes.First();
        }

        public static string[] GetMimeTypes(this Enum mime)
        {
            MimeTypeAttribute mimeAttribute = (MimeTypeAttribute)mime.GetType()
                .GetMember(mime.ToString())
                .First()
                .GetCustomAttributes(typeof(MimeTypeAttribute), false)
                .First();
            return mimeAttribute.MimeTypes;
        }

        public static MimeTypes GetMimeTypes(this string mime)
        {
            foreach(MimeTypes m in Enum.GetValues(typeof(MimeTypes)))
                foreach (string mi in m.GetMimeTypes())
                    if (mi == mime)
                        return m;

            return MimeTypes.Null;
        }
    }
}
