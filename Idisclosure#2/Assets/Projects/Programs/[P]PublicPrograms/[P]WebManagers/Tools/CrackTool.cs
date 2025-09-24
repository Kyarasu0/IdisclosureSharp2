using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Text.RegularExpressions;

public class CrackTool : MonoBehaviour
{
    string[] attackInformation;
    string SecretID = "";
    string cookedGuessID = "";
    string MaskSecretID = "";
    int FlagDigit = 0;
    public TMP_Text ShowIDDisplay;

    string showIDDisplay = "";
    
    // 使用するキャラクターセット
    private static readonly string digits = "0123456789";      // 数字
    private static readonly string symbols = "!#$%&()*+,-./:;<=>?@[]/^-{}|~";      // 記号
    private static readonly string lowercase = "abcdefghijklmnopqrstuvwxyz";  // 小文字
    private static readonly string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";  // 大文字
    public void Execute(string command, string WitchBattery)
    {
        // commandをIPとSecretID候補に分割
        attackInformation = command.Split(" ");

        // 正しいカウント処理
        int CountL = Regex.Matches(attackInformation[1], @"\?l").Count;
        int CountU = Regex.Matches(attackInformation[1], @"\?u").Count;
        int CountD = Regex.Matches(attackInformation[1], @"\?d").Count;
        int CountS = Regex.Matches(attackInformation[1], @"\?s").Count;

        // 消費するBatteryを設定
        int drain = CountL * 5 + CountU * 5 + CountD * 8 + CountS * 3;

        // Battery情報を取得
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);

        // Battery的に起動可能 かつ IPが存在する かつ 予測のSecretID(GuessID)が存在する場合
        if (battery - drain >= 0 && attackInformation[0] != "" && attackInformation[1] != "")
        {
            // Battery処理
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            // 実行
            showIDDisplay = "";
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                // SecretIDとMaskSecretID(SecretIDのパターン表現版)とFlagDigit(間違いポイント数)を初期化
                FlagDigit = 0;
                MaskSecretID = "";
                if (player.CustomProperties.ContainsKey("SecretID"))
                {
                    SecretID = ((string)player.CustomProperties["SecretID"]).Replace("\u200B", "");
                }

                // MaskSecretIDを作成
                foreach (char c in SecretID)
                {
                    if (digits.Contains(c))
                    {
                        MaskSecretID += "\u200B";
                    }
                    else if (symbols.Contains(c))
                    {
                        MaskSecretID += "\u200C";
                    }
                    else if (lowercase.Contains(c))
                    {
                        MaskSecretID += "\u200D";
                    }
                    else if (uppercase.Contains(c))
                    {
                        MaskSecretID += "\u2060";
                    }
                    else
                    {
                        MaskSecretID += c; // そのまま追加
                    }
                }

                // cookedGuessIDを作成
                cookedGuessID = attackInformation[1].Replace("?d", "\u200B").Replace("?s", "\u200C").Replace("?l", "\u200D").Replace("?u", "\u2060");

                // GuessIDに対してSecretIDとMaskSecretIDのどちらにも当てはまらないという間違いを見つける
                // 文字の長さが違う時点で不正解
                if (cookedGuessID.Length != SecretID.Length)
                {
                    FlagDigit++;
                }
                // 文字数が同じならGuessIDに対してSecretIDとMaskSecretIDのどちらにも当てはまらないという間違いを確認する
                else if (cookedGuessID.Length == SecretID.Length)
                {
                    for (int i = 0; i < SecretID.Length; i++)
                    {
                        if (!(cookedGuessID[i] == SecretID[i] || cookedGuessID[i] == MaskSecretID[i]))
                        {
                            FlagDigit++;
                        }
                    }
                }

                // 間違いが一つも見つかっていなければCrack成功
                if (FlagDigit == 0)
                {
                    showIDDisplay += ((string)player.CustomProperties["Name"]).Replace("\u200B", "") + "\'s ID was cracked!\n  " + ((string)player.CustomProperties["SecretID"]).Replace("\u200B", "") + "\n";
                }
                else
                {
                    showIDDisplay += ((string)player.CustomProperties["Name"]).Replace("\u200B", "") + "\'s ID was not found...\n";
                }
                Debug.Log("cookedGuessID is " + cookedGuessID + " : " + "SecretID is " + SecretID);
                Debug.Log("cookedGuessID length is " + cookedGuessID.Length + " : " + "SecretID length is " + SecretID.Length);
                Debug.Log(FlagDigit);
            }

            // TerminalDisplayに結果を表示
            PlayerPrefs.SetString("TerminalDisplay", showIDDisplay);
            PlayerPrefs.Save();
        }
    }
}
