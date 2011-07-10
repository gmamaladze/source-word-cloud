using System.Drawing;

namespace Gma.CodeCloud.Base.Geometry
{
    public class LayoutItem
    {
        public LayoutItem(RectangleF rectangle, string word, int occurances)
        {
            this.Rectangle = rectangle;
            Word = word;
            Occurances = occurances;
        }

        public RectangleF Rectangle { get; private set; }
        public string Word { get; private set; }
        public int Occurances { get; private set; }
    }
}
