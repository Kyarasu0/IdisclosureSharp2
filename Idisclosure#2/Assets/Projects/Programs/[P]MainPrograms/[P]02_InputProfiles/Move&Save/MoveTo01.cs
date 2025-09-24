using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveTo01 : MonoBehaviour
{
    // ボタンから呼び出す関数
    public void GoToStartClick()
    {
        SceneManager.LoadScene("01_StartClick");
    }
}
