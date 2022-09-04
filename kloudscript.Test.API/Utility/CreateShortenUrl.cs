namespace kloudscript.Test.API.Utility
{
    public static class CreateShortenUrl
    {
        private static List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        private static List<char> characters = new List<char>()
    {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
    'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B',
    'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S',  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};
        private static string GetQueryRandom()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(64, 122)
              .Where(i => i < 123 || i > 64)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i));
            string token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));

            return token;
        } 
        public static string GetURL()
        {
            string URL = "";
            Random rand = new Random(); 
            for (int i = 0; i < 9; i++)
            {  
                int random = rand.Next(0, 3);
                if (random == 1)
                {                  
                    random = rand.Next(0, numbers.Count);
                    URL += numbers[random].ToString();
                }
                else
                {
                    random = rand.Next(0, characters.Count);
                    URL += characters[random].ToString();
                }
            }
            return "https://"+URL+".com/"+"?rdata="+ GetQueryRandom();
        }
    }
}
