using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
public class LocalNotificationManager : MonoBehaviour
{
    //aaaa
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
        // Local保存された文字列を取得
        showBrowser = PlayerPrefs.GetString("Local_Notification_BrowserDisplay", "");

        // \n\nで分割
        browserParts = showBrowser.Split("\n\n");

        // ページ分割
        pageMaxDisplay = Mathf.CeilToInt(browserParts.Length / (float)pageSize);
        currentPage = PlayerPrefs.GetInt("Local_Notification_CurrentPage", 1);

        int startIndex = (currentPage - 1) * pageSize;
        onePageElement = "";
        for (i = startIndex; i < Mathf.Min(startIndex + pageSize, browserParts.Length); i++)
        {
            onePageElement += browserParts[i] + "\n\n";
        }

        // 表示更新
        Display.text = onePageElement;
        PageDisplay.text = currentPage + "/" + pageMaxDisplay;
    }

    public void MovePage()
    {
        pageMax = Mathf.CeilToInt(browserParts.Length / (float)pageSize);

        direction = leftDirection ? -1 : 1;
        currentPage = PlayerPrefs.GetInt("Local_Notification_CurrentPage", 1);
        currentPage += direction;

        if (pageMax < currentPage) currentPage = 1;
        else if (currentPage < 1) currentPage = pageMax;

        PlayerPrefs.SetInt("Local_Notification_CurrentPage", currentPage);
        PlayerPrefs.Save();
    }
}