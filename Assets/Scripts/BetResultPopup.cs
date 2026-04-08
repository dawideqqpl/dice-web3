using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetResultPopup : MonoBehaviour
{
    private const float POPUP_DISPLAY_DURATION = 2f;
    private const float POPUP_ANIMATE_DURATION = 0.5f;

    public GameObject popupPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI profitText;
    public Image colorBack;
    public Sprite redBack;
    public Sprite greenBack;
    public static BetResultPopup _Instance;
    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        popupPanel.SetActive(false); // Na pocz�tku panel jest ukryty
    }

    public void ShowResultPopup(bool isWin, decimal  betAmount, decimal profit)
    {
        UIManager._Instance.arrowSlider.gameObject.SetActive(true);
        LeanTween.cancel(popupPanel.GetComponent<RectTransform>());
        LeanTween.cancelAll();
        popupPanel.SetActive(true);

        // Ustawienie tekst�w w zale�no�ci od wyniku
        if (isWin)
        {
            resultText.text = "Win!";
            profitText.text = "Profit: +"+ profit + " " +PlayFabManager._Instance.currentCryptoTag; 
            // profitText.color = Color.green;
            colorBack.sprite = greenBack;
        }
        else
        {
            resultText.text = "Lose";
            profitText.text = "Profit: " + profit + " "  + PlayFabManager._Instance.currentCryptoTag;
            // profitText.color = Color.red;
            colorBack.sprite = redBack;
        }

        betAmountText.text = "Bet: " + betAmount + " " + PlayFabManager._Instance.currentCryptoTag;

        // Animacja pop-up przy u�yciu LeanTween
        popupPanel.transform.localScale = Vector3.zero; // Ustaw skal� na 0 na pocz�tku
        LeanTween.scale(popupPanel, Vector3.one, POPUP_ANIMATE_DURATION).setEaseOutBack();

        LeanTween.delayedCall(POPUP_DISPLAY_DURATION, () =>
        {
            LeanTween.scale(popupPanel, Vector3.zero, POPUP_ANIMATE_DURATION).setEaseInBack().setOnComplete(() =>
            {
                popupPanel.SetActive(false); // Ukrycie panelu po zako�czeniu animacji
            });
        });
    }
}
