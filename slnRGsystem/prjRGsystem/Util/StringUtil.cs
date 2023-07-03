using System.Security.Cryptography;

namespace prjRGsystem.Util
{
    public static class StringUtil
    {
        public static string TransformPasswordToSha256(string password)
        {
            string originalPassword, reCodePassword;

            originalPassword = password;
            SHA256 passwordWithSha256 = SHA256.Create();
            byte[] passwordBytes = System.Text.Encoding.Default.GetBytes(originalPassword);
            byte[] hashBytes = passwordWithSha256.ComputeHash(passwordBytes);
            reCodePassword = Convert.ToBase64String(hashBytes);

            return reCodePassword;
        }
    }
}
