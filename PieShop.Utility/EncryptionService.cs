using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassionCare.Utility
{
    public class EncryptionService
    {
        public static string GetPrivateKey()
        {
            return "";
        }
        // define the triple des provider

        private TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();
        // define the string handler

        private UTF8Encoding m_utf8 = new UTF8Encoding();
        // define the local property arrays
        private byte[] m_key;

        private byte[] m_iv;
        // define the local key and vector byte arrays
        private readonly byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private readonly byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
        public EncryptionService()
        {
            this.m_key = key;
            this.m_iv = iv;
        }

        public byte[] Encrypt(byte[] input)
        {
            return Transform(input, m_des.CreateEncryptor(m_key, m_iv));
        }

        public byte[] Decrypt(byte[] input)
        {
            return Transform(input, m_des.CreateDecryptor(m_key, m_iv));
        }

        public string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and convert it back into byte array
            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return result;
        }

        public static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strvalue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue +strvalue);
                SHA256Managed hashstr = new();
                byte[] hash = hashstr.ComputeHash(saltedPassword);
                return $"{Convert.ToBase64String(hash)}:{saltValue}";
            }

            return EncryptValue(valueToEncrypt);
        }

        public static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            SHA256Managed hashstr = new();
            byte[] hash = hashstr.ComputeHash(saltedValue);
            string enteredValueToValidate = Convert.ToBase64String(hash);
            return encryptedDbValue.Equals(enteredValueToValidate);
        }
    }
}
