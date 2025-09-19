using UnityEngine;

public class Battery100 : MonoBehaviour
{
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int add = 100;
        // commandからPCのBatteryを増やすのかServerのBatteryを増やすのかを判別
        if (command == "--PC" || command == "--Server")
        {
            // Batteryの情報を取得
            int battery = PlayerPrefs.GetInt("BatteryMy" + command.Replace("-", ""), 0);

            // 実行
            battery += add;
            PlayerPrefs.SetInt("BatteryMy" + command.Replace("-", ""), battery);
            PlayerPrefs.Save();

            // 表示
            PlayerPrefs.SetString("TerminalDisplay", "Battery increased by 100%!\n\n");
            PlayerPrefs.Save();

            // 削除(TerminalDisplay)
            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("Battery100\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            // 削除(DownloadWeb)
            PlayerPrefs.SetInt("DownloadBattery100", 0);
            PlayerPrefs.Save();
        }
    }
}
