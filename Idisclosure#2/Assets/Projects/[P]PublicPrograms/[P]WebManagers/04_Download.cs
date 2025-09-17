using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;


public class Download : MonoBehaviour
{
    [Header("値段を設定")]
    public string Drain;
    [Header("DownloadするToolの名前を入力")]
    public string webName;
    [Header("素材")]
    public Image greenLine;
    public Image pinkLine;
    public bool testMode;
    public bool phishing;
    /*----------初期値----------*/
    string showTerminal = "";
    string showBrowser = "";
    string webIP = "";
    float progress = 0f;
    private const byte SuccessPhishing = 3;
    private string[] Malwares;

    void Start()
    {
        LoadMalwares();
    }

    public void DownloadTool()
    {
        int drain = int.Parse(Drain);
        string Name = PlayerPrefs.GetString("Name", "").Replace("\u200B", "");
        int tokens = int.Parse(PlayerPrefs.GetString("Tokens", "100").Replace("\u200B", ""));

        if (testMode)
        {
            StartCoroutine(ShowProgressBar());
            return;
        }

        // WebのIPを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(webName + "IP"))
        {
            webIP = (string)PhotonNetwork.CurrentRoom.CustomProperties[webName + "IP"];
        }
        Debug.Log("WebのIPを取得");

        // Tokensの支払い処理
        if ((tokens - drain) >= 0 && !PlayerPrefs.GetString("TerminalDisplay", "").Contains(webName + "\n"))
        {
            // 購入可能 かつ 未所持だった場合

            // 購入手続き
            tokens -= drain;
            PlayerPrefs.SetString("Tokens", tokens.ToString());
            PlayerPrefs.Save();
            Debug.Log("購入手続き");

            if (!phishing)
            {
                // Phishingじゃなければ入手する

                // 購入後に登録
                PlayerPrefs.SetInt("Download" + webName, 1);
                PlayerPrefs.Save();
                Debug.Log("購入後に登録");

                // TerminalDisplayの取得、追加、保存
                showTerminal = PlayerPrefs.GetString("TerminalDisplay", "");
                showTerminal += webName + "\n";
                PlayerPrefs.SetString("TerminalDisplay", showTerminal);
                PlayerPrefs.Save();
                Debug.Log("TerminalDisplayの取得、追加、保存");

                // webの存在Logを消去
                Hashtable RemoveStoreWeb = new Hashtable { { webName, false } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(RemoveStoreWeb);
                Debug.Log("webの存在Logを消去");

                // webをBrowserから削除
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
                {
                    showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
                    if (showBrowser.Contains(webName + "\n→ " + webIP + "\n"))
                    {
                        showBrowser = showBrowser.Replace(webName + "\n→ " + webIP + "\n", "");
                    }
                }
                Hashtable ShowBrowser = new Hashtable { { "BrowserDisplay", showBrowser } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(ShowBrowser);
                Debug.Log("webをBrowserから削除");
            }
            else
            {
                // 宛先探索
                string phisherName = (string)PhotonNetwork.CurrentRoom.CustomProperties["Phisher" + webName];
                Player targetPlayer = null;
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.NickName == phisherName)
                    {
                        targetPlayer = player;
                        break;
                    }
                }

                // ウイルス判定
                string Infector = "";
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Phisher" + webName))
                {
                    Infector = (string)PhotonNetwork.CurrentRoom.CustomProperties["Phisher" + webName];
                }
                foreach (string malware in Malwares)
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(webName + "Infect" + malware) && (bool)PhotonNetwork.CurrentRoom.CustomProperties[webName + "Infect" + malware])
                    {
                        // webにウイルスが存在すればウイルスに感染、仕掛け人の保存
                        PlayerPrefs.SetInt("Infect" + malware, 1);
                        PlayerPrefs.SetString("Infector" + malware, Infector);
                        PlayerPrefs.Save();
                        Debug.Log(Infector + "に" + malware + "を仕掛けられた！");
                    }
                }

                // 支払われたらServerの主に送金
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { targetPlayer.ActorNumber } };
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(SuccessPhishing, drain, raiseEventOptions, sendOptions);
                SceneManager.LoadScene("Failed");

                // Phishingの被害を全員に通知
                string notificationDisplayGlobal = "";
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NotificationDisplayGlobal"))
                {
                    notificationDisplayGlobal = (string)PhotonNetwork.CurrentRoom.CustomProperties["NotificationDisplayGlobal"];
                    notificationDisplayGlobal += "[" + phisherName + "]" + Name + " is hooked!!\n";
                }
                else
                {
                    notificationDisplayGlobal = "[" + phisherName + "]" + Name + " is hooked!!\n";
                }
                Hashtable PhishingNotification = new Hashtable
                {
                    {"NotificationDisplayGlobal", notificationDisplayGlobal},
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(PhishingNotification);
            }


            StartCoroutine(ShowProgressBar());
        }
        else if ((tokens - drain) < 0)
        {
            Debug.Log("所持金が足りていないようです...");
        }
        else if (PlayerPrefs.GetString("TerminalDisplay", "").Contains(webName + "\n"))
        {
            Debug.Log("すでに所持しているようです");
        }
    }

    private IEnumerator ShowProgressBar()
    {
        // ProgressBarを描画
        progress = 0f;
        while (progress <= 1f)
        {
            progress += 0.01f;
            greenLine.fillAmount = progress;
            pinkLine.fillAmount = 1f - progress;
            Debug.Log("progressbarを描画中");
            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadScene("Success");
    }

    private void LoadMalwares()
    {
        // Jsonのパスを指定してテキスト形式で読み取り
        TextAsset jsonFile = Resources.Load<TextAsset>("malwares");
        if (jsonFile != null)
        {
            // 正しい形式にしてconfigに保存、そのconfigの読み出し
            MalwareConfig config = JsonUtility.FromJson<MalwareConfig>(jsonFile.text);
            Malwares = config.malwares;
        }
        else
        {
            Debug.LogError("Malwares config not found!");
            Malwares = new string[0];
        }
    }
}


