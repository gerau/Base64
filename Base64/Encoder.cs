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
            if (!l.Contains("txt"))
            {
                Console.WriteLine("Error: input file not text file.");
                return;
            }

            using (FileStream fs = File.OpenRead(fileNameIn))
            {
                using (StreamWriter writetext = new StreamWriter($"{fileNameOut}.base64"))
                {
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
                            byte[] buf1 = { temp[size / 3] };
                            coded += Convertor.EncodeSymbol(buf1);
                            break;
                        case 2:
                            byte[] buf2 = { temp[size / 3], temp[size / 3 + 1] };
                            coded += Convertor.EncodeSymbol(buf2);
                            break;
                    }

                    writetext.WriteLine(coded);

                    writetext.Close();
                    fs.Close();
                }
            }
        }
        public static void DecodeFile(string fileNameIn, string fileNameOut = "")
        {
            if (!File.Exists(fileNameIn))
            {
                Console.WriteLine($"Error: cannot open file {fileNameIn}.");
                return;
            }
            if (fileNameOut.Length == 0) fileNameOut = fileNameIn.Remove(fileNameIn.IndexOf('.'));
            var l = fileNameIn.Substring(fileNameIn.IndexOf("."));
            try
            {
                string[] lines = File.ReadAllLines(fileNameIn);
                try
                {
                    using (StreamWriter writetext = new StreamWriter($"{fileNameOut}.txt"))
                    {
                        var counter = 1;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i][0] == '-')
                            {
                                continue;
                            }
                            if (((lines[i].Length % 4) != 0) | (lines[i].Length > 76))
                            {
                                Console.WriteLine($"Line{i + 1}: Incorrect length {lines[i].Length}");
                            }
                            if (lines[])
                        }
                        for (int i = 0; i < lines.Length - 1; i++)
                        {
                            for(int j = 0; j < 19; j++)
                            {
                                byte[] buf = new byte[4];
                                string str = lines[i].Substring(j * 4, j * 4 + 3);
                                var err = Convertor.DecodeTriplet(str, buf);
                                if(err != 0)
                                {
                                    Console.WriteLine($"Line{i+1}, Pos{j*4 + err - 1}: Incorrect symbol '{lines[i][j * 4 + err - 1]}' ");
                                    return;
                                }
                            }
                        }





                        //writetext.WriteLine();

                        writetext.Close();
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error: cannot create the file{fileNameOut}.base64 - {ex.ToString()}");
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: cannot open the file{fileNameIn} - {ex.ToString()}");
            }
        }
    }
}
