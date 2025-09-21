using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefsResets : MonoBehaviour
{
    void Start()
    {
        // このシーンが始まった時にデータをリセット
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
        Debug.Log("ゲームシーン開始時にPlayerPrefsがリセットされました。");
    }
}