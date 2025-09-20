using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class RealTimeTimer : MonoBehaviour
{
    [Header("素材")]
    public Image blueCircle;
    public Image pinkCircle;
    public TMP_Text remainingDisplay;
    [Header("残り時間の設定")]
    public int timerMinutes;
    public bool testMode;

    private long startTime;
    private float totalSeconds;

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
        // 開始時間の取得(Long型)
        startTime = Convert.ToInt64(PlayerPrefs.GetString("StartTime", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString()));
        // テスト用
        if (testMode)
        {
            startTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            Debug.Log("StartTime: " + startTime);
        }

        // 制限時間の取得と秒数化
        int totalMinutes = PlayerPrefs.GetInt("RemainingTime", timerMinutes);
        totalSeconds = (float)totalMinutes * 60f;
        Debug.Log("RemainingTime: " + totalMinutes);
    }

    void Update()
    {
        // 経過時間
        float elapsedSeconds = (float)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - startTime);

        // 残り時間
        float remaining = (totalSeconds - elapsedSeconds);
        if (remaining < 0)
        {
            remaining = 0;
        }

        // プログレスバーに反映
        blueCircle.fillAmount = remaining / totalSeconds;
        pinkCircle.fillAmount = elapsedSeconds / totalSeconds;

        // 残り時間のフォーマットを整えて表示
        TimerDisplay(remaining);

        // タイマー終了時の処理
        if (remaining <= 0)
        {
            // 終了処理
        }
    }

    private void TimerDisplay(float timeRemaining)
    {
        // 1分以上あるときは小数点を表示しない
        int minutes = (int)timeRemaining / 60;
        float seconds = timeRemaining % 60;
        remainingDisplay.text = minutes + ":" + seconds.ToString("00");
    }
}
