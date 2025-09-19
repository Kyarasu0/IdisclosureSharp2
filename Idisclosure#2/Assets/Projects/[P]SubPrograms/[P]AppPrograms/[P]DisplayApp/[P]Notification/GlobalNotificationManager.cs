using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

// ================== Global Notification ==================
public class GlobalNotificationManager : MonoBehaviour
{
    string[] browserParts;
    string onePageElement = "";
    int pageSize = 4;
    int i = 0;
    string showBrowser = "";
    int currentPage = 1;
    int direction = 0;
    int pageMax = 0;
    int pageMaxDisplay = 0;
    public bool leftDirection;
    public TMP_Text Display;
    public TMP_Text PageDisplay;

    void Update()
    {
        // BrowserDisplayを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_Notification_BrowserDisplay"))
        {
            showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["Global_Notification_BrowserDisplay"];
        }

        // \n\nで分割
        browserParts = showBrowser.Split("\n\n");

        // ページ分割して保存
        Hashtable ShowBrowser = new Hashtable();
        for (i = 0; i < browserParts.Length; i++)
        {
            onePageElement += browserParts[i] + "\n\n";
            if ((i + 1) % pageSize == 0)
            {
                ShowBrowser[$"Global_DisplayPage{(i + 1) / pageSize}"] = onePageElement;
                ShowBrowser["Global_PageMax"] = (i + 1) / pageSize;
                onePageElement = "";
            }
            else if ((i + 1) == browserParts.Length)
            {
                ShowBrowser[$"Global_DisplayPage{((i + 1) / pageSize) + 1}"] = onePageElement;
                ShowBrowser["Global_PageMax"] = ((i + 1) / pageSize) + 1;
                onePageElement = "";
            }
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(ShowBrowser);

        // 現在ページを取得
        currentPage = PlayerPrefs.GetInt("Global_Notification_CurrentPage", 1);

        // 表示更新
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"Global_DisplayPage{currentPage}"))
        {
            Display.text = (string)PhotonNetwork.CurrentRoom.CustomProperties[$"Global_DisplayPage{currentPage}"];
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_PageMax"))
        {
            pageMaxDisplay = (int)PhotonNetwork.CurrentRoom.CustomProperties["Global_PageMax"];
        }
        PageDisplay.text = currentPage + "/" + pageMaxDisplay;
    }

    public void MovePage()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_PageMax"))
        {
            pageMax = (int)PhotonNetwork.CurrentRoom.CustomProperties["Global_PageMax"];
        }

        direction = leftDirection ? -1 : 1;
        currentPage = PlayerPrefs.GetInt("Global_Notification_CurrentPage", 1);
        currentPage += direction;

        if (pageMax < currentPage) currentPage = 1;
        else if (currentPage < 1) currentPage = pageMax;

        PlayerPrefs.SetInt("Global_Notification_CurrentPage", currentPage);
        PlayerPrefs.Save();
    }
}