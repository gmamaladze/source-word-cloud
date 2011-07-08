using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gma.CodeCloud
{
    internal interface ICloudControl
    {
        void Clear();
        void Show(KeyValuePair<string, int>[] words);
    }
}
