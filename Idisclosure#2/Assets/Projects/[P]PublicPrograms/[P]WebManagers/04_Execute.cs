using UnityEngine;
using System;
using TMPro;
using System.Text.RegularExpressions;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Execute : MonoBehaviour
{
    private string[] Webs;
    public TMP_Text Command;
    public TMP_Text Terminal;
    public string PC_Or_Server = "";
    string WitchBattery = "";
    void Start()
    {
        LoadWebs();
    }

    void RunTools()
    {
        // オブジェクトを文字に変換
        // ここではCrackToolのため小文字に強制しないのでUsageのUIで指示する必要がある
        string command = Command.text;

        // PCかServerどちらのBatteryを使うのかを定義
        if (PC_Or_Server == "PC")
        {
            WitchBattery = "Battery";
        }
        else if (PC_Or_Server == "Server")
        {
            WitchBattery = "BatteryMyServer";
        }
        
        // Terminal実行(検索)を検知する
        foreach (string web in Webs)
        {
            // Patternが一致し、Downloadされていればスクリプトファイルを実行
            if (Regex.IsMatch(command, $@"{web.ToLower()}.*") && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey($"Download{web}") && (bool)PhotonNetwork.LocalPlayer.CustomProperties[$"Download{web}"])
            {
                // CommandHeader部分を削除(例:dostool <ip> → <ip>)
                command = command.Replace(web.ToLower(), "");

                // classNameを作成
                string className = web;

                // Typeを取得
                Type type = Type.GetType(className);

                // 指定したスクリプトを実行
                if (type != null)
                {
                    // インスタンスを作成
                    var instance = Activator.CreateInstance(type);

                    // Scriptを実行
                    type.GetMethod("Execute")?.Invoke(instance, new object[] { command, WitchBattery });
                }

            }
        }
    }

    public void TerminalMenu()
    {
        // TerminalDisplayにTerminalMenuを表示
        string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "");
        PlayerPrefs.SetString("TerminalDisplay", terminalMenu);
        PlayerPrefs.Save();
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
