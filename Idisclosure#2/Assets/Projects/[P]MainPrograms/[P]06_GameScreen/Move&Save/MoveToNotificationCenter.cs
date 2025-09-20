using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class MoveToNotificationCenter : MonoBehaviour
{
    public Button SecuritySettingsButton;
    public Button NotificationCenterButton;
    public Button TerminalButton;
    public Button NotificationCenter_RightArrowButton;
    public Button NotificationCenter_LeftArrowButton;
    public Button SecuritySettings_RightArrowButton;
    public Button Terminal_LeftArrowButton;
    public TMP_Text PageText;
    void Start()
    {
        NotificationCenterButton.gameObject.SetActive(false);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(false);
        NotificationCenter_RightArrowButton.gameObject.SetActive(false);
        NotificationCenterButton.onClick.AddListener(OnClickNotificationCenter);
        NotificationCenter_RightArrowButton.onClick.AddListener(OnClickNotificationCenter_RightArrow);
        NotificationCenter_LeftArrowButton.onClick.AddListener(OnClickNotificationCenter_LeftArrow);
    }
    void OnClickNotificationCenter()
    {
        SceneManager.LoadScene("NotificationCenter");
    }

    void OnClickNotificationCenter_RightArrow()
    {
        PageText.text = "3/3";
        NotificationCenterButton.gameObject.SetActive(false);
        NotificationCenter_RightArrowButton.gameObject.SetActive(false);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(false);

        Terminal_LeftArrowButton.gameObject.SetActive(true);
        TerminalButton.gameObject.SetActive(true);
    }
    void OnClickNotificationCenter_LeftArrow()
    {
        PageText.text = "1/3";
        NotificationCenterButton.gameObject.SetActive(false);
        NotificationCenter_RightArrowButton.gameObject.SetActive(false);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(false);

        SecuritySettings_RightArrowButton.gameObject.SetActive(true);
        SecuritySettingsButton.gameObject.SetActive(true);
    }
}
