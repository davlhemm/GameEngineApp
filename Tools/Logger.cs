using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineApp.Tools
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Debug.WriteLine(message);
        }

        public static void WhatThread(string callee = "callee")
        {
            Debug.WriteLine(String.Format("Thread used by {0}: {1}",
                callee,
                Thread.CurrentThread.ManagedThreadId));
        }
    }
}
