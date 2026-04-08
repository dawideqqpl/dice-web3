using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WithdrawScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static WithdrawScript _Instance;
    public GameObject WithdrawObject;
    public TextMeshProUGUI minimumWithdrawText;
    public TextMeshProUGUI chooseCryptoTitle;
    public TMP_InputField walletInput;

    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (WithdrawObject.active == true)
        {
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "ETH")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.ethMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "USDT")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "USDC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "SHIB")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.shibMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "CRO")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.cronosMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "FDUSD")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.fdusdMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "MATIC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "BNB")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.bnbMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }

                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "APE")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.apeMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text == "LINK")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdraw.options[CryptoWithdrawHistory._Instance.cryptoWithdraw.value].text;

                }




            }
            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "ETH")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.ethMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "USDT")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "USDC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }

                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "FDUSD")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.fdusdMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "MATIC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text == "BNB")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.bnbMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.options[CryptoWithdrawHistory._Instance.cryptoWithdrawtBEP.value].text;

                }



                if (CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text == "LINK")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text;

                }
            }
            if (PlayerPrefs.GetString("NewChain") == "89")
            {

                if (CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text == "USDT")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text;

                }
                if (CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text == "USDC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text;

                }

                if (CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text == "MATIC")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text;

                }

                if (CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text == "LINK")
                {
                    minimumWithdrawText.text = "Minimum withdraw: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.options[CryptoWithdrawHistory._Instance.cryptoWithdrawMATIC.value].text;

                }
            }
        }
    }


}
