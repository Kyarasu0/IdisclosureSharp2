using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NewTerminal : MonoBehaviour
{
    public TMP_InputField Command;
    public TMP_Text ShowIDDisplay;

    private string[] attackInformation;
    private string SecretID = "";
    private string cookedGuessID = "";
    private string MaskSecretID = "";
    private int FlagDigit = 0;
    private string showIDDisplay = "";

    // 使用するキャラクターセット
    private static readonly string digits = "0123456789";
    private static readonly string symbols = "!#$%&()*+,-./:;<=>?@[]/^-{}|~";
    private static readonly string lowercase = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // 利用可能なツール一覧
    private string[] Webs = { "CrackTool", "Sniffing" };

    // === 実行エントリーポイント ===
    public void RunCommand()
    {
        string cmd = Command.text.Trim();
        if (string.IsNullOrEmpty(cmd)) return;

        // コマンドをスペースで分解
        string[] parts = cmd.Split(' ');
        string tool = parts[0]; // 先頭がコマンド名
        string arg1 = parts.Length > 1 ? parts[1] : "";
        string arg2 = parts.Length > 2 ? parts[2] : "";

        switch (tool.ToLower())
        {
            case "antivirussoftware":
                antivirussoftware(arg1, "BatteryMyPC"); 
                break;

            case "cracktool":
                CrackTool(arg1 + " " + arg2, "BatteryMyPC");
                break;

            case "sniffing":
                Sniffing(arg1, "BatteryMyPC");
                break;

            default:
                PlayerPrefs.SetString("TerminalDisplay", "Unknown command: " + tool);
                PlayerPrefs.Save();
                break;
        }

        // 実行後に入力欄をクリア
        Command.text = "";
    }

    // === AntivirusSoftware ===
    public void antivirussoftware(string command, string WitchBattery)
    {
        int add = 25;

        if (command == "--PC" || command == "--Server")
        {
            int battery = PlayerPrefs.GetInt("BatteryMy" + command.Replace("-", ""), 0);

            battery += add;
            PlayerPrefs.SetInt("BatteryMy" + command.Replace("-", ""), battery);
            PlayerPrefs.Save();

            PlayerPrefs.SetString("TerminalDisplay", "Battery increased by 25%!\n\n");
            PlayerPrefs.Save();

            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("Battery25\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("DownloadBattery25", 0);
            PlayerPrefs.Save();
        }
    }

    // === CrackTool ===
    public void CrackTool(string command, string WitchBattery)
    {
        attackInformation = command.Split(" ");
        if (attackInformation.Length < 2) return;

        int CountL = Regex.Matches(attackInformation[1], @"\?l").Count;
        int CountU = Regex.Matches(attackInformation[1], @"\?u").Count;
        int CountD = Regex.Matches(attackInformation[1], @"\?d").Count;
        int CountS = Regex.Matches(attackInformation[1], @"\?s").Count;

        int drain = CountL * 5 + CountU * 5 + CountD * 8 + CountS * 3;
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);

        if (battery - drain >= 0 && !string.IsNullOrEmpty(attackInformation[0]) && !string.IsNullOrEmpty(attackInformation[1]))
        {
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            showIDDisplay = "";
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                FlagDigit = 0;
                MaskSecretID = "";

                if (player.CustomProperties.ContainsKey("SecretID"))
                {
                    SecretID = ((string)player.CustomProperties["SecretID"]).Replace("\u200B", "");
                }

                foreach (char c in SecretID)
                {
                    if (digits.Contains(c)) MaskSecretID += "\u200B";
                    else if (symbols.Contains(c)) MaskSecretID += "\u200C";
                    else if (lowercase.Contains(c)) MaskSecretID += "\u200D";
                    else if (uppercase.Contains(c)) MaskSecretID += "\u2060";
                    else MaskSecretID += c;
                }

                cookedGuessID = attackInformation[1]
                    .Replace("?d", "\u200B")
                    .Replace("?s", "\u200C")
                    .Replace("?l", "\u200D")
                    .Replace("?u", "\u2060");

                if (cookedGuessID.Length != SecretID.Length)
                {
                    FlagDigit++;
                }
                else
                {
                    for (int i = 0; i < SecretID.Length; i++)
                    {
                        if (!(cookedGuessID[i] == SecretID[i] || cookedGuessID[i] == MaskSecretID[i]))
                        {
                            FlagDigit++;
                        }
                    }
                }

                if (FlagDigit == 0)
                {
                    showIDDisplay += ((string)player.CustomProperties["Name"]).Replace("\u200B", "") +
                                     "'s ID was cracked!\n  " +
                                     ((string)player.CustomProperties["SecretID"]).Replace("\u200B", "") + "\n";
                }
                else
                {
                    showIDDisplay += ((string)player.CustomProperties["Name"]).Replace("\u200B", "") +
                                     "'s ID was not found...\n";
                }

                Debug.Log($"cookedGuessID: {cookedGuessID} | SecretID: {SecretID}");
                Debug.Log($"cookedGuessID length: {cookedGuessID.Length} | SecretID length: {SecretID.Length}");
                Debug.Log($"FlagDigit: {FlagDigit}");
            }

            PlayerPrefs.SetString("TerminalDisplay", showIDDisplay);
            PlayerPrefs.Save();
        }
    }

    // === Sniffing ===
    public void Sniffing(string command, string WitchBattery)
    {
        int drain = 10;
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);

        if (battery - drain >= 0)
        {
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            string wifiInformation1 = PlayerPrefs.GetString("WiFi1").Replace("\u200B", "");
            PlayerPrefs.SetString("TerminalDisplay", wifiInformation1);
            PlayerPrefs.Save();
        }
    }
}
