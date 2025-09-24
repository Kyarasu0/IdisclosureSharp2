using UnityEngine;
using TMPro;
using System;

public class CurrentTime: MonoBehaviour
{
    public TMP_Text ClockText;
    void Update()
    {
        DateTime now = DateTime.Now;

        // 日付＋時刻を表示
        ClockText.text = now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
