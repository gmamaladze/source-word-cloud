using System.Globalization;

namespace Gma.CodeCloud.Base.Geometry.Portability
{
    public struct Size
    {
        public Size(double width, double height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[Width={0}, Height={1}]", Width, Height);
        }

        public override int GetHashCode()
        {
            return 10 + Width.GetHashCode() >> 3 + Height.GetHashCode() << 4;
        }
    }
}
