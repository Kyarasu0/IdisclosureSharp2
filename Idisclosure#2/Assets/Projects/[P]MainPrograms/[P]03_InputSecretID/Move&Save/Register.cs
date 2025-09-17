using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public TMP_InputField SecretIDInputField;
    public TextMeshProUGUI TokensText;
    public Button submitButton;
    private Regex secretIDRegex = new Regex(
        @"^[A-Za-z0-9!#$%&'-=^~|@`;+:*,<.>/?_\[\](){}""]+$"
        );

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);

    }

    void OnSubmit()
    {
        string SecretID = SecretIDInputField.text;
        string Tokens = TokensText.text;

        if (!secretIDRegex.IsMatch(SecretID))
        {
            Debug.Log("Invalid Secret ID format.");
            return;
        }
        Debug.Log("Secret ID and Tokens saved.");
        PlayerPrefs.SetString("SecretID", SecretID);
        PlayerPrefs.SetString("Tokens", Tokens);
        PlayerPrefs.Save();
        SceneManager.LoadScene("04_CreateJoin");
    }
}
