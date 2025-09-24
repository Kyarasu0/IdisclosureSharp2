using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PhishingMenu : MonoBehaviour
{
    string[] Webs;
    string[] Malwares;
    string phishingMenuDisplay = "";
    string showBrowser = "";
    public TMP_InputField InputPhishing;
    public int Drain;
    void Start()
    {
        LoadWebs();
        LoadMalwares();
    }
    void Update()
    {
        // 空の状態からスタート
        phishingMenuDisplay = "";
        // 建ち上がっていないWebを探索してそれを表示する
        foreach (string Web in Webs)
        {
            // Webのデータがある かつ Webが存在しないならPhishingMenuDisplayに追加
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Web) && !((bool)PhotonNetwork.CurrentRoom.CustomProperties[Web]))
            {
                phishingMenuDisplay += Web + "\n\n";
            }
        }
        // データを保存
        Hashtable PhishingMenuDisplay = new Hashtable
        {
            {"PhishingMenuDisplay", phishingMenuDisplay},
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(PhishingMenuDisplay);
    }

    public void PhishingStart()
    {
        // 検索に引っかかるかを確認
        foreach (string Web in Webs)
        {
            // 検索に引っかかる かつ (Webのデータがない または Webが存在しない)なら時間を記録してPhishingを開始
            if (InputPhishing.text.ToLower() == Web.ToLower() && !(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Web) && (bool)PhotonNetwork.CurrentRoom.CustomProperties[Web]))
            {
                // Battery情報を取得
                int battery = PlayerPrefs.GetInt("BatteryMyServer", 0);
                if ((battery - Drain >= 0))
                {
                    // Batteryを減らす処理
                    battery -= Drain;
                    PlayerPrefs.SetInt("BatteryMyServer", battery);
                    PlayerPrefs.Save();

                    // サーバーにWebの存在とPhishingWebの存在を保存
                    Hashtable fishing = new Hashtable
                    {
                        { Web, true },
                        { "Phishing" + Web, true },
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(fishing);

                    // ローカルにPhishingの有無とPhishing中のWeb名を保存
                    PlayerPrefs.SetInt("PhishingNow", 1);
                    PlayerPrefs.SetString("PhishingWeb", Web);
                    PlayerPrefs.Save();

                    // Browserに追加

                    // MyServerのIPを取得
                    string ServerIP = PlayerPrefs.GetString("ServerIP", "0.0.0.0").Replace("\u200B", "");

                    // Browserの情報を取得
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
                    {
                        showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
                    }

                    // Browser上に存在しなければ表示
                    if (!(showBrowser.Contains(Web + "\n\n")))
                    {
                        showBrowser += Web + "\n→ " + ServerIP + "\n\n";
                        Hashtable ShowDisplay = new Hashtable { { "BrowserDisplay", showBrowser } };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(ShowDisplay);
                    }

                    // 宛先を保存
                    Hashtable fisher = new Hashtable
                    {
                        {"Phisher" + Web, PhotonNetwork.NickName},
                        {"Phishing" + Web + "IP", ServerIP}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(fisher);

                    // ウイルス判定
                    foreach (string Malware in Malwares)
                    {
                        // 仕掛ける準備が済んだウイルスを持っている場合
                        if (PlayerPrefs.GetInt("Have" + Malware, 0) == 1)
                        {
                            // 手持ちのウイルスを消去
                            PlayerPrefs.SetInt("Have" + Malware, 0);
                            PlayerPrefs.Save();

                            // PhishingにMalwareを感染させる
                            Hashtable IMalware = new Hashtable{
                                {Web + "Infect" + Malware, true},
                            };
                            PhotonNetwork.CurrentRoom.SetCustomProperties(IMalware);
                            Debug.Log(Web + "に" + Malware + "を付与した！");
                        }
                    }

                    // Phishing開始時間を保存
                    PlayerPrefs.SetString("PhishingStartTime",((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString());
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("Success");
                }
            }
        }
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

    private void LoadMalwares()
    {
        // Jsonのパスを指定してテキスト形式で読み取り
        TextAsset jsonFile = Resources.Load<TextAsset>("Jsons/malwares");
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
