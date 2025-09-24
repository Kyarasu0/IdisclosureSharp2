using UnityEngine;
using UnityEngine.SceneManagement;

public class Return02 : MonoBehaviour
{
    // ボタンから呼び出す関数
    public void GoToStartClick()
    {
        SceneManager.LoadScene("02_InputProfiles");
    }
}
