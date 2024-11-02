using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("This is a ransomeware sim");
        Console.Write("Enter the path of the directory you nerd");
        string directoryPath = Console.ReadLine();

        Console.Write("Enter the password so you don't lose your shit: ");
        string password = Console.ReadLine();

        EncryptFiles(directoryPath, password);
        Console.WriteLine("files encrypted");

        Console.Write("Do you want to decrypt them (yes/no)");
        if (Console.ReadLine().ToLower() == "yes")
        {
            DecryptFiles(directoryPath, password);
            Console.WriteLine("files decrypted successfully");
        }
    }

    private static void DecryptFiles(string directoryPath, string password)
    {
        foreach (var filePath in Directory.GetFiles(directoryPath))
        {
            byte[] encryptedBytes = File.ReadAllBytes(filePath);
            byte[] decryptedBytes = Decrypt(encryptedBytes, password);
            File.WriteAllBytes(filePath, decryptedBytes);
        }
    }

    private static byte[] Encrypt(byte[] data, string password)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GenerateKey(password);
            aes.GenerateIV();

            using (var ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                }
                return ms.ToArray();
            }
        }
    }

    private static byte[] Decrypt(byte[] data, string password)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = GenerateKey(password);
            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (var ms = new MemoryStream(data, iv.Length, data.Length - iv.Length))
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        cs.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }

                }
            }
        }
    }

    private static byte[] GenerateKey(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
