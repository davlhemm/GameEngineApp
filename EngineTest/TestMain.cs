
using Algo;
using System.Diagnostics;

namespace EngineTest
{
    public class TestMain
    {
        [Fact]
        public void TestDelegate()
        {
            DelegateManager.UseDelegate();
        }

        [Fact]
        public void TestDijkstra()
        {
            Dijkstra.Main();
        }

        [Fact]
        public void TestPalindrome() 
        {
            StringManipulators.IsPalindrome("Test");
            StringManipulators.IsPalindrome("Testtset");
            StringManipulators.IsPalindrome("Testxtset");
        }

        [Fact]
        public void TestReverse()
        {
            Debug.WriteLine(StringManipulators.ReverseString("reversethis"));
            Debug.WriteLine(StringManipulators.ReverseString("reversethis123"));
            Debug.WriteLine(StringManipulators.ReverseString("reversethis1234"));
        }

        [Fact]
        public void TestVersioning()
        {
            Debug.WriteLine("Comparison result: "+StringManipulators.VersionCompare("2", "2.0.0.0.1"));
            Debug.WriteLine("Comparison result: " + StringManipulators.VersionCompare("2", "2.0.0.0.0"));
            Debug.WriteLine("Comparison result: " + StringManipulators.VersionCompare("2.1.0", "2.0.1"));
            Debug.WriteLine("Comparison result: " + StringManipulators.VersionCompare("2.0.1", "1.2000.1"));

        }
    }
}