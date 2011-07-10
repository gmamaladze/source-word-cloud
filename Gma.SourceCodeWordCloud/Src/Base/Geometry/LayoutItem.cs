using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class LayoutItem
    {
        public LayoutItem(RectangleF rectangle, string word, int occurances)
        {
            this.Rectangle = rectangle;
            Word = word;
            Weight = occurances;
        }

        public RectangleF Rectangle { get; private set; }
        public string Word { get; private set; }
        public int Weight { get; private set; }

        public LayoutItem Clone()
        {
            return new LayoutItem(this.Rectangle, this.Word, this.Weight);
        }
    }
}
