using System;
using System.Collections.Generic;

namespace Gma.CodeCloud
{
    internal class WordRegistry : Dictionary<string, int>, IWordRegistry
    {
        public WordRegistry() 
            : this(0xfff, StringComparer.InvariantCultureIgnoreCase)
        {
            
        } 

        public WordRegistry(int initalCapacity, StringComparer comparer) 
            : base(initalCapacity, comparer)
        {

        }

        public void AddOccurance(string word)
        {
            AddOccurances(word, 1);
        }

        public void AddOccurances(string word, int increment)
        {
            int count = GetOccurances(word);
            count += increment;
            SetOccurances(word, count);
        }

        public KeyValuePair<string, int>[] GetSortedByOccurances()
        {
            KeyValuePair<string, int>[] result = new KeyValuePair<string, int>[this.Count];
            int i=0;
            foreach (KeyValuePair<string, int> entry in this)
            {
                result[i] = entry;
                i++;
            }
            Array.Sort(result, OccuranceComparer);
            return result;
        }

        private static int OccuranceComparer(KeyValuePair<string, int> entryLeft, KeyValuePair<string, int> entryRight)
        {
            return entryRight.Value - entryLeft.Value;
        }

        private int GetOccurances(string word)
        {
            int currentCount;
            this.TryGetValue(word, out currentCount);
            return currentCount;
        }

        private void SetOccurances(string word, int count)
        {
            this[word] = count;
        }
    }
}
