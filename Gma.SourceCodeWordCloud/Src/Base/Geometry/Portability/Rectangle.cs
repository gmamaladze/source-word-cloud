#region

using System;
using System.Globalization;

#endregion

namespace Gma.CodeCloud.Base.Geometry.Portability
{
    public struct Rectangle
    {
        public static readonly Rectangle Empty;

        static Rectangle()
        {
            Empty = new Rectangle();
        }

        public Rectangle(double x, double y, double width, double height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point location, Size size)
            : this()
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public double Left
        {
            get { return X; }
        }

        public double Top
        {
            get { return Y; }
        }

        public double Right
        {
            get { return (X + Width); }
        }

        public double Bottom
        {
            get { return (Y + Height); }
        }

        public bool IsEmpty
        {
            get { return Width <= 0f || Height <= 0f; }
        }

        public override bool Equals(object another)
        {
            if (!(another is Rectangle))
            {
                return false;
            }
            return this==(Rectangle) another;
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return ((((left.X == right.X) && (left.Y == right.Y)) && (left.Width == right.Width)) && (left.Height == right.Height));
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }

        public void Offset(Point pos)
        {
            Offset(pos.X, pos.Y);
        }

        public void Offset(double x, double y)
        {
            X += x;
            Y += y;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
        }

        public bool Contains(double x, double y)
        {
            return ((((X <= x) && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(Rectangle rectangle)
        {
            return ((((X <= rectangle.X) && ((rectangle.X + rectangle.Width) <= (X + Width))) && (Y <= rectangle.Y)) && ((rectangle.Y + rectangle.Height) <= (Y + Height)));
        }

        public override int GetHashCode()
        {
            return X.GetHashCode()>>13 + Y.GetHashCode()>>10 + Width.GetHashCode()>>3 + Height.GetHashCode()>>4;
        }

        public void Inflate(double x, double y)
        {
            X -= x;
            Y -= y;
            Width += 2f*x;
            Height += 2f*y;
        }

        public void Inflate(Size size)
        {
            Inflate(size.Width, size.Height);
        }

        public static Rectangle Inflate(Rectangle rect, double x, double y)
        {
            Rectangle ef = rect;
            ef.Inflate(x, y);
            return ef;
        }

        public void Intersect(Rectangle rect)
        {
            Rectangle ef = Intersect(rect, this);
            X = ef.X;
            Y = ef.Y;
            Width = ef.Width;
            Height = ef.Height;
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            double x = Math.Max(a.X, b.X);
            double num2 = Math.Min((a.X + a.Width), (b.X + b.Width));
            double y = Math.Max(a.Y, b.Y);
            double num4 = Math.Min((a.Y + a.Height), (b.Y + b.Height));
            if ((num2 >= x) && (num4 >= y))
            {
                return new Rectangle(x, y, num2 - x, num4 - y);
            }
            return Empty;
        }

        public bool IntersectsWith(Rectangle rect)
        {
            return ((((rect.X < (X + Width)) && (X < (rect.X + rect.Width))) && (rect.Y < (Y + Height))) && (Y < (rect.Y + rect.Height)));
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            double x = Math.Min(a.X, b.X);
            double num2 = Math.Max((a.X + a.Width), (b.X + b.Width));
            double y = Math.Min(a.Y, b.Y);
            double num4 = Math.Max((a.Y + a.Height), (b.Y + b.Height));
            return new Rectangle(x, y, num2 - x, num4 - y);
        }
    }
}