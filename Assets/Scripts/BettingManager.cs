using UnityEngine;

public class BettingManager : MonoBehaviour
{
    public static BettingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaceBet(decimal betAmount, string multiplier, float chance, string crypto)
    {
        // Generowanie losowego wyniku
        float roll = Random.Range(0f, 100f);
        decimal winAmount = 0;
        bool isWinner = false;
       // Debug.Log("CHANCE " + chance);
        float newChance = 100 - chance;
        decimal valueToUpdate = 0;
        // Sprawdzenie, czy gracz wygra³
        if (roll > newChance)
        {
             winAmount = betAmount * decimal.Parse(multiplier);
            isWinner = true;
            valueToUpdate = winAmount;
           // UpdateBalance(winAmount);
        }
        else
        {
            isWinner = false;
            valueToUpdate = -betAmount;

            //  UpdateBalance(-betAmount);
        }



        decimal profit = ((decimal)winAmount - (decimal)betAmount);
        Debug.Log("VALUE TO UPDATE " + valueToUpdate);
        PlayFabManager._Instance.ExecuteCloudScriptBetSetCode(valueToUpdate, betAmount, multiplier, chance, crypto, isWinner, winAmount, roll, profit);



 
    }

    public void UpdateBalance(decimal amount)
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        decimal newBalance = decimal.Parse(uiManager.balanceText.text.Split(' ')[0]) + amount;
        uiManager.UpdateBalance(newBalance);
    }
}
