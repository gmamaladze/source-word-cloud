using System.Drawing;
using PortableRectangle=Gma.CodeCloud.Base.Portability.Rectangle;
using PortableSize = Gma.CodeCloud.Base.Portability.Size;

namespace Gma.CodeCloud.Controls
{
    internal static class PortabilityExtensions
    {
        public static PortableSize ToPortable(this Size size)
        {
            return new PortableSize(size.Width, size.Height);
        }

        public static PortableSize ToPortable(this SizeF size)
        {
            return new PortableSize(size.Width, size.Height);
        }

        public static PortableRectangle ToPortable(this RectangleF rectangle)
        {
            return new PortableRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static PortableRectangle ToPortable(this Rectangle rectangle)
        {
            return new PortableRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle ToDrawing(this PortableRectangle rectangle)
        {
            return new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }
    }
}