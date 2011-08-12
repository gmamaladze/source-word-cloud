using System;
using System.Collections.Generic;
using Gma.CodeCloud.Base.Geometry.Portability;
using System.Linq;
using Gma.CodeCloud.Base.Geometry.DataStructures;
using Gma.CodeCloud.Base.TextAnalyses.Processing;

namespace Gma.CodeCloud.Base.Geometry
{
    public abstract class BaseLayout : ILayout
    {
        protected QuadTree<LayoutItem> QuadTree { get; set; }
        protected Point Center { get; set; }
        protected Rectangle Surface { get; set; }

        protected BaseLayout(Size size)
        {
            Surface = new Rectangle(new Point(0, 0), size);
            QuadTree = new QuadTree<LayoutItem>(Surface);
            Center = new Point(Surface.X + size.Width / 2, Surface.Y + size.Height / 2);
        }

        public int Arrange(IEnumerable<IWord> words, IGraphicEngine graphicEngine)
        {
            if (words == null)
            {
                throw new ArgumentNullException("words");
            }

            if (words.First() == null)
            {
                return 0;
            }


            foreach (IWord word in words)
            {
                Size size = graphicEngine.Measure(word.Text, word.Occurrences);
                Rectangle freeRectangle;
                if (!TryFindFreeRectangle(size, out freeRectangle))
                {
                    break;
                }
                LayoutItem item = new LayoutItem(freeRectangle, word);
                QuadTree.Insert(item);
            }
            return QuadTree.Count;
        }

        public abstract bool TryFindFreeRectangle(Size size, out Rectangle foundRectangle);

        public IEnumerable<LayoutItem> GetWordsInArea(Rectangle area)
        {
            return QuadTree.Query(area);
        }

        protected bool IsInsideSurface(Rectangle targetRectangle)
        {
            return IsInside(Surface, targetRectangle);
        }

        private static bool IsInside(Rectangle outer, Rectangle inner)
        {
            return
                inner.X >= outer.X &&
                inner.Y >= outer.Y &&
                inner.Bottom <= outer.Bottom &&
                inner.Right <= outer.Right;
        }
    }
}
