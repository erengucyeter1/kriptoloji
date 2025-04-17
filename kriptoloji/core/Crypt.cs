using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kriptoloji
{
    public interface ICryptAlgorithm
    {
        string Crypt(string input);
        string DeCrypt(string input);
    }

    public abstract class CryptAlgorithmSchema : ICryptAlgorithm
    {
        public abstract string Crypt(string input);
        public abstract string DeCrypt(string input);
    }

    public class Algorithm 
    {

        public Dictionary<string, string> Keys;

        public Algorithm(Dictionary<string, string> keys)
        {
                this.Keys = keys;
        }


    }


    public class Kaydirmali : Algorithm, ICryptAlgorithm
    {


        public static string[] optionNames { get;  } = { "shift" };   // tüm sınıflarda bu şekilde property olarak tanımlanmalı!
        public Kaydirmali(Dictionary<string, string> keys) : base(keys) {
        
        }

        int shift => int.Parse(Keys["shift"]);

        public string Crypt(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach(char ch in input)
            {
                builder.Append(AlphabetHelper.ShiftLatter(ch, shift));
            }
            return builder.ToString();
        }

        public string DeCrypt(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char ch in input)
            {
                builder.Append(AlphabetHelper.ShiftLatter(ch, (-1 *shift)));
            }
            return builder.ToString();
        }
    }

    public class Dogrusal : Algorithm, ICryptAlgorithm
    {

        public static string[] optionNames { get; } = { "a","b" };
        public Dogrusal(Dictionary<string, string> keys) : base(keys) { }

        int a => int.Parse(Keys["a"]);
        int b => int.Parse(Keys["b"]);

        public string Crypt(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char ch in input)
            {
                int index = AlphabetHelper.GetLetterIndex(ch);
                int newIndex = ((a * index)+ b) % 29;
                builder.Append(AlphabetHelper.GetLetter(newIndex));
            }

            return builder.ToString();
        }

        public  string DeCrypt(string input)
        {
            StringBuilder builder = new StringBuilder();

            int inverseA = AlphabetHelper.ModInverse(a);

            foreach (char ch in input)
            {
                int index = AlphabetHelper.GetLetterIndex(ch);
                int newIndex = (index - b);

                newIndex = AlphabetHelper.Mod((inverseA * newIndex));

                    



                builder.Append(AlphabetHelper.GetLetter(newIndex));
            }

            return builder.ToString();
        }
    }

    public class YerDegistirme : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "RastgeleAlfabe" };

        char[] RandomAlphabet => Parse(Keys["RastgeleAlfabe"]);


        public YerDegistirme(Dictionary<string, string> keys) : base(keys) { }


        private char[] Parse(string randomAlphabet)
        {
            HashSet<char> uniqueLetters = new HashSet<char>();

            foreach(char ch in RandomAlphabet)
            {
                if (uniqueLetters.Contains(ch))
                {
                    throw new Exception("Duplicate letters in random alphabet");
                }
                else
                {
                    uniqueLetters.Add(ch);
                }
            }

            return uniqueLetters.ToArray();
        }

        public static string GetRandomRastgeleAlfabe()   // tüm sınıflar için aynı formatta isimlendirilmeli! GetRandom+DeğişkenAdı
        {
            return  AlphabetHelper.GetShuffledAlphabet();
        }

        public  string Crypt(string input) => "not implemented yet!";
        public  string DeCrypt(string input) => "not implemented yet!";


    }

    public class Permutasyon : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "BlokUzunlupu", "Sıralama" };

        int blockLenght => int.Parse(Keys["BlokUzunlupu"]);
        int[] order => Parse(Keys["Sıralama"]);

        



        public Permutasyon(Dictionary<string, string> keys) : base(keys) {
        

        }

        public  string Crypt(string input){

            if(!CheckOrder(order, blockLenght))
            {
                throw new Exception("Invalid order");
            }

            StringBuilder builder = new StringBuilder();

            List<string> blocks = TextParser.ParseTextIntoBlocks(input, blockLenght);

            List<string> result = new List<string>();

            foreach (string block in blocks)
            {
                char[] temp = new char[block.Length];
                for (int i = 0; i < order.Length; i++)
                {
                    temp[i] = block[order[i]];
                }
                result.Add(new string(temp));
            }

            foreach (string block in result)
            {
                builder.Append(block);
            }

            return builder.ToString();



        }
        public  string DeCrypt(string input)
        {
            if (!CheckOrder(order, blockLenght))
            {
                throw new Exception("Invalid order");
            }
            StringBuilder builder = new StringBuilder();

            List<string> blocks = TextParser.ParseTextIntoBlocks(input, blockLenght);

            List<string> result = new List<string>();

            foreach (string block in blocks)
            {
                char[] temp = new char[block.Length];
                for (int i = 0; i < order.Length; i++)
                {
                    temp[order[i]] = block[i];
                }
                result.Add(new string(temp));
            }
            foreach (string block in result)
            {
                builder.Append(block);
            }
            return builder.ToString();





        }

        private int[] Parse(string order)
        {
            string[] parts = order.Split(',');
            int[] result = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out int value))
                {
                    result[i] = value;
                }
                else
                {
                    throw new Exception("Invalid order format");
                }
            }
            return result;
        }

        private bool CheckOrder(int[] order, int blockLenght)
        {
            HashSet<int> uniqueNumbers = new HashSet<int>();
            
            foreach (int num in order)
            {
                if (num < 0 || num >= blockLenght || uniqueNumbers.Contains(num))
                {
                    return false;
                }
                uniqueNumbers.Add(num);
                 
            }

            for (int i = 0; i < blockLenght; i++)
            {
                if (!uniqueNumbers.Contains(i))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class SayiAnahtarli : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "StunSayisi" };

        int columnCount => int.Parse(Keys["StunSayisi"]);

        public SayiAnahtarli(Dictionary<string, string> keys) : base(keys) { }

        public  string Crypt(string input) {

            StringBuilder builder = new StringBuilder();
            List<string> blocks = TextParser.ParseTextIntoBlocks(input, columnCount);

            for (int i = 0; i < columnCount; i++)
            {
                foreach (string block in blocks)
                {
                   
                        builder.Append(block[i]);
                   
                }
            }

            return builder.ToString();
        }
        public  string DeCrypt(string input)
        {

            StringBuilder builder = new StringBuilder();
            int step = input.Length / columnCount;

            for(int i = 0; i < step; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    builder.Append(input[i + (j * step)]);
                }
            }



            return builder.ToString();
        }
    }

    public class Rota : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "StunSayisi" };

        int columnCount => int.Parse(Keys["StunSayisi"]);

        public Rota(Dictionary<string, string> keys) : base(keys) { }

        public  string Crypt(string input) => "not implemented yet!";
        public  string DeCrypt(string input) => "not implemented yet!";
    }

    public class ZikZak : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "SatirSayisi" };
        int rowCount => int.Parse(Keys["SatirSayisi"]);
        public ZikZak(Dictionary<string, string> keys) : base(keys) { }

        public  string Crypt(string input){

            StringBuilder builder = new StringBuilder();
            List<char[]> rows = new List<char[]>();
            int amount = 1;

            int index = 0;
            while(rows.Count < input.Length)
            {
                char[] row = new char[rowCount];
                row[index] = input[rows.Count];

                index += amount;

                if (index == (rowCount-1) || index == 0)
                {
                    amount *= -1;
                }
                
                rows.Add(row);
            }


            for (int i = 0; i < rowCount; i++)
            {
                foreach (char[] row in rows)
                {
                    if(row[i] != '\0')
                    {
                        builder.Append(row[i]);
                    }
                }
            }


            return builder.ToString();
        }
        public  string DeCrypt(string input)
        {
            StringBuilder builder = new StringBuilder();
            List<char[]> rows = new List<char[]>();
            int amount = 1;

            int index = 0;
            while (rows.Count < input.Length)
            {
                char[] row = new char[rowCount];
                row[index] = 'x';

                index += amount;

                if (index == (rowCount - 1) || index == 0)
                {
                    amount *= -1;
                }

                rows.Add(row);
            }

            int idx = 0;
            for (int i = 0; i < rowCount; i++)
            {
                
                foreach (char[] row in rows)
                {
                    if (row[i] == 'x')
                    {
                        row[i] = input[idx];
                        idx++;
                    }
                }
            }

            foreach (char[] row in rows)
            {
                foreach (char ch in row)
                {
                    if (ch != '\0')
                    {
                        builder.Append(ch);
                    }
                }

            }

                return builder.ToString();

        }
    }

    public class Vigenere : Algorithm, ICryptAlgorithm
    {

        public static string[] optionNames { get; } = { "AnahtarKelime" };

        string key => Keys["AnahtarKelime"];
        int keyLength => key.Length;
        public Vigenere(Dictionary<string, string> keys) : base(keys) { }

        public  string Crypt(string input) => "not implemented yet!";
        public  string DeCrypt(string input) => "not implemented yet!";
    }

    public class DortKare : Algorithm, ICryptAlgorithm
    {
        public static string[] optionNames { get; } = { "StunSayisi", "RastgeleMatris1", "RastgeleMatris2" };

        int columnCount => int.Parse(Keys["StunSayisi"]);
        string randomMatrix1 => Keys["RastgeleMatris1"];

        string randomMatrix2 => Keys["RastgeleMatris2"];

        public DortKare(Dictionary<string, string> keys) : base(keys) { }

        public  string Crypt(string input) => "not implemented yet!";
        public  string DeCrypt(string input) => "not implemented yet!";
    }

    public class Hill : Algorithm, ICryptAlgorithm
    {

        public static string[] optionNames { get; } = { "BlokUzunlugu", "AnahtarMatris" };

        int blockLenght => int.Parse(Keys["BlokUzunlugu"]);
        string inputKeyMatrix => (Keys["AnahtarMatris"]);
        public Hill(Dictionary<string, string> keys) : base(keys) { }

        public string Crypt(string input)
        {
            string[,] keyMatrix = TextParser.ParseStringToMatrix(inputKeyMatrix);

            int count = input.Length / keyMatrix.GetLength(0);
            int keyMatrixLength = keyMatrix.GetLength(0);
            int[,] keyMatrixValues = getMatrixValues(keyMatrix);

            if(calculateDeterminant(keyMatrixValues) == 0)
            {
                throw new Exception("Key matrix is not invertible");
            }

            StringBuilder cryptedTextBuilder = new StringBuilder();

            List<string> blockString = TextParser.ParseTextIntoBlocks(input, keyMatrixLength);

            for (int i = 0; i < count; i++)
            {
                string substInput = blockString[i];
                int[] indexLetter = new int[substInput.Length];

                for (int j = 0; j < keyMatrixLength; j++)
                {
                    indexLetter[j] = AlphabetHelper.GetLetterIndex(substInput[j]);
                }

                for (int l = 0; l < indexLetter.Length; l++)
                {
                    int value = 0;

                    for (int k = 0; k < indexLetter.Length; k++)
                    {
                        value += indexLetter[k] * keyMatrixValues[l, k];
                    }
                    value = AlphabetHelper.Mod(value);
                    cryptedTextBuilder.Append( AlphabetHelper.GetLetter(value));
                }
            }

            return cryptedTextBuilder.ToString();
        }

        public string DeCrypt(string input)
        {
            string[,] keyMatrix = TextParser.ParseStringToMatrix(inputKeyMatrix);
            int[,] keyMatrixValues = getMatrixValues(keyMatrix);
            int keyMatrixLength = keyMatrix.GetLength(0);
            int determinant = calculateDeterminant(keyMatrixValues);

            if(determinant == 0)
            {
                throw new Exception("Key matrix is not invertible");
            }

            int[,] minors = calculateMinors(keyMatrixValues);
            int[,] cofactorMatrix = calculateCofactorMatrix(minors);
            cofactorMatrix = transpoz(cofactorMatrix);
            int[,] invertedMatrix = calculateInvertedMatrix(cofactorMatrix, determinant);
            int count = input.Length / keyMatrix.GetLength(0);

            StringBuilder decryptedTextBuilder = new StringBuilder();

            List<string> blockString = TextParser.ParseTextIntoBlocks(input, keyMatrixLength);

            for (int i = 0; i < count; i++)
            {
                string substInput = blockString[i];
                int[] indexLetter = new int[substInput.Length];

                for (int j = 0; j < keyMatrixLength; j++)
                {
                    indexLetter[j] = AlphabetHelper.GetLetterIndex(substInput[j]);
                }

                for (int l = 0; l < indexLetter.Length; l++)
                {
                    int value = 0;
                    for (int k = 0; k < indexLetter.Length; k++)
                    {
                        value += indexLetter[k] * invertedMatrix[l, k];
                    }
                    value = AlphabetHelper.Mod(value);
                    decryptedTextBuilder.Append(AlphabetHelper.GetLetter(value));
                }
            }

            return decryptedTextBuilder.ToString();
        }
        private int[,] calculateInvertedMatrix(int[,] matrix, int determinant)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = matrix[i, j] * AlphabetHelper.ModInverse(determinant);
                    matrix[i, j] = AlphabetHelper.Mod(matrix[i, j]);
                }
            }

            return matrix;
        }

        private int[,] transpoz(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = i; j < matrix.GetLength(1); j++)
                {
                    int temp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = temp;
                }
            }

            for (int k = 0; k < matrix.GetLength(0); k++)
            {
                for (int l = 0; l < matrix.GetLength(1); l++)
                {
                    matrix[k, l] = AlphabetHelper.Mod(matrix[k, l]);
                }
            }

            return matrix;
        }
        private int[,] calculateCofactorMatrix(int[,] minors)
        {
            int n = minors.GetLength(0);
            int[,] cofactorMatrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    cofactorMatrix[i, j] = (int)Math.Pow(-1, i + j) * minors[i, j];
                }
            }
            return cofactorMatrix;
        }
        private int[,] calculateMinors(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[,] minors = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int[,] subMatrix = getSubMatrix(matrix, i, j);
                    minors[i, j] = calculateDeterminant(subMatrix);
                    minors[i, j] = AlphabetHelper.Mod(minors[i, j]);
                }
            }

            return minors;
        }

        private int[,] getSubMatrix(int[,] matrix, int rowToRemove, int colToRemove)
        {
            int n = matrix.GetLength(0);
            int[,] subMatrix = new int[n - 1, n - 1];
            int subMatrixRow = 0, subMatrixCol = 0;

            for (int i = 0; i < n; i++)
            {
                if (i == rowToRemove) continue;
                subMatrixCol = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == colToRemove) continue;
                    subMatrix[subMatrixRow, subMatrixCol] = matrix[i, j];
                    subMatrixCol++;
                }
                subMatrixRow++;
            }

            return subMatrix;
        }

        private int calculateDeterminant(int[,] matrix)
        {
            int determinant = 0;
            int n = matrix.GetLength(0);
            if (n == 1)
            {
                return matrix[0, 0];
            }
            else if (n == 2)
            {
                return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    int[,] subMatrix = new int[n - 1, n - 1];
                    for (int j = 1; j < n; j++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (k < i)
                            {
                                subMatrix[j - 1, k] = matrix[j, k];
                            }
                            else if (k > i)
                            {
                                subMatrix[j - 1, k - 1] = matrix[j, k];
                            }
                        }
                    }
                    determinant += (int)Math.Pow(-1, i) * matrix[0, i] * calculateDeterminant(subMatrix);
                }
            }
            return AlphabetHelper.Mod(determinant);
        }

        private int[,] getMatrixValues(string[,] keyMatrix)
        {
            int matrixLength = keyMatrix.GetLength(0);
            int[,] keyMatrixValues = new int[matrixLength, matrixLength];
            for (int i = 0; i < matrixLength; i++)
            {
                for (int j = 0; j < matrixLength; j++)
                {
                    int.TryParse(keyMatrix[i, j], out int parsedNumber);
                    keyMatrixValues[i, j] = parsedNumber;
                }
            }
            return keyMatrixValues;
        }
    }

}
