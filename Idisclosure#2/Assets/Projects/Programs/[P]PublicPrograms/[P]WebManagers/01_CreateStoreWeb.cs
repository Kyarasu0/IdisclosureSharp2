using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections.Generic;


public class CreateStoreWeb : MonoBehaviour
{
    int randomValue = 0;
    string[] Webs;
    string showBrowser = "";

    string snsServerIP = "";

    string getWebIP = "";

    void Start()
    {
        LoadWebs();
        // 起動してから1秒間隔でランダムを回す
        InvokeRepeating(nameof(RandomCreator), 0f, 1f);
        InvokeRepeating(nameof(UpdateWebs), 0f, 0.3f);
        Debug.Log("CreateStoreWeb始動!");
    }

    void UpdateWebs()
    {
        for (int i = 0; i < Webs.Length; i++)
        {
            // showBrowserに含まれておらず、webがtrueだったらshowBrowserに追加する
            if (!(showBrowser.Contains(Webs[i] + "\n")) && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Webs[i]) && (bool)PhotonNetwork.CurrentRoom.CustomProperties[Webs[i]])
            {
                // WebのIPを取得
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Webs[i] + "IP"))
                {
                    getWebIP = (string)PhotonNetwork.CurrentRoom.CustomProperties[Webs[i] + "IP"];
                }

                // Browserに追加
                showBrowser += Webs[i] + "\n→ " + getWebIP + ": 80\n\n";
                Hashtable ShowDisplay = new Hashtable { { "BrowserDisplay", showBrowser } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(ShowDisplay);
                Debug.Log(Webs[i] + "が描画された！");
            }
        }
    }

    void RandomCreator()
    {
        // Browserのデータを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
        {
            showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
        }

        // SNS Serverが登録されていない場合
        if (!(showBrowser.Contains("SNS Server\n")))
        {
            // SNSServerのIPを取得
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SNSServerIP"))
            {
                snsServerIP = (string)PhotonNetwork.CurrentRoom.CustomProperties["SNSServerIP"];
                // Browserに追加
                showBrowser += "SNS Server\n→ " + snsServerIP + ": 80\n\n";
                Hashtable ShowDisplay = new Hashtable { { "BrowserDisplay", showBrowser } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(ShowDisplay);
            }
        }

        // 特定のIDが当たればWebが建ち上がる
        randomValue = Random.Range(0, Webs.Length + 150);
        for (int i = 0; i < Webs.Length; i++)
        {
            // Webのデータがない または Webが存在しないなら確率が当たっているか確認
            if (!(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Webs[i]) && (bool)PhotonNetwork.CurrentRoom.CustomProperties[Webs[i]]))
            {
                if (randomValue == i)
                {
                    // 判別しているプロパティをtrueにする
                    Hashtable props = new Hashtable { { Webs[i], true } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                    MakeIP(Webs[i]);
                    Debug.Log("Create" + Webs[i]);
                }
            }
        }
    }

    public void MakeIP(string webName)
    {
        // IPアドレスのリストを取得する
        List<string> IPList = new List<string>();
        string NewIP = "";
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IPList"))
        {
            IPList = new List<string>(PhotonNetwork.CurrentRoom.CustomProperties["IPList"].ToString().Split(','));
        }

        // 一意性の確認と追加
        while (true)
        {
            // IPを生成
            int first = UnityEngine.Random.Range(0, 256);
            int second = UnityEngine.Random.Range(0, 256);
            int third = UnityEngine.Random.Range(0, 256);
            int fourth = UnityEngine.Random.Range(0, 256);

            NewIP = $"{first}.{second}.{third}.{fourth}";

            // 一意性が保証されたら追加
            if (!(IPList.Contains(NewIP)))
            {
                IPList.Add(NewIP);
                break;
            }
        }

        // サーバー上に保存
        Hashtable props = new Hashtable
        {
            { "IPList", string.Join(",", IPList) },
            { webName + "IP", NewIP }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        Debug.Log("IPList is saved! : " + string.Join(",", IPList));
    }
    
    private void LoadWebs()
    {
        // Jsonのパスを指定してテキスト形式で読み取り
        TextAsset jsonFile = Resources.Load<TextAsset>("Jsons/webs");
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

