using System;
using Gma.CodeCloud.Base.Portability;

namespace Gma.CodeCloud.Base.Geometry
{
    public class FibonacciLayout : BaseLayout
    {
        public FibonacciLayout(Size size)
            : base(size)
        {
        }

        public override bool TryFindFreeRectangle(Size size, out Rectangle foundRectangle)
        {
            throw new NotImplementedException(); 
        }
    }
}