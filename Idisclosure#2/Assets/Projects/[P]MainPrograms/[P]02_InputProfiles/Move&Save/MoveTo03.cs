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
    [SerializeField] private TextMeshProUGUI ErrorNameText;
    [SerializeField] private TextMeshProUGUI ErrorBirthText;


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
        ErrorNameText.gameObject.SetActive(false);
        ErrorBirthText.gameObject.SetActive(false);
        submitButton.onClick.AddListener(OnSubmit);


    }

    void OnSubmit()
    {
        string nameText = nameInputField.text;
        string birthdayText = birthdayInputField.text;

        ErrorNameText.gameObject.SetActive(false);
        ErrorBirthText.gameObject.SetActive(false);
        // 名前形式チェック
        if (!nameRegex.IsMatch(nameText))
        {
            ErrorNameText.text = "Please use 1–8 alphabet letters.";
            ErrorNameText.gameObject.SetActive(true);
            return;
        }

        // 日付形式チェック
        if (!birthdayRegex.IsMatch(birthdayText))
        {
            ErrorBirthText.text = "Please enter birthday in yyyy/MM/dd format.";
            ErrorBirthText.gameObject.SetActive(true);
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
            ErrorBirthText.text = "Please enter a valid date.";
            ErrorBirthText.gameObject.SetActive(true);
            return;
        }

        // 年齢計算
        DateTime today = DateTime.Today;
        int age = today.Year - BirthDay.Year;
        if (today < BirthDay.AddYears(age)) age--;

        if (age < 0 || age > 99)
        {
            ErrorBirthText.text = "Please enter an age between 0 and 99.";
            ErrorBirthText.gameObject.SetActive(true);
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

