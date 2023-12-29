using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTest
{
    public static class Palindrome
    {
        public static bool IsPalindrome(string inputString)
        {
            string s = inputString.ToLowerInvariant();
            Debug.WriteLine("String input: "+s);
            for(int i = 0; i < s.Length/2; i++)
            {
                bool compareChar = s[i].Equals(s[s.Length - 1 - i]);
                if(compareChar != true)
                    return false;
            }
            return true;
        }
    }

    public static class Dijkstra
    {
        public static void Main()
        {
            var max = Int32.MaxValue;
            DijkstraAlgo algo = new DijkstraAlgo(
                new List<List<int>>()
                {
                    new List<int>(){ max,   1,   2, max },
                    new List<int>(){   1, max,   3,   5 },
                    new List<int>(){   2,   3, max,   1 },
                    new List<int>(){ max,   5,   1, max }
                }
                );
            algo.Log();
        }
    }

    public class DijkstraAlgo
    {
        List<string> UnvisitedNodes { get; set; } = new List<string>();
        List<string> VisitedNodes { get; set; } = new List<string>();

        List<List<int>> Vertices { get; set; }

        public DijkstraAlgo(List<List<int>> vertices) 
        {
            Vertices = vertices;
            int index = 0;
            foreach (var v in Vertices)
            {
                UnvisitedNodes.Add(index.ToString());
                var numOfNodes = Vertices.Count;
                foreach (var node in v)
                {

                }
                index++;
            }
        }

        public void Log()
        {
            Debug.WriteLine(UnvisitedNodes.ToString());
        }
    }
}
