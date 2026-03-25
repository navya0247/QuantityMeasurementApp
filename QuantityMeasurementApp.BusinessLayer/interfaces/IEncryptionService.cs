namespace QuantityMeasurementApp.BusinessLayer.Interfaces
{
    /// <summary>Contract for AES-256 encryption and decryption operations.</summary>
    public interface IEncryptionService
    {
        /// <summary>Encrypts plain text using AES-256 algorithm.</summary>
        string Encrypt(string plainText);

        /// <summary>Decrypts AES-256 cipher text back to plain text.</summary>
        string Decrypt(string cipherText);
    }
}
