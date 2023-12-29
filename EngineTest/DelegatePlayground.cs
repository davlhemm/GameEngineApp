using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineTest
{
    public static class DelegateManager
    {
        public static void UseDelegate()
        {
            DelegatePlayground.DelegatePlaygroundCallback aDelegate = DelegatePlayground.DelegateMethod;
            //aDelegate("Test Delegate Input");
            aDelegate += DelegatePlayground.DelegateMethod2;
            //aDelegate += DelegatePlayground.DelegateMethod;
            aDelegate("Test Delegate Input");
            //TODO: Multi-cast return?!
        }
    }

    public static class DelegatePlayground
    {
        //Delegate for callback mechanism
        public delegate void DelegatePlaygroundCallback(string input);

        public static void DelegateMethod(string input)
        {
            Debug.WriteLine(input);
        }
        public static void DelegateMethod2(string input)
        {
            Debug.WriteLine("Delegate 2");
        }
    }
}
