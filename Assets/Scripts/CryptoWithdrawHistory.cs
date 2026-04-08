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
using Nethereum.Signer;

public class CryptoWithdrawHistory : MonoBehaviour
{
    // Prefab, który bêdzie instancjowany
    public static CryptoWithdrawHistory _Instance;

    public GameObject withdrawPrefab;
    public Transform historyParent;
    public List<GameObject> recentGames = new List<GameObject>();
    private int betNumber = 0; // Numer zak³adu


    public TMP_InputField withdrawInput;
    public TMP_Dropdown cryptoWithdraw;
    public TMP_Dropdown cryptoWithdrawtBEP;
    public TMP_Dropdown cryptoWithdrawMATIC;
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

    // Lista do przechowywania historii depozytów
    private List<DepositEntry> depositHistory = new List<DepositEntry>();

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        // Odczytaj historiê depozytów przy starcie
        LoadDepositHistory();
        betNumber = ProtectedPlayerPrefs.GetInt("TotalBetWithdrawNumber", 0); // Wczytanie ostatniego numeru zak³adu
    }

    public void AddDeposit(string time, string crypto, string amount, string tx, string chain)
    {
        GameObject newGame = Instantiate(withdrawPrefab, historyParent);
        newGame.GetComponent<Button>().onClick.AddListener(OpenTX);
        newGame.GetComponent<WithdrawDetails>().chain = chain;

        betNumber += 1;
        ProtectedPlayerPrefs.SetInt("TotalBetWithdrawNumber", betNumber); // Zapisanie numeru zak³adu w PlayerPrefs
        txURL = tx;
        // Ustawienie odpowiednich wartoœci tekstowych
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
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoWithdraw.options[cryptoWithdraw.value].text, withdrawInput.text, tx, "ETH");
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoWithdrawtBEP.options[cryptoWithdrawtBEP.value].text, withdrawInput.text, tx,"BSC");
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), cryptoWithdrawMATIC.options[cryptoWithdrawMATIC.value].text, withdrawInput.text, tx, "MATIC");
        }
    }
    public void OpenTX()
    {
        if (txURL != "")
        {


            if (PlayerPrefs.GetString("NewChain") == "89")
            {
                Application.OpenURL("https://polygonscan.com/tx/" + txURL);

            }

            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                Application.OpenURL("https://etherscan.io/tx/" + txURL);

            }
            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                Application.OpenURL("https://bscscan.com/tx/" + txURL);

            }
        }
        if (txURL == "")
        {

        }



        }

    private void SaveDepositHistory()
    {
        for (int i = 0; i < recentGames.Count; i++)
        {
            var game = recentGames[i];

            // Konwertuj listê na JSON i zapisz w PlayerPrefs
            //string number = game.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text;
            string time = game.transform.Find("date").GetComponent<TextMeshProUGUI>().text;
            string depositAmount = game.transform.Find("crypto").GetComponent<TextMeshProUGUI>().text;
            string tx = txURL;
            string chain = game.GetComponent<WithdrawDetails>().chain;

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
                GameObject newGame = Instantiate(withdrawPrefab, historyParent);
                //  newGame.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = gameData.Number;
                newGame.GetComponent<Button>().onClick.AddListener(OpenTX);
                newGame.GetComponent<WithdrawDetails>().chain = gameData.Chain;

                newGame.transform.Find("date").GetComponent<TextMeshProUGUI>().text = gameData.Time;
                newGame.transform.Find("crypto").GetComponent<TextMeshProUGUI>().text = gameData.DepositAmount;
                txURL = gameData.TX;

                recentGames.Add(newGame);
            }
        }
    }

    private void DisplayHistory()
    {
        // Wyczyœæ poprzednie wyœwietlane depozyty
        foreach (Transform child in historyParent)
        {
            Destroy(child.gameObject);
        }

        // Instancjuj prefaby dla ka¿dego wpisu w historii
        foreach (DepositEntry entry in depositHistory)
        {
            GameObject newEntry = Instantiate(withdrawPrefab, historyParent);
            newEntry.GetComponent<Button>().onClick.AddListener(OpenTX);

            newEntry.transform.Find("CryptoText").GetComponent<Text>().text = entry.crypto;
            newEntry.transform.Find("AmountText").GetComponent<Text>().text = entry.amount.ToString("0.00");
            newEntry.transform.Find("DateText").GetComponent<Text>().text = entry.date;
        }
    }


}
[System.Serializable]
public class RecentWithdraw 
{
    public string Number;

    public string Time;
    public string Chain;
    public string TX;
    public string DepositAmount;

}
