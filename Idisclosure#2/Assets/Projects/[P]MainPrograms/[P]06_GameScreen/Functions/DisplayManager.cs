using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class DisplayManager: MonoBehaviour
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
        // \n\nを4つごとに区切って各Pageに常に分ける

        // BrowserDisplayを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("BrowserDisplay"))
        {
            showBrowser = (string)PhotonNetwork.CurrentRoom.CustomProperties["BrowserDisplay"];
        }

        // \n\nを境にして配列の要素に分割
        browserParts = showBrowser.Split("\n\n");

        // 順々に要素を追加し、4つごとに新しいPageを作成する
        Hashtable ShowBrowser = new Hashtable();
        for (i = 0; i < browserParts.Length; i++)
        {
            onePageElement += browserParts[i] + "\n\n";
            // 要素数がPageSizeに達したら保存とPageの切り替え
            if ((i + 1) % pageSize == 0)
            {
                ShowBrowser[$"DisplayPage{(i + 1) / pageSize}"] = onePageElement;
                // Pageの最大値を更新
                ShowBrowser["PageMax"] = (i + 1) / pageSize;
                // 保存後にリセット
                onePageElement = "";
            }
            // すべての項目を登録しきったら4つそろってなくても保存
            else if ((i + 1) == browserParts.Length)
            {
                ShowBrowser[$"DisplayPage{((i + 1) / pageSize) + 1}"] = onePageElement;
                // Pageの最大値を更新
                ShowBrowser["PageMax"] = ((i + 1) / pageSize) + 1;
                // 保存後にリセット
                onePageElement = "";
            }
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(ShowBrowser);

        // Pageを表示

        // 現在のページ数を取得
        currentPage = PlayerPrefs.GetInt("CurrentPage", 1);

        // Pageの内容を表示
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"DisplayPage{currentPage}"))
        {
            Display.text = (string)PhotonNetwork.CurrentRoom.CustomProperties[$"DisplayPage{currentPage}"];
        }

        // Pageの最大値とCurrentPageを表示
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PageMax"))
        {
            pageMaxDisplay = (int)PhotonNetwork.CurrentRoom.CustomProperties["PageMax"];
        }
        PageDisplay.text = currentPage + "/" + pageMaxDisplay;

    }

    public void MovePage()
    {
        // Pageの最大値を取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PageMax"))
        {
            pageMax = (int)PhotonNetwork.CurrentRoom.CustomProperties["PageMax"];
        }

        // 方向判定
        if (leftDirection)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        // 現在ページの取得
        currentPage = PlayerPrefs.GetInt("CurrentPage", 1);

        // 現在ページの書き換え
        currentPage += direction;

        // 範囲内に強制
        if (pageMax < currentPage)
        {
            currentPage = 1;
        }
        else if (currentPage < 1)
        {
            currentPage = pageMax;
        }

        // Pageの保存
        PlayerPrefs.SetInt("CurrentPage", currentPage);
        PlayerPrefs.Save();
    }
}
