using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using OPS.AntiCheat.Prefs;
using TMPro;
using Org.BouncyCastle.Math.EC.Multiplier;
using System.Security.Cryptography;
using OPS.AntiCheat.Field;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Nethereum.Signer;

public class CryptoDepositHistory : MonoBehaviour
{
    // Prefab, kt�ry b�dzie instancjowany
    public static CryptoDepositHistory _Instance; 

    public GameObject depositPrefab;
    public Transform historyParent;
    public List<GameObject> recentGames = new List<GameObject>();
    private int betNumber = 0; // Numer zak�adu

    // Klucze do przechowywania danych w PlayerPrefs
    private const string DepositHistoryKey = "CryptoDepositHistory";

    public TMP_InputField depositInput;
    public TMP_Dropdown cryptoDeposit;
    public TMP_Dropdown cryptoDepositBEP;
    public TMP_Dropdown cryptoDepositMATIC;
    // Struktura do przechowywania informacji o depozycie

    public ProtectedString txURL;

    [Serializable]
    public class DepositEntry
    {
        public string crypto;
        public float amount;
        public string date;
        public string tx;
    }

    // Lista do przechowywania historii depozyt�w
    private List<DepositEntry> depositHistory = new List<DepositEntry>();

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        // Odczytaj histori� depozyt�w przy starcie
        LoadDepositHistory();
        betNumber = ProtectedPlayerPrefs.GetInt("TotalBetNumber", 0); // Wczytanie ostatniego numeru zak�adu
    }

    public void AddDeposit(string time, string crypto, string amount, string tx, string chain)
    {
        GameObject newGame = Instantiate(depositPrefab, historyParent);
        newGame.GetComponent<Button>().onClick.AddListener(OpenTX);
        newGame.GetComponent<DepositDetails>().chain = chain;
        betNumber += 1;
        ProtectedPlayerPrefs.SetInt("TotalBetNumber", betNumber); // Zapisanie numeru zak�adu w PlayerPrefs
        txURL = tx;
        // Ustawienie odpowiednich warto�ci tekstowych
      //  newGame.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = betNumber.ToString();

        newGame.transform.Find("date").GetComponent<TextMeshProUGUI>().text = time;
        newGame.transform.Find("crypto").GetComponent<TextMeshProUGUI>().text = "+" + amount + " " + crypto;



        recentGames.Add(newGame);
        SaveDepositHistory();
     //   DisplayHistory();
    }
    public void NewDeposit(string tx)
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        { 
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoDeposit.options[cryptoDeposit.value].text, depositInput.text, tx, "ETH");
    }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoDepositBEP.options[cryptoDepositBEP.value].text, depositInput.text, tx, "BSC");
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoDepositMATIC.options[cryptoDepositMATIC.value].text, depositInput.text, tx, "MATIC");
        }
    }
    public void OpenTX()
    {
        string chain = EventSystem.current.currentSelectedGameObject.GetComponent<DepositDetails>().chain;
        string tx = txURL;

        if (chain == "MATIC")
            Application.OpenURL("https://polygonscan.com/tx/" + tx);
        else if (chain == "ETH")
            Application.OpenURL("https://etherscan.io/tx/" + tx);
        else if (chain == "BSC")
            Application.OpenURL("https://bscscan.com/tx/" + tx);
    }

    private void SaveDepositHistory()
    {
        for (int i = 0; i < recentGames.Count; i++)
        {
            var game = recentGames[i];

            // Konwertuj list� na JSON i zapisz w PlayerPrefs
            //string number = game.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text;
            string time = game.transform.Find("date").GetComponent<TextMeshProUGUI>().text;
            string depositAmount = game.transform.Find("crypto").GetComponent<TextMeshProUGUI>().text;
            string tx = txURL;
            string chain = game.GetComponent<DepositDetails>().chain;
            RecentDeposit gameData = new RecentDeposit
            {
                Time = time,
                DepositAmount = depositAmount,
                TX = tx,
                Chain = chain,
               
            };


            string jsonData = JsonUtility.ToJson(gameData);
            ProtectedPlayerPrefs.SetString("RecentDeposit_" + i, jsonData);
        }
        ProtectedPlayerPrefs.SetInt("RecentDepositCount", recentGames.Count);
        ProtectedPlayerPrefs.Save();
    }
   
    private void LoadDepositHistory()
    {
        int count = ProtectedPlayerPrefs.GetInt("RecentDepositCount", 0);

        for (int i = 0; i < count; i++)
        {
            string jsonData = ProtectedPlayerPrefs.GetString("RecentDeposit_" + i, "");

            if (!string.IsNullOrEmpty(jsonData))
            {
                RecentDeposit gameData = JsonUtility.FromJson<RecentDeposit>(jsonData);

                // Tworzenie nowego recent game na podstawie zapisanych danych
                GameObject newGame = Instantiate(depositPrefab, historyParent);
                //  newGame.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = gameData.Number;
                newGame.GetComponent<Button>().onClick.AddListener(OpenTX);
                newGame.GetComponent<DepositDetails>().chain = gameData.Chain;
                newGame.transform.Find("date").GetComponent<TextMeshProUGUI>().text = gameData.Time;
                newGame.transform.Find("crypto").GetComponent<TextMeshProUGUI>().text = gameData.DepositAmount;
                txURL = gameData.TX;

                recentGames.Add(newGame);
            }
        }
    }

    private void DisplayHistory()
    {
        // Wyczy�� poprzednie wy�wietlane depozyty
        foreach (Transform child in historyParent)
        {
            Destroy(child.gameObject);
        }

        // Instancjuj prefaby dla ka�dego wpisu w historii
        foreach (DepositEntry entry in depositHistory)
        {
            GameObject newEntry = Instantiate(depositPrefab, historyParent);
            newEntry.GetComponent<Button>().onClick.AddListener(OpenTX);

            newEntry.transform.Find("CryptoText").GetComponent<Text>().text = entry.crypto;
            newEntry.transform.Find("AmountText").GetComponent<Text>().text = entry.amount.ToString("0.00");
            newEntry.transform.Find("DateText").GetComponent<Text>().text = entry.date;
        }
    }

 
}
[System.Serializable]
public class RecentDeposit
{
    public string Number;

    public string Time;
    public string Chain;
    public string TX;
    public string DepositAmount;
   
}
