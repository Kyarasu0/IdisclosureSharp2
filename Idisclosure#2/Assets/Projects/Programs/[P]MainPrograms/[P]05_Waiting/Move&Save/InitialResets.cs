using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class InitialResets : MonoBehaviour
{
    string[] Webs;
    void Start()
    {
        LoadWebs();
    }
    public void ResetButton()
    {
        // PlyerPrefs
        PlayerPrefs.SetInt("Battery", 100);
        PlayerPrefs.SetInt("BatteryMyServer", 100);
        PlayerPrefs.SetInt("PhishingNow", 0);
        foreach (string Web in Webs)
        {
            PlayerPrefs.SetInt($"Download{Web}", 0);
        }
        PlayerPrefs.Save();

        // Hashtables
        Hashtable props = new Hashtable
        {
            // Security
            { "Port", 22 },
            { "PortMyServer", 22 },
            { "PasswordMyServer", 000 },
        };
        // WebManagers
        Hashtable webs = new Hashtable();
        foreach (string Web in Webs)
        {
            webs[Web] = false;
            webs[$"Phishing{Web}"] = false;
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(webs);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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
