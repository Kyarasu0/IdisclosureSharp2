using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class MoveToFirewall : MonoBehaviour
{
    public Button FireWallButton;
    public Button FireWall_RightArrowButton;
    public Button NotificationCenterButton;
    public Button NotificationCenter_RightArrowButton;
    public Button NotificationCenter_LeftArrowButton;
    public TMP_Text PageText;
    void Start()
    {
        FireWallButton.gameObject.SetActive(true);
        FireWall_RightArrowButton.gameObject.SetActive(true);
        FireWallButton.onClick.AddListener(OnClickFireWall);
        FireWall_RightArrowButton.onClick.AddListener(OnClickFireWall_RightArrow);
    }
    void OnClickFireWall()
    {
        SceneManager.LoadScene("まだない");
    }
    void OnClickFireWall_RightArrow()
    {
        FireWallButton.gameObject.SetActive(false);
        FireWall_RightArrowButton.gameObject.SetActive(false);

        PageText.text = "2/3";
        NotificationCenterButton.gameObject.SetActive(true);
        NotificationCenter_RightArrowButton.gameObject.SetActive(true);
        NotificationCenter_LeftArrowButton.gameObject.SetActive(true);
        
    }

}
