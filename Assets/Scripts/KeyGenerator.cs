using System;
using Unity;
using UnityEngine;
using System.Diagnostics;
using System.Security.Cryptography;

public class KeyGenerator : MonoBehaviour
{
    public static string GenerateAESKey()
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;  // U¿yjemy 256-bitowego klucza
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);  // Klucz jest konwertowany do Base64
        }
    }

    void Start()
    {
      //   string secretKey = GenerateAESKey();
      //   UnityEngine.Debug.Log("Wygenerowany klucz AES: " + secretKey);
    }
}
