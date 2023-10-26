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
    }
}
