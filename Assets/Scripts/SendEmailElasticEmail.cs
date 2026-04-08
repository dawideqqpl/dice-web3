using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendEmailElasticEmail : MonoBehaviour
{
    // Adres API Elastic Email
    private string apiUrl = "https://api.elasticemail.com/v2/email/send";

    // Klucz API wczytywany z Secrets.cs (patrz Secrets.example.cs — nie commituj Secrets.cs)
    private string apiKey = Secrets.ElasticEmailApiKey;

    // Funkcja wysy�aj�ca e-mail
    public void SendEmail(string to, string subject, string body)
    {
        StartCoroutine(SendEmailCoroutine(to, subject, body));
    }
    private void Start()
    {
        SendEmail("buddydiceweb3@outlook.com", "test", "hejo"   );
    }
    // Funkcja wysy�aj�ca e-mail za pomoc� Coroutine
    IEnumerator SendEmailCoroutine(string to, string subject, string body)
    {
        // Przygotowanie parametr�w do wys�ania
        WWWForm form = new WWWForm();
        form.AddField("apikey", apiKey);
        form.AddField("from", "testowapocztamalpaxd@gmail.com");  // Adres nadawcy (musi by� zweryfikowany w Elastic Email)
        form.AddField("to", to);                          // Adres odbiorcy
        form.AddField("subject", subject);                // Temat wiadomo�ci
        form.AddField("bodyText", body);                  // Tre�� wiadomo�ci (tekstowa)
        form.AddField("isTransactional", "true");         // Typ wiadomo�ci

        // Wys�anie zapytania POST do Elastic Email API
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, form);

        // Oczekiwanie na odpowied� serwera
        yield return request.SendWebRequest();

        // Sprawdzenie, czy wyst�pi� b��d
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error sending email: " + request.error);
        }
        else
        {
            Debug.Log("Email sent successfully: " + request.downloadHandler.text);
        }
    }
}
