using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Text.RegularExpressions;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SecuritySettings : MonoBehaviour
{
    public TMP_InputField inputField;
    public bool PC;

    [Header("表示するtextを設定")]
    public TMP_Text ShowText;
    public TMP_Text Password;
    public TMP_Text Port;
    public TMP_Text InputPasswordText;
    public TMP_Text InputPortText;
    private string whichBlockedIP = "";
    private int maxBlocks = 0;

    void Start()
    {
        maxBlocks = (int)PhotonNetwork.PlayerList.Length;
    }

    void Update()
    {
        whichBlockedIP = PC ? PlayerPrefs.GetString("BlockedIP", "").Replace("\u200B", "") : PlayerPrefs.GetString("BlockedIPMyServer", "").Replace("\u200B", "");
        if (ShowText != null)
        {
            ShowText.text = (string)whichBlockedIP;
        }

        // Serverの場合の特別処理
        if (!PC)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PasswordMyServer"))
            {
                if (Password != null)
                {
                    Password.text = PhotonNetwork.LocalPlayer.CustomProperties["PasswordMyServer"].ToString();   
                }
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PortMyServer"))
            {
                if (Port != null)
                {
                    Port.text = PhotonNetwork.LocalPlayer.CustomProperties["PortMyServer"].ToString();   
                }
            }
        }
        else
        {
            if (Password != null)
            {
                Password.text = "Nothing";   
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Port"))
            {
                if (Port != null)
                {
                    Port.text = PhotonNetwork.LocalPlayer.CustomProperties["Port"].ToString();   
                }
            }
        }
    }

    public void AppendAndSaveText()
    {
        // 現在の入力値を取得
        string currentText = inputField.text;

        // 最大個数を超えたら
        while (GetBlockCount(whichBlockedIP) >= maxBlocks)
        {
            // 最初の\nの位置を取得(例: aaaaa\nbbbbb\nccccc → 5)
            int firstNewlineIndex = whichBlockedIP.IndexOf('\n');

            // 見つかれば(-1じゃなければ)(0 <= x < 6)の範囲を消す
            if (firstNewlineIndex != -1)
            {
                whichBlockedIP = whichBlockedIP.Remove(0, firstNewlineIndex + 1);
            }
            // 見つからなければ(-1ならば)何もないので念のためwhichBlockedIPを初期化
            else
            {
                whichBlockedIP = "";
                break;
            }
        }

        // ブロックするIPを追加
        whichBlockedIP += currentText + "\n";

        // IPを保存
        if (PC)
        {
            PlayerPrefs.SetString("BlockedIP", whichBlockedIP);
        }
        else
        {
            PlayerPrefs.SetString("BlockedIPMyServer", whichBlockedIP);
        }
        PlayerPrefs.Save();

        // ボタンを押したら入力欄をクリア
        if (inputField != null)
        {
            inputField.text = "";   
        }

        Debug.Log("保存されたテキスト: " + whichBlockedIP);
    }

    // 改行文字の数を検知
    private int GetBlockCount(string text)
    {
        int count = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                count++;
            }
        }
        return count;
    }

    public void InputPassword()
    {
        if (Regex.IsMatch(InputPasswordText.text, @"^\d{3}$"))
        {
            // 形式があっているので登録
            Hashtable PasswordChanges = new Hashtable
            {
                {"Password", InputPasswordText.text},
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(PasswordChanges);
        }
        else
        {
            Debug.Log("形式に合っていません");
        }
    }
    
    public void InputPort()
    {
        int port = 0;
        if (int.TryParse(InputPortText.text, out port) && ((port >= 1024 && port <= 65537) || port == 22))
        {
            // 形式があっているので登録
            Hashtable PortChanges = new Hashtable
            {
                {"Port", InputPortText.text},
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(PortChanges);
        }
        else
        {
            Debug.Log("形式に合っていません");
        }
    }
}