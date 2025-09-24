using UnityEngine;

public class Battery25
{
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int add = 25;

        // commandの形式を最適化
        command = command.Replace("\u200B", "");

        // commandからPCのBatteryを増やすのかServerのBatteryを増やすのかを判別
        if (command == "--PC".ToLower() || command == "--Server".ToLower())
        {
            // Batteryの情報を取得
            int battery = PlayerPrefs.GetInt("BatteryMy" + command.Replace("-", ""), 0);

            // 実行
            battery += add;
            PlayerPrefs.SetInt("BatteryMy" + command.Replace("-", ""), battery);
            PlayerPrefs.Save();

            // 表示
            PlayerPrefs.SetString("TerminalDisplay", "Battery increased by 25%!\n\n");
            PlayerPrefs.Save();

            // 削除(TerminalDisplay)
            string terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");
            terminalMenu = terminalMenu.Replace("Battery25\n\n", "");
            PlayerPrefs.SetString("TerminalMenu", terminalMenu);
            PlayerPrefs.Save();

            // 削除(DownloadWeb)
            PlayerPrefs.SetInt("DownloadBattery25", 0);
            PlayerPrefs.Save();

            Debug.Log("Battery25の実行が完了しました！");
        }
        else
        {
            Debug.Log($"実行はできていますが何かが違います。command = {command}, length = {command.Length}");
        }
    }
}
