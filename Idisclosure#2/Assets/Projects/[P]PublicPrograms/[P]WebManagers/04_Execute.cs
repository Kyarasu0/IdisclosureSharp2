using UnityEngine;
using System;
using TMPro;
using System.Text.RegularExpressions;

public class Execute : MonoBehaviour
{
    private string[] Webs;
    public TMP_Text Command;
    public TMP_Text Terminal;
    void Start()
    {
        LoadWebs();
    }

    void Update()
    {
        // オブジェクトを文字に変換
        string command = Command.text;
        
        // Terminal実行(検索)を検知する
        foreach (string web in Webs)
        {
            // Patternが一致したらスクリプトファイルを実行
            if (Regex.IsMatch(command, $@"{web.ToLower()}.*"))
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
                    type.GetMethod("Execute")?.Invoke(instance, new object[] { command });
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
