using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveTo02 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("02_InputProfiles");
        }
    }
}
