using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OPS;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using OPS.AntiCheat.Prefs;

public class UIManager : MonoBehaviour
{
    public static UIManager _Instance;

    public TMP_InputField potentialWinInput;
    public TMP_InputField betAmountInput;
    public TMP_InputField multiplierInput;
    public TMP_InputField chanceInput;
    public TMP_InputField aboveInput;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI totalBetText;
    public TextMeshProUGUI balanceText;
    public Slider chanceSlider;
    public Button betButton;
    public Slider arrowSlider;
    public TextMeshProUGUI arrowSliderValue;
    public ProtectedFloat winNumber;
    public Button manualBtn;
    public Button automatBtn;

    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI betAutomaticAmountText;
    
    

    private float balance = 1000f;

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        UIManager._Instance.arrowSlider.gameObject.SetActive(false);
        betAmountInput.text = "0";
        betButton.onClick.AddListener(OnBetButtonClicked);
        chanceSlider.onValueChanged.AddListener(OnChanceSliderChanged);
        betAmountInput.onEndEdit.AddListener(OnBetChanged);
        chanceInput.onEndEdit.AddListener(OnChanceInputEndEdit); // Dodajemy obs³ugê zmiany wartoœci w InputField
        chanceSlider.minValue = 2f;
        chanceSlider.maxValue = 98f;
        chanceSlider.wholeNumbers = true;
        UpdateReverseChance();

        chanceSlider.value = 50f; // Ustawienie pocz¹tkowej wartoœci na 50% szans
        UpdateUI();
    }
    private void Update()
    {
        betAmountText.text = "Bet amount <size=15>(minimum " + ((float)CryptoPriceReader._Instance.currentMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag + ")";
        betAutomaticAmountText.text = "Bet amount <size=15>(minimum " + ((float)CryptoPriceReader._Instance.currentMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag + ")";
        betAmountInput.characterLimit = CryptoPriceReader._Instance.currentCharacterLimit;
    }
    public void UpdateSliderResultArrow(float result)
    {
        // Aktualizacja pozycji strza³ki wyniku na suwaku
      //  float sliderValue = chanceSlider.value;
        arrowSlider.value = (result / 100);
        arrowSliderValue.text = (arrowSlider.value * 100).ToString("00.00");
    }
    void UpdateUI()
    {
        balanceText.text = balance.ToString("0.00000000") + " " + PlayFabManager._Instance.currentCryptoTag;
        profitText.text = "0.00000000 " + PlayFabManager._Instance.currentCryptoTag;
        totalBetText.text = "0.00000000 " + PlayFabManager._Instance.currentCryptoTag;
       
    }
    private void UpdateReverseChance()
    {
        float chance;
        if (float.TryParse(chanceInput.text, out chance))
        {
            aboveInput.text = (100f - chance).ToString("0.00");
        }
    }
    public void OnBetButtonClicked()
    {
        if (betAmountInput.text.Length <= 0 || betAmountInput.text == "0")
        {
            PopupError._Instance.ShowError("Bet amount is null");
            return;

        }
        if (  CryptoPriceReader._Instance.currentMinimumBet > float.Parse(betAmountInput.text))
        {
            PopupError._Instance.ShowError("Bet amount is lower than minimum");

            return;
        }
        if (CryptoPriceReader._Instance.currentMinimumBet <= float.Parse(betAmountInput.text) && float.Parse(betAmountInput.text) > CryptoPriceReader._Instance.currentBalance)
        {
            PopupError._Instance.ShowError("You do not have enough balance");

            return;
        }

        if (betAmountInput.text.Length > 0 && betAmountInput.text != "0" && float.Parse(betAmountInput.text) > 0 && CryptoPriceReader._Instance.currentMinimumBet <= float.Parse(betAmountInput.text) && float.Parse(betAmountInput.text) <= CryptoPriceReader._Instance.currentBalance)
        {
            chanceSlider.interactable = false;

            decimal betAmount = decimal.Parse(betAmountInput.text);
            decimal multiplier = decimal.Parse(multiplierInput.text);
            float chance = float.Parse(chanceInput.text);
            betButton.interactable = false;
            // Oblicz potencjalny zysk
            decimal potentialProfit = betAmount * multiplier;

            // Aktualizuj UI z wynikiem
            profitText.text = potentialProfit.ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag; 
          //  totalBetText.text = betAmount.ToString("0.0000000000") + " " + PlayFabManager._Instance.currentCryptoTag;
            totalBetText.text = betAmount.ToString(CryptoPriceReader._Instance.currentDivider) + " " + PlayFabManager._Instance.currentCryptoTag;

            // Wywo³aj metodê obstawiania
            BettingManager.Instance.PlaceBet(betAmount, multiplier.ToString("0.00"), chance, PlayFabManager._Instance.currentCryptoTag.ToString());
        }
       
    }
    public void OnBetChanged(string value)
    {
        CalculateMultiplier(chanceSlider.value);
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

    float CalculateMultiplier(float chance)
    {
        // if (chance <= 50f)
        // {
        //     // Obliczenie dla szansy od 2% do 50%
        //     float a = Mathf.Log(49.5f / 1.98f) / Mathf.Log(50f / 2f);
        //     float b = 1.98f * Mathf.Pow(50f, a);
        //
        //     potentialWinInput.text = ((b / Mathf.Pow(chance, a) * float.Parse(betAmountInput.text)) - float.Parse(betAmountInput.text)).ToString(CryptoPriceReader._Instance.currentDivider);
        //     return b / Mathf.Pow(chance, a);
        // }
        // else
        // {
        //     // Obliczenie dla szansy od 50% do 98%
        //    // float a = Mathf.Log(1.98f / 1.0102f) / Mathf.Log(50f / 98f);
        //     float a = Mathf.Log(1.98f / 1.01f) / Mathf.Log(50f / 98f);
        //     float b = 1.98f / Mathf.Pow(50f, a);
        //     potentialWinInput.text = ((b * Mathf.Pow(chance, a) * float.Parse(betAmountInput.text)) - float.Parse(betAmountInput.text)).ToString(CryptoPriceReader._Instance.currentDivider);
        //
        //     return b * Mathf.Pow(chance, a);
        //
        // }

        if ((float)chance <= 50f)
        {
            // Obliczenie dla szansy od 2% do 50%
            decimal a = (decimal)Mathf.Log(49.5f / 1.98f) / (decimal)Mathf.Log(50f / 2f);
            decimal b = 1.98m * (decimal)Mathf.Pow(50f, (float)a);

            //     float potentialWin = (float)((b / (decimal)Mathf.Pow(chance, (float)a) * decimal.Parse(betAmountInput.text)) - decimal.Parse(betAmountInput.text));
            decimal potentialWin = (decimal)((b / (decimal)Mathf.Pow(chance, (float)a) * decimal.Parse(betAmountInput.text)) - decimal.Parse(betAmountInput.text));
            string toMultiplier = ((float)(b * (decimal)Mathf.Pow(chance, (float)a))).ToString("0.00");

              potentialWinInput.text = potentialWin.ToString("0.#######");
           // potentialWinInput.text = ((decimal.Parse(betAmountInput.text) * decimal.Parse(toMultiplier)) - decimal.Parse(betAmountInput.text)).ToString();

            return (float)(b / (decimal)Mathf.Pow(chance, (float)a));
        }
        else
        {
            // Obliczenie dla szansy od 50% do 98%
            decimal a = (decimal)Mathf.Log(1.98f / 1.01f) / (decimal)Mathf.Log(50f / 98f);
            decimal b = 1.98m / (decimal)Mathf.Pow(50f, (float)a);

            //  float potentialWin = (float)((b * (decimal)Mathf.Pow(chance, (float)a) * decimal.Parse(betAmountInput.text)) - decimal.Parse(betAmountInput.text));
            decimal potentialWin = (decimal)((b * (decimal)Mathf.Pow(chance, (float)a) * decimal.Parse(betAmountInput.text)) - decimal.Parse(betAmountInput.text));
            string toMultiplier = ((float)(b * (decimal)Mathf.Pow(chance, (float)a))).ToString("0.#######");

            potentialWinInput.text = ((decimal.Parse(betAmountInput.text) * decimal.Parse(toMultiplier)) - decimal.Parse(betAmountInput.text)).ToString("0.#######");

            return (float)(b * (decimal)Mathf.Pow(chance, (float)a));
        }

    }

    public void UpdateBalance(decimal newBalance)
    {
        balance = (float)newBalance;
        UpdateUI();
    }
}