using System.Drawing;
using System.Collections.Generic;
using Gma.CodeCloud.Base.Geometry;

namespace Gma.CodeCloud.Base.DataStructures
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
        /// <summary>
        /// The root QuadTreeNode
        /// </summary>
        private readonly QuadTreeNode<T> m_Root;

        /// <summary>
        /// The bounds of this QuadTree
        /// </summary>
        private readonly RectangleF m_Rectangle;

        /// <summary>
        /// An delegate that performs an action on a QuadTreeNode
        /// </summary>
        /// <param name="obj"></param>
        public delegate void QuadTreeAction(QuadTreeNode<T> obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        public QuadTree(RectangleF rectangle)
        {
            m_Rectangle = rectangle;
            m_Root = new QuadTreeNode<T>(m_Rectangle);
        }

        /// <summary>
        /// Get the count of items in the QuadTree
        /// </summary>
        public int Count { get { return m_Root.Count; } }

        /// <summary>
        /// Insert the feature into the QuadTree
        /// </summary>
        /// <param name="item"></param>
        public void Insert(T item)
        {
            m_Root.Insert(item);
        }

        /// <summary>
        /// Query the QuadTree, returning the items that are in the given area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(RectangleF area)
        {
            return m_Root.Query(area);
        }

        public bool HasContent(RectangleF area)
        {
            return m_Root.Query(area).Count>0;
        } 
        
        /// <summary>
        /// Do the specified action for each item in the quadtree
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(QuadTreeAction action)
        {
            m_Root.ForEach(action);
        }

    }

}
