using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class MoveToTerminal : MonoBehaviour
{
    public Button TerminalButton;
    public Button Terminal_LeftArrowButton;
    public Button NotificationCenterButton;
    public Button NotificationCenter_RightArrowButton;
    public Button NotificationCenter_LeftArrowButton;
    public TMP_Text PageText;
    void Start()
    {
        TerminalButton.gameObject.SetActive(false);
        Terminal_LeftArrowButton.gameObject.SetActive(false);
        TerminalButton.onClick.AddListener(OnClickTerminal);
        Terminal_LeftArrowButton.onClick.AddListener(OnClickTerminal_LeftArrow);
    }
    void OnClickTerminal()
    {
        SceneManager.LoadScene("Terminal");
    }
    void OnClickTerminal_LeftArrow()
    {
        TerminalButton.gameObject.SetActive(false);
        Terminal_LeftArrowButton.gameObject.SetActive(false);

        PageText.text = "2/3";
        NotificationCenterButton.gameObject.SetActive(true);
        NotificationCenter_RightArrowButton.gameObject.SetActive(true);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(true);
        
    }

}
