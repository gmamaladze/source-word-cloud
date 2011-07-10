﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
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
                    return new TypewriterLayout(size);

                case LayoutType.Spiral:
                    return new SpiralLayout(size);
            
                default:
                    throw new ArgumentException(string.Format("No constructor specified to create a layout instance for {0}.", layoutType), "layoutType");
            }
        }
    }
}