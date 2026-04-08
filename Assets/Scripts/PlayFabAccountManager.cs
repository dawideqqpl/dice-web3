using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using OPS.AntiCheat.Prefs;

public class PlayFabAccountManager : MonoBehaviour
{
    // UI elementy do tworzenia konta
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField userInput;

    // UI elementy do logowania
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    // UI elementy do resetowania has³a
    public TMP_InputField resetEmailInput;

    public GameObject loadingObject;

    public Toggle rememberMe;

    private void Start()
    {
        if(ProtectedPlayerPrefs.GetBool("isRememberMe") == true)
        {
            loginEmailInput.text = ProtectedPlayerPrefs.GetString("rememberEmail");
            loginPasswordInput.text = ProtectedPlayerPrefs.GetString("rememberPassword");
        }
    }
    // Metoda do tworzenia konta
    public void RegisterAccount()
    {
        loadingObject.gameObject.SetActive(true);
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        string user = userInput.text;
        // Sprawdzenie czy has³a s¹ zgodne
        if (password != confirmPassword)
        {
            loadingObject.gameObject.SetActive(false);

            PopupError._Instance.ShowError("Passwords are not the same");
         //   Debug.LogError("Has³a siê nie zgadzaj¹.");
            return;
        }

        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = user,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = true // Jeœli nie chcesz wymagaæ nazwy u¿ytkownika
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    // Sukces rejestracji konta
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        loadingObject.gameObject.SetActive(false);

        PopupError._Instance.ShowError("Account created! Now you can Sign In");

        //  Debug.Log("Konto utworzone pomyœlnie. Wys³ano email z kodem weryfikacyjnym.");
        SendAccountVerificationEmail(emailInput.text);
    }

    // B³¹d rejestracji konta
    private void OnRegisterFailure(PlayFabError error)
    {
        loadingObject.gameObject.SetActive(false);

        PopupError._Instance.ShowError("Sign Up error: " + error.GenerateErrorReport());

     //   Debug.LogError("B³¹d tworzenia konta: " + error.GenerateErrorReport());
    }

    // Metoda do logowania
    public void LoginAccount()
    {
        loadingObject.gameObject.SetActive(true);

        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            TitleId = PlayFabSettings.TitleId // Upewnij siê, ¿e TitleId jest ustawione
        };

        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnLoginFailure);
    }

    // Sukces logowania
    private void OnLoginSuccess(LoginResult result)
    {
        loadingObject.gameObject.SetActive(false);
        PopupError._Instance.ShowError("Signed in!");
        if(rememberMe.isOn == true)
        {
            ProtectedPlayerPrefs.SetBool("isRememberMe", true);
            ProtectedPlayerPrefs.SetString("rememberEmail", loginEmailInput.text);
            ProtectedPlayerPrefs.SetString("rememberPassword", loginPasswordInput.text);
        }
        if (rememberMe.isOn == false)
        {
            ProtectedPlayerPrefs.SetBool("isRememberMe", false);

        }
        // Debug.Log("Zalogowano pomyœlnie! PlayFabId: " + result.PlayFabId);
        // Mo¿esz teraz przejœæ do nastêpnej sceny lub kontynuowaæ grê
        ProtectedPlayerPrefs.SetString("EmailSignIn", loginEmailInput.text);
        ProtectedPlayerPrefs.SetString("PasswordSignIn", loginPasswordInput.text);
        SceneManager.LoadScene("GameDice");
    }

    // B³¹d logowania
    private void OnLoginFailure(PlayFabError error)
    {
        loadingObject.gameObject.SetActive(false);

        PopupError._Instance.ShowError("Sign In error: " +   error.GenerateErrorReport());

        // Debug.LogError("B³¹d logowania: " + error.GenerateErrorReport());
    }

    // Metoda do resetowania has³a
    public void ResetPassword()
    {
        string email = resetEmailInput.text;

        var passwordResetRequest = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.TitleId // Twój Title ID z PlayFab
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(passwordResetRequest, OnPasswordResetSuccess, OnPasswordResetError);
    }

    private void OnPasswordResetSuccess(SendAccountRecoveryEmailResult result)
    {
        loadingObject.gameObject.SetActive(false);

        PopupError._Instance.ShowError("Link with password reset has been sent to your email!");

        //   Debug.Log("Wys³ano email z linkiem do resetowania has³a.");
    }

    private void OnPasswordResetError(PlayFabError error)
    {
        loadingObject.gameObject.SetActive(false);

        PopupError._Instance.ShowError("Reset password error: " + error.GenerateErrorReport());

        // Debug.LogError("B³¹d resetowania has³a: " + error.GenerateErrorReport());
    }
    public void SendAccountVerificationEmail(string email)
    {
        var emailRequest = new AddOrUpdateContactEmailRequest
        {
            EmailAddress = email
        };

        PlayFabClientAPI.AddOrUpdateContactEmail(emailRequest, OnEmailVerificationSuccess, OnEmailVerificationError);
    }

    private void OnEmailVerificationSuccess(AddOrUpdateContactEmailResult result)
    {
        loadingObject.gameObject.SetActive(false);

        Debug.Log("Email u¿ytkownika zosta³ zaktualizowany.");
    }

    private void OnEmailVerificationError(PlayFabError error)
    {
       Debug.LogError("B³¹d aktualizacji emaila: " + error.GenerateErrorReport());
    }
}
