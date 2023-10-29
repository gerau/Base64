using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64
{
    internal static class Encoder
    {
        public static void EncodeFile(string fileNameIn, string fileNameOut = "")
        {
            if (!File.Exists(fileNameIn))
            {
                Console.WriteLine($"Error: cannot open file {fileNameIn}.");
                return;
            }
            if (fileNameOut.Length == 0) fileNameOut = fileNameIn.Remove(fileNameIn.LastIndexOf('.'));
            var l = fileNameIn.Substring(fileNameIn.LastIndexOf('.'));
            if (l != ".txt")
            {
                Console.WriteLine("Error: input file not text file.");
                return;
            }

            using FileStream fs = File.OpenRead(fileNameIn);
            using StreamWriter writetext = new($"{fileNameOut}.base64");
            var size = fs.Length / 57;
            var coded = string.Empty;
            byte[] temp = new byte[57];
            for (int i = 0; i < size; i++)
            {
                fs.Read(temp, 0, 57);

                for (int j = 0; j < 19; j++)
                {
                    byte[] buf = { temp[3 * j], temp[3 * j + 1], temp[3 * j + 2] };
                    coded += Convertor.EncodeTriplet(buf);
                }
                writetext.WriteLine(coded);

                coded = string.Empty;
                temp = new byte[57];
            }

            fs.Read(temp, 0, 57);
            size = fs.Length % 57;
            for (int i = 0; i < size / 3; i++)
            {
                byte[] buf = { temp[3 * i], temp[3 * i + 1], temp[3 * i + 2] };
                coded += Convertor.EncodeTriplet(buf);
            }
            switch (size % 3)
            {
                case 0:
                    break;
                case 1:
                    byte[] buf1 = { temp[size - 1] };
                    coded += Convertor.EncodeSymbol(buf1);
                    break;
                case 2:
                    byte[] buf2 = { temp[size - 2], temp[size - 1] };
                    coded += Convertor.EncodeDuplet(buf2);
                    break;
            }
            writetext.WriteLine(coded);

            writetext.Close();
            fs.Close();
        }

        public static void DecodeFile(string fileNameIn, string fileNameOut = "")
        {
            if (!File.Exists(fileNameIn))
            {
                Console.WriteLine($"Error: cannot open file {fileNameIn}.");
                return;
            }
            if (fileNameOut.Length == 0) fileNameOut = fileNameIn.Remove(fileNameIn.IndexOf('.'));
            var l = fileNameIn.Substring(fileNameIn.LastIndexOf("."));
            if (l != ".base64")
            {
                Console.WriteLine("Error: input file not .base64 file.");
                return;
            }
            try
            {
                string[] lines = File.ReadAllLines(fileNameIn);
                try
                {
                    using StreamWriter writetext = new($"{fileNameOut}");
                    var lastLine = 0;
                    var write = "";
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i][0] == '-')
                        {
                            continue;
                        }
                        if (((lines[i].Length % 4) != 0) | (lines[i].Length > 76))
                        {
                            Console.WriteLine($"Line {i + 1}: Incorrect length {lines[i].Length}");
                            return;
                        }
                        lastLine = i;
                        if ((lines[i][lines[i].Length - 1] == '=') & (lines.Length != i + 1))
                        {
                            for(int j = lastLine + 1; j < lines.Length; j++)
                            {
                                if (lines[j][0] != '-')
                                {
                                    Console.WriteLine("Warning: available data after the end of the message");
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < lastLine; i++) 
                    {
                        if (lines[i][0] == '-' | lastLine == i) continue;

                        if (lines[i].Length != 76)
                        {
                            Console.WriteLine($"Line {i + 1}: Incorrect length - expected 76, but {lines[i].Length}");
                            return;
                        }

                        if (lines[i].Contains('='))
                        {
                            Console.WriteLine($"Line {i + 1}: Incorrect symbol '=' - expected on end of the line {lastLine + 1}");
                            return;
                        }

                        for (int j = 0; j < 19; j++)
                        {
                            byte[] buf = new byte[3];
                            string str = lines[i].Substring(j * 4,4);
                            var err = Convertor.DecodeTriplet(str, buf);
                            if (err != 0)
                            {
                                Console.WriteLine($"Line{i + 1}, Pos{j * 4 + err}: Incorrect symbol '{lines[i][j * 4 + err]}' ");
                                return;
                            }
                            write = System.Text.Encoding.Default.GetString(buf);
                            writetext.Write(write);
                        }
                    }
                    string s = lines[lastLine].TrimEnd('=');
                    for (int j = 0; j < s.Length / 4; j++)
                    {
                        byte[] buf = new byte[3];
                        string str = s.Substring(j * 4,4);
                        var err = Convertor.DecodeTriplet(str, buf);
                        if (s.Contains('='))
                        {
                            Console.WriteLine($"Line {lastLine + 1}: Incorrect symbol '=' - expected on end of the line {lastLine + 1}");
                            return;
                        }
                        if (err != 0)
                        {
                            Console.WriteLine($"Line {lastLine + 1}, Pos{j * 4 + err - 1}: Incorrect symbol '{s[j * 4 + err - 1]}' ");
                            return;
                        }
                        write = System.Text.Encoding.Default.GetString(buf);
                        writetext.Write(write);
                    }
                    var k = 0; var error = 0; var subs = "";
                    switch (s.Length % 4)
                    {
                        case 3:
                            byte[] buf = new byte[2];
                            k = s.Length - 3;
                            subs = s.Substring(k);
                            error = Convertor.DecodeDuplet(subs, buf);
                            if (error != 0)
                            {
                                Console.WriteLine($"Line {lastLine + 1}, Pos{k + error}: Incorrect symbol '{s[k + error]}' ");
                                return;
                            }
                            write = System.Text.Encoding.Default.GetString(buf);
                            writetext.Write(write);
                            break;
                        case 2:
                            buf = new byte[1];
                            k = s.Length - 2;
                            subs = s.Substring(k);
                            error = Convertor.DecodeSymbol(subs, buf);
                            if (error != 0)
                            {
                                Console.WriteLine($"Line {lastLine + 1}, Pos{k + error}: Incorrect symbol '{s[k + error]}' ");
                                return;
                            }
                            write = System.Text.Encoding.Default.GetString(buf);
                            writetext.Write(write);
                            break;
                    }
                    writetext.Close();
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error: cannot create the file {fileNameOut}.base64 - {ex}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: cannot open the file{fileNameIn} - {ex}");
            }
        }
    }
}
