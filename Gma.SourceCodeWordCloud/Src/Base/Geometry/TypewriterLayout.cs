using System;
using System.Collections.Generic;
using Gma.CodeCloud.Base.Portability;

namespace Gma.CodeCloud.Base.Geometry
{
    public class TypewriterLayout : BaseLayout
    {
        public TypewriterLayout(Size size) : base(size)
        {
            m_Carret = new Point(size.Width, 0);
        }

        private Point m_Carret;
        private double m_LineHeight;
 
        public override bool TryFindFreeRectangle(Size size, out Rectangle foundRectangle)
        {
            foundRectangle = new Rectangle(m_Carret, size);
            if (HorizontalOverflow(foundRectangle))
            {
                foundRectangle = LineFeed(foundRectangle);
                if (!IsInsideSurface(foundRectangle))
                {
                    return false;
                }
            }
            m_Carret = new Point(foundRectangle.Right, foundRectangle.Y);
            
            return true;
        }

        private Rectangle LineFeed(Rectangle rectangle)
        {
            Rectangle result = new Rectangle(new Point(0, m_Carret.Y + m_LineHeight), rectangle.Size);
            m_LineHeight = rectangle.Height;
            return result;
        }

        private bool HorizontalOverflow(Rectangle rectangle)
        {
            return rectangle.Right > Surface.Right;
        }
    }
}
