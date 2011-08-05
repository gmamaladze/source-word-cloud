using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Base.TextAnalyses.Processing;

namespace Gma.CodeCloud.Controls
{
    public class CloudControl : Panel
    {
        private List<IWord> m_Words;
        readonly Color[] m_DefaultPalette = new[] { Color.DarkRed, Color.DarkBlue, Color.DarkGreen, Color.Navy, Color.DarkCyan, Color.DarkOrange, Color.DarkGoldenrod, Color.DarkKhaki, Color.Blue, Color.Red, Color.Green };
        private Color[] m_Palette;
        private LayoutType m_LayoutType;

        private int m_MaxFontSize;
        private int m_MinFontSize;
        private ILayout m_Layout;
        private Color m_BackColor;
        private Timer m_BuildLayoutPostponeTimer;
        private int m_MinWordWeight;
        private int m_MaxWordWeight;

        public CloudControl() 
        {
            m_MinWordWeight = 0;
            m_MaxWordWeight = 0;

            MaxFontSize = 68;
            MinFontSize = 6;
           
            this.BorderStyle = BorderStyle.FixedSingle;
            this.ResizeRedraw = false;
            
            m_Palette = m_DefaultPalette;
            m_BackColor = Color.White;
            m_LayoutType = LayoutType.Spiral;
            m_BuildLayoutPostponeTimer = new Timer();
            m_BuildLayoutPostponeTimer.Interval = 100;
            m_BuildLayoutPostponeTimer.Tick += ResizePostponeTimerTick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_Words == null) { return; }
            if (m_Layout == null) { return; }

            IEnumerable<LayoutItem> wordsToRedraw = m_Layout.GetWordsInArea(e.ClipRectangle.ToPortable());
            using (Graphics graphics = e.Graphics)
            using (IGraphicEngine graphicEngine =
                    new GdiGraphicEngine(graphics, this.Font.FontFamily, FontStyle.Regular, m_Palette, MinFontSize, MaxFontSize, m_MinWordWeight, m_MaxWordWeight))
            {
                foreach (LayoutItem currentItem in wordsToRedraw)
                {
                    if (ItemUnderMouse == currentItem)
                    {
                        graphicEngine.DrawEmphasized(currentItem);
                    }
                    else
                    {
                        graphicEngine.Draw(currentItem);                        
                    }
                }
            }
        }

        public void BuildLayout()
        {
            m_Layout = LayoutFactory.CrateLayout(m_LayoutType, this.Size);
            ItemsCount = 0;
            if (m_Words == null || m_Words.Count==0) { return; }

            CaclulateMinMaxWordWeights();

            using (Graphics graphics = this.CreateGraphics())
            {
                IGraphicEngine graphicEngine =
                    new GdiGraphicEngine(graphics, this.Font.FontFamily, FontStyle.Regular, m_Palette, MinFontSize, MaxFontSize, m_MinWordWeight, m_MaxWordWeight);
                ItemsCount = m_Layout.Arrange(this.m_Words, graphicEngine);
            }
        }

        private void CaclulateMinMaxWordWeights()
        {
            IWord first = m_Words[0];
            if (first != null)
            {
                m_MaxWordWeight = first.Occurrences;
                const int empiricNumberOfWordsThatFitInControl = 400;
                IWord lastVisible = m_Words.Count >= empiricNumberOfWordsThatFitInControl
                                        ? m_Words[empiricNumberOfWordsThatFitInControl]
                                        : m_Words[m_Words.Count - 1];
                m_MinWordWeight = lastVisible.Occurrences;
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            LayoutItem nextItemUnderMouse;
            Point mousePositionRelativeToControl = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            this.TryGetItemAtLocation(mousePositionRelativeToControl, out nextItemUnderMouse);
            if (nextItemUnderMouse != ItemUnderMouse)
            {
                if (nextItemUnderMouse != null)
                {
                    Rectangle newRectangleToInvalidate = RectangleGrow(nextItemUnderMouse.Rectangle.ToDrawing(), 6);
                    this.Invalidate(newRectangleToInvalidate);
                }
                if (ItemUnderMouse != null)
                {
                    Rectangle prevRectangleToInvalidate = RectangleGrow(ItemUnderMouse.Rectangle.ToDrawing(), 6);
                    this.Invalidate(prevRectangleToInvalidate);
                }
                ItemUnderMouse = nextItemUnderMouse;
            }
            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            BuildLayoutAsync(100);
            base.OnResize(eventargs);
        }

        public void BuildLayoutAsync(int delay)
        {
            m_BuildLayoutPostponeTimer.Enabled = false;
            m_BuildLayoutPostponeTimer.Interval = delay;
            m_BuildLayoutPostponeTimer.Enabled = true;
        }

        private void ResizePostponeTimerTick(object sender, EventArgs e)
        {
            BuildLayout();
            m_BuildLayoutPostponeTimer.Enabled = false;
            Invalidate();
        }


        private static Rectangle RectangleGrow(Rectangle original, int growByPixels)
        {
            return new Rectangle(
                (original.X - growByPixels),
                (original.Y - growByPixels),
                (original.Width + growByPixels + 1),
                (original.Height + growByPixels + 1));
        }

        public LayoutItem ItemUnderMouse { get; private set; }

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

        public Color[] Palette
        {
            get { return m_Palette; }
            set
            {
                m_Palette = value;
                BuildLayout();
                Invalidate();
            }
        }

        public void SetWeightedWords(List<IWord> words)
        {
            m_Words = words;
            BuildLayout();
            Invalidate();
        }

        public List<IWord> WeightedWords
        {
            get { return m_Words; }
            set { SetWeightedWords(value); }
        }

        public int ItemsCount { get; private set; }

        public IEnumerable<LayoutItem> GetItemsInArea(RectangleF area)
        {
            if (m_Layout == null)
            {
                return new LayoutItem[] {};
            }

            return m_Layout.GetWordsInArea(area.ToPortable());
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

        protected override void Dispose(bool disposing)
        {
            if (m_BuildLayoutPostponeTimer!=null)
            {
                m_BuildLayoutPostponeTimer.Enabled = false;
                m_BuildLayoutPostponeTimer.Tick -= ResizePostponeTimerTick;
                m_BuildLayoutPostponeTimer.Dispose();
                m_BuildLayoutPostponeTimer = null;
            }
            base.Dispose(disposing);
        }
    }
}
