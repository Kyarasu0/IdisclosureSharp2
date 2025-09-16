using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System; // DateTime用

public class MoveTo03 : MonoBehaviour
{
    //inspectorで設定
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField birthdayInputField;

    // 名前はアルファベット1〜8文字
    private Regex nameRegex = new Regex("^[A-Za-z]{1,8}$");
    // 生年月日の形式チェック（yyyy/MM/dd）
    private Regex birthdayRegex = new Regex("^[0-9]{4}/[0-9]{2}/[0-9]{2}$");

    private string playerName;
    private string playerBirthYear;
    private string playerBirthday;  // MM/dd
    private int playerAge;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    void OnSubmit()
    {
        string nameText = nameInputField.text;
        string birthdayText = birthdayInputField.text;

        // 名前形式チェック
        if (!nameRegex.IsMatch(nameText))
        {
            Debug.Log("入力エラー: 名前はアルファベット1〜8文字で入力してください");
            return;
        }

        // 日付形式チェック
        if (!birthdayRegex.IsMatch(birthdayText))
        {
            Debug.Log("入力エラー: 生年月日は yyyy/MM/dd の形式で入力してください");
            return;
        }

        // 実際に存在する日付か確認（うるう年対応）
        DateTime BirthDay;

        if (DateTime.TryParse(birthdayText, out BirthDay))
        {
            Debug.Log(BirthDay.ToString(@"yyyy\/MM\/dd") + " を取得しました。"); //←こちらが表示されます。
        }
        else
        {
            Debug.Log("存在しない日付です。");
            return;
            }

        // 年齢計算
        DateTime today = DateTime.Today;
        int age = today.Year - BirthDay.Year;
        if (today < BirthDay.AddYears(age)) age--;

        if (age < 0 || age > 99)
        {
            Debug.Log("年齢エラー: 0〜99歳の範囲で入力してください");
            return;
        }

        // 変数に格納
        playerName = nameText;
        playerBirthYear = BirthDay.Year.ToString();
        playerBirthday = BirthDay.ToString("MM/dd");
        playerAge = age;

        // PlayerPrefs に保存
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("PlayerBirthYear", playerBirthYear);
        PlayerPrefs.SetString("PlayerBirthday", playerBirthday);
        PlayerPrefs.SetInt("PlayerAge", playerAge);
        PlayerPrefs.Save();

        Debug.Log($"保存完了! 名前: {playerName}, 生年: {playerBirthYear}, 誕生日: {playerBirthday}, 年齢: {playerAge}");

        // 次のシーンへ
        SceneManager.LoadScene("03_InputSecretID");
    }
}
