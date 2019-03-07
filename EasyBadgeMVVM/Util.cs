using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM
{
    public class Util
    {

        private static char[] charsToReplace = new char[] { '_', '.', '-', '#', ':' };

        public static String TrimWord(string word)
        {
            return charsToReplace.Aggregate(word, (c1, c2) => c1.Replace(c2, ' ')).Replace(" ", string.Empty).ToLower();
        }

        public static void AddCharToReplace(char c)
        {
            charsToReplace[charsToReplace.Length] = c;
        }
    }
}
