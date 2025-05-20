namespace Kintech.WebServices.Transversal.Security
{
    using System.Security.Cryptography;
    using System.Text;

    namespace TFC.AppEventos.Transversal.Utils
    {
        public class PasswordUtils
        {
            public static string publickey = "1234567812345678"; // 16 bytes para AES-128
            public static string secretkey = "8765432187654321"; // 16 bytes para AES-128
            public static byte[] secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
            public static byte[] publickeybyte = Encoding.UTF8.GetBytes(publickey);
            public static byte[] result;
            public static string PasswordEncoder(string password)
            {
                try
                {
                    string textToEncrypt = password;
                    byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = publickeybyte;
                        aes.IV = secretkeyByte;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                                cs.FlushFinalBlock();
                                result = ms.ToArray();
                            }
                        }
                    }
                    return Convert.ToBase64String(result);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            public static string PasswordDecoder(string encryptedPasswordBase64)
            {
                try
                {
                    // Decodificar el string Base64 a un arreglo de bytes
                    byte[] encryptedPassword = Convert.FromBase64String(encryptedPasswordBase64);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = publickeybyte;
                        aes.IV = secretkeyByte;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(encryptedPassword, 0, encryptedPassword.Length);
                                cs.FlushFinalBlock();
                                result = ms.ToArray();
                            }
                        }
                    }
                    return Encoding.UTF8.GetString(result);
                }
                catch (FormatException ex)
                {
                    throw new Exception("El formato del string proporcionado no es válido para Base64.", ex);
                }
                catch (CryptographicException ex)
                {
                    throw new Exception("Error al desencriptar la contraseña. Asegúrate de que los datos encriptados y las claves sean correctos.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            public static bool IsPasswordEncrypted(string password)
            {
                try
                {
                    byte[] decodedBytes = Convert.FromBase64String(password);
                    return decodedBytes.Length % 16 == 0;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public static string CreatePassword(int length)
            {
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                while (0 < length--)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                }

                return res.ToString();
            }
        }
    }
}