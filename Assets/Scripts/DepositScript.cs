using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DepositScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static DepositScript _Instance;
    public GameObject DepositObject;
    public TextMeshProUGUI minimumDepositText;
    public TextMeshProUGUI chooseCryptoTitle;
    public TextMeshProUGUI nextDepositText;
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
        if (DepositObject.active == true)
        {
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "ETH")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.ethMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "USDT")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "USDC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "SHIB")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.shibMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "CRO")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.cronosMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "FDUSD")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.fdusdMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "MATIC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "BNB")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.bnbMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }

                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "APE")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.apeMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text == "LINK")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;

                }




            }
            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "ETH")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.ethMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "USDT")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "USDC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
             
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "FDUSD")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.fdusdMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "MATIC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "BNB")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.bnbMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }

             
                
                if (CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text == "LINK")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;

                }
            }
            if (PlayerPrefs.GetString("NewChain") == "89")
            {
              
                if (CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text == "USDT")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdtMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text;

                }
                if (CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text == "USDC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.usdcMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text;

                }
              
                if (CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text == "MATIC")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.maticMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text;

                }
             
                if (CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text == "LINK")
                {
                    minimumDepositText.text = "Minimum deposit: " + ((float)CryptoPriceReader._Instance.linkMinimumBet).ToString(CryptoPriceReader._Instance.currentDivider) + " " + CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text;

                }
            }
        }
    }


}
