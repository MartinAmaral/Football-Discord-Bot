namespace Discord_Bot
{
    public static class StringExtension
    {
        public static string RemoveSpanish(this string text)
        {
            string newvalue = "";
            foreach (char c in text)
            {
                switch (c)
                {
                    case 'ñ':
                        newvalue += 'n';
                        continue;
                    case 'á':
                        newvalue += 'a';
                        continue;
                    case 'é':
                        newvalue += 'e';
                        continue;
                    case 'í':
                        newvalue += 'i';
                        continue;
                    case 'ó':
                        newvalue += 'o';
                        continue;
                    case 'ú':
                        newvalue += 'u';
                        continue;

                }
                newvalue += c;
            }
            return newvalue;
        }

        public static string UpperFirstLetter(this string text)
        {
            return text[..1].ToUpper() + text[1..];
        }
    }
}
