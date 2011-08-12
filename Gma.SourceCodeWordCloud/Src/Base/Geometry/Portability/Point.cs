using System.Globalization;

namespace Gma.CodeCloud.Base.Geometry.Portability
{
    public struct Point
    {
        public Point(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }

        public double Y { get; set; }
        public double X { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[X={0}, Y={1}]", X, Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() << 13 + Y.GetHashCode() >> 10;
        }
    }
}
