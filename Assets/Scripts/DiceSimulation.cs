using UnityEngine;

public class DiceSimulation : MonoBehaviour
{
    public float numberOfBets = 10000; // Liczba zak³adów, które chcesz przetestowaæ
    public float betAmount = 1; // Kwota pojedynczego zak³adu
    public float winMultiplier = 1.98f; // Mno¿nik wygranej (np. wygrana = 2x zak³ad)

    void Start()
    {
        SimulateBets();
    }

    void SimulateBets()
    {
        float totalBets = numberOfBets;
        float totalWinnings = 0;
        float winnings = 0;
        float loses = 0;

        for (int i = 0; i < totalBets; i++)
        {
            bool win = RollDice();
            if (win)
            {
                winnings += 1;
                totalWinnings += betAmount * winMultiplier;
            }
            else
            {
                loses += 1;
                totalWinnings -= betAmount;
            }
        }

        Debug.Log("Total won " + winnings);
        Debug.Log("Total lose " + loses);
        Debug.Log("Total spend " + numberOfBets * betAmount);
        Debug.Log("Total winnings after " + numberOfBets + " bets: " + totalWinnings);
    }

    bool RollDice()
    {
        float diceRoll = Random.Range(0, 100); // Losowanie liczby od 1 do 6 (rzut kostk¹)

        float winCondition = 50; // Warunek wygranej, np. wygrana jeœli wypadnie 6

        if(winCondition > diceRoll)  
        {
            return true; // Wygrana
        }

        return false; // Przegrana
    }
}
