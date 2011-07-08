using System;
using System.Collections.Generic;
using System.Drawing;
using Gma.CodeCloud.Base.DataStructures;

namespace Gma.CodeCloud.Base.Geometry
{
    public class RandomCentricLayout : ILayout
    {
        private readonly Random m_Rnd = new Random();
        private readonly QuadTree<WordRectangle> m_QuadTree;
        private readonly PointF m_Center;
        private RectangleF m_Surface;

        public RandomCentricLayout(SizeF size)
        {
            m_Surface = new RectangleF(new PointF(0, 0), size);
            m_QuadTree = new QuadTree<WordRectangle>(m_Surface);
            m_Center = new PointF(size.Width / 2, size.Height / 2);
        }

        public virtual RectangleF Add(SizeF size)
        {
            double r = 0;// Math.Sqrt(m_Center.X * m_Center.X + m_Center.Y * m_Center.Y) * m_Rnd.NextDouble() * 0.25;
            double alpha = m_Rnd.NextDouble() * Math.PI * 2;
            const double stepAlpha = Math.PI / 120;
            const double stepR = 1;
            for(int retryCount = 0; retryCount<5000; retryCount++) 
            {
                double dX = r*Math.Sin(alpha);
                double dY = r*Math.Cos(alpha);
                RectangleF targetRectangle = new RectangleF((float)(m_Center.X + dX)-size.Width / 2, (float)(m_Center.Y + dY)-size.Height/2, size.Width, size.Height);

                r += stepR;
                alpha += stepAlpha;
                if (IsInRange(targetRectangle) &&
                    !m_QuadTree.HasContent(targetRectangle))
                {
                    m_QuadTree.Insert(new WordRectangle(targetRectangle, null, 0));
                    return targetRectangle;
                }
            }
        
            return RectangleF.Empty;
        }

        private bool IsInRange(RectangleF targetRectangle)
        {
            return
                targetRectangle.X >= m_Surface.X &&
                targetRectangle.Y >= m_Surface.Y &&
                targetRectangle.Bottom <= m_Surface.Bottom &&
                targetRectangle.Right <= m_Surface.Right;
        }
    }
}
