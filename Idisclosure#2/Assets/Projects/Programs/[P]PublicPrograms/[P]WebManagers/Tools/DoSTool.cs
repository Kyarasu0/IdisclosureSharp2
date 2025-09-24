using UnityEngine;
using Photon.Pun;                
using Photon.Realtime;        
using ExitGames.Client.Photon;    
using UnityEngine.SceneManagement; 
using System.Text.RegularExpressions;

public class DoSTool : MonoBehaviour
{
    private const byte DoSToolPlayer = 101;
    private const byte DoSToolServer = 102;
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int drain = 10;
        // Battery情報を取得
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);
        // Battery的に起動可能な場合 かつ IPが入力されている場合
        if (battery - drain >= 0 && Regex.IsMatch(command, @"^(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\." + @"(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\." + @"(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\." + @"(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$"))
        {
            // Battery処理
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            // 実行

            // IPが一致するエンティティを探索
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string PlayerIP = "";
                if ((string)player.CustomProperties["PlayerIP"] == command)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerIP"))
                    {
                        PlayerIP = (string)PhotonNetwork.LocalPlayer.CustomProperties["PlayerIP"];
                    }
                    string message = PlayerIP;
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent(DoSToolPlayer, message, raiseEventOptions, sendOptions);
                    SceneManager.LoadScene("Success");
                }
                else if ((string)player.CustomProperties["ServerIP"] == command)
                {
                    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerIP"))
                    {
                        PlayerIP = (string)PhotonNetwork.LocalPlayer.CustomProperties["PlayerIP"];
                    }
                    string message = PlayerIP;
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent(DoSToolServer, message, raiseEventOptions, sendOptions);
                    SceneManager.LoadScene("Success");
                }
            }

            // 使い終わったら削除

            // 削除(TerminalDisplay)
            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("DoSTool\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            // 削除(DownloadWeb)
            PlayerPrefs.SetInt("DownloadDoSTool", 0);
            PlayerPrefs.Save();
        }
    }
}
