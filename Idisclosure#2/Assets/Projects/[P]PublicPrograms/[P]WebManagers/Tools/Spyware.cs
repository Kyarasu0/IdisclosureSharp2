using UnityEngine;

public class Spyware : MonoBehaviour
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
            PlayerPrefs.SetInt("HaveSpyware", 1);
            PlayerPrefs.Save();
            Debug.Log("Spywareをゲットした!");

            // 表示
            PlayerPrefs.SetString("TerminalDisplay", "The spyware is ready on the phishing server!\n\n");
            PlayerPrefs.Save();
        }
    }
}
