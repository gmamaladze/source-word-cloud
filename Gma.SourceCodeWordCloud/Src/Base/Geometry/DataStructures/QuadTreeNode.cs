﻿using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace Gma.CodeCloud.Base.DataStructures
{
    /// <summary>
    /// The QuadTreeNode
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuadTreeNode<T> where T : IRectangleContent
    {
        /// <summary>
        /// Construct a quadtree node with the given bounds 
        /// </summary>
        /// <param name="bounds"></param>
        public QuadTreeNode(RectangleF bounds)
        {
            m_Bounds = bounds;
        }

        /// <summary>
        /// The area of this node
        /// </summary>
        RectangleF m_Bounds;

        /// <summary>
        /// The contents of this node.
        /// Note that the contents have no limit: this is not the standard way to impement a QuadTree
        /// </summary>
        readonly List<T> m_Contents = new List<T>();

        /// <summary>
        /// The child nodes of the QuadTree
        /// </summary>
        readonly List<QuadTreeNode<T>> m_Nodes = new List<QuadTreeNode<T>>(4);

        /// <summary>
        /// Is the node empty
        /// </summary>
        public bool IsEmpty { get { return m_Bounds.IsEmpty || m_Nodes.Count == 0; } }

        /// <summary>
        /// Area of the quadtree node
        /// </summary>
        public RectangleF Bounds { get { return m_Bounds; } }

        /// <summary>
        /// Total number of nodes in the this node and all SubNodes
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;

                foreach (QuadTreeNode<T> node in m_Nodes)
                    count += node.Count;

                count += this.Contents.Count;

                return count;
            }
        }

        /// <summary>
        /// Return the contents of this node and all subnodes in the true below this one.
        /// </summary>
        public List<T> SubTreeContents
        {
            get
            {
                List<T> results = new List<T>();

                foreach (QuadTreeNode<T> node in m_Nodes)
                    results.AddRange(node.SubTreeContents);

                results.AddRange(this.Contents);
                return results;
            }
        }

        public List<T> Contents { get { return m_Contents; } }


        public bool HasContent(RectangleF queryArea)
        {
            // this quad contains items that are not entirely contained by
            // it's four sub-quads. Iterate through the items in this quad 
            // to see if they intersect.
            foreach (T item in this.Contents)
            {
                if (queryArea.IntersectsWith(item.Rectangle))
                {
                    return true;
                }
            }

            foreach (QuadTreeNode<T> node in m_Nodes)
            {
                if (node.IsEmpty)
                    continue;

                // Case 1: search area completely contained by sub-quad
                // if a node completely contains the query area, go down that branch
                // and skip the remaining nodes (break this loop)
                if (node.Bounds.Contains(queryArea))
                {
                    if (node.HasContent(queryArea))
                    {
                        return true;
                    }
                    break;
                }

                // Case 2: Sub-quad completely contained by search area 
                // if the query area completely contains a sub-quad,
                // just add all the contents of that quad and it's children 
                // to the result set. You need to continue the loop to test 
                // the other quads
                if (queryArea.Contains(node.Bounds))
                {
                    if (node.SubTreeContents.Count>0)
                    {
                        return true;
                    }
                    continue;
                }

                // Case 3: search area intersects with sub-quad
                // traverse into this quad, continue the loop to search other
                // quads
                if (node.Bounds.IntersectsWith(queryArea))
                {
                    if (node.HasContent(queryArea))
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// Query the QuadTree for items that are in the given area
        /// </summary>
        /// <param name="queryArea">
        /// <returns></returns></param>
        public List<T> Query(RectangleF queryArea)
        {
            // create a list of the items that are found
            List<T> results = new List<T>();

            // this quad contains items that are not entirely contained by
            // it's four sub-quads. Iterate through the items in this quad 
            // to see if they intersect.
            foreach (T item in this.Contents)
            {
                if (queryArea.IntersectsWith(item.Rectangle))
                    results.Add(item);
            }

            foreach (QuadTreeNode<T> node in m_Nodes)
            {
                if (node.IsEmpty)
                    continue;

                // Case 1: search area completely contained by sub-quad
                // if a node completely contains the query area, go down that branch
                // and skip the remaining nodes (break this loop)
                if (node.Bounds.Contains(queryArea))
                {
                    results.AddRange(node.Query(queryArea));
                    break;
                }

                // Case 2: Sub-quad completely contained by search area 
                // if the query area completely contains a sub-quad,
                // just add all the contents of that quad and it's children 
                // to the result set. You need to continue the loop to test 
                // the other quads
                if (queryArea.Contains(node.Bounds))
                {
                    results.AddRange(node.SubTreeContents);
                    continue;
                }

                // Case 3: search area intersects with sub-quad
                // traverse into this quad, continue the loop to search other
                // quads
                if (node.Bounds.IntersectsWith(queryArea))
                {
                    results.AddRange(node.Query(queryArea));
                }
            }


            return results;
        }

        /// <summary>
        /// Insert an item to this node
        /// </summary>
        /// <param name="item"></param>
        public void Insert(T item)
        {
            // if the item is not contained in this quad, there's a problem
            if (!m_Bounds.Contains(item.Rectangle))
            {
                Trace.TraceWarning("feature is out of the bounds of this quadtree node");
                return;
            }

            // if the subnodes are null create them. may not be sucessfull: see below
            // we may be at the smallest allowed size in which case the subnodes will not be created
            if (m_Nodes.Count == 0)
                CreateSubNodes();

            // for each subnode:
            // if the node contains the item, add the item to that node and return
            // this recurses into the node that is just large enough to fit this item
            foreach (QuadTreeNode<T> node in m_Nodes)
            {
                if (node.Bounds.Contains(item.Rectangle))
                {
                    node.Insert(item);
                    return;
                }
            }

            // if we make it to here, either
            // 1) none of the subnodes completely contained the item. or
            // 2) we're at the smallest subnode size allowed 
            // add the item to this node's contents.
            this.Contents.Add(item);
        }

        public void ForEach(QuadTree<T>.QuadTreeAction action)
        {
            action(this);

            // draw the child quads
            foreach (QuadTreeNode<T> node in this.m_Nodes)
                node.ForEach(action);
        }

        /// <summary>
        /// Internal method to create the subnodes (partitions space)
        /// </summary>
        private void CreateSubNodes()
        {
            // the smallest subnode has an area 
            if ((m_Bounds.Height * m_Bounds.Width) <= 10)
                return;

            float halfWidth = (m_Bounds.Width / 2f);
            float halfHeight = (m_Bounds.Height / 2f);

            m_Nodes.Add(new QuadTreeNode<T>(new RectangleF(m_Bounds.Location, new SizeF(halfWidth, halfHeight))));
            m_Nodes.Add(new QuadTreeNode<T>(new RectangleF(new PointF(m_Bounds.Left, m_Bounds.Top + halfHeight), new SizeF(halfWidth, halfHeight))));
            m_Nodes.Add(new QuadTreeNode<T>(new RectangleF(new PointF(m_Bounds.Left + halfWidth, m_Bounds.Top), new SizeF(halfWidth, halfHeight))));
            m_Nodes.Add(new QuadTreeNode<T>(new RectangleF(new PointF(m_Bounds.Left + halfWidth, m_Bounds.Top + halfHeight), new SizeF(halfWidth, halfHeight))));
        }

    }
}