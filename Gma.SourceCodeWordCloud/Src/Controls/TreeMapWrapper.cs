using System;
using System.Collections.Generic;
using Microsoft.Research.CommunityTechnologies.Treemap;

namespace Gma.CodeCloud.Controls
{
    internal class TreeMapWrapper : ICloudControl
    {
        private readonly TreemapControl m_TreemapControl;

        public TreeMapWrapper(TreemapControl treemapControl)
        {
            m_TreemapControl = treemapControl;
        }

        public void BeginUpdate()
        {
            m_TreemapControl.BeginUpdate();
        }

        public void EndUpdate()
        {
            m_TreemapControl.EndUpdate();
        }

        public void Clear()
        {
            m_TreemapControl.Clear();
        }

        public void Show(KeyValuePair<string, int>[] words)
        {

            m_TreemapControl.Clear();
            m_TreemapControl.BeginUpdate();
               
            double sum = 0;
            foreach (KeyValuePair<string, int> pair in words)
            {
                sum += pair.Value;
            }

            foreach (KeyValuePair<string, int> pair in words)
            {
                m_TreemapControl.Nodes.Add(pair.Key, pair.Value, pair.Value, null, String.Format("{0} - {1}% - {2} occurances", pair.Key, Math.Round(pair.Value * 100 / sum, 2), pair.Value));

            }
            m_TreemapControl.EndUpdate();

        }
    }
}