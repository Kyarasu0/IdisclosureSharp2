using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AISecuritySettings : MonoBehaviour
{
    [Header("入力フィールド")]
    public TMP_InputField inputBlockedIPField;
    public TMP_InputField inputPasswordField;
    public TMP_InputField inputPortField;

    [Header("表示用テキスト")]
    public TMP_Text blockedIPText;
    public TMP_Text passwordText;
    public TMP_Text portText;

    public bool PC;

    // キューで管理
    private Queue<string> blockedIPs = new Queue<string>();
    private string currentPassword = "";
    private string currentPort = "";

    private const int MaxBlockedIP = 3;

    void Start()
    {
        // PlayerPrefs から復元
        LoadPrefs();
        UpdateUI();
    }

    private void LoadPrefs()
    {
        // BlockedIP
        string savedIPs = PlayerPrefs.GetString(PC ? "BlockedIP" : "BlockedIPMyServer", "");
        string[] ipArray = savedIPs.Split('\n');
        foreach (var ip in ipArray)
        {
            if (!string.IsNullOrWhiteSpace(ip))
            {
                blockedIPs.Enqueue(ip);
            }
        }

        // Password
        currentPassword = PlayerPrefs.GetString("Password", "");
        // Port
        currentPort = PlayerPrefs.GetString("Port", "");
    }

    private void SavePrefs()
    {
        // BlockedIP
        PlayerPrefs.SetString(PC ? "BlockedIP" : "BlockedIPMyServer", string.Join("\n", blockedIPs));
        // Password
        PlayerPrefs.SetString("Password", currentPassword);
        // Port
        PlayerPrefs.SetString("Port", currentPort);
        PlayerPrefs.Save();
    }

    private void UpdateUI()
    {
        if (blockedIPText != null)
            blockedIPText.text = string.Join("\n", blockedIPs);

        if (passwordText != null)
            passwordText.text = string.IsNullOrEmpty(currentPassword) ? "None" : currentPassword;

        if (portText != null)
            portText.text = string.IsNullOrEmpty(currentPort) ? "None" : currentPort;
    }

    public void AddBlockedIP()
    {
        string ip = inputBlockedIPField.text.Trim();
        if (string.IsNullOrEmpty(ip)) return;

        // 最大数を超えたら古いものを削除
        if (blockedIPs.Count >= MaxBlockedIP)
        {
            blockedIPs.Dequeue();
        }
        blockedIPs.Enqueue(ip);

        SavePrefs();
        UpdateUI();

        // 入力欄クリア
        inputBlockedIPField.text = "";
    }

    public void SetPassword()
    {
        string pw = inputPasswordField.text.Trim();
        if (Regex.IsMatch(pw, @"^\d{3}$"))
        {
            currentPassword = pw;

            Hashtable passwordChange = new Hashtable { { "Password", pw } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(passwordChange);

            SavePrefs();
            UpdateUI();

            inputPasswordField.text = "";
        }
        else
        {
            Debug.Log("Password format invalid (must be 3 digits)");
        }
    }

    public void SetPort()
    {
        string portStr = inputPortField.text.Trim();
        int port;
        if (int.TryParse(portStr, out port) && ((port >= 1024 && port <= 65535) || port == 22))
        {
            currentPort = portStr;

            Hashtable portChange = new Hashtable { { "Port", portStr } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(portChange);

            SavePrefs();
            UpdateUI();

            inputPortField.text = "";
        }
        else
        {
            Debug.Log("Port format invalid (1024–65535 or 22 allowed)");
        }
    }
}
