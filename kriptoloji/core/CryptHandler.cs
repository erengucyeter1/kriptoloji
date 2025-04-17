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

        public String Apply(String inputText, bool cryptFlag) 
        {
            String cleanInput = TextParser.FilterText(inputText);

            if (cryptFlag)
            {
                return algorithm.Crypt(cleanInput);
                
            }
            else
            {
                return algorithm.DeCrypt(cleanInput);
            }

        }



    }
}
