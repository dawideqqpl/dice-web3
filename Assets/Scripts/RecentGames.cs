using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using OPS.AntiCheat.Prefs;


public class RecentGames : MonoBehaviour
{
    public GameObject recentGamePrefab; // Prefab dla ka¿dej recent game
    public Transform recentGamesContainer; // Kontener, w którym umieszczamy recent games

    private List<GameObject> recentGames = new List<GameObject>();
    private const int maxRecentGames = 99;
    private int betNumber = 0; // Numer zak³adu

    private void Start()
    {
        LoadRecentGames();
                    betNumber = ProtectedPlayerPrefs.GetInt("TotalBetNumber", 0); // Wczytanie ostatniego numeru zak³adu

    }

    public void AddGame(string time, decimal betAmount, float multiplier, float chance, bool isWin, decimal profit)
    {
        // Tworzenie nowego recent game
        GameObject newGame = Instantiate(recentGamePrefab, recentGamesContainer);
        betNumber += 1;
        ProtectedPlayerPrefs.SetInt("TotalBetNumber", betNumber); // Zapisanie numeru zak³adu w PlayerPrefs

        // Ustawienie odpowiednich wartoœci tekstowych
        newGame.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = betNumber.ToString();

        newGame.transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = time;
        newGame.transform.Find("BetAmountText").GetComponent<TextMeshProUGUI>().text = betAmount.ToString("0.##########") + " " + PlayFabManager._Instance.currentCryptoTag;
       // newGame.transform.Find("BetAmountText").GetComponent<TextMeshProUGUI>().text = Mathf.Abs((float)betAmount).ToString("0.##########") + " " + PlayFabManager._Instance.currentCryptoTag;
        newGame.transform.Find("MultiplierText").GetComponent<TextMeshProUGUI>().text = multiplier.ToString("0.00") + "x";
        newGame.transform.Find("ChanceText").GetComponent<TextMeshProUGUI>().text = chance.ToString("0.00") + "%";
        newGame.transform.Find("WinLossText").GetComponent<TextMeshProUGUI>().text = isWin ? "Win" : "Loss";
        newGame.transform.Find("ProfitText").GetComponent<TextMeshProUGUI>().text = (isWin ? "+" : "-") + Mathf.Abs((float)profit).ToString("0.##########") + " " + PlayFabManager._Instance.currentCryptoTag;
        newGame.transform.Find("ProfitText").GetComponent<TextMeshProUGUI>().color = isWin ? Color.green : Color.red;

        // Dodanie nowej gry do listy recent games
        recentGames.Add(newGame);

        // Zapisywanie recent game w PlayerPrefs
        SaveRecentGames();

        // Ograniczenie liczby recent games do np. 10
        if (recentGames.Count > maxRecentGames)
        {
            Destroy(recentGames[0]);
            recentGames.RemoveAt(0);
        }
    }

    private void SaveRecentGames()
    {
        // Zapisz ka¿d¹ grê w PlayerPrefs jako JSON
        for (int i = 0; i < recentGames.Count; i++)
        {
            var game = recentGames[i];
            string number = game.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text;

            string time = game.transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text;
            string betAmount = game.transform.Find("BetAmountText").GetComponent<TextMeshProUGUI>().text;
            string multiplier = game.transform.Find("MultiplierText").GetComponent<TextMeshProUGUI>().text;
            string chance = game.transform.Find("ChanceText").GetComponent<TextMeshProUGUI>().text;
            string winLoss = game.transform.Find("WinLossText").GetComponent<TextMeshProUGUI>().text;
            string profit = game.transform.Find("ProfitText").GetComponent<TextMeshProUGUI>().text;

            RecentGameData gameData = new RecentGameData
            {
                Number = number,
                Time = time,
                BetAmount = betAmount,
                Multiplier = multiplier,
                Chance = chance,
                WinLoss = winLoss,
                Profit = profit
            };

            string jsonData = JsonUtility.ToJson(gameData);
            ProtectedPlayerPrefs.SetString("RecentGame_" + i, jsonData);
        }
        ProtectedPlayerPrefs.SetInt("RecentGamesCount", recentGames.Count);
        ProtectedPlayerPrefs.Save();
    }

    private void LoadRecentGames()
    {
        int count = ProtectedPlayerPrefs.GetInt("RecentGamesCount", 0);

        for (int i = 0; i < count; i++)
        {
            string jsonData = ProtectedPlayerPrefs.GetString("RecentGame_" + i, "");

            if (!string.IsNullOrEmpty(jsonData))
            {
                RecentGameData gameData = JsonUtility.FromJson<RecentGameData>(jsonData);

                // Tworzenie nowego recent game na podstawie zapisanych danych
                GameObject newGame = Instantiate(recentGamePrefab, recentGamesContainer);
                newGame.transform.Find("NumberText").GetComponent<TextMeshProUGUI>().text = gameData.Number;

                newGame.transform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = gameData.Time;
                newGame.transform.Find("BetAmountText").GetComponent<TextMeshProUGUI>().text = gameData.BetAmount;
                newGame.transform.Find("MultiplierText").GetComponent<TextMeshProUGUI>().text = gameData.Multiplier;
                newGame.transform.Find("ChanceText").GetComponent<TextMeshProUGUI>().text = gameData.Chance;
                newGame.transform.Find("WinLossText").GetComponent<TextMeshProUGUI>().text = gameData.WinLoss;
                newGame.transform.Find("ProfitText").GetComponent<TextMeshProUGUI>().text = gameData.Profit;
                newGame.transform.Find("ProfitText").GetComponent<TextMeshProUGUI>().color = gameData.WinLoss == "Win" ? Color.green : Color.red;

                recentGames.Add(newGame);
            }
        }
    }
}

[System.Serializable]
public class RecentGameData
{
    public string Number;

    public string Time;
    public string BetAmount;
    public string Multiplier;
    public string Chance;
    public string WinLoss;
    public string Profit;
}
