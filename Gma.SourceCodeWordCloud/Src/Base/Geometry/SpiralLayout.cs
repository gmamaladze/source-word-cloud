using System;
using System.Collections.Generic;
using System.Drawing;
using Gma.CodeCloud.Base.DataStructures;

namespace Gma.CodeCloud.Base.Geometry
{
    public class SpiralLayout : ILayout
    {
        private readonly QuadTree<LayoutItem> m_QuadTree;
        private readonly PointF m_Center;
        private readonly RectangleF m_Surface;

        public SpiralLayout(SizeF size)
        {
            m_Surface = new RectangleF(new PointF(0, 0), size);
            m_QuadTree = new QuadTree<LayoutItem>(m_Surface);
            m_Center = new PointF(m_Surface.X + size.Width / 2, m_Surface.Y + size.Height / 2);
        }

        public void DrawWords(KeyValuePair<string, int>[] words, IGraphicEngine graphicEngine)
        {
            if (words == null)
            {
                throw new ArgumentNullException("words");
            }

            if (words.Length == 0)
            {
                return;
            }


            foreach (KeyValuePair<string, int> pair in words)
            {
                string word = pair.Key;
                int weight = pair.Value;
                SizeF size = graphicEngine.Measure(word, weight);
                RectangleF freeRectangle;
                if (!TryFindFreeRectangle(size, out freeRectangle))
                {
                    return;
                }
                LayoutItem item = new LayoutItem(freeRectangle, word, weight);
                m_QuadTree.Insert(item);
                graphicEngine.Draw(item);
            }
        }

        public bool TryFindFreeRectangle(SizeF size, out RectangleF foundRectangle)
        {
            foundRectangle = RectangleF.Empty;
            double alpha = GetPseudoRandomStartAngle(size);
            const double stepAlpha = Math.PI / 60;

            const double pointsOnSpital = 500;


            Math.Min(m_Center.Y, m_Center.X);
            for (int pointIndex = 0; pointIndex < pointsOnSpital; pointIndex++)
            {
                double dX = pointIndex / pointsOnSpital * Math.Sin(alpha) * m_Center.X;
                double dY = pointIndex / pointsOnSpital * Math.Cos(alpha) * m_Center.Y;
                foundRectangle = new RectangleF((float)(m_Center.X + dX) - size.Width / 2, (float)(m_Center.Y + dY) - size.Height / 2, size.Width, size.Height);

                alpha += stepAlpha;
                if (!IsInsideSurface(foundRectangle))
                {
                    return false;
                }

                if (!m_QuadTree.HasContent(foundRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<LayoutItem> GetWordsInArea(RectangleF area)
        {
            return m_QuadTree.Query(area);
        }

        private static float GetPseudoRandomStartAngle(SizeF size)
        {
            return size.Height*size.Width;
        }

        private bool IsInsideSurface(RectangleF targetRectangle)
        {
            return IsInside(m_Surface, targetRectangle);
        }

        private static bool IsInside(RectangleF outer, RectangleF inner)
        {
            return
                inner.X >= outer.X &&
                inner.Y >= outer.Y &&
                inner.Bottom <= outer.Bottom &&
                inner.Right <= outer.Right;
        }
    }
}
