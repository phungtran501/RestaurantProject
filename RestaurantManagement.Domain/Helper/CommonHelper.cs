namespace RestaurantManagement.Domain.Helper
{
    public class CommonHelper
    {
        private static readonly Random random = new Random();
        private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@!";

        public static string GenerateCode(int length)
        {
            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = characters[random.Next(characters.Length)];
            }

            return new string(code);
        }

    }
}
