using System.Collections.Generic;
using System.Collections.ObjectModel;
using MediaColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;

namespace RetiraTracker.Model
{
    public static class Extension
    {
        public static List<T> RemoveDuplicates<T>(this IEnumerable<T> list)
        {
            List<T> output = new();

            foreach (T t in list)
                if (!output.Contains(t))
                    output.Add(t);

            return output;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            ObservableCollection<T> output = new();

            foreach (T t in list)
                output.Add(t);

            return output;
        }

        public static MediaColor ToMediaColor(this Color color)
        {
            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}
