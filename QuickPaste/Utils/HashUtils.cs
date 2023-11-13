namespace QuickPaste.Utils
{
    public class HashUtils
    {
        public static string GetHash(string input)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return System.Convert.ToBase64String(hash);
            }
        }
    }
}

