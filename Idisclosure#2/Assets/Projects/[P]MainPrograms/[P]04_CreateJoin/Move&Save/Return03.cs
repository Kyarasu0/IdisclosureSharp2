using UnityEngine;
using UnityEngine.SceneManagement;

public class Return03 : MonoBehaviour
{
    // ボタンから呼び出す関数
    public void GoToStartClick()
    {
        SceneManager.LoadScene("03_InputSecretID");
    }
}
