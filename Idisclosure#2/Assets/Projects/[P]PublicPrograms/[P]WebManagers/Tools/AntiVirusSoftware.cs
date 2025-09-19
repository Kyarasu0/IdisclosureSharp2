using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AntiVirusSoftware : MonoBehaviour
{
    string notificationDisplayLocal = "";
    private string[] Malwares;
    void Start()
    {
        LoadMalwares();
    }
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int drain = 10;
        // Battery情報を取得
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);
        // Battery的に起動可能な場合
        if (battery - drain >= 0)
        {
            // NotificationDisplayLocalを取得
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("NotificationDisplayLocal"))
            {
                notificationDisplayLocal = (string)PhotonNetwork.LocalPlayer.CustomProperties["NotificationDisplayLocal"];
            }

            // Battery処理
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            // 各Malwareを検査
            foreach (string Malware in Malwares)
            {
                // Malware検知の有無を通知
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Infect" + Malware) && (bool)PhotonNetwork.LocalPlayer.CustomProperties["Infect" + Malware])
                {
                    Hashtable Antivirus = new Hashtable
                    {
                        {"Infect" + Malware, false},
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(Antivirus);
                    Debug.Log("[Antivirus]" + Malware + " is eliminated!");
                    notificationDisplayLocal = "[Antivirus]" + Malware + " is eliminated!\n\n" + notificationDisplayLocal;
                }
                else
                {
                    Debug.Log("[Antivirus]" + Malware + "was not detected...\n");
                    notificationDisplayLocal = "[Antivirus]No threats were detected...\n\n" + notificationDisplayLocal;
                }

                // Notificationに保存
                Hashtable Notification = new Hashtable
                {
                    {"NotificationDisplayLocal", notificationDisplayLocal}
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(Notification);
            }

            // 削除(TerminalDisplay)
            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("AntiVirusSoftware\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            // 削除(DownloadWeb)
            PlayerPrefs.SetInt("DownloadAntiVirusSoftware", 0);
            PlayerPrefs.Save();
        }
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
