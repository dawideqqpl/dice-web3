using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net;
using System;
using System.Threading.Tasks;

public class PlayFabGoogleLogin : MonoBehaviour
{
    private string googleOAuthClientId = Secrets.GoogleOAuthClientId;
    private string googleOAuthRedirectUri = "https://google.com/";  // OAuth redirect URI
    private string googleOAuthToken;
    private HttpListener httpListener;
    private string authorizationCode;

    // Start the local server to listen for redirect from Google
    private void StartLocalServer()
    {
        httpListener = new HttpListener();
        httpListener.Prefixes.Add("https://google.com/");
        httpListener.Start();
        httpListener.BeginGetContext(OnRequest, null);
        Debug.Log("Local server started, waiting for authorization code...");
    }

    // This method gets called when Google redirects to localhost with the code
    private void OnRequest(IAsyncResult result)
    {
        if (httpListener == null || !httpListener.IsListening) return;

        var context = httpListener.EndGetContext(result);
        var request = context.Request;

        // Parse the 'code' parameter from the query string
        string code = request.QueryString["code"];
        if (!string.IsNullOrEmpty(code))
        {
            Debug.Log("Authorization Code received: " + code);
            authorizationCode = code;

            // Send a response to the browser
            var response = context.Response;
            string responseString = "<html><body>Login successful! You can close this window.</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                httpListener.Stop();
                Debug.Log("Local server stopped.");
            });

            // Authenticate with PlayFab
            AuthenticateWithPlayFab(authorizationCode);
        }
    }
    public void LoginWithGoogle()
    {
        string googleOAuthUrl = "https://accounts.google.com/o/oauth2/auth" +
             "?client_id=" + Secrets.GoogleOAuthClientId +
             "&redirect_uri=https://google.com/" +
             "&response_type=code" +
             "&scope=openid%20email%20profile";

        Application.OpenURL(googleOAuthUrl);
        StartLocalServer();
    }

    // Krok 1: Otw�rz przegl�dark� i zaloguj si� przez Google OAuth
  

    // Krok 3: Uwierzytelnienie w PlayFab za pomoc� tokena Google
    private void AuthenticateWithPlayFab(string googleIdToken)
    {
        var request = new LoginWithGoogleAccountRequest
        {
            CreateAccount = true,
            ServerAuthCode = googleIdToken, // Wykorzystujemy token OAuth
        };

        PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Pomy�lnie zalogowano za pomoc� konta Google! PlayFabId: " + result.PlayFabId);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("B��d logowania przez Google: " + error.GenerateErrorReport());
    }
}
