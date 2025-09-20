using UnityEngine;
using UnityEngine.UI;

public class WiFiActive: MonoBehaviour
{
    public GameObject WiFi_1_Shadow;
    public GameObject WiFi_2_Shadow;
    public GameObject WiFi_3_Shadow;
    public Button WiFi_1_Button;
    public Button WiFi_2_Button;
    public Button WiFi_3_Button;
    void Start()
    {
        WiFi_1_Shadow.SetActive(true);
        WiFi_2_Shadow.SetActive(false);
        WiFi_3_Shadow.SetActive(false);
        PlayerPrefs.SetInt("WifiNumber", 1);
        PlayerPrefs.Save();

        WiFi_1_Button.onClick.AddListener(OnWiFi_1_Click);
        WiFi_2_Button.onClick.AddListener(OnWiFi_2_Click);
        WiFi_3_Button.onClick.AddListener(OnWiFi_3_Click);
    }

    void OnWiFi_1_Click()
    {
        WiFi_1_Shadow.SetActive(true);
        WiFi_2_Shadow.SetActive(false);
        WiFi_3_Shadow.SetActive(false);
        PlayerPrefs.SetInt("WifiNumber", 1);
        PlayerPrefs.Save();
    }
    void OnWiFi_2_Click()
    {
        WiFi_1_Shadow.SetActive(false);
        WiFi_2_Shadow.SetActive(true);
        WiFi_3_Shadow.SetActive(false);
        PlayerPrefs.SetInt("WifiNumber", 2);
        PlayerPrefs.Save();
    }
    void OnWiFi_3_Click()
    {
        WiFi_1_Shadow.SetActive(false);
        WiFi_2_Shadow.SetActive(false);
        WiFi_3_Shadow.SetActive(true);
        PlayerPrefs.SetInt("WifiNumber", 3);
        PlayerPrefs.Save();
    }
}
