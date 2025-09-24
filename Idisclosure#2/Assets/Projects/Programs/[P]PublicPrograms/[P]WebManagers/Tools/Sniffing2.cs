using UnityEngine;

public class Sniffing2 : MonoBehaviour
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
            string wifiInformation1 = PlayerPrefs.GetString("WiFi2").Replace("\u200B", "");
            PlayerPrefs.SetString("TerminalDisplay", wifiInformation1);
            PlayerPrefs.Save();
        }
    }
}
