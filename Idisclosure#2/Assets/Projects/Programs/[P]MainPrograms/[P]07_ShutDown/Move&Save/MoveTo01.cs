using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveToTitle : MonoBehaviour
{
    public Button TitleButton;
    void Start()
    {
        TitleButton.onClick.AddListener(OnClickTitle);
    }
    void OnClickTitle()
    {
        SceneManager.LoadScene("01_StartClick");
    }
}
