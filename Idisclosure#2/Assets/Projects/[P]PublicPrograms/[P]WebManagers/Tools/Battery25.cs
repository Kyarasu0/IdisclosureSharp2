using UnityEngine;

public class Battery25 : MonoBehaviour
{
    public void Execute(string command, string WitchBattery)
    {
        // 消費するBatteryを設定
        int add = 25;
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
            PlayerPrefs.SetString("TerminalDisplay", "Battery increased by 25%!");
            PlayerPrefs.Save();
        }
    }
}
