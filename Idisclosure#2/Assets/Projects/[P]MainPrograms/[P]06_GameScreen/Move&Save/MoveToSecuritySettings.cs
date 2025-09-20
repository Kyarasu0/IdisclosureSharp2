using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class MoveToSecuritySettings : MonoBehaviour
{
    public Button SecuritySettingsButton;
    public Button SecuritySettings_RightArrowButton;
    public Button NotificationCenterButton;
    public Button NotificationCenter_RightArrowButton;
    public Button NotificationCenter_LeftArrowButton;
    public TMP_Text PageText;
    void Start()
    {
        SecuritySettingsButton.gameObject.SetActive(true);
        SecuritySettings_RightArrowButton.gameObject.SetActive(true);
        SecuritySettingsButton.onClick.AddListener(OnClickSecuritySettings);
        SecuritySettings_RightArrowButton.onClick.AddListener(OnClickSecuritySettings_RightArrow);
    }
    void OnClickSecuritySettings()
    {
        SceneManager.LoadScene("SecuritySettings");
    }
    void OnClickSecuritySettings_RightArrow()
    {
        SecuritySettingsButton.gameObject.SetActive(false);
        SecuritySettings_RightArrowButton.gameObject.SetActive(false);

        PageText.text = "2/3";
        NotificationCenterButton.gameObject.SetActive(true);
        NotificationCenter_RightArrowButton.gameObject.SetActive(true);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(true);
        
    }

}
