using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using OPS;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using OPS.AntiCheat.Prefs;
using TMPro;

public class Web3PrivateKeySend20Example : MonoBehaviour
{
    public static Web3PrivateKeySend20Example _Instance;
    public ProtectedString response;
    public GameObject withdrawObject;

    public Sprite greenSprite;
    public Sprite greySprite;
    public ProtectedString whatWithdraw;

    private void Awake()
    {
        _Instance = this;
    }
    public void WithdrawClicked()
    {
        PlayFabManager._Instance.loadingObject.transform.Find("back").gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Loading...";


        if (PlayerPrefs.GetString("NewChain") == "1")
        {


            whatWithdraw = CryptoDepositHistory._Instance.cryptoDeposit.options[CryptoDepositHistory._Instance.cryptoDeposit.value].text;
        }

        if (PlayerPrefs.GetString("NewChain") == "56")
        {


            whatWithdraw = CryptoDepositHistory._Instance.cryptoDepositBEP.options[CryptoDepositHistory._Instance.cryptoDepositBEP.value].text;
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {


            whatWithdraw = CryptoDepositHistory._Instance.cryptoDepositMATIC.options[CryptoDepositHistory._Instance.cryptoDepositMATIC.value].text;
        }
        if (WithdrawScript._Instance.walletInput.text.Length > 0 && CryptoWithdrawHistory._Instance.withdrawInput.text.Length > 0 && decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) >= (ProtectedDecimal)CryptoPriceReader._Instance.currentMinimumBet)
        {
            if (whatWithdraw == "USDT")
            {
                if(decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.USDT)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "USDC")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.USDC)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "MATIC")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.MATIC)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "ETH")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.ETH)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "FDUSD")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.FDUSD)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "BNB")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.BNB)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "SHIB")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.SHIBA)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "APE")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.APE)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "CRO")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.CRO)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
            if (whatWithdraw == "LINK")
            {
                if (decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text) <= (ProtectedDecimal)PlayFabManager._Instance.LINK)
                {
                    PlayFabManager._Instance.loadingObject.gameObject.SetActive(true);

                    PlayFabManager._Instance.ExecuteCloudScriptWithdrawSetCode();

                }
            }
        }
    }


    public void WithdrawToken()
    {
        OnWithdraw();
    }
    async public void OnWithdraw()
    {

        if (PlayerPrefs.GetString("NewChain") == "1")
        {

            // private key of account
            string privateKey = "0x9ebe4f58bbb5947116467002ff731af759b7718bdce6da0f3a012dcde3888f09";
            // set chain: ethereum, moonbeam, polygon etc
            string chain = "eth";
            // set network mainnet, testnet
            string network = "mainnet";
            // smart contract method to call
            string method = "setTokenPOODLE";
            // account of player 
            string account = Web3PrivateKey.Address(privateKey);
            // smart contract address: https://goerli.etherscan.io/address/0xc7ad46e0b8a400bb3c915120d284aafba8fc4735
            string contract = "0x4b8929d19330a2d6597Ce7A9b94b7a63B72DF568";
            // account to send to
            string toAccount = ProtectedPlayerPrefs.GetString("Wallet");
            // amount of erc20 tokens to send. usually 18 decimals
            string amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000000000000000";
            // amount of wei to send
            string value = "0";
            // abi to interact with contract


            string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdrPOODLE\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"TokenAdrZOINK\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeDepositWallet\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"depositWallet\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawFLR\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenPOODLE\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenZOINK\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

            //  string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdr\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"smartContract\",\"type\":\"address\"}],\"name\":\"changeSmartContract\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawETH\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerToken\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            // optional rpc url
            string rpc = "https://eth.llamarpc.com";

            string[] obj = { toAccount, amount };
            string args = JsonConvert.SerializeObject(obj);
            string chainId = await EVM.ChainId(chain, network, rpc);
            string gasPrice = await EVM.GasPrice(chain, network, rpc);
            string data = await EVM.CreateContractData(abi, method, args);
            string gasLimit = "150000";
            string transaction = await EVM.CreateTransaction(chain, network, account, contract, value, data, gasPrice, gasLimit, rpc);
            string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
            response = await EVM.BroadcastTransaction(chain, network, account, contract, value, data, signature, gasPrice, gasLimit, rpc);
            print(response);

            TxStatus._Instance.checkSetToken = true;
            TxStatus._Instance.checkSetTimer = 0;
            TxStatus._Instance.CheckSetToken(response);

            //Application.OpenURL("https://goerli.etherscan.io/tx/" + response);




        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {

            // private key of account
            string privateKey = "0x9ebe4f58bbb5947116467002ff731af759b7718bdce6da0f3a012dcde3888f09";
            // set chain: ethereum, moonbeam, polygon etc
            string chain = "bsc";
            // set network mainnet, testnet
            string network = "mainnet";
            // smart contract method to call
            string method = "setTokenPOODLE";
            // account of player 
            string account = Web3PrivateKey.Address(privateKey);
            // smart contract address: https://goerli.etherscan.io/address/0xc7ad46e0b8a400bb3c915120d284aafba8fc4735
            string contract = "0x4b8929d19330a2d6597Ce7A9b94b7a63B72DF568";
            // account to send to
            string toAccount = ProtectedPlayerPrefs.GetString("Wallet");
            // amount of erc20 tokens to send. usually 18 decimals
            string amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000000000000000";
            // amount of wei to send
            string value = "0";
            // abi to interact with contract


            string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdrPOODLE\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"TokenAdrZOINK\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeDepositWallet\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"depositWallet\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawFLR\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenPOODLE\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenZOINK\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

            //  string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdr\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"smartContract\",\"type\":\"address\"}],\"name\":\"changeSmartContract\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawETH\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerToken\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            // optional rpc url
            string rpc = "https://binance.llamarpc.com";

            string[] obj = { toAccount, amount };
            string args = JsonConvert.SerializeObject(obj);
            string chainId = await EVM.ChainId(chain, network, rpc);
            string gasPrice = await EVM.GasPrice(chain, network, rpc);
            string data = await EVM.CreateContractData(abi, method, args);
            string gasLimit = "150000";
            string transaction = await EVM.CreateTransaction(chain, network, account, contract, value, data, gasPrice, gasLimit, rpc);
            string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
            response = await EVM.BroadcastTransaction(chain, network, account, contract, value, data, signature, gasPrice, gasLimit, rpc);
            print(response);

            TxStatus._Instance.checkSetToken = true;
            TxStatus._Instance.checkSetTimer = 0;
            TxStatus._Instance.CheckSetToken(response);

            //Application.OpenURL("https://goerli.etherscan.io/tx/" + response);




        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            string method = "";
            string amount = "";
            // private key of account
            string privateKey = "0x9ebe4f58bbb5947116467002ff731af759b7718bdce6da0f3a012dcde3888f09";
            // set chain: ethereum, moonbeam, polygon etc
            string chain = "polygon";
            // set network mainnet, testnet
            string network = "mainnet";
            // smart contract method to call
            if (whatWithdraw == "MATIC")
            {


                method = "withdrawMatic";
                amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000000000000000";

            }
            if (whatWithdraw == "USDT")
            {


                method = "withdrawUSDT";
                amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000";

            }
            if (whatWithdraw == "USDC")
            {

                amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000";

                method = "withdrawUSDC";
            }
            if (whatWithdraw == "LINK")
            {
                amount = CryptoWithdrawHistory._Instance.withdrawInput.text + "000000000000000000";


                method = "withdrawLINK";
            }
            // account of player 
            string account = Web3PrivateKey.Address(privateKey);
            // smart contract address: https://goerli.etherscan.io/address/0xc7ad46e0b8a400bb3c915120d284aafba8fc4735
            string contract = "0x4b8929d19330a2d6597Ce7A9b94b7a63B72DF568";
            // account to send to
            string toAccount = ProtectedPlayerPrefs.GetString("Wallet");
            // amount of erc20 tokens to send. usually 18 decimals
            
            // amount of wei to send
            string value = "0";
            // abi to interact with contract


            string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdrPOODLE\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"TokenAdrZOINK\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeDepositWallet\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"depositWallet\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawFLR\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenPOODLE\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerTokenZOINK\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenPOODLE\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawTokenZOINK\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

            //  string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"TokenAdr\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ceoAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"changeCeo\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"smartContract\",\"type\":\"address\"}],\"name\":\"changeSmartContract\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"depositToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"emergencyWithdrawETH\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"}],\"name\":\"emergencyWithdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"playerToken\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_adr\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"setToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"smartContractAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            // optional rpc url
            string rpc = "https://polygon.llamarpc.com";

            string[] obj = { toAccount, amount };
            string args = JsonConvert.SerializeObject(obj);
            string chainId = await EVM.ChainId(chain, network, rpc);
            string gasPrice = await EVM.GasPrice(chain, network, rpc);
            string data = await EVM.CreateContractData(abi, method, args);
            string gasLimit = "150000";
            string transaction = await EVM.CreateTransaction(chain, network, account, contract, value, data, gasPrice, gasLimit, rpc);
            string signature = Web3PrivateKey.SignTransaction(privateKey, transaction, chainId);
            response = await EVM.BroadcastTransaction(chain, network, account, contract, value, data, signature, gasPrice, gasLimit, rpc);
            print(response);

            TxStatus._Instance.checkSetToken = true;
            TxStatus._Instance.checkSetTimer = 0;
            TxStatus._Instance.CheckSetToken(response);

            //Application.OpenURL("https://goerli.etherscan.io/tx/" + response);




        }
    }




 









}