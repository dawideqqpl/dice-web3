using OPS.AntiCheat.Field;
using OPS.AntiCheat.Prefs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager _Instance;

    public TextMeshProUGUI totalBetsText;
    public TextMeshProUGUI totalProfitText;
    public TextMeshProUGUI winsText;
    public TextMeshProUGUI lossesText;

    private decimal totalBets;
    private decimal totalProfit;
    private int wins;
    private int losses;

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        // £adowanie statystyk z PlayerPrefs
       // LoadStatistics();
        UpdateUI();
    }

    public void UpdateStatistics(decimal betAmount, decimal profit, bool isWin)
    {
        totalBets += betAmount;
        totalProfit += profit;

        if (isWin)
        {
            wins++;
        }
        else
        {
            losses++;
        }

        // Zapisz statystyki
        SaveStatistics();
        UpdateUI();
    }

    public void SaveStatistics()
    {
        ProtectedPlayerPrefs.SetFloat("TotalBets" + PlayFabManager._Instance.currentCryptoTag, (float)totalBets);
        ProtectedPlayerPrefs.SetFloat("TotalProfit" + PlayFabManager._Instance.currentCryptoTag, (float)totalProfit);
        ProtectedPlayerPrefs.SetInt("Wins" + PlayFabManager._Instance.currentCryptoTag, wins);
        ProtectedPlayerPrefs.SetInt("Losses" + PlayFabManager._Instance.currentCryptoTag, losses);
        ProtectedPlayerPrefs.Save();
    }

    public void LoadStatistics()
    {
        totalBets = (decimal)ProtectedPlayerPrefs.GetFloat("TotalBets" + PlayFabManager._Instance.currentCryptoTag, 0f);
        totalProfit =(decimal) ProtectedPlayerPrefs.GetFloat("TotalProfit" + PlayFabManager._Instance.currentCryptoTag, 0f);
        wins = ProtectedPlayerPrefs.GetInt("Wins" + PlayFabManager._Instance.currentCryptoTag, 0);
        losses = ProtectedPlayerPrefs.GetInt("Losses" + PlayFabManager._Instance.currentCryptoTag, 0);
        UpdateUI();

    }

    private void UpdateUI()
    {
        totalBetsText.text = "Total bet \n<b>" +  totalBets.ToString("0.########") + " " + PlayFabManager._Instance.currentCryptoTag;
     //   totalBetsText.text = "Total bet \n<b>" +  totalBets.ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag;
      //  totalProfitText.text = "Total profit \n<b>" + totalProfit.ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag;
        totalProfitText.text = "Total profit \n<b>" + totalProfit.ToString("0.########") + " " + PlayFabManager._Instance.currentCryptoTag;
        winsText.text = "Wins <color=green><b>"  + wins.ToString();
        lossesText.text = "Lose <color=red><b>" + losses.ToString();
    }

    public void ResetStatistics()
    {
        totalBets = 0;
        totalProfit = 0;
        wins = 0;
        losses = 0;

        SaveStatistics();
        UpdateUI();
    }
}
