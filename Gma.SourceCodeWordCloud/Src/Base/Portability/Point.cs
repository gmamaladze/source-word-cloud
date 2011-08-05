namespace Gma.CodeCloud.Base.Portability
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
    }
}
