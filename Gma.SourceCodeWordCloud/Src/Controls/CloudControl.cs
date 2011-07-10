using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gma.CodeCloud.Base.Geometry;

namespace Gma.CodeCloud.Controls
{
    public class CloudControl : Panel
    {
        private KeyValuePair<string, int>[] m_Words;
        readonly Brush[] m_DefaultPalette = new[] { Brushes.DarkRed, Brushes.DarkBlue, Brushes.DarkGreen, Brushes.DarkGray, Brushes.DarkCyan, Brushes.DarkOrange, Brushes.DarkGoldenrod, Brushes.DarkKhaki };
        private Brush[] m_Palette;
        private LayoutType m_LayoutType;

        private int m_MaxFontSize;
        private int m_MinFontSize;
        private ILayout m_Layout;
        private Color m_BackColor;

        public CloudControl()
        {
            MaxFontSize = 68;
            MinFontSize = 6;
           
            this.BorderStyle = BorderStyle.FixedSingle;
            this.ResizeRedraw = true;
            
            m_Palette = m_DefaultPalette;
            m_BackColor = Color.White;
            m_LayoutType = LayoutType.Spiral;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_Words == null || m_Words.Length == 0) { return; }
            if (m_Layout == null) { return; }

            int maxWordWeight = m_Words[0].Value;
            int minWordWeight = m_Words[m_Words.Length - 1].Value;

            IEnumerable<LayoutItem> wordsToRedraw = m_Layout.GetWordsInArea(e.ClipRectangle);
            using (Graphics graphics = e.Graphics)
            using (IGraphicEngine graphicEngine =
                    new GdiGraphicEngine(graphics, this.Font.FontFamily, FontStyle.Regular, m_Palette, MinFontSize, MaxFontSize, minWordWeight, maxWordWeight))
            {
                foreach (LayoutItem currentItem in wordsToRedraw)
                {
                    graphicEngine.Draw(currentItem);
                }
            }
        }

        private void BuildLayout()
        {
            if (m_Words == null || m_Words.Length == 0) { return; }

            int maxWordWeight = m_Words[0].Value;
            int minWordWeight = m_Words[m_Words.Length - 1].Value;

            using (Graphics graphics = this.CreateGraphics())
            {
                IGraphicEngine graphicEngine =
                    new GdiGraphicEngine(graphics, this.Font.FontFamily, FontStyle.Regular, m_Palette, MinFontSize, MaxFontSize, minWordWeight, maxWordWeight);
                m_Layout = LayoutFactory.CrateLayout(m_LayoutType, this.Size);
                m_Layout.Arrange(m_Words, graphicEngine);
            }
        }

        public LayoutType LayoutType
        {
            get { return m_LayoutType; }
            set
            {
                if (value == m_LayoutType)
                {
                    return;
                }

                m_LayoutType = value;
                BuildLayout();
                Invalidate();
            }
        }
 
        public override Color BackColor
        {
            get
            {
                return m_BackColor;
            }
            set
            {
                if (m_BackColor == value)
                {
                    return;
                }
                m_BackColor = value;
                Invalidate();
            }
        }

        public int MaxFontSize
        {
            get { return m_MaxFontSize; }
            set
            {
                m_MaxFontSize = value;
                BuildLayout();
                Invalidate();
            }
        }

        public int MinFontSize
        {
            get { return m_MinFontSize; }
            set
            {
                m_MinFontSize = value;
                BuildLayout();
                Invalidate();
            }
        }

        public Brush[] Palette
        {
            get { return m_Palette; }
            set
            {
                m_Palette = value;
                BuildLayout();
                Invalidate();
            }
        }

        public KeyValuePair<string, int>[] WeightedWords
        {
            get { return m_Words; }
            set
            {
                m_Words = value;
                BuildLayout();
                Invalidate();
            }
        }

        public IEnumerable<LayoutItem> GetItemsInArea(RectangleF area)
        {
            if (m_Layout == null)
            {
                return new LayoutItem[] {};
            }

            return m_Layout.GetWordsInArea(area);
        }

        public bool TryGetItemAtLocation(Point location, out LayoutItem foundItem)
        {
            foundItem = null;
            IEnumerable<LayoutItem> itemsInArea = GetItemsInArea(new RectangleF(location, new SizeF(0, 0)));
            foreach (LayoutItem item in itemsInArea)
            {
                foundItem = item;
                return true;
            }
            return false;
        }
    }
}
