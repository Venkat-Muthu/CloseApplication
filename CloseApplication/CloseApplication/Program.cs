using System;
using System.Linq;
using CloseApplication.Interop;

namespace CloseApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string imageName;
            if (args.Length == 0)
            {
                Console.Write("Please enter image name to close : ");
                imageName = Console.ReadLine();
            }
            else
            {
                imageName = args.First();
            }
            WinApi.CloseApplication(imageName);
        }
    }
}
