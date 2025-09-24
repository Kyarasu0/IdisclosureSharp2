using UnityEngine;
using TMPro;
using Photon.Pun;

public class Firewall : MonoBehaviour
{
    public TMP_InputField inputField;

    public TMP_Text ShowText;
    private string BlockedIP = "";
    private int maxBlocks = (int)PhotonNetwork.PlayerList.Length;

    void Update()
    {
        BlockedIP = PlayerPrefs.GetString("BlockedIP", "");
        ShowText.text = (string)BlockedIP;
    }

    public void AppendAndSaveText()
    {
        string currentText = inputField.text;

        while (GetBlockCount(BlockedIP) >= maxBlocks)
        {
            int firstNewlineIndex = BlockedIP.IndexOf('\n');
            if (firstNewlineIndex != -1)
            {
                BlockedIP = BlockedIP.Remove(0, firstNewlineIndex + 1);
            }
            else
            {
                BlockedIP = "";
                break;
            }
        }

        // ブロックするIPを追加
        BlockedIP += currentText + "\n";

        // IPを保存
        PlayerPrefs.SetString("BlockedIP", BlockedIP);
        PlayerPrefs.Save();
        // ボタンを押したら入力欄をクリア
        inputField.text = "";

        Debug.Log("保存されたテキスト: " + BlockedIP);
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
}