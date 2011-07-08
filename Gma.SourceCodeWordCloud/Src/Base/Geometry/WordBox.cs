using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Gma.CodeCloud.Base.DataStructures;

namespace Gma.CodeCloud.Base.Geometry
{
    public class WordRectangle : IRectangleContent
    {
        public WordRectangle(RectangleF rectangle, string word, int occurances)
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
