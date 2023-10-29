using System.Runtime.InteropServices;

namespace Base64
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = { 
                0xCF,
                0xF1,
                0x2E };
            var triplet = Convertor.EncodeTriplet(bytes);
            var duplet = Convertor.EncodeDuplet(bytes);
            var symbol = Convertor.EncodeSymbol(bytes);

            Console.WriteLine($"triplet = {triplet}");
            Console.WriteLine($"duplet = {duplet}");
            Console.WriteLine($"symbol = {symbol}");

            var result = new byte[3];
            Convertor.DecodeTriplet("ZQ==", result);
            Console.WriteLine($"decoded triplet = {result[0].ToString("X")}, {result[1].ToString("X")}, {result[2].ToString("X")}");
            Console.WriteLine(System.Text.Encoding.Default.GetString(result));
            result = new byte[2];
            Convertor.DecodeDuplet(duplet, result);
            Console.WriteLine($"decoded duplet = {result[0]:X}, {result[1].ToString("X")}");
            result = new byte[1];
            Convertor.DecodeSymbol(symbol, result);
            Console.WriteLine($"decoded symbol = {result[0]:X}");

            //Encoder.EncodeFile("C:\\data\\test.txt");

            Encoder.DecodeFile("C:\\data\\test.base64", "C:\\data\\test2.txt");
            Console.ReadLine();
        }
    }
}