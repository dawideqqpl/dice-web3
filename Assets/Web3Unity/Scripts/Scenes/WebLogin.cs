using OPS.AntiCheat.Prefs;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    ProjectConfigScriptableObject projectConfigSO = null;

    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    public static WebLogin _Instance;
    public Dropdown chain;
    private void Awake()
    {
        _Instance = this;
    }
    public void OnLogin()
    {
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
       // projectConfigSO.ProjectId = "d025c9d1-8f7a-4d0b-88ea-116dd3ab768a";

        if (ProtectedPlayerPrefs.GetString("NewChain") == "1")
        {
            projectConfigSO.ChainId = "1";
            projectConfigSO.Chain = "Ethereum";
            projectConfigSO.Network = "Mainnet";
            projectConfigSO.Rpc = "https://eth.llamarpc.com";
        }
        if (ProtectedPlayerPrefs.GetString("NewChain") == "56")
        {
            projectConfigSO.ChainId = "56";
            projectConfigSO.Chain = "Binance";
            projectConfigSO.Network = "Mainnet";
            projectConfigSO.Rpc = "https://binance.llamarpc.com";
        }
        if (ProtectedPlayerPrefs.GetString("NewChain") == "89")
        {
            projectConfigSO.ChainId = "89";
            projectConfigSO.Chain = "Polygon";
            projectConfigSO.Network = "Mainnet";
            projectConfigSO.Rpc = "https://polygon.llamarpc.com";
        }
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectId);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainId);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.Rpc);


        Web3Connect();
        OnConnected();
    }
    
    void Start() 
    {
    //  projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
    //  PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectId);
    //  PlayerPrefs.SetString("ChainID", projectConfigSO.ChainId);
    //  PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
    //  PlayerPrefs.SetString("Network", projectConfigSO.Network);
    //  PlayerPrefs.SetString("RPC", projectConfigSO.Rpc);
    }
    public void SetChain(string chain)
    {
        Debug.Log("WHAT CHAIN " + chain);
        PlayerPrefs.SetString("NewChain", chain);


    }
    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        ProtectedPlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");
        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
