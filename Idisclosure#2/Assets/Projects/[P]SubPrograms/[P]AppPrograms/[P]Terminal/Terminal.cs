using UnityEngine;

public class Terminal : MonoBehaviour
{
    string terminalMenu = "";

    void Start()
    {
        ReturnToTerminalMenu();
    }
    public void ReturnToTerminalMenu()
    {
        // TerminalMenuを取得
        terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");

        // TerminalDisplayに保存
        PlayerPrefs.SetString("TerminalDisplay", terminalMenu);
        PlayerPrefs.Save();

    }
}
