using System;
using System.Drawing;
using Gma.CodeCloud.Base.Geometry;

namespace Gma.CodeCloud.Controls
{
    public static class LayoutFactory
    {
        public static ILayout CrateLayout(LayoutType layoutType, SizeF size)
        {
            switch (layoutType)
            {
                case LayoutType.Typewriter:
                    return new TypewriterLayout(size.ToPortable());

                case LayoutType.Spiral:
                    return new SpiralLayout(size.ToPortable());
            
                default:
                    throw new ArgumentException(string.Format("No constructor specified to create a layout instance for {0}.", layoutType), "layoutType");
            }
        }
    }
}
