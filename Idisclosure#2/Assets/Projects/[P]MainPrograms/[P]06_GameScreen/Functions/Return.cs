using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Return : MonoBehaviour
{
    public Button TitleButton;

    public void ToGameScreen(){
        SceneManager.LoadScene("06_GameScreen");
    }
}