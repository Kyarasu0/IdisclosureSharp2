using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using System.Linq;

public class DisplayManager: MonoBehaviour
{
    string[] showParts;
    string onePageElement = "";
    public int pageSize;
    int i = 0;
    string show = "";
    int currentPage = 1;
    int direction = 0;
    int pageMax = 0;
    int pageMaxDisplay = 0;
    public bool leftDirection;
    public TMP_Text Display;
    public TMP_Text PageDisplay;
    [Header("参照元ファイル")]
    public string Source;
    [Header("識別子")]
    public string Identifier;

    void Start()
    {
        InvokeRepeating(nameof(DisplayPerSecond), 0f, 0.3f);
    }

    void DisplayPerSecond()
    {
        // \n\nを4つごとに区切って各Pageに常に分ける

        // sourceを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Source))
        {
            show = (string)PhotonNetwork.CurrentRoom.CustomProperties[Source];
        }
        else
        {
            show = PlayerPrefs.GetString(Source, "").Replace("\u200B", "");
        }

        // \n\nを境にして配列の要素に分割
        showParts = show.Split("\n\n");

        // 空文字を取り除く
        showParts = showParts.Where(s => !string.IsNullOrEmpty(s)).ToArray();

        // 順々に要素を追加し、4つごとに新しいPageを作成する
        Hashtable Show = new Hashtable();
        for (i = 0; i < showParts.Length; i++)
        {
            onePageElement += showParts[i] + "\n\n";
            // 要素数がPageSizeに達したら保存とPageの切り替え
            if ((i + 1) % pageSize == 0)
            {
                Show[$"DisplayPage{Identifier}{(i + 1) / pageSize}"] = onePageElement;
                // Pageの最大値を更新
                Show[$"PageMax{Identifier}"] = (i + 1) / pageSize;
                // 保存後にリセット
                onePageElement = "";
            }
            // すべての項目を登録しきったら4つそろってなくても保存
            else if ((i + 1) == showParts.Length && (i + 1) % pageSize != 0)
            {
                Show[$"DisplayPage{Identifier}{((i + 1) / pageSize) + 1}"] = onePageElement;
                // Pageの最大値を更新
                Show[$"PageMax{Identifier}"] = ((i + 1) / pageSize) + 1;
                // 保存後にリセット
                onePageElement = "";
            }
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(Show);

        // Pageを表示

        // 現在のページ数を取得
        currentPage = PlayerPrefs.GetInt($"CurrentPage{Identifier}", 1);

        // Pageの内容を表示
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"DisplayPage{Identifier}{currentPage}"))
        {
            Display.text = (string)PhotonNetwork.CurrentRoom.CustomProperties[$"DisplayPage{Identifier}{currentPage}"];
        }

        // Pageの最大値とCurrentPageを表示
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"PageMax{Identifier}"))
        {
            pageMaxDisplay = (int)PhotonNetwork.CurrentRoom.CustomProperties[$"PageMax{Identifier}"];
        }
        PageDisplay.text = currentPage + "/" + pageMaxDisplay;

    }

    public void MovePage()
    {
        // Pageの最大値を取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"PageMax{Identifier}"))
        {
            pageMax = (int)PhotonNetwork.CurrentRoom.CustomProperties[$"PageMax{Identifier}"];
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
        currentPage = PlayerPrefs.GetInt($"CurrentPage{Identifier}", 1);

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
        PlayerPrefs.SetInt($"CurrentPage{Identifier}", currentPage);
        PlayerPrefs.Save();
    }
}
