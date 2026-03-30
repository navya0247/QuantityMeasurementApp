using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using SystemException = System.Exception;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>
    /// AES-256 encryption and decryption service.
    /// Key configured in appsettings.json under Encryption:Key.
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[]                     _key;
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(IConfiguration config, ILogger<EncryptionService> logger)
        {
            _logger = logger;
            string keyString = config["Encryption:Key"] ?? "QuantityApp-DefaultKey-32Chars!!";
            _key = Encoding.UTF8.GetBytes(keyString.PadRight(32).Substring(0, 32));
        }

        /// <summary>Encrypts plain text using AES-256. Returns Base64 encoded cipher.</summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.GenerateIV();

                using var encryptor    = aes.CreateEncryptor(aes.Key, aes.IV);
                using var memStream    = new MemoryStream();
                using var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
                using (var writer = new StreamWriter(cryptoStream))
                    writer.Write(plainText);

                var iv        = aes.IV;
                var encrypted = memStream.ToArray();
                var result    = new byte[iv.Length + encrypted.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                _logger.LogInformation("Text encrypted successfully.");
                return Convert.ToBase64String(result);
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Encryption failed.");
                throw;
            }
        }

        /// <summary>Decrypts Base64 encoded AES-256 cipher back to plain text.</summary>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);
                var iv         = new byte[16];
                var cipher     = new byte[fullCipher.Length - 16];
                Buffer.BlockCopy(fullCipher, 0,  iv,     0, 16);
                Buffer.BlockCopy(fullCipher, 16, cipher, 0, cipher.Length);

                using var aes          = Aes.Create();
                aes.Key                = _key;
                aes.IV                 = iv;
                using var decryptor    = aes.CreateDecryptor(aes.Key, aes.IV);
                using var memStream    = new MemoryStream(cipher);
                using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
                using var reader       = new StreamReader(cryptoStream);

                _logger.LogInformation("Text decrypted successfully.");
                return reader.ReadToEnd();
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Decryption failed.");
                throw;
            }
        }
    }
}
