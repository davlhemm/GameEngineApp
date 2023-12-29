using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{

    public static class StringManipulators
    {
        public static bool IsPalindrome(string inputString)
        {
            string s = inputString.ToLowerInvariant();
            Debug.WriteLine("String input: " + s);
            for (int i = 0; i < s.Length / 2; i++)
            {
                bool compareChar = s[i].Equals(s[s.Length - 1 - i]);
                if (compareChar != true)
                    return false;
            }
            return true;
        }

        public static string? ReverseString(string inputString)
        {
            var charArray = inputString.ToCharArray();
            char tempC;
            for (int i = 0; i < charArray.Length/2; i++) 
            {
                tempC = charArray[i];
                charArray[i] = charArray[charArray.Length - i - 1];
                charArray[charArray.Length - i - 1] = tempC;
            }
            return new string(charArray);
        }

        /// <summary>
        /// Return -1 for lesser than, 0 for same, 1 for greater than
        /// W.X.Y.Z all positive integers
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static int VersionCompare(string version1, string version2)
        {
            //Delimiter is always "."
            //We are assuming that we have valid non-negative integers here, 
            //otherwise parsing safely and/or regex for lexicographical is a better idea
            List<int> inputs1 = version1.Split(".").Select(x => Int32.Parse(x)).ToList();
            List<int> inputs2 = version2.Split(".").Select(x => Int32.Parse(x)).ToList();

            //Extend zeroes so that you can compare as same length
            while (inputs1.Count < inputs2.Count)
            {
                inputs1.Add(0);
            }
            while (inputs2.Count < inputs1.Count)
            {
                inputs2.Add(0);
            }

            //For each section, return whether a difference is found
            for (int i = 0; i < inputs1.Count; i++)
            {
                if (inputs1[i] < inputs2[i]) 
                    return -1;
                if (inputs1[i] > inputs2[i]) 
                    return 1;
            }

            //If no differential found then equivalence
            return 0;
        }
    }

}
