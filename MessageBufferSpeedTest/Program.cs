using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using MessageBuffers;

namespace MessageBufferSpeedTest
{
    static class Program
    {
        static void Main()
        {
            const uint LOOP = 1000000000;
            Stopwatch watch;

            MessageWriter w = new MessageWriter();
            ushort a = 200;

            for (int times = 0; times < 5; times++)
            {
                /*watch = Stopwatch.StartNew();
                for (uint i = 0; i < LOOP; i++)
                {
                    w.writeInt(300);
                }
                watch.Stop();
                Console.WriteLine("MessageBuffer (bitshift): " + watch.ElapsedMilliseconds + "ms");*/

                /*watch = Stopwatch.StartNew();
                for (uint i = 0; i < LOOP; i++)
                {
                    w.Clear();
                }
                watch.Stop();
                Console.WriteLine("MessageBuffer (new clear): " + watch.ElapsedMilliseconds + "ms");*/

                watch = Stopwatch.StartNew();
                for (uint i = 0; i < LOOP; i++)
                {
                    a = 100;
                    if (a > 130)
                    {
                        a = 200;
                    }
                }
                watch.Stop();
                Console.WriteLine("MessageBuffer (if - minimum): " + watch.ElapsedMilliseconds + "ms");

                watch = Stopwatch.StartNew();
                for (uint i = 0; i < LOOP; i++)
                {
                    a = 100;
                    a = (ushort)Math.Min((int)a, 130);
                }
                watch.Stop();
                Console.WriteLine("MessageBuffer (Math.Min - minimum): " + watch.ElapsedMilliseconds + "ms");
            }

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
