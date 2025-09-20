using UnityEngine;
using TMPro;
using Photon.Pun;

public class Firewall : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text ShowText;
    
    private string BlockedIP = "";

    //スクリプト初期化時にプレイヤー数を取得し、自分を人数から抜く
    private int maxBlocks = PhotonNetwork.PlayerList.Length - 1;

    void Start()
    {
        BlockedIP = PlayerPrefs.GetString("BlockedIP", "");
        UpdateShowText();
    }

    //UIのボタンから呼び出す
    public void AppendAndSaveText()
    {
        string newEntry = inputField.text;

        //入力が空の場合は何もしない
        if (string.IsNullOrEmpty(newEntry))
        {
            return;
        }

        // 上限を超える場合は古いものから削除
        // maxBlocks > 0 のチェックを追加して、0人のルームで無限ループになるのを防ぐ
        while (GetBlockCount(BlockedIP) >= maxBlocks && maxBlocks > 0)
        {
            int firstNewlineIndex = BlockedIP.IndexOf('\n');
            if (firstNewlineIndex != -1)
            {
                // 先頭のIPを削除
                BlockedIP = BlockedIP.Remove(0, firstNewlineIndex + 1);
            }
            else
            {
                //改行がなく、IPが1つだけの場合は全体をクリア
                //通常はあり得ないが無限ループの可能性を無くす
                BlockedIP = "";
                break;
            }
        }

        // ブロックするIPをリストに追加
        BlockedIP += newEntry + "\n";
        PlayerPrefs.SetString("BlockedIP", BlockedIP);
        PlayerPrefs.Save(); 

        // 入力欄をクリア
        inputField.text = "";

        //UIはデータが変更されたこのタイミングでのみ更新する
        UpdateShowText();

        Debug.Log("保存されたテキスト: " + BlockedIP);
    }

    //UIテキストを更新する
    private void UpdateShowText()
    {
        if (ShowText != null)
        {
            ShowText.text = BlockedIP;
        }
    }

    // 文字列内の改行文字の数を数える
    private int GetBlockCount(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        int count = 0;
        // 文字列をループして改行を数える
        foreach (char c in text)
        {
            if (c == '\n')
            {
                count++;
            }
        }
        return count;
    }
}