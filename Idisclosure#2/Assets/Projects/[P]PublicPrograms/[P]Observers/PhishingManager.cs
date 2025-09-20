using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class PhishingManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte SuccessPhishing = 3;
    long startUnix = 0;
    long nowUnix = 0;
    string showBrowser = "";

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SuccessPhishing)
        {
            // Tokensを受け取る
            int income = (int)photonEvent.CustomData;
            int Tokens = PlayerPrefs.GetInt("Tokens", 0);
            Tokens += income;
            PlayerPrefs.SetInt("Tokens", Tokens);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        startUnix = long.Parse(PlayerPrefs.GetString("PhishingStartTime", "0"));
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PhishingNow") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["PhishingNow"])
        {
            // 現在の時間を取得
            nowUnix = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

            // 3分を越したらストップ
            if (nowUnix - startUnix >= 90)
            {
                StopPhishingApplication();
            }
        }
    }

    void StopPhishingApplication()
    {
        // Phishing中のWeb名を取得
        string phishingWeb = PlayerPrefs.GetString("PhishingWeb", "").Replace("\u200B", "");

        // ServerのIPを取得
        string ServerIP = (string)PlayerPrefs.GetString("ServerIP","0.0.0.0");

        // Webの存在とPhishingのFlagを消す
        Hashtable webs = new Hashtable 
        { 
            { phishingWeb, false },
            { "Phishing" + phishingWeb, false },
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(webs);

        // Browserから削除
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
        {
            showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
        }
        if (showBrowser.Contains(phishingWeb + "\n"))
        {
            showBrowser = showBrowser.Replace(phishingWeb + "\n→ " + ServerIP + "\n\n","");
            Hashtable ShowDisplay = new Hashtable { { "BrowserDisplay", showBrowser } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(ShowDisplay);
        }

        // PhihsingNow
        PlayerPrefs.SetInt("PhishingNow", 0);
        PlayerPrefs.Save();
        Debug.Log("Done!");
    }
}