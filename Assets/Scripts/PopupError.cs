using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupError : MonoBehaviour
{
    // Referencja do Panelu b³êdu
    public static PopupError _Instance;
    public GameObject errorPanel;

    // Referencja do komponentu Text, który wyœwietla wiadomoœæ b³êdu
    public TextMeshProUGUI errorMessageText;

    // Czas trwania animacji pop-up (sekundy)
    public float animationDuration = 0.5f;

    // Czas wyœwietlania komunikatu (sekundy)
    public float displayDuration = 2.0f;

    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        // Upewnij siê, ¿e panel pocz¹tkowo jest niewidoczny (schowany)
        errorPanel.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Wywo³aj ten metodê, aby wyœwietliæ komunikat b³êdu
    /// </summary>
    /// <param name="message">Treœæ b³êdu</param>
    public void ShowError(string message)
    {
        LeanTween.cancelAll(this.gameObject);
        // Ustaw wiadomoœæ b³êdu w elemencie Text
        errorMessageText.text = message;

        // Animuj panel, aby siê pojawi³ (powiêkszanie do pe³nego rozmiaru)
        LeanTween.scale(errorPanel, Vector3.one, animationDuration).setEase(LeanTweenType.easeOutBack);

        // Poczekaj chwilê, a nastêpnie zniknij panel
        Invoke(nameof(HideError), displayDuration);
    }

    /// <summary>
    /// Ukryj panel b³êdu z animacj¹
    /// </summary>
    private void HideError()
    {
        LeanTween.cancelAll(this.gameObject);

        // Animuj panel, aby siê schowa³ (zmniejszanie do zera)
        LeanTween.scale(errorPanel, Vector3.zero, animationDuration).setEase(LeanTweenType.easeInBack);
    }
}
