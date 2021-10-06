using System.Windows.Media;

namespace RetiraTracker.Core
{
    public struct EnumeratedBrush
    {
        public int Enumerator { get; set; }
        public char Type { get; set; }
        public Brush Brush { get; set; }

        public EnumeratedBrush(int enumerator, char type, Brush brush)
        {
            Enumerator = enumerator;
            Type = type;
            Brush = brush;
        }
    }
}
