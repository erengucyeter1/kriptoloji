using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kriptoloji
{
    internal class TextParser
    {
        public TextParser(string language)
        {
        }

        public static string FilterText(string text)
        {

            StringBuilder builder = new StringBuilder();
            foreach (char c in text.ToUpper())
            {
                bool charCheck = char.IsLetter(c);
                bool langCheck = AlphabetHelper.IsLetter(c);

                if (charCheck && langCheck)
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public static string[,] ParseStringToMatrix(string text)
        {
            string[] rows = text.Split(';');
            int rowCount = rows.Length;
            int colCount = rows[0].Split(',').Length;

            string[,] matrix = new string[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] cols = rows[i].Split(',');
                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = cols[j];
                }
            }
            return matrix;
        }

        public static List<string> ParseTextIntoBlocks(string text, int blockSize)
        {
            List<string> blocks = new List<string>();

            int remainder = text.Length % blockSize;

            if (remainder != 0)
            {
                StringBuilder builder = new StringBuilder(text);

                for (int i = 0; i < blockSize - remainder; i++)
                {
                    builder.Append(AlphabetHelper.GetRandomLetter());
                }

                text = builder.ToString();
            }

            for (int i = 0; i < text.Length; i += blockSize)
            {
                blocks.Add(text.Substring(i, blockSize));
            }


            return blocks;
        }
    }
}
