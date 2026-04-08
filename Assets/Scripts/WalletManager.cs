using Jint.Parser;
using NBitcoin.RPC;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using OPS.AntiCheat.Field;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class RpcResponse
{
    public string jsonrpc;
    public string id;
    public string result;
}
public class WalletManager : MonoBehaviour
{
    // Start is called before the first frame update
    public ProtectedString rpcUrl = "";  // Zamie� na sw�j Infura ID
                                         // Web3 instance
    public Web3 web3;

    // Czas od�wie�ania (co 5 sekund)
    private float checkInterval = 10.0f;
    private float timer = 0;
    private HexBigInteger initialBalance;
    private BigInteger initialUSDTBalance;
    private BigInteger initialUSDCBalance;
    private BigInteger initialLINKBalance;

    private BigInteger initialETHBalance;
    private BigInteger initialFDUSDBalance;
    private BigInteger initialMATICBalance;

    private BigInteger initialSHIBBalance;
    private BigInteger initialAPEBalance;
    private BigInteger initialCROBalance;
    private ProtectedDecimal initialStaticBalance;
    private BigInteger initialStatic;

    public static WalletManager _Instance;
    private string usdtContractAddress = ""; // Zamie� na odpowiedni adres USDC
    private string usdcContractAddress = ""; // Zamie� na odpowiedni adres USDC
    private string linkContractAddress = ""; // Zamie� na odpowiedni adres USDC
    public ProtectedString token;

    private void Awake()
    {
        _Instance = this;
    }
    public void Start()
    {
        //    if (PlayerPrefs.GetString("NewChain") == "89")
        //    {
        //
        //        rpcUrl = "https://polygon-mainnet.infura.io/v3/03e50e1506024636a6300bf7b9a6816f";
        //    }
        //    if (PlayerPrefs.GetString("NewChain") == "56")
        //    {
        //
        //        rpcUrl = "https://bsc-mainnet.infura.io/v3/03e50e1506024636a6300bf7b9a6816f";
        //    }
        //    if (PlayerPrefs.GetString("NewChain") == "1")
        //    {
        //
        //        rpcUrl = "https://mainnet.infura.io/v3/03e50e1506024636a6300bf7b9a6816f";
        //       // rpcUrl = "https://eth-mainnet.alchemyapi.io/v2/03e50e1506024636a6300bf7b9a6816f";
        //    }
        //    web3 = new Web3(rpcUrl);
        //    Debug.Log("RPC " + rpcUrl);

        GetRPC();
        // SendERC20TransactionAsync();
        
    }
    public async Task SendERC20TransactionAsync()
    {
        try
        {
            // Tworzenie konta z prywatnym kluczem
            // TODO (architecture): Private key must be moved to a secure server-side signing service.
            var account = new Nethereum.Web3.Accounts.Account(Secrets.EthPrivateKey);
            web3 = new Web3(account, rpcUrl);
            var abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"userAddress\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"address payable\",\"name\":\"relayerAddress\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bytes\",\"name\":\"functionSignature\",\"type\":\"bytes\"}],\"name\":\"MetaTransactionExecuted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"previousAdminRole\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"newAdminRole\",\"type\":\"bytes32\"}],\"name\":\"RoleAdminChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleGranted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleRevoked\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"CHILD_CHAIN_ID\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"CHILD_CHAIN_ID_BYTES\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEFAULT_ADMIN_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEPOSITOR_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ERC712_VERSION\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ROOT_CHAIN_ID\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ROOT_CHAIN_ID_BYTES\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"name_\",\"type\":\"string\"}],\"name\":\"changeName\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"depositData\",\"type\":\"bytes\"}],\"name\":\"deposit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"userAddress\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"functionSignature\",\"type\":\"bytes\"},{\"internalType\":\"bytes32\",\"name\":\"sigR\",\"type\":\"bytes32\"},{\"internalType\":\"bytes32\",\"name\":\"sigS\",\"type\":\"bytes32\"},{\"internalType\":\"uint8\",\"name\":\"sigV\",\"type\":\"uint8\"}],\"name\":\"executeMetaTransaction\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getChainId\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getDomainSeperator\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"}],\"name\":\"getNonce\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"nonce\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleAdmin\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"uint256\",\"name\":\"index\",\"type\":\"uint256\"}],\"name\":\"getRoleMember\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleMemberCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"grantRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"hasRole\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"name_\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"symbol_\",\"type\":\"string\"},{\"internalType\":\"uint8\",\"name\":\"decimals_\",\"type\":\"uint8\"},{\"internalType\":\"address\",\"name\":\"childChainManager\",\"type\":\"address\"}],\"name\":\"initialize\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"renounceRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"revokeRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
            // Załaduj kontrakt ERC-20 USDT
            var contract = web3.Eth.GetContract(abi, "0xc2132d05d31c914a87c6611c10748aeb04b58e8f");

            // Pobieranie funkcji 'transfer'
            var transferFunction = contract.GetFunction("transfer");

            // Konwersja ilości tokenów na najmniejsze jednostki (6 miejsc po przecinku dla USDT/USDC)
            var amountInSmallestUnit = UnitConversion.Convert.ToWei(0.1f, 6);

            // Szacowanie limitu gazu dla transakcji transferu ERC-20
            var gasLimit = await transferFunction.EstimateGasAsync("0xfc7306F82B701bE09Cf7fe547D05301bEB624e12", amountInSmallestUnit);

            // Pobieranie ceny gazu
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

            // Obliczanie kosztu gazu
            var gasCost = gasPrice.Value * gasLimit.Value;

            // Sprawdzenie, czy użytkownik ma wystarczającą ilość MATIC do pokrycia kosztów gazu
            var balanceInWei = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            if (balanceInWei.Value < gasCost)
            {
           //     Debug.LogError("Niewystarczające środki na gaz.");
                return;
            }

            // Przygotowanie transakcji (wywołanie funkcji transfer)
            var transactionInput = transferFunction.CreateTransactionInput("0xfc7306F82B701bE09Cf7fe547D05301bEB624e12", amountInSmallestUnit);

            // Wysyłanie transakcji
            var transactionHash = await web3.TransactionManager.SendTransactionAsync(transactionInput);

            Debug.Log("Transaction sent! TxHash: " + transactionHash);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending ERC-20 transaction: " + e.Message);
        }
    }
    public void GetRPC()
    {
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            rpcUrl = "https://polygon-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {

            rpcUrl = "https://bsc-mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
        }
        if (PlayerPrefs.GetString("NewChain") == "1")
        {

            rpcUrl = "https://mainnet.infura.io/v3/" + Secrets.InfuraApiKey;
        }
        web3 = new Web3(rpcUrl);
        Debug.Log("RPC " + rpcUrl);

        InvokeRepeating("StartCheckingForTransactions", 9, 10); // Wywo�anie asynchronicznej metody bez StartCoroutine
        timer = 0;

        // Startujemy licznik czasu
        StartCoroutine(UpdateTimer());
    }
    void StartCheckingForTransactions()
    {
        StartCoroutine(CheckForNewTransactions());  // Uruchamiamy coroutine
    }
    IEnumerator UpdateTimer()
    {
        // Nieskończona pętla odliczająca czas
        while (true)
        {
            if (timer <= 10)
            {
                timer += Time.deltaTime;  // Odliczanie czasu
            }

            yield return null;  // Poczekaj do następnej klatki
        }
    }
    async public void SendClicked()
    {
        if (PlayerPrefs.GetString("NewChain") == "1")
        {

            if (token == "ETH")
            {
                await SendTransactionAsync();
            }
            if (token == "USDT")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xc2132d05d31c914a87c6611c10748aeb04b58e8f"));

             //   await SendERC20TransactionAsync("0xc2132d05d31c914a87c6611c10748aeb04b58e8f");
            }
            if (token == "USDC")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48"));

             //   await SendERC20TransactionAsync("0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48");
            }
            if (token == "LINK")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x514910771af9ca656af840dff83e8264ecf986ca"));

             //   await SendERC20TransactionAsync("0x514910771af9ca656af840dff83e8264ecf986ca");
            }
            if (token == "FDUSD")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409"));

            //    await SendERC20TransactionAsync("0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409");
            }
            if (token == "MATIC")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0"));

             //   await SendERC20TransactionAsync("0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0");
            }
            if (token == "SHIB")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce"));

                //await SendERC20TransactionAsync("0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce");
            }
            if (token == "APE")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x4d224452801aced8b2f0aebe155379bb5d594381"));

            //    await SendERC20TransactionAsync("0x4d224452801aced8b2f0aebe155379bb5d594381");
            }
            if (token == "CRO")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b"));

            //    await SendERC20TransactionAsync("0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b");
            }
        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {

            if (token == "USDT")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x55d398326f99059ff775485246999027b3197955"));
             //   await SendERC20TransactionAsync("0x55d398326f99059ff775485246999027b3197955");
            }
            if (token == "USDC")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d"));
     //           await SendERC20TransactionAsync("0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d");
            }
            if (token == "LINK")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd"));

            //    await SendERC20TransactionAsync("0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd");
            }
            if (token == "ETH")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x2170ed0880ac9a755fd29b2688956bd959f933f8"));

              //  await SendERC20TransactionAsync("0x2170ed0880ac9a755fd29b2688956bd959f933f8");
            }
            if (token == "FDUSD")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409"));

           //     await SendERC20TransactionAsync("0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409");
            }
            if (token == "MATIC")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xcc42724c6683b7e57334c4e856f4c9965ed682bd"));

               // await SendERC20TransactionAsync("0xcc42724c6683b7e57334c4e856f4c9965ed682bd");
            }
            if (token == "BNB")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xcc42724c6683b7e57334c4e856f4c9965ed682bd"));

              //  await SendTransactionAsync();
            }
        }
        if (PlayerPrefs.GetString("NewChain") == "89")
        {
            if (token == "USDT")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0xc2132d05d31c914a87c6611c10748aeb04b58e8f"));

            //    await SendERC20TransactionAsync("0xc2132d05d31c914a87c6611c10748aeb04b58e8f");
            }
            if (token == "USDC")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359"));

              //  await SendERC20TransactionAsync("0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359");
            }
            if (token == "LINK")
            {
                StartCoroutine(SendERC20TransactionCoroutine("0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39"));

              //  await SendERC20TransactionAsync("0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39");
            }
            if (token == "MATIC")
            {
                await SendTransactionAsync();
            }
        }
    }
    private IEnumerator SendERC20TransactionCoroutine(string tokenAdr)
    {
        // TODO (architecture): Move to server-side signing service.
        string privateKey = Secrets.EthPrivateKey;
        string recipientAddress = WithdrawScript._Instance.walletInput.text;

        // Tworzenie JSON-a dla transakcji (RPC JSON-RPC call)
        string json = CreateTransactionJson(tokenAdr, recipientAddress, privateKey);

        // Wykonanie żądania HTTP POST z użyciem UnityWebRequest
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(rpcUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // Oczekiwanie na odpowiedź serwera
            yield return www.SendWebRequest();

            // Sprawdzenie błędów i wyników
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                // Obsługa błędu
                HandleTransactionError(www.error);
            }
            else
            {
                // Wyświetlenie odpowiedzi na transakcję
                HandleTransactionSuccess(www.downloadHandler.text);
            }
        }
    }    // Funkcja do obsługi błędów transakcji
    private void HandleTransactionError(string errorMessage)
    {
        Debug.LogError("Error while sending ERC-20 transaction: " + errorMessage);
        PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);
    }

    // Funkcja do obsługi sukcesu transakcji
    private void HandleTransactionSuccess(string response)
    {
        Debug.Log("Transaction response: " + response);
        PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);
    }

    private string CreateTransactionJson(string tokenAdr, string recipientAddress, string privateKey)
    {

        // Wartości takie jak gasPrice i gasLimit mogą być statyczne lub dynamiczne, np. pobierane z API
        string gasPrice = "0x09184e72a000"; // Ustalony gasPrice
        string gasLimit = "0x2710";          // Ustalony gasLimit
        string value = "0x0";                // Ustalony value (ERC-20 transfer nie wymaga ETH transferu)

        string transactionJson =
        @"{
            ""jsonrpc"": ""2.0"",
            ""method"": ""eth_sendTransaction"",
            ""params"": [{
                ""from"": ""YOUR_ADDRESS"",
                ""to"": """ + tokenAdr + @""",
                ""gas"": """ + gasLimit + @""",
                ""gasPrice"": """ + gasPrice + @""",
                ""value"": """ + value + @""",
                ""data"": ""YOUR_ENCODED_FUNCTION_CALL""
            }],
            ""id"": 1
        }";

        return transactionJson;
    }
    public async Task SendERC20TransactionAsync(string tokenAdr)
    {
        // TODO (architecture): Move to server-side signing service.
        ProtectedString privateKey = Secrets.EthPrivateKey;
        ProtectedString recipientAddress = WithdrawScript._Instance.walletInput.text;
        try
        {
            // Tworzenie konta z prywatnym kluczem
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            web3 = new Web3(account, rpcUrl);

            // Pobranie ABI kontraktu ERC-20 (standardowy dla wszystkich token�w ERC-20)
            var abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"userAddress\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"address payable\",\"name\":\"relayerAddress\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bytes\",\"name\":\"functionSignature\",\"type\":\"bytes\"}],\"name\":\"MetaTransactionExecuted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"previousAdminRole\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"newAdminRole\",\"type\":\"bytes32\"}],\"name\":\"RoleAdminChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleGranted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleRevoked\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"CHILD_CHAIN_ID\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"CHILD_CHAIN_ID_BYTES\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEFAULT_ADMIN_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEPOSITOR_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ERC712_VERSION\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ROOT_CHAIN_ID\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"ROOT_CHAIN_ID_BYTES\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"name_\",\"type\":\"string\"}],\"name\":\"changeName\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"depositData\",\"type\":\"bytes\"}],\"name\":\"deposit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"userAddress\",\"type\":\"address\"},{\"internalType\":\"bytes\",\"name\":\"functionSignature\",\"type\":\"bytes\"},{\"internalType\":\"bytes32\",\"name\":\"sigR\",\"type\":\"bytes32\"},{\"internalType\":\"bytes32\",\"name\":\"sigS\",\"type\":\"bytes32\"},{\"internalType\":\"uint8\",\"name\":\"sigV\",\"type\":\"uint8\"}],\"name\":\"executeMetaTransaction\",\"outputs\":[{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getChainId\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"pure\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getDomainSeperator\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"}],\"name\":\"getNonce\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"nonce\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleAdmin\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"uint256\",\"name\":\"index\",\"type\":\"uint256\"}],\"name\":\"getRoleMember\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleMemberCount\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"grantRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"hasRole\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"name_\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"symbol_\",\"type\":\"string\"},{\"internalType\":\"uint8\",\"name\":\"decimals_\",\"type\":\"uint8\"},{\"internalType\":\"address\",\"name\":\"childChainManager\",\"type\":\"address\"}],\"name\":\"initialize\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"renounceRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"revokeRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

            // Za�aduj kontrakt ERC-20 USDT
            var contract = web3.Eth.GetContract(abi, tokenAdr);

            // Pobieranie funkcji 'transfer'
            var transferFunction = contract.GetFunction("transfer");

            // Konwersja ilo�ci token�w na najmniejsze jednostki (zale�y od tokena, USDT ma 6 miejsc po przecinku, USDC r�wnie�)
            //var amountInSmallestUnit = UnitConversion.Convert.ToWei(decimal.Parse(amountInToken), 6); // 6 miejsc po przecinku dla USDT/USDC

            if (token == "USDT" || token == "USDC")
            {


                var amountInSmallestUnit = UnitConversion.Convert.ToWei(decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text), 6); // 6 miejsc po przecinku dla USDT/USDC
            }
            if (token == "LINK")
            {


                var amountInSmallestUnit = UnitConversion.Convert.ToWei(decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text), 18); // 6 miejsc po przecinku dla USDT/USDC
            }
            // Przygotowanie transakcji (wywo�anie funkcji transfer)
            var transactionInput = transferFunction.CreateTransactionInput(account.Address, recipientAddress, decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text));

            // Pobieranie ceny gazu
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

            // Szacowanie limitu gazu dla transakcji transferu ERC-20
            var gasLimit = await transferFunction.EstimateGasAsync(account.Address, recipientAddress, decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text));

            // Obliczanie kosztu gazu
            var gasCost = gasPrice.Value * gasLimit.Value;

            // Sprawdzenie, czy u�ytkownik ma wystarczaj�c� ilo�� MATIC do pokrycia koszt�w gazu
            var balanceInWei = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
            if (balanceInWei.Value < gasCost)
            {
                Debug.LogError("Niewystarczaj�ce �rodki na gaz.");
                return;
            }

            // Wysy�anie transakcji (metoda transfer z kontraktu ERC-20)
            var transactionHash = await web3.TransactionManager.SendTransactionAsync(transactionInput);
            PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);
            Debug.Log("Transaction sent! TxHash: " + transactionHash);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending ERC-20 transaction: " + e.Message);
        }
    }

    public async Task SendTransactionAsync()
    {
        // TODO (architecture): Move to server-side signing service.
        ProtectedString privateKey = Secrets.EthPrivateKey;
        ProtectedString recipientAddress = WithdrawScript._Instance.walletInput.text;
        try
        {
            // Tworzenie konta z prywatnym kluczem
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            web3 = new Web3(account, rpcUrl);
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();

            // Ustawienie limitu gazu dla transakcji
            var gasLimit = new HexBigInteger(21000); // Typowa warto�� dla wysy�ania MATIC/ETH
            // Pobieranie numeru nonce (ilo�� transakcji z konta)
            var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.Address);
            var gasCost = gasPrice.Value * gasLimit.Value; // Koszt gazu w Wei
            // Konwersja kwoty z MATIC do Wei (1 MATIC = 10^18 Wei)
            var amountInWei = UnitConversion.Convert.ToWei(decimal.Parse(CryptoWithdrawHistory._Instance.withdrawInput.text));
            if (amountInWei > gasCost)
            {
                // Zmniejsz amountInWei o koszt gazu
                amountInWei -= gasCost;
            }
            else
            {
                Debug.LogError("Amount to send is too low to cover gas fees.");
                return;
            }
            // Tworzenie transakcji z wymaganymi parametrami
            var transactionInput = new Nethereum.RPC.Eth.DTOs.TransactionInput()
            {
                From = account.Address,
                To = recipientAddress,
                Value = new HexBigInteger(amountInWei),
                GasPrice = new HexBigInteger(50000000000), // Cena gazu
                Gas = new HexBigInteger(21000), // Limit gazu
                Nonce = new HexBigInteger(nonce)
            };

            // Wysy�anie podpisanej transakcji za pomoc� TransactionManager
            var transactionHash = await web3.TransactionManager.SendTransactionAsync(transactionInput);
            PlayFabManager._Instance.loadingObject.gameObject.SetActive(false);

            Debug.Log("Transaction sent! TxHash: " + transactionHash);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending transaction: " + e.Message);
        }
    }
    public IEnumerator TestWebRequest2()
    {
        string walletAddress = (string)PlayFabManager._Instance.addressEVM;

        if (string.IsNullOrEmpty(walletAddress))
        {
            Debug.LogError("Wallet address is invalid or empty");
            yield break;
        }

        // Przygotowanie zapytania o saldo
        string jsonRpcData = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBalance\",\"params\":[\"" + walletAddress + "\", \"latest\"],\"id\":1}";

        // Wysłanie zapytania
        UnityWebRequest www = new UnityWebRequest(rpcUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRpcData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error checking balance: " + www.error);
            yield break;
        }
        else
        {
            // Obsługa odpowiedzi
            Debug.Log("Balance Response: " + www.downloadHandler.text);

            // Odczytanie wyniku z odpowiedzi JSON
            var jsonResponse = www.downloadHandler.text;
            var parsedResponse = JsonUtility.FromJson<RpcResponse>(jsonResponse);
            if (parsedResponse.result != null)
            {

                initialStatic = BigInteger.Parse(parsedResponse.result.Substring(2), System.Globalization.NumberStyles.HexNumber); // Parse hex
                Debug.Log("big integar INITIAL BALANCE " + initialStatic);

                //       Debug.Log("FULL NUMBER " + initialStatic);
                initialStaticBalance = Web3.Convert.FromWei(initialStatic, 18);
                Debug.Log("INITIAL BALANCE " + initialStaticBalance);
            }
        }

        // Dalej przetwarzaj ERC20, jeśli to konieczne...

        // Składanie zapytania o saldo tokenów ERC-20 (jeśli potrzebujesz)
        yield return GetERC20Balances(walletAddress);
    }

  

    IEnumerator GetERC20Balances(string walletAddress)
    {
        // ABI ERC-20 balanceOf
        string erc20Abi = @"[{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""}]";
        string usdtAddress = "";
        string usdcAddress = "";
        string linkAddress = "";
        string fdusdAddress = "";
        string maticAddress = "";
        string shibAddress = "";
        string apeAddress = "";
        string croAddress = "";
        string ethAddress = "";


        // Przykład dla USDT, USDC, LINK (możesz rozszerzyć dla innych tokenów)
        if (PlayerPrefs.GetString("NewChain") == "1")
        { 
            usdtAddress = "0xdac17f958d2ee523a2206206994597c13d831ec7"; // USDT
        usdcAddress = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48"; // USDC
        linkAddress = "0x514910771af9ca656af840dff83e8264ecf986ca"; // LINK

        fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
        maticAddress = "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0";

        shibAddress = "0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce";
        apeAddress = "0x4d224452801aced8b2f0aebe155379bb5d594381";
        croAddress = "0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b";
    }
            if (PlayerPrefs.GetString("NewChain") == "89")
        {

            usdtAddress = "0xc2132d05d31c914a87c6611c10748aeb04b58e8f"; // Adres USDT na sieci Polygon
            usdcAddress = "0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359"; // Adres USDC na sieci Polygon
            linkAddress = "0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39"; // Adre

        }
        if (PlayerPrefs.GetString("NewChain") == "56")
        {

            usdtAddress = "0x55d398326f99059ff775485246999027b3197955"; // USDT
            usdcAddress = "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d"; // USDC
            linkAddress = "0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd"; // LINK

            ethAddress = "0x2170ed0880ac9a755fd29b2688956bd959f933f8";
            fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
            maticAddress = "0xcc42724c6683b7e57334c4e856f4c9965ed682bd";
        }
        // Lista tokenów do sprawdzenia
        (string address, Action<BigInteger> setBalance, int decimals)[] tokens = new (string, Action<BigInteger>, int)[]
      {
        (usdtAddress, balance => initialUSDTBalance = balance, 6),  // USDT ma 6 miejsc dziesiętnych
        (usdcAddress, balance => initialUSDCBalance = balance, 6),  // USDC ma 6 miejsc dziesiętnych
        (linkAddress, balance => initialLINKBalance = balance, 18),  // LINK ma 18 miejsc dziesiętnych
        (fdusdAddress, balance => initialFDUSDBalance = balance, 18),  // FDUSD ma 18 miejsc dziesiętnych
        (maticAddress, balance => initialMATICBalance = balance, 18),  // MATIC ma 18 miejsc dziesiętnych
        (shibAddress, balance => initialSHIBBalance = balance, 18),  // SHIB ma 18 miejsc dziesiętnych
        (apeAddress, balance => initialAPEBalance = balance, 18),  // APE ma 18 miejsc dziesiętnych
        (croAddress, balance => initialCROBalance = balance, 8)  // CRO ma 8 miejsc dziesiętnych
      };

        foreach (var (contractAddress, setBalance, decimals) in tokens)
        {
            if (string.IsNullOrEmpty(contractAddress)) continue;  // Pomijanie pustych adresów

            var jsonRpcData = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{\"to\":\"" + contractAddress + "\",\"data\":\"0x70a08231000000000000000000000000" + walletAddress.Substring(2) + "\"},\"latest\"],\"id\":1}";

            UnityWebRequest www = new UnityWebRequest(rpcUrl, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRpcData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error checking token balance for " + contractAddress + ": " + www.error);
                yield break;
            }
            else
            {
                var jsonResponse = www.downloadHandler.text;
                var parsedResponse = JsonUtility.FromJson<RpcResponse>(jsonResponse);
                if (parsedResponse.result != null)
                {
                    BigInteger tokenBalance = BigInteger.Parse(parsedResponse.result.Substring(2), System.Globalization.NumberStyles.HexNumber); // Parse hex
                    setBalance(tokenBalance);  // Przypisanie odpowiedniej zmiennej
                //    BigInteger tokenBalance = BigInteger.Parse(tokenRpcResponse.result.Substring(2), System.Globalization.NumberStyles.HexNumber); // Parsowanie Hex na BigInteger

                    // Konwersja balansu na odpowiednią liczbę miejsc dziesiętnych
                    decimal balanceInProperUnit = Web3.Convert.FromWei(tokenBalance, decimals);
                   // Debug.Log("Token Balance for " + contractAddress + ": " + balanceInProperUnit);
                   // Debug.Log("Token Balance for " + contractAddress + ": " + tokenBalance);
                }
            }
        }
    }
    async public Task CheckInitialBalance()
    {
        try
        {
            // Adres portfela gracza
            var walletAddress = (string)PlayFabManager._Instance.addressEVM;

            // Pobieranie pocz�tkowego salda MATIC (Polygon)



            initialBalance = await web3.Eth.GetBalance.SendRequestAsync(walletAddress);
            //initialBalance = await web3.Eth.GetBalance.SendRequestAsync("0xfc7306F82B701bE09Cf7fe547D05301bEB624e12");
            Debug.Log("INITIAL BALANCE: " + initialBalance);

            var usdtAddress = "";
            var usdcAddress = "";
            var linkAddress = "";

            var ethAddress = "";
            var fdusdAddress = "";
            var maticAddress = "";

            var shibAddress = "";
            var apeAddress = "";
            var croAddress = "";

            // Adresy kontrakt�w ERC-20 dla USDT, USDC i LINK
            if (PlayerPrefs.GetString("NewChain") == "89")
            {
                usdtAddress = "0xc2132d05d31c914a87c6611c10748aeb04b58e8f"; // USDT
                usdcAddress = "0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359"; // USDC
                linkAddress = "0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39"; // LINK

            }
            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                usdtAddress = "0x55d398326f99059ff775485246999027b3197955"; // USDT
                usdcAddress = "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d"; // USDC
                linkAddress = "0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd"; // LINK

                ethAddress = "0x2170ed0880ac9a755fd29b2688956bd959f933f8";
                fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
                maticAddress = "0xcc42724c6683b7e57334c4e856f4c9965ed682bd";

            }
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                usdtAddress = "0xdac17f958d2ee523a2206206994597c13d831ec7"; // USDT
                usdcAddress = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48"; // USDC
                linkAddress = "0x514910771af9ca656af840dff83e8264ecf986ca"; // LINK

                fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
                maticAddress = "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0";

                shibAddress = "0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce";
                apeAddress = "0x4d224452801aced8b2f0aebe155379bb5d594381";
                croAddress = "0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b";

            }

            // ABI ERC-20 dla balanceOf
            var erc20Abi = @"[{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""}]";


            // Inicjalizacja kontrakt�w ERC-20 dla USDT, USDC i LINK

            if (PlayerPrefs.GetString("NewChain") == "89")
            { 
                    var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);

                // Pobieranie funkcji balanceOf
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");

                // Pobieranie pocz�tkowego salda USDT, USDC i LINK
                initialUSDTBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialUSDCBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialLINKBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);

           }

            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);
                var ethContract = web3.Eth.GetContract(erc20Abi, ethAddress);
                var fdusdContract = web3.Eth.GetContract(erc20Abi, fdusdAddress);
                var maticContract = web3.Eth.GetContract(erc20Abi, maticAddress);

                // Pobieranie funkcji balanceOf
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");
                var ethBalanceOfFunction = ethContract.GetFunction("balanceOf");
                var fdusdBalanceOfFunction = fdusdContract.GetFunction("balanceOf");
                var maticBalanceOfFunction = maticContract.GetFunction("balanceOf");

                // Pobieranie pocz�tkowego salda USDT, USDC i LINK
                initialUSDTBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialUSDCBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialLINKBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialETHBalance = await ethBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialFDUSDBalance = await fdusdBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialMATICBalance = await maticBalanceOfFunction.CallAsync<BigInteger>(walletAddress);

            }
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);
                var fdusdContract = web3.Eth.GetContract(erc20Abi, fdusdAddress);
                var maticContract = web3.Eth.GetContract(erc20Abi, maticAddress);
                var apeContract = web3.Eth.GetContract(erc20Abi, apeAddress);
                var shibContract = web3.Eth.GetContract(erc20Abi, shibAddress);
                var croContract = web3.Eth.GetContract(erc20Abi, croAddress);

                // Pobieranie funkcji balanceOf
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");
                var fdusdBalanceOfFunction = fdusdContract.GetFunction("balanceOf");
                var maticBalanceOfFunction = maticContract.GetFunction("balanceOf");
                var shibBalanceOfFunction = shibContract.GetFunction("balanceOf");
                var apeBalanceOfFunction = apeContract.GetFunction("balanceOf");
                var croBalanceOfFunction = croContract.GetFunction("balanceOf");

                // Pobieranie pocz�tkowego salda USDT, USDC i LINK
                initialUSDTBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialUSDCBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialLINKBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialFDUSDBalance = await fdusdBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialMATICBalance = await maticBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialSHIBBalance = await shibBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialAPEBalance = await apeBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                initialCROBalance = await croBalanceOfFunction.CallAsync<BigInteger>(walletAddress);

            }


        }
        catch (Exception e)
        {
            Debug.LogError("Error checking initial balances: " + e.Message);
        }
    }

    // Funkcja sprawdzaj�ca, czy przysz�a nowa transakcja
    async public Task CheckForNewTransactionsOLD()
    {
        try
        {
            // Adres portfela gracza
            var walletAddress = (string)PlayFabManager._Instance.addressEVM;
           // var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
           // BigInteger currentBlockNumber = latestBlock.Value;
            string txHash = "";
            decimal value = 0;
            string whatChain = "";
            // Przejd� przez transakcje w bie��cym bloku
             //var blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(latestBlock));

            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                whatChain = "ETH";
            }
            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                whatChain = "BSC";

            }
            if (PlayerPrefs.GetString("NewChain") == "89")
            {
                whatChain = "MATIC";

            }
          //  foreach (var tx in blockWithTransactions.Transactions)
           // {
       //       //          Debug.Log(blockWithTransactions.Transactions);
       //       // Sprawd�, czy transakcja dotyczy adresu gracza
       //       if (tx.To != null && tx.To.ToLower() == walletAddress.ToLower())
       //       {
       //           Debug.Log($"Transaction detected for address {walletAddress}");
       //           Debug.Log($"TX Hash: {tx.TransactionHash}");
       //           Debug.Log($"From: {tx.From}");
       //           Debug.Log($"To: {tx.To}");
       //           Debug.Log($"Value: {Web3.Convert.FromWei(tx.Value)} ETH");
       //           value = Web3.Convert.FromWei(tx.Value);
       //           txHash = tx.TransactionHash;
       //           // Dodatkowe przetwarzanie lub logika, np. aktualizacja salda
       //         }
         //   }


            // Sprawdzenie bie��cego salda MATIC (Polygon)
            var currentBalance = await web3.Eth.GetBalance.SendRequestAsync(walletAddress);

            // Sprawdzenie USDT, USDC i LINK kontrakt�w ERC-20
            var usdtAddress = "";
            var usdcAddress = "";
            var linkAddress = "";

            var ethAddress = "";
            var fdusdAddress = "";
            var maticAddress = "";

            var shibAddress = "";
            var apeAddress = "";
            var croAddress = "";
            var bnbAddress = "";



            if (PlayerPrefs.GetString("NewChain") == "89")
            {
                usdtAddress = "0xc2132d05d31c914a87c6611c10748aeb04b58e8f"; // Adres USDT na sieci Polygon
                usdcAddress = "0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359"; // Adres USDC na sieci Polygon
                linkAddress = "0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39"; // Adres LINK na sieci Polygon
            }

            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                usdtAddress = "0x55d398326f99059ff775485246999027b3197955"; // USDT
                usdcAddress = "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d"; // USDC
                linkAddress = "0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd"; // LINK

                ethAddress = "0x2170ed0880ac9a755fd29b2688956bd959f933f8";
                fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
                maticAddress = "0xcc42724c6683b7e57334c4e856f4c9965ed682bd";
            }

            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                usdtAddress = "0xdac17f958d2ee523a2206206994597c13d831ec7"; // USDT
                usdcAddress = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48"; // USDC
                linkAddress = "0x514910771af9ca656af840dff83e8264ecf986ca"; // LINK

                fdusdAddress = "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409";
                maticAddress = "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0";

                shibAddress = "0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce";
                apeAddress = "0x4d224452801aced8b2f0aebe155379bb5d594381";
                croAddress = "0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b";
            }


            // ABI ERC-20 dla balanceOf
            var erc20Abi = @"[{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""}]";





            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);

                var fdusdContract = web3.Eth.GetContract(erc20Abi, fdusdAddress);
                var maticContract = web3.Eth.GetContract(erc20Abi, maticAddress);


                var shibContract = web3.Eth.GetContract(erc20Abi, shibAddress);
                var apeContract = web3.Eth.GetContract(erc20Abi, apeAddress);
                var croContract = web3.Eth.GetContract(erc20Abi, croAddress);




                // Pobieranie funkcji balanceOf dla token�w
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");

                var fdusdBalanceOfFunction = fdusdContract.GetFunction("balanceOf");
                var maticBalanceOfFunction = maticContract.GetFunction("balanceOf");

                var shibBalanceOfFunction = shibContract.GetFunction("balanceOf");
                var apeBalanceOfFunction = apeContract.GetFunction("balanceOf");
                var croBalanceOfFunction = croContract.GetFunction("balanceOf");


                // Pobieranie pocz�tkowego salda USDT, USDC i LINK

                // Pobieranie salda USDT, USDC i LINK
                var usdtBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var usdcBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var linkBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var fdusdBalance = await fdusdBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var maticBalance = await maticBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var shibBalance = await shibBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var apeBalance = await apeBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var croBalance = await croBalanceOfFunction.CallAsync<BigInteger>(walletAddress);


                // Sprawdzenie zmian w saldzie MATIC (Polygon)
                if (currentBalance.Value > initialBalance.Value)
                {
                    //   Debug.Log("New MATIC transaction detected! New balance: " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC");
                    BigInteger subtract = currentBalance.Value - initialBalance.Value;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.ethMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "ETH", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 1000000000000).ToString(), "ETH");
                    }
                    initialBalance = currentBalance; // Aktualizacja salda

                }

                // Sprawdzenie zmian w saldzie USDT
                if (usdtBalance > initialUSDTBalance)
                {
                    //  Debug.Log("New USDT transaction detected! New balance: " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT"); // USDT ma 6 miejsc po przecinku


                    BigInteger subtract = usdtBalance - initialUSDTBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdtMinimumBet)
                    {


                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", (subtract / 1000000).ToString(), txHash, whatChain);


                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDT");
                    }
                        initialUSDTBalance = usdtBalance; // Aktualizacja salda USDT
                    

                }

                // Sprawdzenie zmian w saldzie USDC
                if (usdcBalance > initialUSDCBalance)
                {
                    // Debug.Log("New USDC transaction detected! New balance: " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC"); // USDC ma 6 miejsc po przecinku
                    BigInteger subtract = usdcBalance - initialUSDCBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdcMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDC", (subtract / 1000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDC");

                    }

                    initialUSDCBalance = usdcBalance; // Aktualizacja salda USDC


                }

                // Sprawdzenie zmian w saldzie LINK
                if (linkBalance > initialLINKBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = linkBalance - initialLINKBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.linkMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "LINK", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "LINK");

                    }
                    initialLINKBalance = linkBalance; // Aktualizacja salda LINK

                }
               
                if (fdusdBalance > initialFDUSDBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = fdusdBalance - initialFDUSDBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.fdusdMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "FDUSD", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "FDUSD");

                    }

                    initialFDUSDBalance = fdusdBalance; // Aktualizacja salda LINK

                }
                if (maticBalance > initialMATICBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = maticBalance - initialMATICBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.maticMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "POL", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "POL");

                    }

                    initialMATICBalance = maticBalance; // Aktualizacja salda LINK

                }
                if (shibBalance > initialSHIBBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = shibBalance - initialSHIBBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.shibMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "SHIB", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "SHIB");
                    }
                    initialSHIBBalance = shibBalance; // Aktualizacja salda LINK

                }
                if (apeBalance > initialAPEBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = apeBalance - initialAPEBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.apeMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "APE", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "APE");

                    }

                    initialAPEBalance = apeBalance; // Aktualizacja salda LINK

                }
                if (croBalance > initialCROBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = croBalance - initialCROBalance;
                    decimal withDecimals = ((decimal)subtract / 100000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.cronosMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "CRO", (subtract / 100000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 1000000).ToString(), "CRO");

                    }

                    initialCROBalance = croBalance; // Aktualizacja salda LINK

                }
                // Brak nowych transakcji
                if (currentBalance.Value == initialBalance.Value && usdtBalance == initialUSDTBalance && usdcBalance == initialUSDCBalance && linkBalance == initialLINKBalance && shibBalance == initialSHIBBalance && fdusdBalance == initialFDUSDBalance && maticBalance == initialMATICBalance && apeBalance == initialAPEBalance && croBalance == initialCROBalance)
                {
                    //     Debug.Log("No new transactions. Current balances: MATIC - " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC, USDT - " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT, USDC - " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC, LINK - " + Web3.Convert.FromWei(linkBalance, 18) + " LINK");
                }

            }
























            if (PlayerPrefs.GetString("NewChain") == "56")
            {
                var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);

                var ethContract = web3.Eth.GetContract(erc20Abi, ethAddress);
                var fdusdContract = web3.Eth.GetContract(erc20Abi, fdusdAddress);
                var maticContract = web3.Eth.GetContract(erc20Abi, maticAddress);

                // Pobieranie funkcji balanceOf dla token�w
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");
           
                var ethBalanceOfFunction = ethContract.GetFunction("balanceOf");
                var fdusdBalanceOfFunction = fdusdContract.GetFunction("balanceOf");
                var maticBalanceOfFunction = maticContract.GetFunction("balanceOf");


                // Pobieranie pocz�tkowego salda USDT, USDC i LINK
           
                // Pobieranie salda USDT, USDC i LINK
                var usdtBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var usdcBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var linkBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var ethBalance = await ethBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var fdusdBalance = await fdusdBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var maticBalance = await maticBalanceOfFunction.CallAsync<BigInteger>(walletAddress);

                // Sprawdzenie zmian w saldzie MATIC (Polygon)
                if (currentBalance.Value > initialBalance.Value)
                {
                    //   Debug.Log("New MATIC transaction detected! New balance: " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC");
                    BigInteger subtract = currentBalance.Value - initialBalance.Value;
                    decimal withDecimals = ((decimal)subtract / 100000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.bnbMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "BNB", (subtract / 100000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 1000000).ToString(), "BNB");
                    }
                    initialBalance = currentBalance; // Aktualizacja salda

                }

                // Sprawdzenie zmian w saldzie USDT
                if (usdtBalance > initialUSDTBalance)
                {
                    //  Debug.Log("New USDT transaction detected! New balance: " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT"); // USDT ma 6 miejsc po przecinku


                    BigInteger subtract = usdtBalance - initialUSDTBalance;

                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdtMinimumBet)
                    {

                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", (subtract / 1000000).ToString(), txHash, whatChain);


                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDT");

                    }
                    initialUSDTBalance = usdtBalance; // Aktualizacja salda USDT

                }

                // Sprawdzenie zmian w saldzie USDC
                if (usdcBalance > initialUSDCBalance)
                {
                    // Debug.Log("New USDC transaction detected! New balance: " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC"); // USDC ma 6 miejsc po przecinku
                    BigInteger subtract = usdcBalance - initialUSDCBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdtMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDC", (subtract / 1000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDC");

                    }

                    initialUSDCBalance = usdcBalance; // Aktualizacja salda USDC


                }

                // Sprawdzenie zmian w saldzie LINK
                if (linkBalance > initialLINKBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = linkBalance - initialLINKBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.linkMinimumBet)
                    {

                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "LINK", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "LINK");

                    }

                    initialLINKBalance = linkBalance; // Aktualizacja salda LINK

                }
                if (ethBalance > initialETHBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = ethBalance - initialETHBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.ethMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "ETH", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 1000000000000).ToString(), "ETH");


                    }
                    initialETHBalance = ethBalance; // Aktualizacja salda LINK

                }
                if (fdusdBalance > initialFDUSDBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = fdusdBalance - initialFDUSDBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.fdusdMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "FDUSD", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "FDUSD");


                    }
                    initialFDUSDBalance = fdusdBalance; // Aktualizacja salda LINK

                }
                if (maticBalance > initialMATICBalance)
                {
                    //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = maticBalance - initialMATICBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.maticMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "POL", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "POL");

                    }
                    initialMATICBalance = maticBalance; // Aktualizacja salda LINK

                }
                // Brak nowych transakcji
                if (currentBalance.Value == initialBalance.Value && usdtBalance == initialUSDTBalance && usdcBalance == initialUSDCBalance && linkBalance == initialLINKBalance && ethBalance == initialETHBalance && fdusdBalance == initialFDUSDBalance && maticBalance == initialMATICBalance)
                {
                    //     Debug.Log("No new transactions. Current balances: MATIC - " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC, USDT - " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT, USDC - " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC, LINK - " + Web3.Convert.FromWei(linkBalance, 18) + " LINK");
                }

            }




















            // Inicjalizacja kontrakt�w USDT, USDC i LINK
            if (PlayerPrefs.GetString("NewChain") == "89")
            {
                var usdtContract = web3.Eth.GetContract(erc20Abi, usdtAddress);
                var usdcContract = web3.Eth.GetContract(erc20Abi, usdcAddress);
                var linkContract = web3.Eth.GetContract(erc20Abi, linkAddress);

                // Pobieranie funkcji balanceOf dla token�w
                var usdtBalanceOfFunction = usdtContract.GetFunction("balanceOf");
                var usdcBalanceOfFunction = usdcContract.GetFunction("balanceOf");
                var linkBalanceOfFunction = linkContract.GetFunction("balanceOf");

                // Pobieranie salda USDT, USDC i LINK
                var usdtBalance = await usdtBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var usdcBalance = await usdcBalanceOfFunction.CallAsync<BigInteger>(walletAddress);
                var linkBalance = await linkBalanceOfFunction.CallAsync<BigInteger>(walletAddress);

                // Sprawdzenie zmian w saldzie MATIC (Polygon)
                if (currentBalance.Value > initialBalance.Value)
                {
                 //   Debug.Log("New MATIC transaction detected! New balance: " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC");
                    BigInteger subtract = currentBalance.Value - initialBalance.Value;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.maticMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "POL", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "MATIC");

                    }
                    initialBalance = currentBalance; // Aktualizacja salda

                }

                // Sprawdzenie zmian w saldzie USDT
                if (usdtBalance > initialUSDTBalance)
                {
                  //  Debug.Log("New USDT transaction detected! New balance: " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT"); // USDT ma 6 miejsc po przecinku


                    BigInteger subtract = usdtBalance - initialUSDTBalance;


                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdtMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", (subtract / 1000000).ToString(), txHash, whatChain);


                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDT");

                    }
                    initialUSDTBalance = usdtBalance; // Aktualizacja salda USDT

                }

                // Sprawdzenie zmian w saldzie USDC
                if (usdcBalance > initialUSDCBalance)
                {
                   // Debug.Log("New USDC transaction detected! New balance: " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC"); // USDC ma 6 miejsc po przecinku
                    BigInteger subtract = usdcBalance - initialUSDCBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdcMinimumBet)
                    {

                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDC", (subtract / 1000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000).ToString(), "USDC");

                    }

                    initialUSDCBalance = usdcBalance; // Aktualizacja salda USDC


                }

                // Sprawdzenie zmian w saldzie LINK
                if (linkBalance > initialLINKBalance)
                {
                  //  Debug.Log("New LINK transaction detected! New balance: " + Web3.Convert.FromWei(linkBalance, 18) + " LINK"); // LINK ma 18 miejsc po przecinku
                    BigInteger subtract = linkBalance - initialLINKBalance;
                    decimal withDecimals = ((decimal)subtract / 1000000000000000000);

                    if (withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.linkMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "LINK", (subtract / 1000000000000000000).ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((subtract / 10000000000000000).ToString(), "LINK");
                    }
                    initialLINKBalance = linkBalance; // Aktualizacja salda LINK

                }

                // Brak nowych transakcji
                if (currentBalance.Value == initialBalance.Value && usdtBalance == initialUSDTBalance && usdcBalance == initialUSDCBalance && linkBalance == initialLINKBalance)
                {
               //     Debug.Log("No new transactions. Current balances: MATIC - " + Web3.Convert.FromWei(currentBalance.Value) + " MATIC, USDT - " + Web3.Convert.FromWei(usdtBalance, 6) + " USDT, USDC - " + Web3.Convert.FromWei(usdcBalance, 6) + " USDC, LINK - " + Web3.Convert.FromWei(linkBalance, 18) + " LINK");
                }

            }












        }
        catch (Exception e)
        {
            Debug.LogError("Error checking for new transactions: " + e.Message);
        }
    }
    public IEnumerator CheckForNewTransactions()
    {
        if (PlayFabManager._Instance.addressEVM != null && PlayFabManager._Instance.addressEVM != "No address" && (DepositScript._Instance.DepositObject.gameObject.active == true))
        {
            CancelInvoke("StartCheckingForTransactions");

            Debug.Log("CHECKING FOR NEW TRANSACTIONS...");
            string walletAddress = (string)PlayFabManager._Instance.addressEVM;
            string whatChain = "";
            string txHash = "";
            decimal value = 0;

            // Określ łańcuch na podstawie ustawienia
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                whatChain = "ETH";
            }
            else if (PlayerPrefs.GetString("NewChain") == "56")
            {
                whatChain = "BSC";
            }
            else if (PlayerPrefs.GetString("NewChain") == "89")
            {
                whatChain = "MATIC";
            }

            // Pobieranie bieżącego salda portfela za pomocą UnityWebRequest (zamiast web3.Eth.GetBalance)
            string jsonRpcRequest = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBalance\",\"params\":[\"" + walletAddress + "\",\"latest\"],\"id\":1}";

            UnityWebRequest balanceRequest = new UnityWebRequest(rpcUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRpcRequest);
            balanceRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            balanceRequest.downloadHandler = new DownloadHandlerBuffer();
            balanceRequest.SetRequestHeader("Content-Type", "application/json");

            yield return balanceRequest.SendWebRequest();

            if (balanceRequest.result == UnityWebRequest.Result.ConnectionError || balanceRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + balanceRequest.error);
                yield break;
            }

            // Parsowanie odpowiedzi
            RpcResponse rpcResponse = JsonUtility.FromJson<RpcResponse>(balanceRequest.downloadHandler.text);
            if (rpcResponse != null && rpcResponse.result != null)
            {
                BigInteger currentBalance = BigInteger.Parse(rpcResponse.result.Substring(2), System.Globalization.NumberStyles.HexNumber); // Parsowanie Hex na BigInteger
              //  Debug.Log("big integer currenct bal: " + currentBalance);
                decimal balanceInEther = Web3.Convert.FromWei(currentBalance, 18);  // Konwersja do Ether

                Debug.Log("Balance: " + balanceInEther + " ETH");

                // Sprawdzenie tokenów ERC-20 (zamiast web3.Eth.GetContract)
                var erc20Abi = @"[{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""}]";
                var tokenAddresses = GetTokenAddressesForChain(PlayerPrefs.GetString("NewChain"));

                foreach (var token in tokenAddresses)
                {
                    // Tworzymy JSON-RPC request dla tokena ERC-20 (balanceOf)
                    string jsonRpcTokenRequest = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_call\",\"params\":[{\"to\":\"" + token.Address + "\",\"data\":\"0x70a08231000000000000000000000000" + walletAddress.Substring(2) + "\"},\"latest\"],\"id\":1}";

                    UnityWebRequest tokenRequest = new UnityWebRequest(rpcUrl, "POST");
                    byte[] tokenBodyRaw = Encoding.UTF8.GetBytes(jsonRpcTokenRequest);
                    tokenRequest.uploadHandler = new UploadHandlerRaw(tokenBodyRaw);
                    tokenRequest.downloadHandler = new DownloadHandlerBuffer();
                    tokenRequest.SetRequestHeader("Content-Type", "application/json");

                    yield return tokenRequest.SendWebRequest();

                    if (tokenRequest.result == UnityWebRequest.Result.ConnectionError || tokenRequest.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError("Error fetching token balance: " + tokenRequest.error);
                        continue;
                    }

                    RpcResponse tokenRpcResponse = JsonUtility.FromJson<RpcResponse>(tokenRequest.downloadHandler.text);
                    if (tokenRpcResponse != null && tokenRpcResponse.result != null)
                    {
                        BigInteger tokenBalance = BigInteger.Parse(tokenRpcResponse.result.Substring(2), System.Globalization.NumberStyles.HexNumber); // Parsowanie Hex na BigInteger
                        decimal withDecimals = Web3.Convert.FromWei(tokenBalance, token.Decimals);  // Konwersja na odpowiednią jednostkę
                                                                                                    //    Debug.Log("TOKEN " + token.Symbol);
                                                                                                    //     Debug.Log("INITIAL BALANCE " + token.InitialBalance);
                                                                                                    //     Debug.Log("TOKEN BALANCE " + tokenBalance);
                    //    Debug.Log("WITH DECIMALS " + withDecimals);
                        if (tokenBalance > token.InitialBalance)
                        {

                            BigInteger subtract = tokenBalance - token.InitialBalance;

                            // decimal ToSubtract = Web3.Convert.FromWei(subtract, token.Decimals);
                            decimal ToSubtract = (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000;

                            //Debug.Log("TO SUBTRACT " + ToSubtract);
                            // Sprawdzamy minimalny zakład dla każdego tokena
                            if (token.Symbol == "USDT" && (float)ToSubtract >= ((float)CryptoPriceReader._Instance.usdtMinimumBet))
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", ToSubtract.ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(ToSubtract.ToString(), "USDT");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000);

                                initialUSDTBalance = tokenBalance;

                            }
                            if (token.Symbol == "USDC" && (float)ToSubtract  >= ((float)CryptoPriceReader._Instance.usdcMinimumBet))
                            {

                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDC", ToSubtract.ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(ToSubtract.ToString(), "USDC");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000);

                                // TokenData usdcToken = tokenAddresses.Find(token => token.Symbol == "USDC");
                                //    usdcToken.InitialBalance = tokenBalance;
                                initialUSDCBalance = tokenBalance;
                            }
                            else if (token.Symbol == "LINK" && (float)ToSubtract / 1000000000000 >= ((float)CryptoPriceReader._Instance.linkMinimumBet))
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "LINK", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "LINK");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialLINKBalance = tokenBalance;

                            }
                            else if (token.Symbol == "SHIB" && (float)ToSubtract / 1000000000000 >= (float)CryptoPriceReader._Instance.shibMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "SHIB", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "SHIB");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialSHIBBalance = tokenBalance;

                            }
                            else if (token.Symbol == "APE" && (float)ToSubtract / 1000000000000 >= (float)CryptoPriceReader._Instance.apeMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "APE", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "APE");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialAPEBalance = tokenBalance;

                            }
                            else if (token.Symbol == "MATIC" && (float)ToSubtract / 1000000000000 >= (float)CryptoPriceReader._Instance.maticMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "MATIC", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "MATIC");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialUSDCBalance = tokenBalance;

                            }
                            else if (token.Symbol == "CRO" && (float)ToSubtract / 1000000000000 >= (float)CryptoPriceReader._Instance.cronosMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "CRO", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "CRO");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);
                                initialUSDCBalance = tokenBalance;

                            }
                            else if (token.Symbol == "ETH" && (float)ToSubtract / 1000000000000 >= (float)CryptoPriceReader._Instance.bnbMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "ETH", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "ETH");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialETHBalance = tokenBalance;

                            }
                         
                            else if (token.Symbol == "FDUSD" && (float)ToSubtract  / 1000000000000 >= (float)CryptoPriceReader._Instance.fdusdMinimumBet)
                            {
                                CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "FDUSD", (ToSubtract / 1000000000000).ToString(), txHash, whatChain);
                                PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode((ToSubtract / 1000000000000).ToString(), "FDUSD");
                                BetResultPopup._Instance.ShowResultPopup(true, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000, (((decimal)tokenBalance - (decimal)token.InitialBalance)) / 1000000000000000000);

                                initialFDUSDBalance = tokenBalance;

                            }

                            // Aktualizacja salda tokena
                        }
                    }
                }

                // Logika dla MATIC/ETH/BSC (native token)
               // if (currentBalance > initialStaticBalance)
                if (currentBalance > initialStatic)
                {
                    BigInteger subtract = currentBalance - initialStatic;
                 //   decimal withDecimals = Web3.Convert.FromWei((BigInteger)subtract);
                    decimal ToSubtract = (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000;

                    if (whatChain == "ETH" && (float)ToSubtract >= (float)CryptoPriceReader._Instance.ethMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "ETH", ToSubtract.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(ToSubtract.ToString(), "ETH");
                        BetResultPopup._Instance.ShowResultPopup(true, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000);


                    }
                    else if (whatChain == "BSC" && (float)ToSubtract >= (float)CryptoPriceReader._Instance.bnbMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "BNB", ToSubtract.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(ToSubtract.ToString(), "BNB");
                        BetResultPopup._Instance.ShowResultPopup(true, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000);
                    }
                    else if (whatChain == "MATIC" && (float)ToSubtract >= (float)CryptoPriceReader._Instance.maticMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "MATIC", ToSubtract.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(ToSubtract.ToString(), "MATIC");
                        BetResultPopup._Instance.ShowResultPopup(true, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000, (((decimal)currentBalance - (decimal)initialStatic)) / 1000000000000000000);
                    }

                    initialStatic = currentBalance;  // Aktualizacja salda MATIC/ETH/BSC
                }
            }

            // Resetowanie licznika po każdym wywołaniu
        }

        InvokeRepeating("StartCheckingForTransactions", 9, 10);  // Ponowne wywołanie co 10 sekund
        timer = 0;
        yield break;
    }
    public IEnumerator CheckForNewTransactions2()
    {
        if (PlayFabManager._Instance.addressEVM != null && PlayFabManager._Instance.addressEVM != "No address" && (DepositScript._Instance.DepositObject.gameObject.active == true))
        {
            CancelInvoke("StartCheckingForTransactions");

            Debug.Log("CHECKKING FOR NEW TRANSACTIONS...");
            string walletAddress = (string)PlayFabManager._Instance.addressEVM;
            string whatChain = "";
            string txHash = "";
            decimal value = 0;
            string jsonRpcRequest = "{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBalance\",\"params\":[\"" + walletAddress + "\",\"latest\"],\"id\":1}";

            UnityWebRequest request = new UnityWebRequest(rpcUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRpcRequest);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Określ łańcuch na podstawie ustawienia
            if (PlayerPrefs.GetString("NewChain") == "1")
            {
                whatChain = "ETH";
            }
            else if (PlayerPrefs.GetString("NewChain") == "56")
            {
                whatChain = "BSC";
            }
            else if (PlayerPrefs.GetString("NewChain") == "89")
            {
                whatChain = "MATIC";
            }
            if (web3 == null)
            {
                Debug.LogError("web3 object is null");
                yield break;
            }

            // Pobieranie bieżącego salda portfela
            var balanceRequest = web3.Eth.GetBalance.SendRequestAsync(walletAddress);
            while (!balanceRequest.IsCompleted) yield return null; // Czekanie na zakończenie zadania
            var currentBalance = balanceRequest.Result;

            // Sprawdzenie tokenów ERC-20
            var erc20Abi = @"[{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""}]";

            // Zdefiniowanie tokenów dla różnych łańcuchów (używane są przykładowe adresy)
            var tokenAddresses = GetTokenAddressesForChain(PlayerPrefs.GetString("NewChain"));

            foreach (var token in tokenAddresses)
            {
                var contract = web3.Eth.GetContract(erc20Abi, token.Address);
                var balanceOfFunction = contract.GetFunction("balanceOf");

                var balanceRequestToken = balanceOfFunction.CallAsync<BigInteger>(walletAddress);
                while (!balanceRequestToken.IsCompleted) yield return null; // Czekanie na zakończenie zadania dla każdego tokena

                var balance = balanceRequestToken.Result;
                if (balance > token.InitialBalance)
                {
                    BigInteger subtract = balance - token.InitialBalance;
                    decimal withDecimals = Web3.Convert.FromWei(subtract, token.Decimals);

                    // Zamiast `GetMinimumBetForToken`, bezpośrednio odnosimy się do odpowiednich pól `ProtectedFloat`
                    if (token.Symbol == "USDT" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdtMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDT", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "USDT");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);
                    }
                    else if (token.Symbol == "USDC" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.usdcMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "USDC", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "USDC");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "LINK" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.linkMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "LINK", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "LINK");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "SHIB" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.shibMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "SHIB", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "SHIB");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "APE" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.apeMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "APE", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "APE");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "MATIC" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.maticMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "MATIC", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "MATIC");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "CRO" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.cronosMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "CRO", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "CRO");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "BNB" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.bnbMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "BNB", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "BNB");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }
                    else if (token.Symbol == "FDUSD" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.fdusdMinimumBet)
                    {
                        CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "FDUSD", withDecimals.ToString(), txHash, whatChain);
                        PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "FDUSD");
                        BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                    }

                    // Aktualizujemy początkowe saldo
                    token.InitialBalance = balance;
                }
            }
  

            // Logika dla MATIC/ETH/BSC (native token)
            if (currentBalance > initialStatic)
            {
                BigInteger subtract = currentBalance.Value - initialStatic;
                decimal withDecimals = Web3.Convert.FromWei((BigInteger)subtract);

                // Bezpośrednie odwołanie do odpowiednich pól, np. `ethMinimumBet`
                if (whatChain == "ETH" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.ethMinimumBet)
                {
                    CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "ETH", withDecimals.ToString(), txHash, whatChain);
                    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "ETH");
                    BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                }
                else if (whatChain == "BSC" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.bnbMinimumBet)
                {
                    CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "BNB", withDecimals.ToString(), txHash, whatChain);
                    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "BNB");
                    BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                }
                else if (whatChain == "MATIC" && withDecimals >= (ProtectedDecimal)CryptoPriceReader._Instance.maticMinimumBet)
                {
                    CryptoDepositHistory._Instance.AddDeposit(System.DateTime.Now.ToString("dd.MM.yyyy"), "MATIC", withDecimals.ToString(), txHash, whatChain);
                    PlayFabManager._Instance.ExecuteCloudScriptDepositSetCode(withDecimals.ToString(), "MATIC");
                    BetResultPopup._Instance.ShowResultPopup(true, withDecimals, withDecimals);

                }

                initialStaticBalance = (decimal)(BigInteger)currentBalance;  // Aktualizacja salda MATIC/ETH/BSC
            }
             // Resetowanie licznika po każdym wywołaniu

        }
        InvokeRepeating("StartCheckingForTransactions", 9, 10); // Wywo�anie asynchronicznej metody bez StartCoroutine

        timer = 0;
        yield break;
    }


    // Funkcja do pobrania adresów tokenów dla danego łańcucha
    List<TokenData> GetTokenAddressesForChain(string chain)
        {
            List<TokenData> tokens = new List<TokenData>();

            if (chain == "1")
            {
                tokens.Add(new TokenData("USDT", "0xdac17f958d2ee523a2206206994597c13d831ec7", initialUSDTBalance, 6));
                tokens.Add(new TokenData("USDC", "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48", initialUSDCBalance, 6));
                tokens.Add(new TokenData("LINK", "0x514910771af9ca656af840dff83e8264ecf986ca", initialLINKBalance, 18));
                tokens.Add(new TokenData("FDUSD", "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409", initialLINKBalance, 18));
                tokens.Add(new TokenData("MATIC", "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0", initialMATICBalance, 18));
                tokens.Add(new TokenData("SHIB", "0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce", initialSHIBBalance, 18));
                tokens.Add(new TokenData("APE", "0x4d224452801aced8b2f0aebe155379bb5d594381", initialAPEBalance, 18));
                tokens.Add(new TokenData("CRO", "0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b", initialCROBalance, 18));
                // Dodaj inne tokeny...
            }
            else if (chain == "56")
            {
                tokens.Add(new TokenData("USDT", "0x55d398326f99059ff775485246999027b3197955", initialUSDTBalance, 6));
                tokens.Add(new TokenData("USDC", "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d", initialUSDCBalance, 6));
                tokens.Add(new TokenData("LINK", "0xf8a0bf9cf54bb92f17374d9e9a321e6a111a51bd", initialLINKBalance, 18));
                tokens.Add(new TokenData("MATIC", "0xcc42724c6683b7e57334c4e856f4c9965ed682bd", initialMATICBalance, 18));
                tokens.Add(new TokenData("FDUSD", "0xc5f0f7b66764F6ec8C8Dff7BA683102295E16409", initialFDUSDBalance, 18));
                tokens.Add(new TokenData("ETH", "0x2170ed0880ac9a755fd29b2688956bd959f933f8", initialETHBalance, 18));
                // Dodaj inne tokeny...
            }
            else if (chain == "89")
            {
                tokens.Add(new TokenData("USDT", "0xc2132d05d31c914a87c6611c10748aeb04b58e8f", initialUSDTBalance, 6));
                tokens.Add(new TokenData("USDC", "0x3c499c542cEF5E3811e1192ce70d8cC03d5c3359", initialUSDCBalance, 6));
                tokens.Add(new TokenData("LINK", "0x53e0bca35ec356bd5dddfebbd1fc0fd03fabad39", initialLINKBalance, 18));
                // Dodaj inne tokeny...
            }


            return tokens;

        
    }
   

   
// Update sprawdza saldo co 5 sekund
void Update()
    {
        //  if (PlayFabManager._Instance.addressEVM != null && PlayFabManager._Instance.addressEVM != "No address" && (DepositScript._Instance.DepositObject.gameObject.active == true))
        //  {
        //
        //      DepositScript._Instance.nextDepositText.text = "Next deposit check in: " + (checkInterval - timer).ToString("F0") + " seconds\n<color=red>Do not close this scene when you deposit!";
        //      timer += Time.deltaTime;
        //      if (timer >= checkInterval)
        //      {
        //
        //          timer = 0; // Reset timera
        //      }
        //  }
        DepositScript._Instance.nextDepositText.text = "Next deposit check in: " + (checkInterval - timer).ToString("F0") + " seconds\n<color=red>Do not close this scene when you deposit!";
    }
}
public class TokenData
{
    public string Symbol;
    public string Address;
    public BigInteger InitialBalance;
    public int Decimals;

    public TokenData(string symbol, string address, BigInteger initialBalance, int decimals)
    {
        Symbol = symbol;
        Address = address;
        InitialBalance = initialBalance;
        Decimals = decimals;
    }
}