using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class FirstFitLayout : ILayout
    {
        private const int s_MinimalSideLength = 2;
        private readonly List<RectangleF> m_EmptyBoxes;

        public FirstFitLayout(SizeF size)
        {
            m_EmptyBoxes = new List<RectangleF>();
            m_EmptyBoxes.Add(new RectangleF(0,0, size.Width, size.Height));
        }

        public bool IsTooSmall(RectangleF rectangle)
        {
            return rectangle.Size.Width < s_MinimalSideLength || rectangle.Size.Height < s_MinimalSideLength;
        }

        public virtual RectangleF Add(SizeF size)
        {
            int index = FindFirstFitIndex(size);
            if (index<0)
            {
                return RectangleF.Empty;
            }

            RectangleF found = m_EmptyBoxes[index];
            m_EmptyBoxes.RemoveAt(index);

            CutResult cutResult = BoxCutter.CutFromCorner(found, size, 0);

            for (int i = 0; i < cutResult.EdgeBoxes.Length; i++ )
            {
                RectangleF edgeBox = cutResult.EdgeBoxes[i];
                if (!IsTooSmall(edgeBox))
                {
                    m_EmptyBoxes.Add(edgeBox);
                }
            }
            return cutResult.Middle;
        }

        protected virtual int FindFirstFitIndex(SizeF size)
        {
            for (int i = 0; i < m_EmptyBoxes.Count; i++)
            {
                RectangleF emptyBox = m_EmptyBoxes[i];
                if (Fits(size, emptyBox))
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool Fits(SizeF size, RectangleF target)
        {
            return target.Size.Width >= size.Width && target.Size.Height >= size.Height;
        }

        public void DrawWords(KeyValuePair<string, int>[] words, IGraphicEngine graphicEngine)
        {
            throw new NotImplementedException();
        }
    }
}
