using System;

namespace SFB_Profile_Converter
{
    public class ConsoleOut
    {
        public bool quiet { get; set; }

        public ConsoleOut(bool q)
        {
            quiet = q;
        }

        public void write(string val)
        {
            if (!quiet)
            {
                Console.WriteLine(val);
            }
        }

        public void write(int val)
        {
            if (!quiet)
            {
                Console.WriteLine(val);
            }
        }

        public void write(bool val)
        {
            if (!quiet)
            {
                Console.WriteLine(val);
            }
        }
    }
}