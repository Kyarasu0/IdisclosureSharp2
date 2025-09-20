using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;


public class TokensCalc : MonoBehaviour
{
    public TMP_InputField SecretIDInputField;
    public TextMeshProUGUI InputText;
    public TextMeshProUGUI TokensText;
    public TextMeshProUGUI ErrorText;
    string playerName, playerBirthYear, playerBirthday;
    float Costs, RawTokens;
    int Tokens;
    // 英数字と一部記号のみ許可
    private Regex secretIDRegex = new Regex(
    @"^[A-Za-z0-9!#$%&'-=^~|@`;+:*,<.>/?_\[\](){}""]+$"
);
    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Unknown").Replace("\u200B", "");
        playerBirthYear = PlayerPrefs.GetString("PlayerBirthyear", "0000").Replace("\u200B", "");
        playerBirthday = PlayerPrefs.GetString("PlayerBirthday", "0000").Replace("\u200B", "");
        int playerAge = PlayerPrefs.GetInt("PlayerAge", -1);
        // 確認用にログ出力
        Debug.Log("Name: " + playerName);
        Debug.Log("BirthYear: " + playerBirthYear);
        Debug.Log("Birthday: " + playerBirthday);
        Debug.Log("Age: " + playerAge);
        ErrorText.gameObject.SetActive(false);
        SecretIDInputField.onValueChanged.AddListener(CheckInput);
    }

    void CheckInput(string SecretID)
    {
        Costs = 0;
        // 正規表現チェック
        if (SecretID.Length != 0)
        {
            if (!secretIDRegex.IsMatch(SecretID))
            {
                ErrorText.text = "Alphabet, numbers, and symbols only.";
                ErrorText.gameObject.SetActive(true);
                return;
            }
        }

        ErrorText.gameObject.SetActive(false);
        string CheckID = SecretID; //チェック用の変数
        // トークン計算
        //名前判定
        if(CheckID.IndexOf(playerName) >= 0)
        {
            int nameCount = Regex.Matches(CheckID, playerName).Count;
            Costs += 10*nameCount;
            CheckID = CheckID.Replace(playerName, "");
            Debug.Log(CheckID);
        }
        //誕生年判定
        if(CheckID.IndexOf(playerBirthYear) >= 0)
        {
            int yearCount = Regex.Matches(CheckID, playerBirthYear).Count;
            Costs += 15*yearCount;
            CheckID = CheckID.Replace(playerBirthYear, "");
            Debug.Log(CheckID);
        }
        //誕生日判定
        if(CheckID.IndexOf(playerBirthday) >= 0)
        {
            int birthdayCount = Regex.Matches(CheckID, playerBirthday).Count;
            Costs += 15*birthdayCount;
            CheckID = CheckID.Replace(playerBirthday, "");
            Debug.Log(CheckID);
        }
        // 残りの文字数に応じたポイント加算
        if (CheckID.Length > 0)
        {
            Costs += Mathf.Pow(5, CheckID.Length);
            Debug.Log(CheckID + "の部分で" + Costs + "ポイント加算");
        }
        Debug.Log("現在のトークン数は" + Costs + "です");
        RawTokens = 100000 / Costs;
        //10の位以下を切り捨て
        Tokens = Mathf.FloorToInt(RawTokens / 100) * 100; 
        TokensText.text = Tokens.ToString()+" pt";
    }
}
