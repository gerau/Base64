using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Base64
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var s = "";

            var r = new Regex(@"(?i)base64\s+(?<cmd>\w)(?-i)\s+(?<input>[\w:\\.]+)(?<output>\s[\w:\\.]+)?");
            var rhelp = new Regex(@"(?i)base64\s+help\s*(?-i)");
            while (true)
            {
                Console.WriteLine("Enter command(type 'base64 help' for more information):");
                s = Console.ReadLine();
                if(s == "")
                {
                    Console.WriteLine("Error: empty input");
                    continue;
                }
                if (r.IsMatch(s))
                {
                    var match = r.Match(s);
                    if (match.Groups[1].ToString().Equals("d", StringComparison.OrdinalIgnoreCase))
                    {
                        Encoder.DecodeFile(match.Groups[2].ToString(), match.Groups[3].ToString().TrimStart());
                    }
                    else if (match.Groups[1].ToString().Equals("e", StringComparison.OrdinalIgnoreCase))
                    {
                        Encoder.EncodeFile(match.Groups[2].ToString(), match.Groups[3].ToString().TrimStart());
                    }
                    else
                    {
                        Console.WriteLine($"Error - unknown command '{match.Groups[1]}'");
                    }
                    continue;
                }
                if (rhelp.IsMatch(s))
                {
                    Console.WriteLine("Usage: base64 <cmd> <input> <output>");
                    Console.WriteLine("Where: <cmd> - 'd' or 'e' - decode or encode file");
                    Console.WriteLine("<input> - input file, which we need decode or encode in .base64 or .txt extension");
                    Console.WriteLine("<output> - output file, where we save result of decode or encode. Can be empty, so result saves on file with same name, but different extension");
                    Console.WriteLine("type 'exit' to close the console");
                    continue;
                }
                if (s.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                Console.WriteLine("Unknown command - commands starts from 'base64'");
            }
        }
    }
}