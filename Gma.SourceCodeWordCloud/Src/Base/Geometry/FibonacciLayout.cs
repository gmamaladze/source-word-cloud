using System;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class FibonacciLayout : BaseLayout
    {
        public FibonacciLayout(SizeF size)
            : base(size)
        {
        }

        public override bool TryFindFreeRectangle(SizeF size, out RectangleF foundRectangle)
        {
            throw new NotImplementedException(); 
        }
    }
}