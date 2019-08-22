using System;
using DTV;

namespace Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            var hmmm = new Hmmm();
            hmmm.AddN(0, 1);
            hmmm.AddN(1, 255);
            hmmm.Add(0, 0, 1);
            hmmm.Mul(0, 0, 1);
            hmmm.Mul(0, 0, 1);
            hmmm.Write(0);  
            hmmm.Halt();
        }
    }
}
