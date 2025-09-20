using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

using System.Linq;
using System;
using System.Net;

public class BrowserSearch : MonoBehaviour
{
    public TMP_Text SearchWords;
    public string PC_Or_Server;
    string WitchBattery;
    string WitchIP;
    string[] Webs;
    [Header("検索に使用するBattery")]
    public int Drain;

    void Start()
    {
        LoadWebs();
    }

    public void SearchAndMove()
    {
        // 検索ワードの圧縮化
        string searchWords = SearchWords.text.Trim().ToLower().Replace("\u200B", "").Replace(" ", "");
        Debug.Log("Browser起動:" + searchWords);

        // BatteryとIPの設定
        if (PC_Or_Server == "PC")
        {
            WitchBattery = "Battery";
            WitchIP = "PlayerIP";
        }
        else if (PC_Or_Server == "Server")
        {
            WitchBattery = "BatteryMyServer";
            WitchIP = "ServerIP";
        }

        foreach (string web in Webs)
        {
            /*----------検索----------*/
            // データとwebの存在を確認
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(web) && (bool)PhotonNetwork.CurrentRoom.CustomProperties[web] || searchWords == "SNSServer".ToLower())
            {
                if (searchWords == web.ToLower())
                {
                    // Battery関係を設定
                    int Battery = PlayerPrefs.GetInt(WitchBattery, 0);

                    if ((Battery - Drain >= 0))
                    {
                        // バッテリー減算処理
                        Battery -= Drain;
                        PlayerPrefs.SetInt(WitchBattery, Battery);
                        PlayerPrefs.Save();

                        // Wifi番号を取得
                        int WifiNumber = PlayerPrefs.GetInt("WifiNumber", 1);

                        // 送信元IPを取得
                        string IP = "";
                        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(WitchIP))
                        {
                            IP = (string)PhotonNetwork.LocalPlayer.CustomProperties[WitchIP];
                        }

                        // Phishingの場合
                        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Phishing" + web) && (bool)PhotonNetwork.CurrentRoom.CustomProperties["Phishing" + web])
                        {
                            // PhishingWebIPを取得
                            string PhishingWebIP = "";
                            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Phishing" + web + "IP"))
                            {
                                PhishingWebIP = (string)PhotonNetwork.CurrentRoom.CustomProperties["Phishing" + web + "IP"];
                            }

                            // WiFiに保存
                            SortAndSave(PhishingWebIP, IP, WifiNumber);

                            // Scene遷移
                            SceneManager.LoadScene("Phishing" + web);
                        }

                        // SNS Serverの場合
                        else if (searchWords == "SNSServer".ToLower())
                        {
                            // SNSServerIPを取得
                            string snsServerIP = "";
                            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SNSServerIP"))
                            {
                                snsServerIP = (string)PhotonNetwork.CurrentRoom.CustomProperties["SNSServerIP"];
                            }

                            // WiFiに保存
                            SortAndSave(snsServerIP, IP, WifiNumber);

                            // Scene遷移
                            SceneManager.LoadScene("SNSServer");
                        }

                        // Phishingではないの場合
                        else
                        {
                            // WebIPを取得
                            string WebIP = "";
                            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(web + "IP"))
                            {
                                WebIP = (string)PhotonNetwork.CurrentRoom.CustomProperties[web + "IP"];
                            }

                            // WiFiに保存
                            SortAndSave(WebIP, IP, WifiNumber);

                            // Scene遷移
                            SceneManager.LoadScene(web);
                        }

                        // 誰かのPCまたはサーバーに侵入する場合
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            // 名前を取得
                            string playerName = "";
                            string playerIP = "";
                            string playerPort = "";
                            string serverIP = "";
                            string serverPort = "";
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                playerName = (string)player.CustomProperties["Name"];
                            }

                            // PCのIPを取得
                            if (player.CustomProperties.ContainsKey("PlayerIP"))
                            {
                                playerIP = (string)player.CustomProperties["PlayerIP"];
                            }
                            // PCのPortを取得
                            if (player.CustomProperties.ContainsKey("Port"))
                            {
                                playerPort = (string)player.CustomProperties["Port"];
                            }

                            // 検索がPCにかかっていた場合
                            if (searchWords == playerIP + playerPort)
                            {
                                // 誰のログイン画面かを記録
                                PlayerPrefs.SetString("whoLogin", playerName);
                                PlayerPrefs.Save();

                                // シーンを遷移
                                SceneManager.LoadScene("Login");
                            }

                            // ServerのIPを取得
                            if (player.CustomProperties.ContainsKey("ServerIP"))
                            {
                                serverIP = (string)player.CustomProperties["ServerIP"];
                            }
                            // ServerのPortを取得
                            if (player.CustomProperties.ContainsKey("PortMyServer"))
                            {
                                serverPort = (string)player.CustomProperties["PortMyServer"];
                            }

                            // 検索がServerにかかっていた場合
                            if (searchWords == serverIP + serverPort)
                            {
                                // 誰のログイン画面かを記録
                                PlayerPrefs.SetString("whoLogin", playerName);
                                PlayerPrefs.Save();

                                // シーンを遷移
                                SceneManager.LoadScene("LoginMyServer");
                            }
                        }
                    }

                }
            }

        }
    }

    public void SortAndSave(string IP1, string IP2, int WifiNumber)
    {
        int Permutation = UnityEngine.Random.Range(0, 2);

        string NewRecord = (Permutation == 0) ? $"{IP1}:{IP2}\n" : $"{IP2}:{IP1}\n";

        // 既存のWiFi記録を取得して追加
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("WiFi" + WifiNumber))
        {
            string Record = (string)PhotonNetwork.CurrentRoom.CustomProperties["WiFi" + WifiNumber];
            NewRecord = Record + NewRecord;
        }

        // カスタムプロパティを更新
        Hashtable props = new Hashtable
        {
            { "WiFi" + WifiNumber, NewRecord },
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
    
    private void LoadWebs()
    {
        // Jsonのパスを指定してテキスト形式で読み取り
        TextAsset jsonFile = Resources.Load<TextAsset>("webs");
        if (jsonFile != null)
        {
            // 正しい形式にしてconfigに保存、そのconfigの読み出し
            WebsConfig config = JsonUtility.FromJson<WebsConfig>(jsonFile.text);
            Webs = config.webs;
        }
        else
        {
            Debug.LogError("Webs config not found!");
            Webs = new string[0];
        }
    }
}

