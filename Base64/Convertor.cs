using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64
{
   /// <summary>
   /// Basic convertor for Base64 coder
   /// </summary>
    internal class Convertor
    {
        const string BASE64_ALPH = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+\\";

        public static string EncodeTriplet(byte[] input)
        {
            char[] result = "====".ToCharArray();
            byte temp;

            temp = (byte)(input[0] >> 2);
            result[0] = BASE64_ALPH[temp];

            temp = (byte)(((input[0] & 3) << 4));
            temp = (byte)(temp | (input[1]) >> 4);
            result[1] = BASE64_ALPH[temp];

            temp = (byte)((input[1] & 15) << 2);
            temp = (byte)(temp | (input[2] >> 6));
            result[2] = BASE64_ALPH[temp];

            temp = (byte)(input[2] & 63);
            result[3] = BASE64_ALPH[temp];

            
            return new string(result);
        }

        public static string EncodeDuplet(byte[] input)
        {
            char[] result = "====".ToCharArray();
            byte temp;

            temp = (byte)(input[0] >> 2);
            result[0] = BASE64_ALPH[temp];

            temp = (byte)(((input[0] & 3) << 4));
            temp = (byte)(temp | (input[1]) >> 4);
            result[1] = BASE64_ALPH[temp];

            temp = (byte)((input[1] & 15) << 2);
            result[2] = BASE64_ALPH[temp];

            return new string(result);
        }
        public static string EncodeSymbol(byte[] input)
        {
            char[] result = "====".ToCharArray();
            byte temp;

            temp = (byte)(input[0] >> 2);
            result[0] = BASE64_ALPH[temp];

            temp = (byte)(((input[0] & 3) << 4));
            result[1] = BASE64_ALPH[temp];

            return new string(result);
        }

        public static int DecodeTriplet(string code,byte[] result) 
        {
            var no = BASE64_ALPH.IndexOf(code[0]);
            if (no == -1) return 1;

            result[0] = (byte)(no << 2);

            no = BASE64_ALPH.IndexOf(code[1]);
            if (no == -1) return 2;

            result[0] = (byte)(result[0] | no >> 4);
            result[1] = (byte)(no << 4);  

            no = BASE64_ALPH.IndexOf(code[2]);
            if (no == -1) return 3;

            result[1] = (byte)(result[1] | no >> 2);
            result[2] = (byte)(no << 6);

            no = BASE64_ALPH.IndexOf(code[3]);
            if (no == -1) return 4;

            result[2] = (byte)(result[2] | no);

            return 0;
        }

        public static int DecodeDuplet(string code, byte[] result)
        {
            var no = BASE64_ALPH.IndexOf(code[0]);
            if (no == -1) return 1;

            result[0] = (byte)(no << 2);

            no = BASE64_ALPH.IndexOf(code[1]);
            if (no == -1) return 2;

            result[0] = (byte)(result[0] | no >> 4);
            result[1] = (byte)(no << 4);

            no = BASE64_ALPH.IndexOf(code[2]);
            if (no == -1) return 3;

            result[1] = (byte)(result[1] | no >> 2);

            return 0;
        }

        public static int DecodeSymbol(string code, byte[] result)
        {
            var no = BASE64_ALPH.IndexOf(code[0]);
            if (no == -1) return 1;

            result[0] = (byte)(no << 2);

            no = BASE64_ALPH.IndexOf(code[1]);
            if (no == -1) return 2;

            result[0] = (byte)(result[0] | no >> 4);

            return 0;
        }

    }
}
