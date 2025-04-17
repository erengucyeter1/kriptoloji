using kriptoloji.enums;
using System;


namespace kriptoloji.core
{
    internal class CryptHandler
    {

        ICryptAlgorithm algorithm;

        public CryptHandler(ICryptAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public String Apply(String inputText, bool cryptFlag, bool clearFlag) 
        {
            if (clearFlag)
            {
                inputText = TextParser.FilterText(inputText);
            }

            if (cryptFlag)
            {
                return algorithm.Crypt(inputText);
                
            }
            else
            {
                return algorithm.DeCrypt(inputText);
            }

        }



    }
}
