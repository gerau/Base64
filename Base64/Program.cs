using System.Runtime.InteropServices;

namespace Base64
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = { 0xFF, 0xFF, 0xFF };
            Console.WriteLine(Convertor.EncodeTriplet(bytes));
            Console.WriteLine(Convertor.EncodeDuplet(bytes));
            Console.WriteLine(Convertor.EncodeSymbol(bytes));
        }
    }
}