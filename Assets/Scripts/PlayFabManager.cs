using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using Unity.VisualScripting;
using PlayFab.ClientModels;
using OPS.AntiCheat.Field;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;
using PlayFab.Json;
using System;
using Newtonsoft.Json;
using OPS.AntiCheat.Prefs;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Nethereum.HdWallet;
using NBitcoin;
using System.IO;
using UE.Email;
using Nethereum.Web3;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Math.EC.Multiplier;
using static QRCoder.PayloadGenerator;
using Unity.Collections;
using Nethereum.Util;
using System.Globalization;

public class PlayFabManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayFabManager _Instance;


    public GameObject loadingObject;
    public ProtectedString playerId;
    public TMP_Dropdown cryptoDropdown;
    public TMP_Dropdown cryptoDropdownBSC;
    public TMP_Dropdown cryptoDropdownMATIC;

    public ProtectedFloat ETH;
    public ProtectedFloat USDT;
    public ProtectedFloat USDC;
    public ProtectedFloat FDUSD;
    public ProtectedFloat CRO;
    public ProtectedFloat BNB;
    public ProtectedFloat MATIC;
    public ProtectedFloat APE;
    public ProtectedFloat LINK;
    public ProtectedFloat SHIBA;

    public ProtectedFloat minimumBet;

    public ProtectedString currentCryptoTag;

    public ProtectedString code;

    public TMP_InputField inviteInput;
    private MyDataClass data;

    public GameObject generateObject;
    public GameObject generatedObject;
    public ProtectedString addressEVM;
    public TMP_InputField addressToSendText;
    private ProtectedString playFabSecretKey = Secrets.AesEncryptionKey; // Klucz AES z Secrets.cs

    public ProtectedString isAutomaticallyWithdrawals;

    public ProtectedString latestTXHash;

    public ProtectedString currentUsername;

    public GameObject manuallyNotificationObject;

    public ProtectedString invitedBy;
    public TextMeshProUGUI invitedByText;

    public GameObject invitedObject;
    public GameObject notInvitedObject;

    public TMP_InputField yourReferralId;

    public TextMeshProUGUI lastEarningsText;

    public TextMeshProUGUI currentInvitedPlayersText;
    public ProtectedInt16 InvitedPlayerCount;

    private void Awake()
    {
        _Instance = this;
    }

    public static string EncryptMessage(string message, string key)
    {
        byte[] keyBytes = Convert.FromBase64String(key);  // Zdekoduj klucz z Base64 na tablic� bajt�w
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.GenerateIV();  // Generujemy wektor inicjalizacyjny (IV)
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new System.IO.MemoryStream())
            {
                // Zapisz IV przed zaszyfrowanym tekstem
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(message);  // Zapisujemy zaszyfrowan� wiadomo��
                    }
                }

                // Zwracamy ca�� wiadomo�� w formacie Base64 (z IV + zaszyfrowane dane)
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    public void SetChain(string chain)
    {
        Debug.Log("WHAT CHAIN " + chain);

        if (chain == "1")
        {
            cryptoDropdown.gameObject.SetActive(true);
            cryptoDropdownBSC.gameObject.SetActive(false);
            cryptoDropdownMATIC.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(true);
            CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(false);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (ERC-20)";
        }
        if (chain == "56")
        {
            cryptoDropdown.gameObject.SetActive(false);
            cryptoDropdownBSC.gameObject.SetActive(true);
            cryptoDropdownMATIC.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(true);
            CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(false);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (BEP-20)";

        }
        if (chain == "89")
        {
            cryptoDropdown.gameObject.SetActive(false);
            cryptoDropdownBSC.gameObject.SetActive(false);
            cryptoDropdownMATIC.gameObject.SetActive(true);
            CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(false);
            CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(true);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (MATIC)";

        }
    }
    void Start()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetString("NewChain", "1");

      //  ProtectedPlayerPrefs.SetString("Account", "0xd12Af87ab6F173b715B8249507D7F071788F5713");

#endif
        currentCryptoTag = "ETH";

     if(PlayerPrefs.GetString("NewChain") == "1")
     {
         cryptoDropdown.gameObject.SetActive(true);
         cryptoDropdownBSC.gameObject.SetActive(false);
         cryptoDropdownMATIC.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(true);
         CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(false);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (ERC-20)";
            WithdrawScript._Instance.chooseCryptoTitle.text = "Choose token to withdraw (ERC-20)";
     }
     if (PlayerPrefs.GetString("NewChain") == "56")
     {
         cryptoDropdown.gameObject.SetActive(false);
         cryptoDropdownBSC.gameObject.SetActive(true);
         cryptoDropdownMATIC.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(true);
         CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(false);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (BEP-20)";

            WithdrawScript._Instance.chooseCryptoTitle.text = "Choose token to withdraw (BEP-20)";
     
     }
     if (PlayerPrefs.GetString("NewChain") == "89")
     {
         cryptoDropdown.gameObject.SetActive(false);
         cryptoDropdownBSC.gameObject.SetActive(false);
         cryptoDropdownMATIC.gameObject.SetActive(true);
         CryptoDepositHistory._Instance.cryptoDeposit.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDepositBEP.gameObject.SetActive(false);
         CryptoDepositHistory._Instance.cryptoDepositMATIC.gameObject.SetActive(true);
            CryptoWithdrawHistory._Instance.cryptoWithdraw.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.gameObject.SetActive(false);
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.gameObject.SetActive(true);
            DepositScript._Instance.chooseCryptoTitle.text = "Choose token to deposit (MATIC)";
            WithdrawScript._Instance.chooseCryptoTitle.text = "Choose token to withdraw (MATIC)";
     
     }
        loadingObject.gameObject.SetActive(true);
        Login();
    }

    public void OnChangedreferralId()
    {
        yourReferralId.text = "Your referral id:\n" + playerId;
    }
    // Update is called once per frame
    void Update()
    {

       
        if(DepositScript._Instance.DepositObject.active == true)
        {
            addressToSendText.text = "Address (send here crypto): \n" + addressEVM;

        }
        if (cryptoDropdown.gameObject.active == true)
        {


            CryptoWithdrawHistory._Instance.cryptoWithdraw.captionText.text = cryptoDropdown.captionText.text;

            cryptoDropdown.options[0].text = "ETH: " + ETH;
            cryptoDropdown.options[1].text = "USDC: " + ((float)USDC).ToString("0.00000");
            cryptoDropdown.options[2].text = "USDT: " + ((float)USDT).ToString("0.00000");
            cryptoDropdown.options[3].text = "BNB: " + BNB;
            cryptoDropdown.options[4].text = "CRO: " + ((float)CRO).ToString("0.00000");
            cryptoDropdown.options[5].text = "FDUSD: " + ((float)FDUSD).ToString("0.00000");
            cryptoDropdown.options[6].text = "LINK: " + ((float)LINK).ToString("0.00000");
            cryptoDropdown.options[7].text = "MATIC: " + ((float)MATIC).ToString("0.00000");
            cryptoDropdown.options[8].text = "APE: " + ((float)APE).ToString("0.00000");
            cryptoDropdown.options[9].text = "SHIB: " + SHIBA;

            if(currentCryptoTag == "ETH")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[0].text;
            }
            if (currentCryptoTag == "USDC")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[1].text;
            }
            if (currentCryptoTag == "USDT")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[2].text;
            }
            if (currentCryptoTag == "BNB")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[3].text;
            }
            if (currentCryptoTag == "CRO")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[4].text;
            }
            if (currentCryptoTag == "FDUSD")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[5].text;
            }
            if (currentCryptoTag == "LINK")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[6].text;
            }
            if (currentCryptoTag == "MATIC")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[7].text;
            }
            if (currentCryptoTag == "APE")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[8].text;
            }
            if (currentCryptoTag == "SHIB")
            {
                cryptoDropdown.captionText.text = cryptoDropdown.options[9].text;
            }
        }
        if (cryptoDropdownBSC.gameObject.active == true)
        {

            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.captionText.text = cryptoDropdownBSC.captionText.text;


            cryptoDropdownBSC.options[0].text = "ETH: " + ETH;
            cryptoDropdownBSC.options[1].text = "USDC: " + ((float)USDC).ToString("0.00000");
            cryptoDropdownBSC.options[2].text = "USDT: " + ((float)USDT).ToString("0.00000");
            cryptoDropdownBSC.options[3].text = "BNB: " + BNB;
            cryptoDropdownBSC.options[4].text = "FDUSD: " + ((float)FDUSD).ToString("0.00000");
            cryptoDropdownBSC.options[5].text = "LINK: " + ((float)LINK).ToString("0.00000");
            cryptoDropdownBSC.options[6].text = "MATIC: " + ((float)MATIC).ToString("0.00000");

            if (currentCryptoTag == "ETH")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[0].text;
            }
            if (currentCryptoTag == "USDC")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[1].text;
            }
            if (currentCryptoTag == "USDT")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[2].text;
            }
            if (currentCryptoTag == "BNB")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[3].text;
            }
          
            if (currentCryptoTag == "FDUSD")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[5].text;
            }
            if (currentCryptoTag == "LINK")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[6].text;
            }
            if (currentCryptoTag == "MATIC")
            {
                cryptoDropdownBSC.captionText.text = cryptoDropdownBSC.options[7].text;
            }
          
        }
        if (cryptoDropdownMATIC.gameObject.active == true)
        {

            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.captionText.text = cryptoDropdownMATIC.captionText.text;

            cryptoDropdownMATIC.options[0].text = "USDC: " + ((float)USDC).ToString("0.00000");
            cryptoDropdownMATIC.options[1].text = "USDT: " + ((float)USDT).ToString("0.00000");
            cryptoDropdownMATIC.options[2].text = "LINK: " + ((float)LINK).ToString("0.00000");
            cryptoDropdownMATIC.options[3].text = "MATIC: " + ((float)MATIC).ToString("0.00000");



            if (currentCryptoTag == "USDC")
            {
                cryptoDropdownMATIC.captionText.text = cryptoDropdownMATIC.options[0].text;
            }
            if (currentCryptoTag == "USDT")
            {
                cryptoDropdownMATIC.captionText.text = cryptoDropdownMATIC.options[1].text;
            }
       
            if (currentCryptoTag == "LINK")
            {
                cryptoDropdownMATIC.captionText.text = cryptoDropdownMATIC.options[2].text;
            }
            if (currentCryptoTag == "MATIC")
            {
                cryptoDropdownMATIC.captionText.text = cryptoDropdownMATIC.options[3].text;
            }
          
           
        }


    }
    private void GetReferralStats()
    {
        var request = new PlayFab.ClientModels.GetPlayerStatisticsRequest
        {
            StatisticNames = new System.Collections.Generic.List<string> { "ReferralAP", "ReferralBN", "ReferralCR", "ReferralET", "ReferralFD", "ReferralLI", "ReferralSH", "ReferralUC", "ReferralUT", "ReferraMA" }
        };

        PlayFabClientAPI.GetPlayerStatistics(request, OnGetStatisticsSuccess, OnGetStatisticsFailure);
    }

    // Funkcja wywo�ywana przy pomy�lnym pobraniu statystyk
    private void OnGetStatisticsSuccess(PlayFab.ClientModels.GetPlayerStatisticsResult result)
    {
        ProtectedFloat ReferralAP = 0;
        ProtectedFloat ReferralBN = 0;
        ProtectedFloat ReferralCR = 0;
        ProtectedFloat ReferralET = 0;
        ProtectedFloat ReferralFD = 0;
        ProtectedFloat ReferralLI = 0;
        ProtectedFloat ReferralSH = 0;
        ProtectedFloat ReferralUC = 0;
        ProtectedFloat ReferralUT = 0;
        ProtectedFloat ReferraMA = 0;


        foreach (var stat in result.Statistics)
        {
            if (stat.StatisticName == "ReferralAP")
            {
                ReferralAP = ((ProtectedFloat)stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferralBN")
            {
                ReferralBN =( stat.Value / 100000000);
            }
            else if (stat.StatisticName == "ReferralCR")
            {
                ReferralCR = (stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferralET")
            {
                Debug.Log("stat " +stat.Value);
                ReferralET = ((ProtectedFloat)stat.Value / 100000000);
                Debug.Log(ReferralET);

            }
            else if (stat.StatisticName == "ReferralFD")
            {
                ReferralFD = (stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferralLI")
            {
                ReferralLI =( stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferralSH")
            {
                ReferralSH = stat.Value;
            }
            else if (stat.StatisticName == "ReferralUC")
            {
                ReferralUC = (stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferralUT")
            {
                ReferralUT = (stat.Value / 100000);
            }
            else if (stat.StatisticName == "ReferraMA")
            {
                ReferraMA =(stat.Value / 100000);
            }
        }

        lastEarningsText.text = "Earnings from last 7 days:\n" + "APE: " + ((float)ReferralAP).ToString("0.0000") + "\nBNB: " + ((float)ReferralBN).ToString("0.000000") + "\nCRO: " + ((float)ReferralCR).ToString("0.0000") + "\nETH: " +((float) ReferralET).ToString("0.0000000") + "\nFDUSD: " + ((float)ReferralFD).ToString("0.0000") + "\nLINK: " + ((float)ReferralLI).ToString("0.0000") + "\nSHIBA: " + ReferralSH + "\nUSDC: " + ((float)ReferralUC).ToString("0.0000") + "\nUSDT: " + ((float)ReferralUT).ToString("0.0000") + "\nMATIC: " + ((float)ReferraMA).ToString("0.0000");
    }

    // Funkcja wywo�ywana w przypadku b��du pobierania statystyk
    private void OnGetStatisticsFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get statistics: " + error.GenerateErrorReport());
    }
    public void Login()
    {
        // WebGLSendContractExample._Instance.loadingObject.gameObject.SetActive(true);
        // wallet = ProtectedPlayerPrefs.GetString("Account");
        loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Loading...";

        loadingObject.gameObject.SetActive(true);




        var request = new LoginWithEmailAddressRequest()
        {
            // CustomId = "test",
           
            Email = ProtectedPlayerPrefs.GetString("EmailSignIn"),
            Password = ProtectedPlayerPrefs.GetString("PasswordSignIn"),
            
        //    CustomId = "test",
        // CustomId = SystemInfo.deviceUniqueIdentifier,

        TitleId = "58B7E",
            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true },
           // CreateAccount = true // Tworzy nowe konto, je�li DisplayID nie zosta� wcze�nisej u�yty
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

    }

  
    void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest()
        {
            PlayFabId = PlayFabSettings.staticPlayer.PlayFabId,  // ID gracza (mo�e by� null je�li chcesz pobra� dane dla zalogowanego gracza)
            Keys = new List<string> { "AddressEVM", "InviterId" , "InvitedPlayerIds" }  // Wskazujemy klucz, kt�ry nas interesuje
        },
        result =>
        {
            loadingObject.gameObject.SetActive(false) ;

            if (!result.Data.ContainsKey("InviterId"))
            {
                invitedObject.gameObject.SetActive(false);
                notInvitedObject.gameObject.SetActive(true);

            }
            if (!result.Data.ContainsKey("InvitedPlayerIds"))
            {
                currentInvitedPlayersText.text = "Currently invited players: 0";

            }
            if (result.Data.ContainsKey("InvitedPlayerIds"))
            {
                string invitedPlayerIds = result.Data["InvitedPlayerIds"].Value;

                // Podzia� listy zaproszonych graczy po przecinku
                string[] invitedPlayers = invitedPlayerIds.Split(',');

                // Liczba zaproszonych os�b to d�ugo�� tablicy
                InvitedPlayerCount = (ProtectedInt16)invitedPlayers.Length;

                currentInvitedPlayersText.text = "Currently invited players: " + InvitedPlayerCount;


            }
            if (result.Data.ContainsKey("InviterId"))
            {
                invitedObject.gameObject.SetActive(true);
                notInvitedObject.gameObject.SetActive(false);
                invitedBy = result.Data["InviterId"].Value;
                invitedByText.text = "You are invited by:\n" + invitedBy;

            }

            if (result.Data != null && result.Data.ContainsKey("AddressEVM"))
            {
                string addressEVMString = result.Data["AddressEVM"].Value;
                if (addressEVMString != null && addressEVMString.Length > 0)
                {
                   addressEVM = result.Data["AddressEVM"].Value;

                  //  addressEVM = DecryptMessage(result.Data["AddressEVM"].Value, playFabSecretKey);
                    generateObject.gameObject.SetActive(false);
                    generatedObject.gameObject.SetActive(true);
                    QRCodeGenerator._Instance.GenerateQR(addressEVM);
                    // WalletManager._Instance.CheckInitialBalance();
                    WalletManager._Instance.StartCoroutine(WalletManager._Instance.TestWebRequest2());
                }

                if(addressEVMString == null || addressEVMString.Length <= 0)
                {
                    addressEVM = "No address";
                    generateObject.gameObject.SetActive(true);
                    generatedObject.gameObject.SetActive(false);
                }
            }
            else
            {
               
                    addressEVM = "No address";
                    generateObject.gameObject.SetActive(true);
                    generatedObject.gameObject.SetActive(false);
                
            }
        },
        error =>
        {
            Debug.LogError("B��d przy pobieraniu danych gracza: " + error.GenerateErrorReport());
        });
    }
    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new PlayFab.ClientModels.GetTitleDataRequest(), OnGetTitleDataSuccess, OnGetTitleDataFailure);
    }

    private void OnGetTitleDataSuccess(PlayFab.ClientModels.GetTitleDataResult result)
    {
        //loadingObject.gameObject.SetActive(false);

        if (result.Data == null)
        {
            Debug.Log("Brak Title Data");
            return;
        }

        string minimumBetValue;
        string bnbValue;
        string ethValue;
        string shibValue;
        string maticValue;
        string linkValue;
        string apeValue;
        string croValue;
        string automaticallyWithdrawals;

 if (result.Data.TryGetValue("AutomaticallyWithdrawals", out automaticallyWithdrawals))
        {

            isAutomaticallyWithdrawals = automaticallyWithdrawals;
        }
        if (result.Data.TryGetValue("MinimumBet", out minimumBetValue))
        {
           
            minimumBet = (ProtectedFloat)float.Parse(minimumBetValue, new CultureInfo("pl-PL"));
        }
        CryptoPriceReader._Instance.usdcPrice = 1;
        CryptoPriceReader._Instance.usdtPrice = 1;
        CryptoPriceReader._Instance.fdusdPrice = 1;
        if (result.Data.TryGetValue("BNB", out bnbValue))
        {
            Debug.Log("BNB VALUE " + bnbValue);
            CryptoPriceReader._Instance.bnbPrice = (ProtectedFloat)float.Parse(bnbValue, new CultureInfo("pl-PL"));
        }
        if (result.Data.TryGetValue("ETH", out ethValue))
        {

            CryptoPriceReader._Instance.ethPrice = float.Parse(ethValue, new CultureInfo("pl-PL"));
        }
        if (result.Data.TryGetValue("SHIB", out shibValue))
        {

            CryptoPriceReader._Instance.shibPrice = float.Parse(shibValue, new CultureInfo("pl-PL"));
        }
        if (result.Data.TryGetValue("MATIC", out maticValue))
        {

            CryptoPriceReader._Instance.maticPrice = float.Parse(maticValue, new CultureInfo("pl-PL"));
        }
       
        if (result.Data.TryGetValue("LINK", out linkValue))
        {

            CryptoPriceReader._Instance.linkPrice = float.Parse(linkValue, new CultureInfo("pl-PL"));
        }
        if (result.Data.TryGetValue("APE", out apeValue))
        {

            CryptoPriceReader._Instance.apePrice = float.Parse(apeValue, new CultureInfo("pl-PL"));
        }
        if (result.Data.TryGetValue("CRO", out croValue))
        {

            CryptoPriceReader._Instance.cronosPrice = float.Parse(croValue, new CultureInfo("pl-PL"));
        }
        CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice("ETH"));

    }

    private void OnGetTitleDataFailure(PlayFabError error)
    {
        Debug.LogError("B��d pobierania Title Data: " + error.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult result)
    {
       GetCurrencyBalance();
        playerId = result.PlayFabId;
        yourReferralId.text = "Your referral id:\n" + playerId;

        GetUserData();
        Debug.Log("Logged in");
        //  RecentBets._Instance.LoadLastBetsFromPlayFab();
        RecentBets._Instance.StartCoroutine(RecentBets._Instance.LoadBetsPeriodically());
        // playerId = result.PlayFabId;
        GetTitleData();
        // result.InfoResultPayload.PlayerProfile.
        GetReferralStats();
        GetPlayerUsername(playerId);

    }
    private void GetPlayerUsername(string playFabId)
    {
        var request = new GetAccountInfoRequest
        {
            PlayFabId = playFabId // Wymagany PlayFabID, kt�ry otrzymali�my podczas logowania
        };

        // Wywo�anie pobierania danych gracza
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    // Funkcja wywo�ywana, gdy pobranie danych gracza si� powiedzie
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        // Pobranie nazwy u�ytkownika
        string username = result.AccountInfo.Username;

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("Gracz nie ma przypisanej nazwy u�ytkownika.");
        }
        else
        {
      
           currentUsername =   username;
        }

        Debug.Log("CURRENT USERNAME " + currentUsername);

    }

    // Funkcja wywo�ywana, gdy pobranie danych gracza zako�czy si� b��dem
    private void OnGetAccountInfoFailure(PlayFabError error)
    {
       
        Debug.LogError("Pobieranie danych gracza nie powiod�o si�: " + error.GenerateErrorReport());
    }
    public void GetCurrencyBalance()
    {
        PlayFabClientAPI.GetUserInventory(new PlayFab.ClientModels.GetUserInventoryRequest(), OnGetCurrencySuccess, OnGetCurrencyFailure);
    }
    private void OnGetCurrencySuccess(PlayFab.ClientModels.GetUserInventoryResult result)
    {
      //  loadingObject.gameObject.SetActive(false);

        if (result.VirtualCurrency.ContainsKey("AP"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            APE = ((ProtectedFloat)result.VirtualCurrency["AP"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("BN"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            BNB = ((ProtectedFloat)result.VirtualCurrency["BN"] / 100000000);

        }
        if (result.VirtualCurrency.ContainsKey("CR"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");

            
            CRO = ((ProtectedFloat)result.VirtualCurrency["CR"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("ET"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            ETH = ((ProtectedFloat)result.VirtualCurrency["ET"] / 100000000);
       

        }
        if (result.VirtualCurrency.ContainsKey("FD"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            FDUSD = ((ProtectedFloat)result.VirtualCurrency["FD"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("LI"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            LINK = ((ProtectedFloat)result.VirtualCurrency["LI"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("MA"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            MATIC = ((ProtectedFloat)result.VirtualCurrency["MA"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("SH"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            SHIBA = (ProtectedFloat)result.VirtualCurrency["SH"];

        }
        if (result.VirtualCurrency.ContainsKey("UC"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            USDC = ((ProtectedFloat)result.VirtualCurrency["UC"] / 100000);

        }
        if (result.VirtualCurrency.ContainsKey("UT"))
        {
            //     coins = result.VirtualCurrency["CO"];

            //    poodleText.text = ((double)coins).ToString("N0");


            USDT = ( (ProtectedFloat)result.VirtualCurrency["UT"] / 100000);

        }

        CryptoPriceReader._Instance.CalculateShibForTwoDollars(CryptoPriceReader._Instance.currentTag);

        // cryptoDropdown.value = 0;

    }
    string GetCurrencyBeforeColon(string input)
    {
        int colonIndex = input.IndexOf(":");
        if (colonIndex > 0)
        {
            return input.Substring(0, colonIndex).Trim();
        }
        return input; // Je�li nie ma dwukropka, zwraca ca�y tekst
    }
    public void changeCurrency()
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            CryptoDepositHistory._Instance.cryptoDeposit.value = cryptoDropdown.value;
            CryptoWithdrawHistory._Instance.cryptoWithdraw.value = cryptoDropdown.value;
            currentCryptoTag = GetCurrencyBeforeColon(cryptoDropdown.options[cryptoDropdown.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(cryptoDropdown.options[cryptoDropdown.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            CryptoDepositHistory._Instance.cryptoDepositBEP.value = cryptoDropdownBSC.value;
            CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value = cryptoDropdownBSC.value;

            currentCryptoTag = GetCurrencyBeforeColon(cryptoDropdownBSC.options[cryptoDropdownBSC.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(cryptoDropdownBSC.options[cryptoDropdownBSC.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            CryptoDepositHistory._Instance.cryptoDepositMATIC.value = cryptoDropdownMATIC.value;
            CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value = cryptoDropdownMATIC.value;

            currentCryptoTag = GetCurrencyBeforeColon(cryptoDropdownMATIC.options[cryptoDropdownMATIC.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(cryptoDropdownMATIC.options[cryptoDropdownMATIC.value].text)));
        }
      
        //  Debug.Log("VALUE " + cryptoDropdown.options[cryptoDropdown.value].text);
    }
    public void changeCurrencyDeposit()
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            cryptoDropdown.value = CryptoDepositHistory._Instance.cryptoDeposit.value;
            currentCryptoTag = GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            cryptoDropdownBSC.value = CryptoDepositHistory._Instance.cryptoDepositBEP.value;

            currentCryptoTag = GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            cryptoDropdownMATIC.value = CryptoDepositHistory._Instance.cryptoDepositMATIC.value;

            currentCryptoTag = GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text)));
        }

        //  Debug.Log("VALUE " + cryptoDropdown.options[cryptoDropdown.value].text);
    }
    public void changeCurrencyWithdraw()
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            cryptoDropdown.value = CryptoWithdrawHistory._Instance.cryptoWithdraw.value;
            currentCryptoTag = GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            cryptoDropdownBSC.value = CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value;
            currentCryptoTag = GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text)));
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            cryptoDropdownMATIC.value = CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value;
            currentCryptoTag = GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text);
            CryptoPriceReader._Instance.StartCoroutine(CryptoPriceReader._Instance.GetCryptoPrice(GetCurrencyBeforeColon(CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text)));
        }

        //  Debug.Log("VALUE " + cryptoDropdown.options[cryptoDropdown.value].text);
    }
    private void OnGetCurrencyFailure(PlayFabError error)
    {
        Debug.Log("Wyst�pi� problem podczas pobierania stanu waluty: " + error.GenerateErrorReport());
    }
    private void OnLoginFailure(PlayFabError error)
    {
        loadingObject.gameObject.SetActive(false);

        Debug.Log("Login issue: " + error.ErrorMessage);
    }

    public void CallBetResultCloudScript(string betAmount, string multiplier, string chance, string isWin, string profit, string crypto)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "BetResult", // Nazwa CloudScript, kt�ry wywo�ujemy
            FunctionParameter = new
            {
                betAmount = betAmount,
                multiplier = multiplier,
                chance = chance,
                isWin = isWin,
                profit = profit,
                crypto = crypto
            },
            GeneratePlayStreamEvent = true // Mo�esz ustawi� na false, je�li nie potrzebujesz tego
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptFailure);
    }

    // Funkcja wywo�ywana po udanym wykonaniu CloudScript
    private void OnCloudScriptSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {
        Debug.Log("CloudScript executed successfully!");
        if (result.FunctionResult != null)
        {
          //  UIManager._Instance.betButton.interactable = true;
         //   Debug.Log("Updated Key: " + result.FunctionResult.GetType().GetProperty("updatedKey")?.GetValue(result.FunctionResult, null));
        }

        if (result.Error != null)
        {
            Debug.LogWarning("CloudScript Error: " + result.Error.Message);
        }
    }

    // Funkcja wywo�ywana w przypadku niepowodzenia
    private void OnCloudScriptFailure(PlayFabError error)
    {
        Debug.LogError("Error executing CloudScript: " + error.GenerateErrorReport());
    }

    public void ExecuteCloudScriptBetSetCodeAutomatic(decimal valueToUpdate2, decimal betAmount2, string multiplier2, float chance2, string crypto2, bool isWinner2, decimal winAmount2, float roll2, decimal profit2, decimal profitFull)
    {

        //Debug.Log("TO UPDATE " + ((valueToUpdate * CryptoPriceReader._Instance.currentMultiplier)));
        if (currentUsername == "")
        {
            currentUsername = "Undefined";
        }
        Debug.Log(betAmount2 + " " + multiplier2 + " " + chance2 + " " + crypto2 + " " + isWinner2 + " " + winAmount2 + " " + roll2 + " " + profit2);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            //FunctionParameter = new { Value = ((valueToUpdate * CryptoPriceReader._Instance.currentMultiplier)).ToString(), Currency = currentCryptoTag.ToString() },
            //FunctionParameter = new { Value = ((profit * CryptoPriceReader._Instance.currentMultiplier)).ToString(), Currency = currentCryptoTag.ToString() },
         
            FunctionParameter = new
            {
                betAmount = betAmount2.ToString(),
                multiplier = multiplier2.ToString(),
                chance = chance2.ToString(),
                isWin = isWinner2.ToString(),
                //profit = (profit2 * CryptoPriceReader._Instance.currentMultiplier).ToString(),
                profit = (profit2 * CryptoPriceReader._Instance.currentMultiplier).ToString(),
                crypto = crypto2.ToString(),
                roll = roll2.ToString("0.00"),
                wallet = (string)currentUsername,
            },
            //    FunctionParameter = new { Value = webgl.ToString("F0") },
            FunctionName = "SetCodeBet",
            GeneratePlayStreamEvent = true,

        },
        cloudResult => {


            JsonObject jsonResult = (JsonObject)cloudResult.FunctionResult;
            object valueObject;
            object valueObject2;
            object valueObject3;
            ProtectedDecimal DepositValue = 0;
            ProtectedString CodeValue = "";
            ProtectedString CurrencyValue = "";



            if (jsonResult.TryGetValue("Code", out valueObject2))
            {


                CodeValue = Convert.ToString(valueObject2);


            }
            if (jsonResult.TryGetValue("Data", out valueObject3))
            {


                CurrencyValue = Convert.ToString(valueObject3);
                data = JsonConvert.DeserializeObject<MyDataClass>(CurrencyValue);

            }


            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) == DepositValue)
            //  {
            //      code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
            //      AddDeposit();
            //  }
            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) != DepositValue)
            //  {
            //
            //
            //  }


            if (((profit2 * CryptoPriceReader._Instance.currentMultiplier)).ToString() == decimal.Parse(data.profit).ToString() && data.wallet == (string)currentUsername && betAmount2.ToString() == data.betAmount.ToString() && multiplier2.ToString() == data.multiplier.ToString() && isWinner2 == bool.Parse(data.isWin) && roll2.ToString("0.00") == ((float.Parse(data.roll)).ToString("0.00")))
            {
                code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
                AddBetAutomatic(betAmount2, multiplier2, chance2, crypto2, isWinner2, winAmount2, roll2, profit2, valueToUpdate2, profitFull);
            }
            //    if ((Mathf.RoundToInt(((float)((float.Parse(WebGLSendContractExample._Instance.depositInput.text) * tokenPrice) * 4)))) != DepositValue)
            //   {
            //

            //   }


            //  }
        },
        cloudError =>
        {
            Debug.Log("Error");

        }); ;
    }


    public void ExecuteCloudScriptBetSetCode(decimal valueToUpdate2, decimal betAmount2, string multiplier2, float chance2, string crypto2, bool isWinner2, decimal winAmount2, float roll2, decimal profit2)
    {

        //Debug.Log("TO UPDATE " + ((valueToUpdate * CryptoPriceReader._Instance.currentMultiplier)));
        if (currentUsername == "")
        {
            currentUsername = "Undefined";
        }
        Debug.Log(betAmount2 + " " + multiplier2 + " " + chance2 + " " + crypto2 + " " + isWinner2 + " " + winAmount2 + " " + roll2 + " " + profit2);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            //FunctionParameter = new { Value = ((valueToUpdate * CryptoPriceReader._Instance.currentMultiplier)).ToString(), Currency = currentCryptoTag.ToString() },
            //FunctionParameter = new { Value = ((profit * CryptoPriceReader._Instance.currentMultiplier)).ToString(), Currency = currentCryptoTag.ToString() },
          
            FunctionParameter = new
            {
                betAmount = betAmount2.ToString(),
                multiplier = multiplier2.ToString(),
                chance = chance2.ToString(),
                isWin = isWinner2.ToString(),
                profit = (profit2 * CryptoPriceReader._Instance.currentMultiplier).ToString(),
                crypto = crypto2.ToString(),
                roll = roll2.ToString("0.00"),
                wallet = (string)currentUsername,
            },
            //    FunctionParameter = new { Value = webgl.ToString("F0") },
            FunctionName = "SetCodeBet",
            GeneratePlayStreamEvent = true,

        },
        cloudResult => {


            JsonObject jsonResult = (JsonObject)cloudResult.FunctionResult;
            object valueObject;
            object valueObject2;
            object valueObject3;
            ProtectedDecimal DepositValue = 0;
            ProtectedString CodeValue = "";
            ProtectedString CurrencyValue = "";


          
            if (jsonResult.TryGetValue("Code", out valueObject2))
            {


                CodeValue = Convert.ToString(valueObject2);


            }
            if (jsonResult.TryGetValue("Data", out valueObject3))
            {


                CurrencyValue = Convert.ToString(valueObject3);
                data = JsonConvert.DeserializeObject<MyDataClass>(CurrencyValue);

            }


            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) == DepositValue)
            //  {
            //      code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
            //      AddDeposit();
            //  }
            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) != DepositValue)
            //  {
            //
            //
            //  }


            if (((profit2 * CryptoPriceReader._Instance.currentMultiplier)).ToString() == decimal.Parse(data.profit).ToString() && data.wallet == (string)currentUsername &&  betAmount2.ToString() == data.betAmount.ToString() && multiplier2.ToString() == data.multiplier.ToString() && isWinner2 == bool.Parse(data.isWin) && roll2.ToString("0.00") == ((float.Parse(data.roll)).ToString("0.00")))
            {
                code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
                AddBet(betAmount2, multiplier2, chance2, crypto2, isWinner2, winAmount2, roll2, profit2, valueToUpdate2);
            }
            //    if ((Mathf.RoundToInt(((float)((float.Parse(WebGLSendContractExample._Instance.depositInput.text) * tokenPrice) * 4)))) != DepositValue)
            //   {
            //

            //   }


            //  }
        },
        cloudError =>
        {
            Debug.Log("Error");

        }); ;
    }

    public void SetInvite()
    {





        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { inviterId = inviteInput.text.ToString() },
            FunctionName = "setInviter",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {




          if (cloudResult.FunctionResult == null || cloudResult.FunctionResult.ToString() == "" || cloudResult.FunctionResult.ToString() == "Inviter does not exist")
          {
              Debug.Log("INVITER ERROR");
              return;
          }
          if (cloudResult.FunctionResult.ToString() == "Inviter set successfully")
              {
                  invitedBy = inviteInput.text.ToString();
                  invitedByText.text = "You are invited by:\n" + invitedBy;
                  invitedObject.gameObject.SetActive(true);
                  notInvitedObject.gameObject.SetActive(false);

              }

          
      

      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }
    public static string DecryptMessage(string encryptedMessage, string key)
    {
        byte[] keyBytes = Convert.FromBase64String(key);
        byte[] fullCipher = Convert.FromBase64String(encryptedMessage);

        using (Aes aesAlg = Aes.Create())
        {
            byte[] iv = new byte[aesAlg.BlockSize / 8];  // Odczytujemy IV (16 bajt�w dla AES-256)
            byte[] cipherText = new byte[fullCipher.Length - iv.Length];

            // Pobieramy IV z pocz�tku pe�nego szyfru
            Array.Copy(fullCipher, iv, iv.Length);
            // Pobieramy zaszyfrowan� wiadomo��
            Array.Copy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

            aesAlg.Key = keyBytes;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipherText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Odszyfruj wiadomo�� i zwr�� j� jako string
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
    public void SetWallet()
    {
        loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Loading...";

        loadingObject.gameObject.SetActive(true);
        Wallet wallet = new Wallet(Wordlist.English, WordCount.Twelve);
       ProtectedString  privateKey = wallet.GetAccount(0).PrivateKey;
        ProtectedString publicKey = wallet.GetAccount(0).Address;

     //   Debug.Log("Generated Wallet Address: " + publicKey);
       // Debug.Log("Private Key (store securely!): " + privateKey);
      //  ProtectedString address = EncryptMessage(publicKey, playFabSecretKey);
        ProtectedString privateKeyEncrypted = EncryptMessage(privateKey, playFabSecretKey);



        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Address = publicKey.ToString(), PrivateKey  = privateKeyEncrypted.ToString()},
            FunctionName = "SetWallet",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {


          if (cloudResult.FunctionResult.ToString() == "Success")
          {
              generateObject.gameObject.SetActive(false);
              generatedObject.gameObject.SetActive(true);
              loadingObject.gameObject.SetActive(false);
              QRCodeGenerator._Instance.GenerateQR(publicKey);
              addressEVM = publicKey;


              if (PlayerPrefs.GetString("NewChain") == "89")
              {

                 WalletManager._Instance.rpcUrl = "https://polygon-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
              }
              if (PlayerPrefs.GetString("NewChain") == "56")
              {
                  WalletManager._Instance.rpcUrl = "https://bsc-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
              }
              if (PlayerPrefs.GetString("NewChain") == "1")
              {
                  WalletManager._Instance.rpcUrl = "https://mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
              }
              WalletManager._Instance.web3 = new Web3(WalletManager._Instance.rpcUrl);
         //     WalletManager._Instance.CheckInitialBalance();
              WalletManager._Instance.StartCoroutine(WalletManager._Instance.TestWebRequest2());

          }


      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }
    public void AddBet(decimal betAmount, string multiplier, float chance, string crypto, bool isWinner, decimal winAmount, float roll, decimal profit, decimal valueToUpdate)
    {



        // loadingObject.gameObject.SetActive(true);


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Code = code.ToString() },
            FunctionName = "AddBetCurrency",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {


          if (cloudResult.FunctionResult.ToString() == "Success")
          {
            UIManager._Instance.chanceSlider.interactable = true;

              UIManager._Instance.winNumber = roll;
              UIManager._Instance.UpdateSliderResultArrow(roll);
              RecentGames recentGames = FindObjectOfType<RecentGames>();
              recentGames.AddGame(System.DateTime.Now.ToString("HH.mm"), betAmount, float.Parse(multiplier), chance, isWinner, (decimal)profit);
              BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
              betResultPopup.ShowResultPopup(isWinner, betAmount, (decimal)profit);
              StatisticsManager._Instance.UpdateStatistics(betAmount, profit, isWinner);
          //    PlayFabManager._Instance.CallBetResultCloudScript(betAmount.ToString(), multiplier.ToString(), chance.ToString(), isWinner.ToString(), profit.ToString(), crypto.ToString());
              GetCurrencyBalance();
              UIManager._Instance.UpdateBalance(valueToUpdate);
              //  UIManager._Instance.betButton.interactable = true;
              StartCoroutine(DelayAfterBet());
              //  loadingObject.gameObject.SetActive(false);
              // GetPlayerItem();

          }


      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }
    public void AddBetAutomatic(decimal betAmount, string multiplier, float chance, string crypto, bool isWinner, decimal winAmount, float roll, decimal profit, decimal valueToUpdate, decimal profitFull)
    {



        // loadingObject.gameObject.SetActive(true);


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Code = code.ToString() },
            FunctionName = "AddBetCurrency",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {


          if (cloudResult.FunctionResult.ToString() == "Success")
          {


              AutoBetManager._Instance.UpdateSliderResultArrow();
              RecentGames recentGames = FindObjectOfType<RecentGames>();
              recentGames.AddGame(System.DateTime.Now.ToString("HH.mm"), betAmount, float.Parse(multiplier), chance, isWinner, (decimal)profit);
              BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
              betResultPopup.ShowResultPopup(isWinner, betAmount, (decimal)profit);
            //  StatisticsManager._Instance.UpdateStatistics(betAmount, winAmount, isWinner);
              StatisticsManager._Instance.UpdateStatistics(betAmount, profit, isWinner);
              //    PlayFabManager._Instance.CallBetResultCloudScript(betAmount.ToString(), multiplier.ToString(), chance.ToString(), isWinner.ToString(), profit.ToString(), crypto.ToString());
              GetCurrencyBalance();


             // UIManager._Instance.UpdateBalance(valueToUpdate);













              AutoBetManager._Instance.currentBetCount += 1;


              if (AutoBetManager._Instance.currentBetCount < int.Parse(AutoBetManager._Instance.numberOfBetsInput.text))
              {
                  if (profitFull <= (-1 * decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text)) && decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) != 0)
                  {


                      AutoBetManager._Instance.isAutoBetting = false;
                      return;

                  }
                  if (profitFull > (-1 *decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text)) || decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) == 0)
                  {

                      if (decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) == 0)
                      {


                          StartCoroutine(DelayAfterAutomaitcBet());
                          return;
                      }
                      if (decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) != 0)
                      {



                          if (profitFull >= decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) && decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) != 0)
                          {

                              AutoBetManager._Instance.isAutoBetting = false;


                              return;
                          }

                          if (profitFull < decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) || decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) == 0)
                          {
                              StartCoroutine(DelayAfterAutomaitcBet());
                              return;

                          }

                      }

                  }
              

                  if (profitFull >= decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) && decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) != 0)
                  {
                      AutoBetManager._Instance.isAutoBetting = false;
                      return;

                  }
                  if (profitFull < decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) || decimal.Parse(AutoBetManager._Instance.stopOnProfitInput.text) == 0)
                  {


                      if(decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) == 0){


                            StartCoroutine(DelayAfterAutomaitcBet());
                            return;
                     }


                      if (decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) != 0)
                      {
                          if (profitFull <= (-1 * decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text)) && decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) != 0)
                          {
                              AutoBetManager._Instance.isAutoBetting = false;
                              return;
                          }


                          if (profitFull > (-1 *decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text)) || decimal.Parse(AutoBetManager._Instance.stopOnLossInput.text) == 0)
                          {
                              StartCoroutine(DelayAfterAutomaitcBet());
                              return;

                          }

                      }


                  }





                  }
              if (AutoBetManager._Instance.currentBetCount >= int.Parse(AutoBetManager._Instance.numberOfBetsInput.text))
              {
                  // AutoBetManager._Instance.chanceSlider.interactable = true;
                  // AutoBetManager._Instance.betAmountInput.interactable = true;
                  // AutoBetManager._Instance.numberOfBetsInput.interactable = true;
                  // AutoBetManager._Instance.multiplierInput.interactable = true;
                  // AutoBetManager._Instance.chanceInput.interactable = true;
                  // AutoBetManager._Instance.stopOnLossInput.interactable = true;
                  // AutoBetManager._Instance.stopOnProfitInput.interactable = true;
                  // AutoBetManager._Instance.increaseOnLossPercentageInput.interactable = true;
                  // AutoBetManager._Instance.startAutoBetButton.interactable = true;
                  // UIManager._Instance.manualBtn.interactable = true;
                  // UIManager._Instance.automatBtn.interactable = true;
                  // AutoBetManager._Instance.increaseOnWinPercentageInput.interactable = true;

                  AutoBetManager._Instance.isAutoBetting = false;
              }






          }


      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }

    private IEnumerator DelayAfterBet()
    {
        yield return new WaitForSeconds(4f);

        UIManager._Instance.betButton.interactable = true;

    }
    public IEnumerator DelayAfterAutomaitcBet()
    {
        yield return new WaitForSeconds(4f);
        if (AutoBetManager._Instance.isStopped == false)
        {
            if (float.Parse(AutoBetManager._Instance.betAmountInput.text) <= CryptoPriceReader._Instance.currentBalance)
            {



                AutoBetManager._Instance.AutoBetIsON();
            }
            if (float.Parse(AutoBetManager._Instance.betAmountInput.text) > CryptoPriceReader._Instance.currentBalance)
            {
                AutoBetManager._Instance.isStopped = true;

                AutoBetManager._Instance.isAutoBetting = false;

            }

        }
            if (AutoBetManager._Instance.isStopped == true)
        {
            AutoBetManager._Instance.isAutoBetting = false;
        }
        



    }

    public void ExecuteCloudScriptDepositSetCode(string depositValue, string currency)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
         //   FunctionParameter = new { Deposit = CryptoDepositHistory._Instance.depositInput.text.ToString(), Currency = WebGLSendContractExample._Instance.whatDeposit.ToString() },
            FunctionParameter = new { Deposit =depositValue.ToString(), Currency = currency.ToString() },
            //    FunctionParameter = new { Value = webgl.ToString("F0") },
            FunctionName = "SetCodeDeposit",
            GeneratePlayStreamEvent = true,

        },
        cloudResult => {


            JsonObject jsonResult = (JsonObject)cloudResult.FunctionResult;
            object valueObject;
            object valueObject2;
            object valueObject3;

            ProtectedDecimal DepositValue = 0;
            ProtectedString CodeValue = "";
            ProtectedString CurrencyValue = "";


            if (jsonResult.TryGetValue("Value", out valueObject))
            {


                DepositValue = Convert.ToDecimal(valueObject);
            }
            if (jsonResult.TryGetValue("Code", out valueObject2))
            {


                CodeValue = Convert.ToString(valueObject2);


            }
            if (jsonResult.TryGetValue("Currency", out valueObject3))
            {


                CurrencyValue = Convert.ToString(valueObject3);


            }


            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) == DepositValue)
            //  {
            //      code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
            //      AddDeposit();
            //  }
            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) != DepositValue)
            //  {
            //
            //
            //  }


         //   if (CryptoDepositHistory._Instance.depositInput.text.ToString() == DepositValue.ToString() && CurrencyValue == WebGLSendContractExample._Instance.whatDeposit.ToString())
            if (depositValue.ToString() == DepositValue.ToString() && CurrencyValue == currency)
            {
                code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
                AddDeposit();
            }
            //    if ((Mathf.RoundToInt(((float)((float.Parse(WebGLSendContractExample._Instance.depositInput.text) * tokenPrice) * 4)))) != DepositValue)
            //   {
            //

            //   }


            //  }
        },
        cloudError =>
        {
            Debug.Log("Error");

        }); ;
    }

    public void ExecuteCloudScriptWithdrawSetCode()
    {
        loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Loading...";
        loadingObject.gameObject.SetActive(true);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Wallet = WithdrawScript._Instance.walletInput.text.ToString(), Value = CryptoWithdrawHistory._Instance.withdrawInput.text.ToString(), Currency = Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() },
            //    FunctionParameter = new { Value = webgl.ToString("F0") },
            FunctionName = "SetCodeWithdraw",
            GeneratePlayStreamEvent = true,

        },
        cloudResult => {


            JsonObject jsonResult = (JsonObject)cloudResult.FunctionResult;
            object valueObject;
            object valueObject2;
            object valueObject3;
            object valueObject4;

            ProtectedDecimal DepositValue = 0;
            ProtectedString CodeValue = "";
            ProtectedString CurrencyValue = "";
            ProtectedString WalletValue = "";


            if (jsonResult.TryGetValue("Value", out valueObject))
            {


                DepositValue = Convert.ToDecimal(valueObject);
            }
            if (jsonResult.TryGetValue("Code", out valueObject2))
            {


                CodeValue = Convert.ToString(valueObject2);


            }
            if (jsonResult.TryGetValue("Currency", out valueObject3))
            {


                CurrencyValue = Convert.ToString(valueObject3);


            }
            if (jsonResult.TryGetValue("Wallet", out valueObject4))
            {


                WalletValue = Convert.ToString(valueObject4);


            }


            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) == DepositValue)
            //  {
            //      code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
            //      AddDeposit();
            //  }
            //  if (double.Parse(WebGLSendContractExample._Instance.depositInput.text) != DepositValue)
            //  {
            //
            //
            //  }


            if (CryptoWithdrawHistory._Instance.withdrawInput.text.ToString() == DepositValue.ToString() && CurrencyValue == Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() && WalletValue == WithdrawScript._Instance.walletInput.text.ToString())
            {
                code = Math.Sqrt((double.Parse(CodeValue.ToString()) / 3) + 1337).ToString();
                RemoveWithdrawFunction();
            }
            //    if ((Mathf.RoundToInt(((float)((float.Parse(WebGLSendContractExample._Instance.depositInput.text) * tokenPrice) * 4)))) != DepositValue)
            //   {
            //

            //   }


            //  }
        },
        cloudError =>
        {
            Debug.Log("Error");

        }); ;
    }
    public void RemoveWithdrawFunction()
    {



        // loadingObject.gameObject.SetActive(true);


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Code = code.ToString() },
            FunctionName = "RemoveWithdrawFunction",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {


          if (cloudResult.FunctionResult.ToString() == "Success")
          {
              // OutOfCoinsManager._Instance.closeWindow();
              //
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.gameObject.GetComponent<Button>().interactable = true;
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").transform.localScale = new Vector3(1, 1, 1);
              //   Web3PrivateKeySend20Example._Instance.whatWithdraw = "";
              //   CryptoWithdrawHistory._Instance.withdrawInput.text = null;
              //   Web3PrivateKeySend20Example._Instance.withdrawObject.gameObject.SetActive(false);
              //  WebGLSendContractExample._Instance.depositObject.transform.parent.gameObject.SetActive(false);
              // LeanTween.cancel(WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<animatiTransfer>().GetComponent<RectTransform>());
              //  WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<animatiTransfer>().StartPulse();

              GetCurrencyBalance();
              string whatChain = "";

              if (PlayerPrefs.GetString("NewChain") == "1")
              {
                  whatChain = "ETH";
              }
              if (PlayerPrefs.GetString("NewChain") == "56")
              {
                  whatChain = "BSC";

              }
              if (PlayerPrefs.GetString("NewChain") == "89")
              {
                  whatChain = "MATIC";

              }
              PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);

              //  Web3PrivateKeySend20Example._Instance.WithdrawToken();

              //  loadingObject.gameObject.SetActive(false);
              // GetPlayerItem();
          if (isAutomaticallyWithdrawals == "No")
          {
         
         
           //  //string body = HTML.Bold("Date: ") + System.DateTime.Now + HTML.P + HTML.Bold("Email: ") + ProtectedPlayerPrefs.GetString("EmailSignIn") + HTML.P + HTML.Bold("Username: ") + currentUsername + HTML.P + HTML.Bold("Address: ") + WithdrawScript._Instance.walletInput.text + HTML.P + HTML.Bold("Value: ") + CryptoWithdrawHistory._Instance.withdrawInput.text.ToString() +  HTML.P + HTML.Bold("Crypto: ") + Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() + HTML.P + HTML.Bold("Chain: ") + whatChain;
           //  string body = "TEST";
           //  string smtp = "smtp.mail.yahoo.com";
           //  string subject = "NEW WITHDRAWAL REQUEST [" + System.DateTime.Now + "]";
           ////  string user = "buddydiceweb3@outlook.com";
           //  string user = "buddydiceweb3@yahoo.com";
           //  //string password = "1VcAP%)7S70o";
           //  string password = "p0(2Oe*L\"0;X";
           ////  string password = "jc@?90p\\\\(4N@";
           // // string to = "buddysakaki@outlook.com";
           //  string to = "testowapocztamalpaxd@gmail.com";
           ////  string from = "buddydiceweb3@outlook.com";
           //  string from = "buddydiceweb3@yahoo.com";
           // // Email.SendEmail(from, to, subject, body, smtp, user, password);
           //  Email.SendEmail(from, to, subject, body, smtp, user,password);

              manuallyNotificationObject.gameObject.SetActive(true);
              manuallyNotificationObject.transform.Find("back").gameObject.transform.GetChild(3).gameObject.SetActive(true);
              manuallyNotificationObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Withdraw requested!\nWill be proceeded in 24/48 hours.";
              CryptoWithdrawHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString(), CryptoWithdrawHistory._Instance.withdrawInput.text.ToString(), null, whatChain);
         
          }
              if (isAutomaticallyWithdrawals == "Yes")
              {

                  //  string body = HTML.Bold("Address: ") + WithdrawScript._Instance.walletInput.text + HTML.P + HTML.Bold("Value: ") + CryptoWithdrawHistory._Instance.withdrawInput.text.ToString() + HTML.P + HTML.Bold("Date: ") + System.DateTime.Now;
                  //  string smtp = "smtp-mail.outlook.com";
                  //  string subject = "NEW WITHDRAWAL REQUEST [" + System.DateTime.Now + "]";
                  //  string user = "buddydiceweb3@outlook.com";
                  //  string password = "jc@?90p\\(4N@";
                  //  string to = "testowapocztamalpaxd@gmail.com";
                  //  string from = "buddydiceweb3@outlook.com";
                  //  Email.SendEmail(from, to, subject, body, smtp, user, password);
                  //  Debug.Log("SENTtrue
                  loadingObject.gameObject.SetActive(true);

                  loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sending " + CryptoWithdrawHistory._Instance.withdrawInput.text.ToString() + " "+ Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() + "...";
        
                  //WalletManager._Instance.SendClicked();

                  if (PlayerPrefs.GetString("NewChain") == "1")
                  {
                      if (Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "ETH")
                      {
                          StartNativeTransaction();
                      }
                  }
                  if (PlayerPrefs.GetString("NewChain") == "56")
                  {
                      if (Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "BNB")
                      {
                          StartNativeTransaction();
                      }
                  }
                  if (PlayerPrefs.GetString("NewChain") == "89")
                  {
                      if(Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "MATIC")
                      {
                          StartNativeTransaction();
                      }
                      if (Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "USDT")
                      {
                          StartTransaction(1000000, "0xc2132d05d31c914a87c6611c10748aeb04b58e8f");
                      }
                      if (Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "USDC")
                      {
                          StartTransaction(1000000, "0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359");
                      }
                      if (Web3PrivateKeySend20Example._Instance.whatWithdraw.ToString() == "LINK")
                      {
                          StartTransaction(100000000000000000, "0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39");
                      }
                  }
              }


          }


      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }
    [DllImport("__Internal")]
    private static extern void SendERC20Transaction(string privateKey, string recipientAddress, string amountInToken, string contractAddress, string callback, string rpc);

    [DllImport("__Internal")]
    private static extern void SendNativeTransaction(string privateKey, string recipientAddress, string amountInEther, string callback, string rpc);
    public void StartTransaction(decimal multiplier, string tokenAdr)
    {
        // TODO (architecture): Private key must be moved to a secure server-side signing service.
        // Signing transactions client-side is a known security debt.
        ProtectedString privateKey = Secrets.EthPrivateKey;
        ProtectedString recipientAddress = WithdrawScript._Instance.walletInput.text;
        ProtectedString amountInToken = (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) * multiplier).ToString();
        ProtectedString contractAddress = tokenAdr;
        ProtectedString rpc = "";

        rpc = "https://polygon-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
        SendERC20Transaction(privateKey, recipientAddress, amountInToken, contractAddress, "OnTransactionComplete", rpc);

        // SendERC20Transaction(privateKey, recipientAddress, amountInToken, "OnTransactionComplete");
    }

    public void StartNativeTransaction()
    {
        // TODO (architecture): Private key must be moved to a secure server-side signing service.
        ProtectedString privateKey = Secrets.EthPrivateKey;
        ProtectedString recipientAddress = WithdrawScript._Instance.walletInput.text;
        ProtectedString amountInToken = CryptoWithdrawHistory._Instance.withdrawInput.text.ToString();
        ProtectedString rpc = "";

        rpc = "https://polygon-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
        SendNativeTransaction(privateKey, recipientAddress, amountInToken, "OnTransactionComplete", rpc);

    }
    public void OnTransactionComplete(string transactionHash)
    {
        latestTXHash = transactionHash;
        loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Transaction completed!\nTxHash: " + transactionHash;
      Debug.Log("Transaction completed! TxHash: " + transactionHash);
        loadingObject.transform.Find("back").gameObject.transform.GetChild(1).gameObject.SetActive(true);
        loadingObject.transform.Find("back").gameObject.transform.GetChild(2).gameObject.SetActive(true);

        string whatChain = "";

        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            whatChain = "ETH";
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            whatChain = "BSC";

        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            whatChain = "MATIC";

        }
        CryptoWithdrawHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", CryptoWithdrawHistory._Instance.withdrawInput.text.ToString(), transactionHash, whatChain);

    }
    public void OpenTX()
    {
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            Application.OpenURL("https://polygonscan.com/tx/" + latestTXHash);

        }

        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            Application.OpenURL("https://etherscan.io/tx/" + latestTXHash);

        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            Application.OpenURL("https://bscscan.com/tx/" + latestTXHash);

        }

    }
    public void AddDeposit()
    {



        // loadingObject.gameObject.SetActive(true);


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionParameter = new { Code = code.ToString() },
            FunctionName = "AddDepositCurrency",
            GeneratePlayStreamEvent = true,

        },
      cloudResult => {


          if (cloudResult.FunctionResult.ToString() == "Success")
          {
              // OutOfCoinsManager._Instance.closeWindow();
              //


          //   WebGLSendContractExample._Instance.depositObject.transform.Find("approve").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;
          //   WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;
          //   WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greenSprite;
          //   WebGLSendContractExample._Instance.depositObject.gameObject.GetComponent<Button>().interactable = true;
          //   WebGLSendContractExample._Instance.depositObject.transform.Find("send").transform.localScale = new Vector3(1, 1, 1);
          //   WebGLSendContractExample._Instance.whatDeposit = "";
          //   CryptoDepositHistory._Instance.depositInput.text = null;
          //   WebGLSendContractExample._Instance.depositObject.gameObject.SetActive(false);


            //  WebGLSendContractExample._Instance.depositObject.transform.parent.gameObject.SetActive(false);
              // LeanTween.cancel(WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<animatiTransfer>().GetComponent<RectTransform>());
              //  WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<animatiTransfer>().StartPulse();

              GetCurrencyBalance();


              //  loadingObject.gameObject.SetActive(false);
              // GetPlayerItem();

          }


      },
      cloudError =>
      {
          Debug.Log("Error");

      });

    }

    [System.Serializable]
    public class MyDataClass
    {
        public string betAmount;
        public string multiplier;
        public string chance;
        public string isWin;
        public string profit;
        public string crypto;
        public string roll;
        public string wallet;
    }


}
