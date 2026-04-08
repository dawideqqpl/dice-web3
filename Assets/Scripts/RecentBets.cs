using OPS.AntiCheat.Prefs;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class RecentBets : MonoBehaviour
{
    public static RecentBets _Instance;
    // Start is called before the first frame update
    public GameObject lastBetPrefab; // Prefab do wyœwietlania danych zak³adu
    public Transform contentParent; // Kontener, w którym umieszczamy prefabrykaty
    private const int maxLastBets = 100;
    public Sprite win;
    public Sprite lose;
    public List<LastBetData> betDataList;

    public Sprite usdt;
    public Sprite usdc;
    public Sprite fdusd;
    public Sprite eth;
    public Sprite bnb;
    public Sprite link;
    public Sprite cro;
    public Sprite polygon;
    public Sprite shib;
    public Sprite ape;
    
    private void Awake()
    {
             CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

 //       CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
   //     CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");
        _Instance = this;
    }
    void Start()
    {
        
    }
    public void LoadLastBetsFromPlayFab()
    {
      

        // Lista kluczy, które chcemy pobraæ
        List<string> keys = new List<string>();
        for (int i = 0; i < maxLastBets; i++)
        {
            keys.Add("LastBet" + i);
        }

        var request = new GetTitleDataRequest
        {
            Keys = keys
        };

        PlayFabClientAPI.GetTitleData(request, OnTitleDataReceived, OnError);
    }

    private void OnTitleDataReceived(GetTitleDataResult result)
    {
    betDataList = new List<LastBetData>();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject); // Destroy each child object
        }

        foreach (var keyValuePair in result.Data)
        {
            // Sprawdzamy czy dany klucz zawiera wartoœæ
            if (!string.IsNullOrEmpty(keyValuePair.Value))
            {
                // Parsowanie danych zak³adu z JSON
                LastBetData betData = JsonUtility.FromJson<LastBetData>(keyValuePair.Value);
             //   Debug.Log($"Parsed Data: Time: {betData.time}, Bet Amount: {betData.betAmount}, Multiplier: {betData.multiplier}, Chance: {betData.chance}, IsWin: {betData.isWin}, Profit: {betData.profit}");

                betDataList.Add(betData);
               // Debug.Log("BET DATA PROFIT " + betData.profit);

            }
        }

        // Sortowanie listy zak³adów po czasie malej¹co (najnowsze najpierw)
        //    betDataList = betDataList.OrderByDescending(bet => DateTime.Parse(bet.time)).ToList();

        betDataList = betDataList
       .OrderByDescending(bet => DateTime.ParseExact(bet.time, "dd/MM, HH:mm:ss", CultureInfo.InvariantCulture)) // Sortowanie najpierw po dacie, a potem po godzinie i minucie
       .ToList();
        // Tworzenie prefabrykatów na podstawie posortowanej listy
        foreach (var betData in betDataList)
        {
            DateTime parsedTime = DateTime.ParseExact(betData.time, "dd/MM, HH:mm:ss", CultureInfo.InvariantCulture);
            string timeOnly = parsedTime.ToString("HH:mm"); // Pobieranie tylko godzin i minut

            GameObject betItem = Instantiate(lastBetPrefab, contentParent);
            if (betData.wallet != null)
            {

                if (betData.wallet.Length > 5)
                {


                    betItem.transform.Find("wallet").GetComponent<TextMeshProUGUI>().text = timeOnly + " " + betData.wallet.Substring(0, 5) + "...";
                }
                if (betData.wallet.Length <= 5)
                {

                    betItem.transform.Find("wallet").GetComponent<TextMeshProUGUI>().text = timeOnly + " " + betData.wallet;
                }

                }
            if (betData.wallet == null)
            {


                betItem.transform.Find("wallet").GetComponent<TextMeshProUGUI>().text = timeOnly + " Unkown...";
            }
            // betItem.transform.Find("BetAmountText").GetComponent<TextMeshProUGUI>().text = betData.betAmount + " BTC";
            //  betItem.transform.Find("multiplier").GetComponent<TextMeshProUGUI>().text = betData.multiplier.ToString("0.00") + "x";
            betItem.transform.Find("multiplier").GetComponent<TextMeshProUGUI>().text =  betData.multiplier.ToString() + "x";
          //  betItem.transform.Find("chance").GetComponent<TextMeshProUGUI>().text = betData.chance.ToString() + "%";
            if (betData.roll != null)
            {


                betItem.transform.Find("rolledDice").GetComponent<TextMeshProUGUI>().text = betData.roll.ToString();
            }
            if (betData.roll == null)
            {


                betItem.transform.Find("rolledDice").GetComponent<TextMeshProUGUI>().text = "---";
            }
            betItem.transform.Find("bet").GetComponent<TextMeshProUGUI>().text = betData.betAmount;

            float fullProfit = float.Parse(betData.profit.Replace(',', '.')) + float.Parse(betData.betAmount.Replace(',', '.'));
           // Debug.Log("FULL PROFIT  " + fullProfit);
            //  betItem.transform.Find("ChanceText").GetComponent<TextMeshProUGUI>().text = betData.chance + "%";
            // betItem.transform.Find("winOrLose").GetComponent<Image>().sprite = (betData.isWin ? win : lose);
            
           if (float.Parse(betData.profit) < 0)
            {
                betItem.transform.Find("win").GetComponent<TextMeshProUGUI>().text = (betData.isWin ? "+" : "-") + float.Parse(betData.betAmount.Replace(',', '.')).ToString("0.##############") + " " + betData.crypto;

            }
            if (float.Parse(betData.profit) >= 0)
            {
                //  betItem.transform.Find("win").GetComponent<TextMeshProUGUI>().text = (betData.isWin ? "+" : "-") + Mathf.Abs(float.Parse(betData.profit) + float.Parse(betData.betAmount)).ToString("0.########") + " " + betData.crypto;
                betItem.transform.Find("win").GetComponent<TextMeshProUGUI>().text = (betData.isWin ? "+" : "-") + fullProfit.ToString("0.##############") + " " + betData.crypto;

            }
            //betItem.transform.Find("win").GetComponent<TextMeshProUGUI>().text = (betData.isWin ? "+" : "-") + Mathf.Abs(float.Parse(betData.profit)).ToString("0.########") + " " + betData.crypto;
            betItem.transform.Find("win").GetComponent<TextMeshProUGUI>().color = betData.isWin ? Color.green : Color.red;

            Sprite spriteCrypto = null;
            if(betData.crypto == "USDT")
            {
                spriteCrypto = usdt;
            }
            if (betData.crypto == "USDC")
            {
                spriteCrypto = usdc;
            }
            if (betData.crypto == "FDUSD")
            {
                spriteCrypto = fdusd;
            }
            if (betData.crypto == "LINK")
            {
                spriteCrypto = link ;
            }
            if (betData.crypto == "BNB")
            {
                spriteCrypto = bnb;
            }
            if (betData.crypto == "ETH")
            {
                spriteCrypto = eth;
            }
            if (betData.crypto == "SHIB")
            {
                spriteCrypto = shib;
            }
            if (betData.crypto == "MATIC")
            {
                spriteCrypto = polygon;
            }
            if (betData.crypto == "APE")
            {
                spriteCrypto = ape;
            }
            if (betData.crypto == "CRO")
            {
                spriteCrypto = cro;
            }
            betItem.transform.Find("bet").transform.Find("crypto").GetComponent<Image>().sprite = spriteCrypto;

            betItem.transform.Find("crypto").GetComponent<Image>().sprite = spriteCrypto;

           // Debug.Log("PROFIT " + betData.profit);
        }
    }
     public IEnumerator LoadBetsPeriodically()
    {
        while (true)
        {
            RecentBets._Instance.LoadLastBetsFromPlayFab();
            yield return new WaitForSeconds(10f);
        }
    }
    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving Title Data: " + error.GenerateErrorReport());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class LastBetData
{
    public string crypto; // Czas zapisu zak³adu, w formacie ISO 8601
    public string time; // Czas zapisu zak³adu, w formacie ISO 8601
    public string betAmount;
    public string multiplier;
    public string chance;
    public bool isWin;
    public string profit;
    public string wallet;
    public string roll;

    // Method to convert string values to their appropriate types after JSON parsing
   // public float GetBetAmount()
  //  {
       // return float.Parse(betAmount.Replace(",", ".")); // Convert string to float
   // }

  ///  public float GetMultiplier()
//{
    //    return float.Parse(multiplier.Replace(",", ".")); // Convert string to float
   // }

   // public float GetChance()
   // {
     //   return float.Parse(chance.Replace(",", ".")); // Convert string to float
    //}

  //  public bool GetIsWin()
  //  {
    //    return bool.Parse(isWin); // Convert string to bool
   // }

   // public float GetProfit()
   // {
       // return float.Parse(profit.Replace(",", ".")); // Convert string to float
   // }
}