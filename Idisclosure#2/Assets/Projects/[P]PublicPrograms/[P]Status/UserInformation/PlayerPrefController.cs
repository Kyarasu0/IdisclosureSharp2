using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerPrefsController : MonoBehaviourPunCallbacks
{
    public TMP_Text TokensText;
    public TMP_Text BatteryText;

    public TMP_Text PlayerIP;
    public TMP_Text ServerIP;
    [Header("素材")]
    public Image greenLine;
    public Image pinkLine;

    void Update()
    {
        // 自分の情報をPlayerPrefsから取得
        string name = PlayerPrefs.GetString("Name", "name").Replace("\u200B", "");
        string secretID = PlayerPrefs.GetString("SecretID", "secret").Replace("\u200B", "");
        int tokens = PlayerPrefs.GetInt("Tokens", 0);
        int battery = PlayerPrefs.GetInt("Battery", 100);
        string birthday = PlayerPrefs.GetString("Birthday", "0101").Replace("\u200B", "");
        string birthyear = PlayerPrefs.GetString("Birthyear", "2000").Replace("\u200B", "");
        int age = PlayerPrefs.GetInt("Age", 00);

        // IPとポート番号を取得
        string serverIP = "";
        PlayerIP.text = PlayerPrefs.GetString("PlayerIP", "0.0.0.0").Replace("\u200B", "");
        serverIP += PlayerPrefs.GetString("ServerIP", "0.0.0.0").Replace("\u200B", "");
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Port"))
        {
            serverIP += PhotonNetwork.LocalPlayer.CustomProperties["Port"];
        }
        ServerIP.text = serverIP;

        // BatteryBarを編集
        greenLine.fillAmount = (float)battery * 0.01f;
        pinkLine.fillAmount = 1f - (float)battery * 0.01f;

        // UIに表示
        if (TokensText != null)
        {
            TokensText.text = tokens.ToString();
        }
        if (BatteryText != null)
        {
            if (battery > 100)
            {
                battery = 100;
                BatteryText.text = battery.ToString();
            }
            else if (battery <= 0)
            {
                battery = 0;
                BatteryText.text = battery.ToString();
                // SceneManager.LoadScene("BatteryDead");
            }
            else
            {
                BatteryText.text = battery.ToString();
            }
            
        }

        // カスタムプロパティに設定
        Hashtable props = new Hashtable();
        props["Name"] = name;
        props["SecretID"] = secretID;
        props["Tokens"] = tokens;
        props["Battery"] = battery;
        props["Birthday"] = birthday;
        props["Birthyear"] = birthyear;
        props["Age"] = age;

        // Photonのネットワークに保存
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}

