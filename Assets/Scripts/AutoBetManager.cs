using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using OPS;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using Org.BouncyCastle.Math.EC.Multiplier;
using UnityEngine.Windows;
using NBitcoin;
public class AutoBetManager : MonoBehaviour
{
    public TMP_InputField betAmountInput;
    public TMP_InputField chanceInput;
    public TMP_InputField multiplierInput;
    public TMP_InputField numberOfBetsInput;
    public TMP_InputField stopOnProfitInput;
    public TMP_InputField stopOnLossInput;

    public Button resetOnWinToggle;
    public Button increaseOnWinToggle;
    public Button resetOnLoseToggle;

    public Button increaseOnLossToggle;

    public TMP_InputField increaseOnWinPercentageInput;
    public TMP_InputField increaseOnLossPercentageInput;

    public TMP_InputField aboveInput;


    public Slider chanceSlider;

    public Button startAutoBetButton;
    public Button stopAutoBetButton;

    public Slider arrowSlider;
    public TextMeshProUGUI arrowSliderValue;

    public bool isAutoBetting = false;
    public decimal totalProfit = 0;
    public decimal betAmount;
    public float winNumber;

    public ProtectedInt16 currentBetCount;

    public Button depositBtn;
    public Button withdrawBtn;

    public static AutoBetManager _Instance;

    public ProtectedBool isStopped;

    private void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        arrowSlider.gameObject.SetActive(false);

        resetOnWinToggle.onClick.AddListener(() => OnButton1Click());
        increaseOnWinToggle.onClick.AddListener(() => OnButton2Click());
        resetOnLoseToggle.onClick.AddListener(() => OnButton3Click());
        increaseOnLossToggle.onClick.AddListener(() => OnButton4Click());
        chanceSlider.onValueChanged.AddListener(OnChanceSliderChanged);

        startAutoBetButton.onClick.AddListener(StartAutoBetting);
       // stopAutoBetButton.onClick.AddListener(StopAutoBetting);
        chanceInput.onEndEdit.AddListener(OnChanceInputEndEdit); // Dodajemy obs³ugê zmiany wartoœci w InputField
        stopOnProfitInput.text = "0";
        stopOnLossInput.text = "0";
        chanceSlider.minValue = 2f;
        chanceSlider.maxValue = 98f;
        chanceSlider.wholeNumbers = true;
        increaseOnWinPercentageInput.text = "0";
        increaseOnLossPercentageInput.text = "0";
        OnButton1Click();
        OnButton3Click();
        chanceSlider.value = 50f; // Ustawienie pocz¹tkowej wartoœci na 50% szans
        UpdateReverseChance();

    }
    public void UpdateSliderResultArrow()
    {
        arrowSlider.gameObject.SetActive(true);

        // Aktualizacja pozycji strza³ki wyniku na suwaku
        float sliderValue = chanceSlider.value;
        arrowSlider.value = (winNumber / 100);
        arrowSliderValue.text = (arrowSlider.value * 100).ToString("00.00");
    }


    void OnButton1Click()
    {
        if (isAutoBetting == false)
        {


            // Deactivate Button2 and reset Button1 (if needed)
            increaseOnWinToggle.GetComponent<Image>().color = new Color(0.509434f, 0.509434f, 0.509434f);
            resetOnWinToggle.GetComponent<Image>().color = Color.white;
            increaseOnWinPercentageInput.interactable = false;
        }
    }

    void OnButton2Click()
    {
        if (isAutoBetting == false)
        {

            // Deactivate Button1 and reset Button2 (if needed)
            resetOnWinToggle.GetComponent<Image>().color = new Color(0.509434f, 0.509434f, 0.509434f);
            increaseOnWinToggle.GetComponent<Image>().color = Color.white;
            increaseOnWinPercentageInput.interactable = true;
        }

    }
    void OnButton3Click()
    {
        if (isAutoBetting == false)
        {

            // Deactivate Button1 and reset Button2 (if needed)
            increaseOnLossToggle.GetComponent<Image>().color = new Color(0.509434f, 0.509434f, 0.509434f);
            resetOnLoseToggle.GetComponent<Image>().color = Color.white;
            increaseOnLossPercentageInput.interactable = false;
        }

    }
    void OnButton4Click()
    {
        if (isAutoBetting == false)
        {

            // Deactivate Button1 and reset Button2 (if needed)
            resetOnLoseToggle.GetComponent<Image>().color = new Color(0.509434f, 0.509434f, 0.509434f);
            increaseOnLossToggle.GetComponent<Image>().color = Color.white;
            increaseOnLossPercentageInput.interactable = true;
        }
    }
    public void AutoBetIsON()
    {
        int numberOfBets = int.Parse(numberOfBetsInput.text);
        float stopOnProfit = float.Parse(stopOnProfitInput.text);
        float stopOnLoss = float.Parse(stopOnLossInput.text);
        decimal winAmount = 0;
        decimal oneProfit = 0;

        // Symulacja zak³adu (tu mo¿na umieœciæ rzeczywiste logiki obstawiania)
        bool isWin = SimulateBet();

        if (isWin)
        {
            //  float winAmount = betAmount * CalculateMultiplier(chanceSlider.value);
            winAmount = betAmount * decimal.Parse(multiplierInput.text);
            totalProfit += (winAmount - betAmount);
            oneProfit += (winAmount - betAmount);
            if (resetOnWinToggle.interactable == true)
            {
                betAmount = decimal.Parse(betAmountInput.text); // Reset do wartoœci pocz¹tkowej
            }

            if (increaseOnWinPercentageInput.text != "0")
            {
                decimal increasePercentage = decimal.Parse(increaseOnWinPercentageInput.text) / 100;
                betAmount += betAmount * increasePercentage;
            }

            //   BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
            //   betResultPopup.ShowResultPopup(isWin, betAmount, (decimal)totalProfit);
        }
        else
        {
            winAmount = -betAmount;
            totalProfit -= betAmount;
            oneProfit = -betAmount;

            if (increaseOnLossToggle.interactable == true && increaseOnLossPercentageInput.text != "0")
            {
                decimal increasePercentage = decimal.Parse(increaseOnLossPercentageInput.text) / 100;
                betAmount += betAmount * increasePercentage;


            }
            //  BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
            //  betResultPopup.ShowResultPopup(isWin, betAmount, (decimal)totalProfit);
        }

        //  RecentGames recentGames = FindObjectOfType<RecentGames>();

        // recentGames.AddGame(System.DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss"), betAmount, float.Parse(multiplierInput.text), float.Parse(chanceInput.text), isWin, (decimal)totalProfit);
        // PlayFabManager._Instance.CallBetResultCloudScript(betAmount.ToString(), multiplierInput.text.ToString(), chanceInput.text.ToString(), isWin.ToString(), totalProfit.ToString(), PlayFabManager._Instance.currentCryptoTag.ToString());





        //      UpdateSliderResultArrow();


        PlayFabManager._Instance.ExecuteCloudScriptBetSetCodeAutomatic(winAmount, betAmount, float.Parse(multiplierInput.text).ToString("0.00"), float.Parse(chanceInput.text), PlayFabManager._Instance.currentCryptoTag.ToString(), isWin, winAmount, winNumber, oneProfit, totalProfit);
    }
    void StartAutoBetting()
    {
      // if (betAmountInput.text.Length > 0 && numberOfBetsInput.text.Length > 0 && float.Parse(betAmountInput.text) * float.Parse(numberOfBetsInput.text) > CryptoPriceReader._Instance.currentBalance)
      // {
      //     PopupError._Instance.ShowError("Not enough balance for this number of bets");
      //     return;
      // }
      if(float.Parse(betAmountInput.text) > CryptoPriceReader._Instance.currentBalance)
        {
            PopupError._Instance.ShowError("You do not have enough balance.");
            return;

        }
        if ((numberOfBetsInput.text.Length == 0))
        {
            PopupError._Instance.ShowError("Number of bets is null");
            return;
        }
        if ((betAmountInput.text.Length == 0))
        {
            PopupError._Instance.ShowError("Bet amount is null");
            return;
        }
      
        if (CryptoPriceReader._Instance.currentMinimumBet > float.Parse(betAmountInput.text))
        {
            PopupError._Instance.ShowError("Bet amount is lower than minimum");

            return;
        }

        //     if (CryptoPriceReader._Instance.currentMinimumBet <= float.Parse(betAmountInput.text) &&  betAmountInput.text.Length > 0 && betAmountInput.text != "0" && float.Parse(betAmountInput.text) > 0 && numberOfBetsInput.text.Length > 0 && numberOfBetsInput.text != "0" && int.Parse(numberOfBetsInput.text) > 0 && isAutoBetting == false && float.Parse(betAmountInput.text) * float.Parse(numberOfBetsInput.text) <= CryptoPriceReader._Instance.currentBalance)
        if (CryptoPriceReader._Instance.currentMinimumBet <= float.Parse(betAmountInput.text) && betAmountInput.text.Length > 0 && betAmountInput.text != "0" && float.Parse(betAmountInput.text) > 0 && numberOfBetsInput.text.Length > 0 && numberOfBetsInput.text != "0" && int.Parse(numberOfBetsInput.text) > 0 && isAutoBetting == false && float.Parse(betAmountInput.text) <= CryptoPriceReader._Instance.currentBalance)
        {
            isStopped = false;
            stopAutoBetButton.gameObject.SetActive(true);

            isAutoBetting = true;
            totalProfit = 0;
            betAmount = decimal.Parse(betAmountInput.text);
            currentBetCount = 0;
            AutoBetIsON();


        }
    }
    private void UpdateReverseChance()
    {
        float chance;
        if (float.TryParse(chanceInput.text, out chance))
        {
            aboveInput.text = (100f - chance).ToString("0.00");
        }
    }
    void StopAutoBetting()
    {
        isAutoBetting = false;
    }
    public void OnChanceSliderChanged(float value)
    {
        chanceInput.text = value.ToString("0.00");
        aboveInput.text = value.ToString("0.00");

        // Oblicz mno¿nik na podstawie wartoœci szansy
        float multiplier = CalculateMultiplier(value);
        multiplierInput.text = multiplier.ToString("0.00");
        UpdateReverseChance();

    }
    public void OnChanceInputEndEdit(string value)
    {
        //   float chance;
        //   if (float.TryParse(value, out chance))
        //   {
        //       // Upewnij siê, ¿e wartoœæ szansy mieœci siê w zakresie
        //       chance = Mathf.Clamp(chance, 2f, 98f);
        //
        //       // Zaktualizuj suwak i mno¿nik
        //       chanceSlider.value = chance;
        //       float multiplier = CalculateMultiplier(chance);
        //       multiplierInput.text = multiplier.ToString("0.00");
        //       UpdateReverseChance();
        //
        //   }

        float chance;
        if (float.TryParse(value, out chance))
        {
            // Upewnij siê, ¿e wartoœæ szansy mieœci siê w zakresie
            chance = Mathf.Clamp(chance, 2f, 98f);

            // Zaktualizuj suwak i mno¿nik
            chanceSlider.value = chance;
            float multiplier = CalculateMultiplier(chance);
            multiplierInput.text = multiplier.ToString("0.00");

            // Aktualizuj równie¿ pole tekstowe z wartoœci¹ szansy
            chanceInput.text = chance.ToString("0.00");

            // Wywo³aj aktualizacjê szansy odwrotnej
            UpdateReverseChance();
        }
        else
        {
            // Jeœli wartoœæ nie jest prawid³ow¹ liczb¹, zresetuj j¹ na domyœln¹ (opcjonalnie)
            chanceInput.text = "2.00";
        }
    }
    public void Update()
    {
        if (isAutoBetting)
        {
            startAutoBetButton.gameObject.SetActive(false);

            PlayFabManager._Instance.cryptoDropdown.interactable = false;
            PlayFabManager._Instance.cryptoDropdownBSC.interactable = false;
            PlayFabManager._Instance.cryptoDropdownMATIC.interactable = false;
            depositBtn.interactable = false;
            withdrawBtn.interactable = false;
            chanceSlider.interactable = false;
            betAmountInput.interactable = false;
            numberOfBetsInput.interactable = false;
            //multiplierInput.interactable = false;
            chanceInput.interactable = false;
            stopOnLossInput.interactable = false;
            stopOnProfitInput.interactable = false;
            increaseOnLossPercentageInput.interactable = false;
            startAutoBetButton.interactable = false;
            increaseOnWinPercentageInput.interactable = false;
            UIManager._Instance.manualBtn.interactable = false;
            UIManager._Instance.automatBtn.interactable = false;
            //   resetOnWinToggle.interactable = false;
            //  increaseOnWinToggle.interactable = false;
            //  resetOnLoseToggle.interactable = false;
            //  increaseOnLossToggle.interactable = false;

        }
        if (!isAutoBetting)
        {
            startAutoBetButton.gameObject.SetActive(true);
            stopAutoBetButton.gameObject.SetActive(false);
            PlayFabManager._Instance.cryptoDropdown.interactable = true;
            PlayFabManager._Instance.cryptoDropdownBSC.interactable = true;
            PlayFabManager._Instance.cryptoDropdownMATIC.interactable = true;
            depositBtn.interactable = true;
            withdrawBtn.interactable = true;
            chanceSlider.interactable = true;
            betAmountInput.interactable = true;
            numberOfBetsInput.interactable = true;
            // multiplierInput.interactable = true;
            chanceInput.interactable = true;
            stopOnLossInput.interactable = true;
            stopOnProfitInput.interactable = true;
            increaseOnLossPercentageInput.interactable = true;
            startAutoBetButton.interactable = true;
            UIManager._Instance.manualBtn.interactable = true;
            UIManager._Instance.automatBtn.interactable = true;
            increaseOnWinPercentageInput.interactable = true;
            //  resetOnWinToggle.interactable = true;
            //  increaseOnWinToggle.interactable = true;
            ////  resetOnLoseToggle.interactable = true;
            //  increaseOnLossToggle.interactable = true;
        }
    }

    public void isStoppedClicked()
    {
        isStopped = true;
        stopAutoBetButton.gameObject.SetActive(false);
    //  StartCoroutine(PlayFabManager._Instance.DelayAfterAutomaitcBet());

    }
    private IEnumerator AutoBetCoroutine()
    {
        int numberOfBets = int.Parse(numberOfBetsInput.text);
        float stopOnProfit = float.Parse(stopOnProfitInput.text);
        float stopOnLoss = float.Parse(stopOnLossInput.text);

        while (isAutoBetting)
        {

            // Symulacja zak³adu (tu mo¿na umieœciæ rzeczywiste logiki obstawiania)
            bool isWin = SimulateBet();

            if (isWin)
            {
              //  float winAmount = betAmount * CalculateMultiplier(chanceSlider.value);
                decimal winAmount = betAmount * decimal.Parse(multiplierInput.text);
                totalProfit += (winAmount - betAmount);

                if (resetOnWinToggle.interactable == true)
                {
                    betAmount = decimal.Parse(betAmountInput.text); // Reset do wartoœci pocz¹tkowej
                }

                if (increaseOnWinPercentageInput.text != "0")
                {
                    decimal increasePercentage = decimal.Parse(increaseOnWinPercentageInput.text) / 100;
                    betAmount += betAmount * increasePercentage;
                }

                BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
                betResultPopup.ShowResultPopup(isWin, betAmount, (decimal)totalProfit);
            }
            else
            {
                totalProfit -= betAmount;

                if (increaseOnLossToggle.interactable == true && increaseOnLossPercentageInput.text != "0")
                {
                    decimal increasePercentage = decimal.Parse(increaseOnLossPercentageInput.text) / 100;
                    betAmount += betAmount * increasePercentage;

                  
                }
                BetResultPopup betResultPopup = FindObjectOfType<BetResultPopup>();
                betResultPopup.ShowResultPopup(isWin, betAmount, (decimal)totalProfit);
            }

            RecentGames recentGames = FindObjectOfType<RecentGames>();

            recentGames.AddGame(System.DateTime.Now.ToString("HH.mm"), betAmount, float.Parse(multiplierInput.text), float.Parse(chanceInput.text), isWin, (decimal)totalProfit);
            PlayFabManager._Instance.CallBetResultCloudScript(betAmount.ToString(), multiplierInput.text.ToString(), chanceInput.text.ToString(), isWin.ToString(), totalProfit.ToString(), PlayFabManager._Instance.currentCryptoTag.ToString());


            if ((stopOnProfit > 0 && totalProfit >=(decimal)stopOnProfit) || (stopOnLoss > 0 && totalProfit <= (decimal)-stopOnLoss))
            {
                UpdateSliderResultArrow();

                yield return new WaitForSeconds(4f);
                StopAutoBetting();
                break;
            }

            // Aktualizacja UI (opcjonalne)
            UpdateUI();
            UpdateSliderResultArrow();
            yield return new WaitForSeconds(4f); // Przerwa miêdzy zak³adami wynosi teraz 3 sekundy
        }

        StopAutoBetting(); // Zakoñczenie trybu automatycznego
    }

    private bool SimulateBet()
    {
        winNumber = Random.Range(0f, 100f);
        // Tu nale¿y zaimplementowaæ logikê rzeczywistego zak³adu lub symulacji
       // return winNumber >  (chanceSlider.value / 100f); // Prosta symulacja zak³adu
        return winNumber > ( 100 - float.Parse(chanceInput.text)); // Prosta symulacja zak³adu
    }
    float CalculateMultiplier(float chance)
    {
        if (chance <= 50f)
        {
            // Obliczenie dla szansy od 2% do 50%
            float a = Mathf.Log(49.5f / 1.9800f) / Mathf.Log(50f / 2f);
            float b = 1.9800f * Mathf.Pow(50f, a);
            return b / Mathf.Pow(chance, a);
        }
        else
        {
            // Obliczenie dla szansy od 50% do 98%
            float a = Mathf.Log(1.9800f / 1.0102f) / Mathf.Log(50f / 98f);
            float b = 1.9800f / Mathf.Pow(50f, a);
            return b * Mathf.Pow(chance, a);
        }
    }
    private void UpdateUI()
    {
        // Tu mo¿na dodaæ aktualizacjê UI
        // Na przyk³ad: aktualizacja sumy postawionych zak³adów, zysków itp.
    }
}
