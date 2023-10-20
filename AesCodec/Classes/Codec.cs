﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AesCodec.Classes {
    internal class Codec {

        private byte[] TextBytes { get; set; }
        private byte[] Password { get; set; }
        private byte[] Iv {  get; set; }

        public Codec(byte[] textBytes, byte[] password) {
            this.TextBytes = textBytes;
            this.Password = password;

            byte[] iv = new byte[16];  // 16-byte initialization vector
            new Random().NextBytes(iv);

            this.Iv = iv;
        }  

        public string Encode() {
            byte[] encodedBytes = null;

            // Set up the encryption objects
            using (Aes aes = Aes.Create()) {
                aes.Key = this.Password;
                aes.IV = this.Iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Encrypt the input plaintext using the AES algorithm
                using (ICryptoTransform encryptor = aes.CreateEncryptor()) {
                    encodedBytes = encryptor.TransformFinalBlock(this.TextBytes, 0, TextBytes.Length);
                }
            }

            string encodedText = Convert.ToBase64String(encodedBytes);

            return encodedText;
        }

        public string Decode() {
            byte[] decodedBytes = null;

            // Set up the encryption objects
            using (Aes aes = Aes.Create()) {
                aes.Key = this.Password;
                aes.IV = this.Iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Decrypt the input ciphertext using the AES algorithm
                using (ICryptoTransform decryptor = aes.CreateDecryptor()) {
                    decodedBytes = decryptor.TransformFinalBlock(this.TextBytes, 0, this.TextBytes.Length);
                }
            }

            string decodedText = Encoding.UTF8.GetString(decodedBytes);
            return decodedText;
        }
    }
}
