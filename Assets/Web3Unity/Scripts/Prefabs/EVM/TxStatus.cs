using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OPS;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using OPS.AntiCheat.Prefs;
using TMPro;

public class TxStatus : MonoBehaviour
{
    public static TxStatus _Instance;

    public bool checkDeposit;
    public float checkDepositTimer;

    public bool checkApprove;
    public float checkApproveTimer;

    public bool checkSetToken;
    public float checkSetTimer;

    public bool checkWithdraw;
    public float checkWithdrawTimer;




    private void Awake()
    {
        _Instance = this;
    }

    private void Update()
    {


        if (checkDeposit)
        {
            checkDepositTimer += Time.deltaTime;

            if (checkDepositTimer >= 3)
            {
                CheckDeposit(WebGLSendContractExample._Instance.response2);

                checkDeposit = false;
                checkDepositTimer = 0;
            }
        }

        if (checkApprove)
        {
            checkApproveTimer += Time.deltaTime;

            if (checkApproveTimer >= 3)
            {
                CheckApprove(WebGLSendContractExample._Instance.response1);

                checkApprove = false;
                checkApproveTimer = 0;
            }
        }

        if (checkWithdraw)
        {
            checkWithdrawTimer += Time.deltaTime;

            if (checkWithdrawTimer >= 3)
            {
                CheckWithdraw(WebGLSendContractExample._Instance.response3);

                checkWithdraw = false;
                checkWithdrawTimer = 0;
            }
        }
        if (checkSetToken)
        {
            checkSetTimer += Time.deltaTime;

            if (checkSetTimer >= 3)
            {
                CheckSetToken(Web3PrivateKeySend20Example._Instance.response);

                checkSetToken = false;
                checkSetTimer = 0;
            }
        }


    }


    async public void CheckSetToken(string response)
    {
        if (PlayerPrefs.GetString("NewChain") == "1")

        {
            string chain = "ethereum";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://eth.llamarpc.com";


            string txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkSetTimer = 0;
                checkSetToken = false;


                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;

                //  WebGLSendContractExample._Instance.OnSendWithdraw();







                // confirmationImage.gameObject.SetActive(true);

                //PlayFabManager._Instance.withdrawSuccess.transform.Find("withdrawConfirmation").GetComponent<CanvasGroup>().alpha = 0.5f;
                // PlayFabManager._Instance.withdrawSuccess.transform.Find("withdrawSending").GetComponent<CanvasGroup>().alpha = 1f;
                // depositScript._Instance.withdrawBackground.transform.Find("backgroundUI").transform.Find("guiCalling").GetComponent<CanvasGroup>().alpha = 0.25f;

            }
            if (txStatus == "pending")
            {
                checkSetToken = true;
                checkSetTimer = 0;
            }
        }


        if (PlayerPrefs.GetString("NewChain") == "56")

        {
            string chain = "bsc";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://binance.llamarpc.com";


            string txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkSetTimer = 0;
                checkSetToken = false;



                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                //  WebGLSendContractExample._Instance.OnSendWithdraw();








            }
            if (txStatus == "pending")
            {
                checkSetToken = true;
                checkSetTimer = 0;
            }


        }
        if (PlayerPrefs.GetString("NewChain") == "89")

        {


            string chain = "polygon";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://polygon.llamarpc.com";


            string txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkSetTimer = 0;
                checkSetToken = false;




                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;

                //  WebGLSendContractExample._Instance.OnSendWithdraw();








            }
            if (txStatus == "pending")
            {
                checkSetToken = true;
                checkSetTimer = 0;
            }

        }


    }

    async public void CheckWithdraw(string response)
    {
        if (PlayerPrefs.GetString("NewChain") == "1")

        {

            string chain = "ethereum";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://eth.llamarpc.com";

            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkWithdraw = false;
                checkWithdrawTimer = 0;






                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;


                Web3PrivateKeySend20Example._Instance.withdrawObject.gameObject.SetActive(false);
                PlayFabManager._Instance.GetCurrencyBalance();

                //  depositScript._Instance.withdrawBackground.transform.Find("backgroundUISuccess").gameObject.SetActive(true);

                //   WebGLSendContractExample._Instance.valueWithdrawInput.interactable = true;
                //   WebGLSendContractExample._Instance.withdrawButton.interactable = true;


                // PlayfabManager._Instance.updateBalance();

                //Debug.Log(DepositValue);
                //  PlayfabManager._Instance.SetDepositCode(DepositValue);



            }
            if (txStatus == "pending")
            {
                checkWithdraw = true;
                checkWithdrawTimer = 0;
            }
        }

        if (PlayerPrefs.GetString("NewChain") == "56")

        {

            string chain = "bsc";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://binance.llamarpc.com";

            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkWithdraw = false;
                checkWithdrawTimer = 0;





                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;


                Web3PrivateKeySend20Example._Instance.withdrawObject.gameObject.SetActive(false);
                PlayFabManager._Instance.GetCurrencyBalance();

                //  depositScript._Instance.withdrawBackground.transform.Find("backgroundUISuccess").gameObject.SetActive(true);

                //   WebGLSendContractExample._Instance.valueWithdrawInput.interactable = true;
                //   WebGLSendContractExample._Instance.withdrawButton.interactable = true;


                // PlayfabManager._Instance.updateBalance();

                //Debug.Log(DepositValue);
                //  PlayfabManager._Instance.SetDepositCode(DepositValue);



            }
            if (txStatus == "pending")
            {
                checkWithdraw = true;
                checkWithdrawTimer = 0;
            }
        }
        if (PlayerPrefs.GetString("NewChain") == "89")

        {

            string chain = "polygon";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://polygon.llamarpc.com";

            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending

            if (txStatus == "success")
            {
                checkWithdraw = false;
                checkWithdrawTimer = 0;





                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("confirm").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("send").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greySprite;
                Web3PrivateKeySend20Example._Instance.withdrawObject.transform.Find("success").GetComponent<Image>().sprite = Web3PrivateKeySend20Example._Instance.greenSprite;



                Web3PrivateKeySend20Example._Instance.withdrawObject.gameObject.SetActive(false);
                PlayFabManager._Instance.GetCurrencyBalance();

                //  depositScript._Instance.withdrawBackground.transform.Find("backgroundUISuccess").gameObject.SetActive(true);

                //   WebGLSendContractExample._Instance.valueWithdrawInput.interactable = true;
                //   WebGLSendContractExample._Instance.withdrawButton.interactable = true;


                // PlayfabManager._Instance.updateBalance();

                //Debug.Log(DepositValue);
                //  PlayfabManager._Instance.SetDepositCode(DepositValue);



            }
            if (txStatus == "pending")
            {
                checkWithdraw = true;
                checkWithdrawTimer = 0;
            }
        }


    }



    async public void CheckApprove(string response)
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {


            string chain = "ethereum";
            string network = "mainnet";
            string transaction = response;
            // string rpc = "https://ethereum-sepolia-rpc.publicnode.com";
            string rpc = "https://eth.llamarpc.com";


            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending
                             // loadingBool = true;


            if (txStatus == "success")
            {
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").transform.Find("icon").GetComponent<Image>().sprite = TxStatus._Instance.acceptSprite;
                //   depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backDeposit").GetComponent<CanvasGroup>().alpha = 1;
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").GetComponent<CanvasGroup>().alpha = 0.5f;

                checkApprove = false;
                checkApproveTimer = 0;

                WebGLSendContractExample._Instance.depositObject.transform.Find("approve").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greenSprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;

                if (WebGLSendContractExample._Instance.whatDeposit == "USDT")
                {


                    WebGLSendContractExample._Instance.OnSendDepositTETHER();
                }
                if (WebGLSendContractExample._Instance.whatDeposit == "USDC")
                {


                    WebGLSendContractExample._Instance.OnSendDepositUSDC();
                }
                if (WebGLSendContractExample._Instance.whatDeposit == "LINK")
                {


                    WebGLSendContractExample._Instance.OnSendDepositLINK();
                }


            }
            if (txStatus == "pending")
            {
                checkApprove = true;
                checkApproveTimer = 0;
            }
        }

        if (PlayerPrefs.GetString("NewChain") == "56")
        {


            string chain = "bsc";
            string network = "mainnet";
            string transaction = response;
            // string rpc = "https://ethereum-sepolia-rpc.publicnode.com";
            string rpc = "https://binance.llamarpc.com";


            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending
                             // loadingBool = true;


            if (txStatus == "success")
            {
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").transform.Find("icon").GetComponent<Image>().sprite = TxStatus._Instance.acceptSprite;
                //   depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backDeposit").GetComponent<CanvasGroup>().alpha = 1;
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").GetComponent<CanvasGroup>().alpha = 0.5f;

                checkApprove = false;
                checkApproveTimer = 0;

                WebGLSendContractExample._Instance.depositObject.transform.Find("approve").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greenSprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;

                if (WebGLSendContractExample._Instance.whatDeposit == "USDT")
                {


                    WebGLSendContractExample._Instance.OnSendDepositTETHER();
                }


            }
            if (txStatus == "pending")
            {
                checkApprove = true;
                checkApproveTimer = 0;
            }
        }

        if (PlayerPrefs.GetString("NewChain") == "89")
        {


            string chain = "polygon";
            string network = "mainnet";
            string transaction = response;
            // string rpc = "https://ethereum-sepolia-rpc.publicnode.com";
            string rpc = "https://polygon.llamarpc.com";


            ProtectedString txStatus = await EVM.TxStatus(chain, network, transaction, rpc);

            print(txStatus); // success, fail, pending
                             // loadingBool = true;


            if (txStatus == "success")
            {
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").transform.Find("icon").GetComponent<Image>().sprite = TxStatus._Instance.acceptSprite;
                //   depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backDeposit").GetComponent<CanvasGroup>().alpha = 1;
                //  depositScript._Instance.depositBackground.transform.Find("backgroundUI").transform.Find("backApprove").GetComponent<CanvasGroup>().alpha = 0.5f;

                checkApprove = false;
                checkApproveTimer = 0;

                WebGLSendContractExample._Instance.depositObject.transform.Find("approve").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("send").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greenSprite;
                WebGLSendContractExample._Instance.depositObject.transform.Find("success").GetComponent<Image>().sprite = WebGLSendContractExample._Instance.greySprite;

                if (WebGLSendContractExample._Instance.whatDeposit == "USDT")
                {


                    WebGLSendContractExample._Instance.OnSendDepositTETHER();
                }


            }
            if (txStatus == "pending")
            {
                checkApprove = true;
                checkApproveTimer = 0;
            }
        }
    }


    async public void CheckDeposit(string response)
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {
            string chain = "ethereum";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://eth.llamarpc.com";



            //txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            ProtectedString txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            print(txStatusDeposit); // success, fail, pending

            // loadingBool = true;

            if (txStatusDeposit == "success")
            {
                WebGLSendContractExample._Instance.response2 = "";
                checkDeposit = false;
                checkDepositTimer = 0;
              CryptoDepositHistory._Instance.NewDeposit(transaction);
           //    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode();




            }
            if (txStatusDeposit == "pending")
            {
                checkDeposit = true;
                checkDepositTimer = 0;
            }

        }

        if (PlayerPrefs.GetString("NewChain") == "56")
        {
            string chain = "bsc";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://binance.llamarpc.com";



            //txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            ProtectedString txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            print(txStatusDeposit); // success, fail, pending

            // loadingBool = true;

            if (txStatusDeposit == "success")
            {
                WebGLSendContractExample._Instance.response2 = "";
                checkDeposit = false;
                checkDepositTimer = 0;
                CryptoDepositHistory._Instance.NewDeposit(transaction);

            //    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode();




            }
            if (txStatusDeposit == "pending")
            {
                checkDeposit = true;
                checkDepositTimer = 0;
            }

        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            string chain = "polygon";
            string network = "mainnet";
            string transaction = response;
            string rpc = "https://polygon.llamarpc.com";



            //txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            ProtectedString txStatusDeposit = await EVM.TxStatus(chain, network, transaction, rpc);
            print(txStatusDeposit); // success, fail, pending

            // loadingBool = true;

            if (txStatusDeposit == "success")
            {
                WebGLSendContractExample._Instance.response2 = "";
                checkDeposit = false;
                checkDepositTimer = 0;
                CryptoDepositHistory._Instance.NewDeposit(transaction);

            //    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode();




            }
            if (txStatusDeposit == "pending")
            {
                checkDeposit = true;
                checkDepositTimer = 0;
            }

        }
    }

}
