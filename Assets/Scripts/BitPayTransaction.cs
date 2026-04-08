using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BitPayTransaction : MonoBehaviour
{
    // Zmienna na tw�j klucz API BitPay
    private string apiKey = Secrets.BitPayApiKey;
    private string apiUrl = "https://bitpay.com/invoices";  // Endpoint do tworzenia faktury

    // Klasa reprezentuj�ca faktur�
    [Serializable]
    public class InvoiceData
    {
        public string price;
        public string currency;
        public string token;
    }

    // Klasa do przechwytywania odpowiedzi
    [Serializable]
    public class InvoiceResponse
    {
        public string id;
        public string url;
    }
    private void Start()
    {
        CreateInvoice(1, "USDT");
    }
    // Funkcja do tworzenia faktury
    public void CreateInvoice(float amount, string currency)
    {
        StartCoroutine(SendInvoiceRequest(amount, currency));
    }

    // Korutyna wysy�aj�ca ��danie HTTP do BitPay API
    private IEnumerator SendInvoiceRequest(float amount, string currency)
    {
        InvoiceData invoice = new InvoiceData
        {
            price = amount.ToString(),
            currency = currency,
            token = apiKey  // Tw�j klucz API do BitPay
        };

        string jsonData = JsonUtility.ToJson(invoice);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Wysy�anie zapytania
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Przetwarzanie odpowiedzi
            InvoiceResponse invoiceResponse = JsonUtility.FromJson<InvoiceResponse>(request.downloadHandler.text);
            Debug.Log("Faktura zosta�a utworzona! ID: " + invoiceResponse.id);
            Debug.Log("Link do p�atno�ci: " + invoiceResponse.url);

            // Mo�esz tu doda� logik� otwieraj�c� stron� p�atno�ci w przegl�darce:
            Application.OpenURL(invoiceResponse.url);
        }
        else
        {
            Debug.Log("B��d podczas tworzenia faktury: " + request.error);
        }
    }
}
