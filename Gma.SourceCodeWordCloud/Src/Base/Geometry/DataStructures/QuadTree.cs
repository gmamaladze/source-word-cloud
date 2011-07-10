using System.Drawing;
using System.Collections.Generic;

namespace Gma.CodeCloud.Base.Geometry.DataStructures
{
    /// <summary>
    /// A Quadtree is a structure designed to partition space so
    /// that it's faster to find out what is inside or outside a given 
    /// area. See http://en.wikipedia.org/wiki/Quadtree
    /// This QuadTree contains items that have an area (RectangleF)
    /// it will store a reference to the item in the quad 
    /// that is just big enough to hold it. Each quad has a bucket that 
    /// contain multiple items.
    /// </summary>
    public class QuadTree<T> where T : LayoutItem
    {
        private readonly QuadTreeNode<T> m_Root;

        private readonly RectangleF m_Rectangle;

        public delegate void QuadTreeAction(QuadTreeNode<T> obj);

        public QuadTree(RectangleF rectangle)
        {
            m_Rectangle = rectangle;
            m_Root = new QuadTreeNode<T>(m_Rectangle);
        }

        public int Count { get { return m_Root.Count; } }

        public void Insert(T item)
        {
            m_Root.Insert(item);
        }

        public IEnumerable<T> Query(RectangleF area)
        {
            return m_Root.Query(area);
        }

        public bool HasContent(RectangleF area)
        {
            return m_Root.HasContent(area);
        } 
        
        public void ForEach(QuadTreeAction action)
        {
            m_Root.ForEach(action);
        }
    }
}
