using System;
using System.Collections.Generic;
using System.Reflection;


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
            return Enum.GetName(GetAlphabetType(), index);

        }


        public static string ShiftLatter(char latter, int shift)
        {
            int index = GetLetterIndex(latter);
            int newIndex = AlphabetHelper.Mod(index + shift);
            return GetLetter(newIndex);

        }
        public static int GetLetterIndex(char letter)
        {
            return (int)Enum.Parse(GetAlphabetType(), letter.ToString());
        }

        public static string GetRandomLetter()
        {
            int index = random.Next(0, 29);
            return GetLetter(index);
        }

        public static string GetNextSpecialKey(int usedCount)
        {
            if(usedCount >= GetEnumLength(typeof(ExternalLettersForTurkish))){
                return null;
            }else
            {
                return Enum.GetName(typeof(ExternalLettersForTurkish), usedCount);
            }
        }

        public static string GetShuffledAlphabet( )
        {
            string[] letters = Enum.GetNames(typeof(TurkishLetters));
            for (int i = letters.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                string temp = letters[i];
                letters[i] = letters[j];
                letters[j] = temp;
            }
            return string.Join("", letters);
        }

        public static string GetAlphabet()
        {
            string[] letters = Enum.GetNames(typeof(TurkishLetters));
            return string.Join("", letters);
        }

        public static bool IsLetter(char c) // büyük harf gelmeli
        {
            return Enum.IsDefined(GetAlphabetType(), c.ToString());
        }


        private static Type GetAlphabetType()
        {
            return typeof(TurkishLetters);
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

        public static int GetEnumLength(Type enumType)
        {
            if (enumType.IsEnum)
            {
                return Enum.GetNames(enumType).Length;
            }
            throw new ArgumentException("Provided type is not an enum.");
        }


    }
}
