using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RealTimeTimer : MonoBehaviour
{
    [Header("素材")]
    public Image blueCircle;
    public Image pinkCircle;
    public TMP_Text remainingDisplay;
    [Header("残り時間の設定")]
    public int timerMinutes;
    public bool testMode;

    private long startTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    private float totalSeconds = 300;

    // // 2025/09/16 12:34:56 UTC(DateTime型)
    // // ↑DateTime.UtcNowでこんな感じの結果がstartTimeに入る
    // startTime = DateTime.UtcNow;

    // // 1726412096(DateTimeOffset型)
    // // ↑型をキャスト+形式変換をする
    // long unixTime = ((DateTimeOffset)startTime).ToUnixTimeSeconds();

    // // 文字列にして保存
    // PlayerPrefs.SetString("StartTime", unixTime.ToString());
    // PlayerPrefs.Save();

    void Start()
    {
        // Debug.Log("startTime" + startTime);
        // Debug.Log("totalSeconds" + totalSeconds);
        // startTime = Convert.ToInt64(PlayerPrefs.GetString("StartTime", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString()));

        // テスト用
        if (testMode)
        {
            startTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            Debug.Log("StartTime: " + startTime);
        }
    }


    void Update()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime"))
        {
            startTime = (long)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RemainingTime"))
        {
            totalSeconds = (float)PhotonNetwork.CurrentRoom.CustomProperties["RemainingTime"];
        }
        // totalSeconds = (float)totalMinutes * 60f;

        // 経過時間
        float elapsedSeconds = (float)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - startTime);

        // 残り時間
        float remaining = (totalSeconds - elapsedSeconds);
        if (remaining < 0)
        {
            remaining = 0;
        }

        // 残り時間に応じてSNSServerを操作
        ShowProfiles(remaining);

        // プログレスバーに反映
        blueCircle.fillAmount = remaining / totalSeconds;
        pinkCircle.fillAmount = elapsedSeconds / totalSeconds;

        // 残り時間のフォーマットを整えて表示
        TimerDisplay(remaining);
        

        // タイマー終了時の処理
        if (remaining <= 0)
        {
            // 終了処理
            SceneManager.LoadScene("Ranking");
            Debug.Log("終了！");
        }
    }

    private void TimerDisplay(float timeRemaining)
    {
        // 1分以上あるときは小数点を表示しない
        int minutes = (int)timeRemaining / 60;
        float seconds = timeRemaining % 60;
        remainingDisplay.text = minutes + ":" + seconds.ToString("00");
    }

    private void ShowProfiles(float timeRemaining)
    {
        // 1: 名前
        // 2: 誕生日
        // 3: 年齢
        // 4: 生年 
        // 5: Tokens

        //イベント数を定義
        int maxEvent = 5;

        // 終了したイベントを検知
        int eventDone = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("EventDone")) {
            eventDone = (int)PhotonNetwork.CurrentRoom.CustomProperties["EventDone"];
        }

        // イベント発火時間か確認、イベント発火時間なら起動する
        string profiles = "";
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Profiles"))
        {
            profiles = (string)PhotonNetwork.CurrentRoom.CustomProperties["Profiles"];
        }
        if (eventDone < maxEvent)
        {
            if (timeRemaining < timeRemaining * (float)(maxEvent - eventDone) / maxEvent)
            {
                // eventDoneを1進めて保存
                eventDone++;
                Hashtable done = new Hashtable
                {
                    {"EventDone", eventDone},
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(done);

                // eventDoneの数字によって挙動を変える
                switch (eventDone)
                {
                    case 1:// 名前
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                string playerName = (string)player.CustomProperties["Name"];
                                profiles += "["+ playerName + "]Hi, I'm" + playerName + "!\n\n";
                            }
                        }
                        Hashtable prof = new Hashtable
                        {
                            {"Profiles", profiles},
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(prof);
                        break;

                    case 2:// 誕生日
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            string playerName = "";
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                playerName = (string)player.CustomProperties["Name"];
                            }
                            if (player.CustomProperties.ContainsKey("Birthday"))
                            {
                                string playerBirthday = (string)player.CustomProperties["Birthday"];
                                profiles += "[" + playerName + "]My birthday is " + playerBirthday + "!\n\n";
                            }
                        }
                        Hashtable prof2 = new Hashtable
                        {
                            {"Profiles", profiles},
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(prof2);
                        break;

                    case 3:// 年齢
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            string playerName = "";
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                playerName = (string)player.CustomProperties["Name"];
                            }
                            if (player.CustomProperties.ContainsKey("Age"))
                            {
                                string playerAge = (string)player.CustomProperties["Age"];
                                profiles += "[" + playerName + "]I'm " + playerAge + " year(s) old!\n\n";
                            }
                        }
                        Hashtable prof3 = new Hashtable
                        {
                            {"Profiles", profiles},
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(prof3);
                        break;

                    case 4:// 生年
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            string playerName = "";
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                playerName = (string)player.CustomProperties["Name"];
                            }
                            if (player.CustomProperties.ContainsKey("Birthyear"))
                            {
                                string playerBirthyear = (string)player.CustomProperties["Birthyear"];
                                profiles += "[" + playerName + "]My birthyear is " + playerBirthyear + "!\n\n";
                            }
                        }
                        Hashtable prof4 = new Hashtable
                        {
                            {"Profiles", profiles},
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(prof4);
                        break;
                        
                    case 5:// Tokens
                        foreach (Player player in PhotonNetwork.PlayerList)
                        {
                            string playerName = "";
                            if (player.CustomProperties.ContainsKey("Name"))
                            {
                                playerName = (string)player.CustomProperties["Name"];
                            }
                            if (player.CustomProperties.ContainsKey("Tokens"))
                            {
                                string playerTokens = (string)player.CustomProperties["Tokens"];
                                profiles += "[" + playerName + "]I have " + playerTokens + " tokens!\n\n";
                            }
                        }
                        Hashtable prof5 = new Hashtable
                        {
                            {"Profiles", profiles},
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(prof5);
                        break;
                }
            }
        }
    }
}
