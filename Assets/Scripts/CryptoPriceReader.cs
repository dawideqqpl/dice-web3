using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using OPS.AntiCheat.Field;
using Unity.VisualScripting;
using System.Linq;


public class CryptoPriceReader : MonoBehaviour
{

    public static CryptoPriceReader _Instance;

    public ProtectedBool currentShibaApi;
    public ProtectedBool currentETHApi;
    public ProtectedBool currentLinkApi;
    public ProtectedBool currentMaticApi;
    public ProtectedBool currentUSDTApi;
    public ProtectedBool currentUSDCApi;
    public ProtectedBool currentFDUSDApi;
    public ProtectedBool currentCronosApi;
    public ProtectedBool currentBNBApi;
    public ProtectedBool currentApeApi;

    public ProtectedString shibaAPI = "https://api.coingecko.com/api/v3/simple/price?ids=shiba-inu&vs_currencies=usd";
    public ProtectedString ethAPI = "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd";
    public ProtectedString linkAPI = "https://api.coingecko.com/api/v3/simple/price?ids=chainlink&vs_currencies=usd";
    public ProtectedString maticAPI = "https://api.coingecko.com/api/v3/simple/price?ids=matic-network&vs_currencies=usd";
    public ProtectedString usdtAPI = "https://api.coingecko.com/api/v3/simple/price?ids=tether&vs_currencies=usd";
    public ProtectedString usdcAPI = "https://api.coingecko.com/api/v3/simple/price?ids=usd-coin&vs_currencies=usd";
    public ProtectedString fdusdAPI = "https://api.coingecko.com/api/v3/simple/price?ids=first-digital-usd&vs_currencies=usd";
    public ProtectedString cronosAPI = "https://api.coingecko.com/api/v3/simple/price?ids=crypto-com-chain&vs_currencies=usd";
    public ProtectedString bnbAPI = "https://api.coingecko.com/api/v3/simple/price?ids=binancecoin&vs_currencies=usd";
    public ProtectedString apeAPI = "https://api.coingecko.com/api/v3/simple/price?ids=apecoin&vs_currencies=usd";
    private string apiUrl = "";
    public ProtectedFloat shibPrice;
    public ProtectedFloat ethPrice;
    public ProtectedFloat linkPrice;
    public ProtectedFloat maticPrice;
    public ProtectedFloat usdtPrice;
    public ProtectedFloat usdcPrice;
    public ProtectedFloat fdusdPrice;
    public ProtectedFloat cronosPrice;
    public ProtectedFloat bnbPrice;
    public ProtectedFloat apePrice;

    public ProtectedFloat shibMinimumBet;
    public ProtectedFloat ethMinimumBet;
    public ProtectedFloat linkMinimumBet;
    public ProtectedFloat maticMinimumBet;
    public ProtectedFloat usdtMinimumBet;
    public ProtectedFloat usdcMinimumBet;
    public ProtectedFloat fdusdMinimumBet;
    public ProtectedFloat cronosMinimumBet;
    public ProtectedFloat bnbMinimumBet;
    public ProtectedFloat apeMinimumBet;

    public ProtectedFloat currentMinimumBet;
    public ProtectedString currentDivider;
    public ProtectedInt16 currentCharacterLimit;
    public ProtectedInt32 currentMultiplier;

    public ProtectedFloat currentBalance;
    public ProtectedString currentTag;

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        CalculateShibForTwoDollars("USDT");
        CalculateShibForTwoDollars("USDC");
        CalculateShibForTwoDollars("MATIC");
        CalculateShibForTwoDollars("ETH");
        CalculateShibForTwoDollars("BNB");
        CalculateShibForTwoDollars("FDUSD");
        CalculateShibForTwoDollars("SHIB");
        CalculateShibForTwoDollars("CRO");
        CalculateShibForTwoDollars("LINK");
        CalculateShibForTwoDollars("APE");

    }

    public IEnumerator GetCryptoPrice(string crypto)
    {


        if (crypto == "ETH")
        {
            currentMinimumBet = ethMinimumBet;
            currentDivider = "0.#######";
            currentCharacterLimit = 9;
            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("ETH");

            if (currentETHApi == true)
            {
                yield break;
            }

            if (currentETHApi == false)
            {
                apiUrl = ethAPI;
                currentETHApi = true;
            }

          

        }
        if (crypto == "SHIB")
        {
            currentMinimumBet = shibMinimumBet;
            currentDivider = "0";
            currentCharacterLimit = 8;
            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("SHIB");

            if (currentShibaApi == true)
            {
                yield break;
            }

            if (currentShibaApi == false)
            {
                apiUrl = shibaAPI;
                currentShibaApi = true;
            }

           
        }
        if (crypto == "USDT")
        {
            currentMinimumBet = usdtMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("USDT");

            if (currentUSDTApi == true)
            {
                yield break;
            }

            if (currentUSDTApi == false)
            {


                apiUrl = usdtAPI;
                usdtPrice = 1;
            //    CalculateShibForTwoDollars("USDT");
                currentUSDTApi = true;
                //

                // UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                // yield return request.SendWebRequest();
                //
                // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // {
                //     Debug.LogError("Error fetching SHIB price: " + request.error);
                // }
                // else
                // {
                //     string jsonResponse = request.downloadHandler.text;
                //     JObject json = JObject.Parse(jsonResponse);
                //     string currencyKey = json.Properties().First().Name;
                //
                //     usdtPrice = json[currencyKey]["usd"].Value<float>();
                //     CalculateShibForTwoDollars("USDT");
                //     currentUSDTApi = true;
                //
                //     // CalculateShibForTwoDollars();
                // }
            }


        }
        if (crypto == "USDC")
        {
            currentMinimumBet = usdcMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("USDC");

            if (currentUSDCApi == true)
            {
                yield break;
            }

            if (currentUSDCApi == false)
            {


                apiUrl = usdcAPI;
usdcPrice = 1;
           //     CalculateShibForTwoDollars("USDC");
                currentUSDCApi = true;

                // UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                // yield return request.SendWebRequest();
                //
                // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // {
                //     Debug.LogError("Error fetching SHIB price: " + request.error);
                // }
                // else
                // {
                //     string jsonResponse = request.downloadHandler.text;
                //     JObject json = JObject.Parse(jsonResponse);
                //     string currencyKey = json.Properties().First().Name;
                //
                //     usdcPrice = json[currencyKey]["usd"].Value<float>();
                //     CalculateShibForTwoDollars("USDC");
                //     currentUSDCApi = true;
                //
                //     // CalculateShibForTwoDollars();
                // }
            }
       
        }
        if (crypto == "BNB")
        {
            currentMinimumBet = bnbMinimumBet;
            currentDivider = "0.#######";
            currentCharacterLimit = 8;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("BNB");

            if (currentBNBApi == true)
            {
                yield break;
            }

            if (currentBNBApi == false)
            {


                apiUrl = bnbAPI;
             //   CalculateShibForTwoDollars("BNB");
                currentBNBApi = true;
                //
                // UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                // yield return request.SendWebRequest();
                //
                // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // {
                //     Debug.LogError("Error fetching SHIB price: " + request.error);
                // }
                // else
                // {
                //     string jsonResponse = request.downloadHandler.text;
                //     JObject json = JObject.Parse(jsonResponse);
                //     string currencyKey = json.Properties().First().Name;
                //
                //     bnbPrice = json[currencyKey]["usd"].Value<float>();
                //     CalculateShibForTwoDollars("BNB");
                //     currentBNBApi = true;
                //
                //     // CalculateShibForTwoDollars();
                // }
            }
         

        }
        if (crypto == "CRO")
        {
            currentMinimumBet = cronosMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("CRO");

            if (currentCronosApi == true)
            {
                yield break;
            }

            if (currentCronosApi == false)
            {


                apiUrl = cronosAPI;
               // CalculateShibForTwoDollars("CRO");
                currentCronosApi = true;

                //  UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                //  yield return request.SendWebRequest();
                //
                //  if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                //  {
                //      Debug.LogError("Error fetching SHIB price: " + request.error);
                //  }
                //  else
                //  {
                //      string jsonResponse = request.downloadHandler.text;
                //      JObject json = JObject.Parse(jsonResponse);
                //      string currencyKey = json.Properties().First().Name;
                //
                //      cronosPrice = json[currencyKey]["usd"].Value<float>();
                //      CalculateShibForTwoDollars("CRO");
                //      currentCronosApi = true;
                //
                //      // CalculateShibForTwoDollars();
                //  }
            }
         

        }
        if (crypto == "APE")
        {
            currentMinimumBet = apeMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("APE");

            if (currentApeApi == true)
            {
                yield break;
            }

            if (currentApeApi == false)
            {


                apiUrl = apeAPI;
             //   CalculateShibForTwoDollars("APE");
                currentApeApi = true;
                // UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                // yield return request.SendWebRequest();
                //
                // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // {
                //     Debug.LogError("Error fetching SHIB price: " + request.error);
                // }
                // else
                // {
                //     string jsonResponse = request.downloadHandler.text;
                //     JObject json = JObject.Parse(jsonResponse);
                //     string currencyKey = json.Properties().First().Name;
                //
                //     apePrice = json[currencyKey]["usd"].Value<float>();
                //     CalculateShibForTwoDollars("APE");
                //     currentApeApi = true;
                //
                //     // CalculateShibForTwoDollars();
                // }
            }
          
        }
        if (crypto == "LINK")
        {
            currentMinimumBet = linkMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("LINK");

            if (currentLinkApi == true)
            {
                yield break;
            }

            if (currentLinkApi == false)
            {


                apiUrl = linkAPI;
                //CalculateShibForTwoDollars("LINK");
                currentLinkApi = true;
                //
                //   UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                //   yield return request.SendWebRequest();
                //
                //   if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                //   {
                //       Debug.LogError("Error fetching SHIB price: " + request.error);
                //   }
                //   else
                //   {
                //       string jsonResponse = request.downloadHandler.text;
                //       JObject json = JObject.Parse(jsonResponse);
                //       string currencyKey = json.Properties().First().Name;
                //
                //       linkPrice = json[currencyKey]["usd"].Value<float>();
                //       CalculateShibForTwoDollars("LINK");
                //       currentLinkApi = true;
                //
                //       // CalculateShibForTwoDollars();
                //   }

            }

        }
        if (crypto == "FDUSD")
        {
            currentMinimumBet = fdusdMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("FDUSD");

            if (currentFDUSDApi == true)
            {
                yield break;
            }

            if (currentFDUSDApi == false)
            {


                apiUrl = fdusdAPI;
                fdusdPrice = 1;
           //     CalculateShibForTwoDollars("FDUSD");
                currentFDUSDApi = true;
                // UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                // yield return request.SendWebRequest();
                //
                // if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // {
                //     Debug.LogError("Error fetching SHIB price: " + request.error);
                // }
                // else
                // {
                //     string jsonResponse = request.downloadHandler.text;
                //     JObject json = JObject.Parse(jsonResponse);
                //     string currencyKey = json.Properties().First().Name;
                //
                //     fdusdPrice = json[currencyKey]["usd"].Value<float>();
                //     CalculateShibForTwoDollars("FDUSD");
                //     currentFDUSDApi = true;
                //
                //     // CalculateShibForTwoDollars();
                // }



            }

        }
        if (crypto == "MATIC")
        {
            currentMinimumBet = maticMinimumBet;
            currentDivider = "0.####";
            currentCharacterLimit = 6;

            StatisticsManager._Instance.LoadStatistics();
            CalculateShibForTwoDollars("MATIC");

            if (currentMaticApi == true)
            {
                yield break;
            }

            if (currentMaticApi == false)
            {


                apiUrl = maticAPI;
              //  CalculateShibForTwoDollars("MATIC");

                currentMaticApi = true;

                //  UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                //  yield return request.SendWebRequest();
                //
                //  if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                //  {
                //      Debug.LogError("Error fetching SHIB price: " + request.error);
                //  }
                //  else
                //  {
                //      string jsonResponse = request.downloadHandler.text;
                //      JObject json = JObject.Parse(jsonResponse);
                //      string currencyKey = json.Properties().First().Name;
                //
                //      maticPrice = json[currencyKey]["usd"].Value<float>();
                //      CalculateShibForTwoDollars("MATIC");
                //      // CalculateShibForTwoDollars();
                //      currentMaticApi = true;
                //
                //  }
            }

        }

      
    }

    public void CalculateShibForTwoDollars(string crypto)
    {
        currentTag = crypto;

        if (crypto == "SHIB")
        {
            currentBalance = PlayFabManager._Instance.SHIBA;

            //if (shibPrice > 0)
            //{
            //     shibMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / shibPrice);
            //}
            //else
            //{
            //    Debug.LogError(" price is not available.");
            //}

            shibMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / shibPrice);
            currentMinimumBet = shibMinimumBet;
            currentDivider = "0";
            currentMultiplier = 1;

        }
        if (crypto == "ETH")
        {
            currentBalance = PlayFabManager._Instance.ETH;


            ethMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / ethPrice);
            currentMinimumBet = ethMinimumBet;
            currentDivider = "0.0000000";
            currentMultiplier = 100000000;


        }
        if (crypto == "LINK")
        {
            currentBalance = PlayFabManager._Instance.LINK;


            linkMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / linkPrice);
            currentMinimumBet = linkMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;



        }
        if (crypto == "USDT")
        {

            currentBalance = PlayFabManager._Instance.USDT;

            usdtMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / usdtPrice);
            currentMinimumBet = usdtMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }
        if (crypto == "USDC")
        {
            currentBalance = PlayFabManager._Instance.USDC;


            usdcMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / usdcPrice);
            currentMinimumBet = usdcMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }
        if (crypto == "FDUSD")
        {

            currentBalance = PlayFabManager._Instance.FDUSD;

            fdusdMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / fdusdPrice);
            currentMinimumBet = fdusdMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }
        if (crypto == "APE")
        {
            currentBalance = PlayFabManager._Instance.APE;


            apeMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / apePrice);
            currentMinimumBet = apeMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }
        if (crypto == "MATIC")
        {

            currentBalance = PlayFabManager._Instance.MATIC;

            maticMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / maticPrice);
            currentMinimumBet = maticMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }
        if (crypto == "BNB")
        {
            currentBalance = PlayFabManager._Instance.BNB;


            bnbMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / bnbPrice);
            currentMinimumBet = bnbMinimumBet;
            currentDivider = "0.000000";
            currentMultiplier = 100000000;

        }
        if (crypto == "CRO")
        {
            currentBalance = PlayFabManager._Instance.CRO;


            cronosMinimumBet = ((ProtectedFloat)PlayFabManager._Instance.minimumBet / cronosPrice);
            currentMinimumBet = cronosMinimumBet;
            currentDivider = "0.0000";
            currentMultiplier = 100000;

        }

    // PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);
    }
}
