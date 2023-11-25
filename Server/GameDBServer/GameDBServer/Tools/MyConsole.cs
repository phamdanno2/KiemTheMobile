﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Tools;

namespace System
{
    public class MyConsole
    {
        private static object mutex = new object();

        public static void WriteLine<T>(T value)
        {
            lock (mutex)
            {
                Console.WriteLine(value);
            }
        }

        public static void WriteLine(string format, params object[] arg)
        {
            lock (mutex)
            {
                Console.WriteLine(format, arg);
            }
        }
    }
}
