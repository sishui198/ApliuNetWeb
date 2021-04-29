using System;

namespace Apliu.Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Apliu Core Console Hello World!");
            Console.WriteLine("------------------------------------------------");
            try
            {
                Apliu.Test.ConsoleApp.RunFuction.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR：" + ex.Message);
            }
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Apliu Core Console End!");
            Console.ReadKey();
        }
    }
}
