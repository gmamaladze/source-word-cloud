using System;
using System.Drawing;
using Gma.CodeCloud.Base.Geometry;

namespace Gma.CodeCloud.Controls
{
    public class GdiGraphicEngine : IGraphicEngine, IDisposable
    {
        private readonly Graphics m_Graphics;

        private readonly int m_MinWordWeight;
        private readonly int m_MaxWordWeight;
     
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public Brush[] Palette { get; private set; }
        public float MinFontSize { get; set; }
        public float MaxFontSize { get; set; }

        public GdiGraphicEngine(Graphics graphics, FontFamily fontFamily, FontStyle fontStyle, Brush[] palette, float minFontSize, float maxFontSize, int minWordWeight, int maxWordWeight)
        {
            m_MinWordWeight = minWordWeight;
            m_MaxWordWeight = maxWordWeight;
            m_Graphics = graphics;
            FontFamily = fontFamily;
            FontStyle = fontStyle;
            Palette = palette;
            MinFontSize = minFontSize;
            MaxFontSize = maxFontSize;
        }

        public SizeF Measure(string text, int weight)
        {
            Font font = GetFont(weight);
            return m_Graphics.MeasureString(text, font);
        }

        public void Draw(LayoutItem layoutItem)
        {
            Font font = GetFont(layoutItem.Weight);
            Brush brush = GetNextBrushFromPalette(layoutItem.Weight);
            m_Graphics.DrawString(layoutItem.Word, font, brush, layoutItem.Rectangle);
        }

        private Font GetFont(int weight)
        {
            float fontSize = (float)(weight - m_MinWordWeight) / (m_MaxWordWeight - m_MinWordWeight) * (MaxFontSize - MinFontSize) + MinFontSize;
            return new Font(this.FontFamily, fontSize, this.FontStyle);
        }

        private Brush GetNextBrushFromPalette(int weight)
        {
            Brush brush = Palette[weight % Palette.Length];
            return brush;
        }

        public void Dispose()
        {
            m_Graphics.Dispose();
        }
    }
}
