using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class PhishingManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte SuccessPhishing = 3;
    double startTime = 0;
    string showBrowser = "";

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SuccessPhishing)
        {
            // Tokensを受け取る
            int income = (int)photonEvent.CustomData;
            int Tokens = int.Parse(PlayerPrefs.GetString("Tokens", "0").Replace("\u200B", ""));
            Tokens += income;
            PlayerPrefs.SetString("Tokens", Tokens.ToString());
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        startTime = double.Parse(PlayerPrefs.GetString("PhishingStartTime","0"));
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PhishingNow") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["PhishingNow"])
        {
            if (Time.time - startTime >= 90)
            {
                StopPhishingApplication();
            }
        }
    }

    void StopPhishingApplication()
    {
        //PhishingAppの取得と無効化
        string phishingAppName = "";
        string ServerIP = (string)PlayerPrefs.GetString("ServerIP","0.0.0.0");
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PhishingAppName"))
        {
            phishingAppName = (string)PhotonNetwork.LocalPlayer.CustomProperties["PhishingAppName"];
        }
        Hashtable webs = new Hashtable 
        { 
            { phishingAppName, false },
            { "Phishing" + phishingAppName, false },
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(webs);
        // Browserから削除
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
        {
            showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
        }
        if (showBrowser.Contains(phishingAppName + "\n"))
        {
            showBrowser = showBrowser.Replace(phishingAppName + "\n→ " + ServerIP + "\n","");
            Hashtable ShowDisplay = new Hashtable { { "BrowserDisplay", showBrowser } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(ShowDisplay);
        }

        Hashtable props = new Hashtable
        {
            { "PhishingNow", false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log("Done!");
    }
}