using UnityEngine;

public class Terminal
{
    string terminalMenu = "";
    public void ReturnToTerminalMenu()
    {
        // TerminalMenuを取得
        terminalMenu = PlayerPrefs.GetString("TerminalMenu", "").Replace("\u200B", "");

        // TerminalDisplayに保存
        PlayerPrefs.SetString("TerminalDisplay", terminalMenu);
        PlayerPrefs.Save();

    }
}
