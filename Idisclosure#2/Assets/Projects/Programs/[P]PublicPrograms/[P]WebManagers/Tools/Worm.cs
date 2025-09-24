using UnityEngine;

public class Worm : MonoBehaviour
{
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int drain = 10;
        // Battery情報を取得
        int battery = PlayerPrefs.GetInt(WitchBattery, 0);
        // Battery的に起動可能な場合
        if (battery - drain >= 0)
        {
            // Battery処理
            battery -= drain;
            PlayerPrefs.SetInt(WitchBattery, battery);
            PlayerPrefs.Save();

            // 実行
            PlayerPrefs.SetInt("HaveWorm", 1);
            PlayerPrefs.Save();
            Debug.Log("WormをPhishingServerに仕掛けた!");

            // 表示
            PlayerPrefs.SetString("TerminalDisplay", "The worm is ready on the phishing server!\n\n");
            PlayerPrefs.Save();

            // 削除(TerminalDisplay)
            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("Worm\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            // 削除(DownloadWeb)
            PlayerPrefs.SetInt("DownloadWorm", 0);
            PlayerPrefs.Save();
        }
    }
}
