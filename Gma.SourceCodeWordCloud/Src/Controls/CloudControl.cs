using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gma.CodeCloud.Base.Geometry;
using Gma.CodeCloud.Controls;

namespace Gma.CodeCloud
{
    public class CloudControl : Panel, ICloudControl
    {
        private KeyValuePair<string, int>[] m_Words;
        private Brush[] m_Palette;

        public CloudControl()
        {
            MaxFontSize = 86;
            MinFontSize = 8;
            Clear();
            //this.BorderStyle = BorderStyle.FixedSingle;
            this.ResizeRedraw = true;
            m_Palette = new[] { Brushes.DarkRed, Brushes.DarkBlue, Brushes.DarkGreen, Brushes.DarkGray, Brushes.DarkCyan, Brushes.DarkOrange, Brushes.DarkGoldenrod, Brushes.DarkKhaki };
        }

        public void Clear()
        {
            using (Graphics graphics = this.CreateGraphics())
            {
                graphics.Clear(Color.White);
            }
        }

        private int m_MaxFontSize;
        private int m_MinFontSize;
        private int m_TopCount;

        public int MaxFontSize
        {
            get { return m_MaxFontSize; }
            set
            {
                m_MaxFontSize = value;
                Invalidate();
            }
        }

        public int MinFontSize
        {
            get { return m_MinFontSize; }
            set
            {
                m_MinFontSize = value;
                Invalidate();
            }
        }

        public int TopCount
        {
            get { return m_TopCount; }
            set
            {
                m_TopCount = value;
                Invalidate();
            }
        }

        public Brush[] Palette
        {
            get { return m_Palette; }
            set
            {
                m_Palette = value;
                Invalidate();
            }
        }

        protected override void  OnPaint(PaintEventArgs e)
        {
 	        base.OnPaint(e);

            Clear();
            if (m_Words==null || m_Words.Length==0)
            {
                return;
            }

            int maxWordWeight = m_Words[0].Value;
            int minWordWeight = m_Words[m_Words.Length - 1].Value;

            //Random randomizer = new Random(maxWordWeight);

            using (Graphics graphics = this.CreateGraphics())
            {
                ILayout layout = new RandomLayout(new[] {new RectangleF(new PointF(0,0), this.Size)}, new BoxCutter(new Random()),  1, new PointF(this.Size.Width / 2, this.Size.Height / 2) );
                int colorIndex = 0;
                foreach (KeyValuePair<string, int> pair in m_Words)
                {
                    float fontSize = (float) (pair.Value - minWordWeight)/(maxWordWeight - minWordWeight)*(MaxFontSize - MinFontSize) + MinFontSize;

                    Font font = new Font(this.Font.FontFamily, fontSize);
                    SizeF size = graphics.MeasureString(pair.Key, font);
                    SizeF sizeWithPadding = size + new SizeF(8, 8);
                    RectangleF rectangle = layout.Add(sizeWithPadding);
                    if (rectangle == RectangleF.Empty)
                    {
                        break;
                    }
                    Brush brush = m_Palette[colorIndex % m_Palette.Length];
                    graphics.DrawString(pair.Key, font, brush, rectangle.X + 4, rectangle.Y + 4);
                    
                    colorIndex++;
                }

                //foreach (var rectangle in layout.EmptyBoxes)
                //{
                //    graphics.DrawRectangle(Pens.Blue, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                //}
            }
        }

        public void Show(KeyValuePair<string, int>[] words)
        {
            m_Words = words;
            Invalidate();
        }
    }
}
