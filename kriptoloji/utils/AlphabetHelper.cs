﻿using System;


namespace kriptoloji
{
    public class AlphabetHelper
    {
        static Random random = new Random();


        /*
         tr - for turkish
         en - for english
        */
        
        public static string GetLetter(int index,string language = "tr" )
        {
            return Enum.GetName(GetAlphabet(language), index);

        }


        public static string ShiftLatter(char latter, int shift)
        {
            int index = GetLetterIndex(latter);
            int newIndex = (index + shift)%29;
            return GetLetter(newIndex);

        }
        public static int GetLetterIndex(char letter, string language = "tr")
        {
            return (int)Enum.Parse(GetAlphabet(language), letter.ToString());
        }

        public static string GetRandomLetter()
        {
            int index = random.Next(0, 29);
            return GetLetter(index);
        }

        public static string GetRandomSpecialLetter(string language = "tr")
        {
            return "";
        }

        public static string GetShuffledAlphabet( )
        {
            string[] letters = Enum.GetNames(typeof(TurkishLetters));
            Random random = new Random();
            for (int i = letters.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                string temp = letters[i];
                letters[i] = letters[j];
                letters[j] = temp;
            }
            return string.Join("", letters);
        }

        public static bool IsLetter(char c, string language = "tr") // büyük harf gelmeli
        {
            return Enum.IsDefined(GetAlphabet(language), c.ToString());
        }


        private static Type GetAlphabet(string language)
        {
            switch (language)
            {
                case "tr":
                    return typeof(TurkishLetters);
                case "en":
                    return typeof(EnglishLetters);
                default:
                    return null;
            }
        }

        public static int ModInverse(int a, int mod = 29)
        {
            for (int x = 1; x < mod; x++)
            {
                if ((a * x) % mod == 1)
                {
                    return x;
                }
            }
            return -1; // Inverse doesn't exist
        }

        public static int Mod(int value, int mod = 29)
        {
            return (value % mod + mod) % mod;
        }


    }
}
