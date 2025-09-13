using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private Slider TimeSlider;          // スライダー
    [SerializeField] private TextMeshProUGUI TimeText;   // 表示用テキスト

    void Start()
    {
        // 5〜15分、1分刻み
        timeSlider.minValue = 5;
        timeSlider.maxValue = 15;
        timeSlider.wholeNumbers = true;
        timeSlider.value = 5;

        UpdateTimeText(timeSlider.value);
        timeSlider.onValueChanged.AddListener(UpdateTimeText);
    }

    private void UpdateTimeText(float value)
    {
        int minutes = Mathf.RoundToInt(value);
        timeText.text = $"{minutes}m";  // ← 「Xm」形式で表示
    }
}
