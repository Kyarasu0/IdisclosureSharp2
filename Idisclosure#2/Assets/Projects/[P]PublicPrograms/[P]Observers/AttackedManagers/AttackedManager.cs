using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using System.Collections;


public class AttackedManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    //private const byte VirusOO = 101;
    private const byte DoSPlayer = 102;
    private const byte DoSServer = 103;
    string AttackIP = "";
    string BlockedIPMyServer = "";
    string BlockedIP = "";


    public void OnEvent(EventData photonEvent)
    {
        /*----------DoSPlayerを受信----------*/
        if (photonEvent.Code == DoSPlayer)
        {
            AttackIP = (string)photonEvent.CustomData;
            StartCoroutine(ShowAndUnloadDoSPlayerScene());
        }
        /*----------DoSServerを受信----------*/
        if (photonEvent.Code == DoSServer)
        {
            AttackIP = (string)photonEvent.CustomData;
            StartCoroutine(ShowAndUnloadDoSServerScene());
        }
    }

    private IEnumerator ShowAndUnloadCandyScene()
    {
        // "Candy" シーンを現在のシーンに重ねる（Additive モード）
        SceneManager.LoadScene("Candy", LoadSceneMode.Additive);
        
        // 10秒待つ
        yield return new WaitForSeconds(10);
        
        // "Candy" シーンを削除（アンロード）
        SceneManager.UnloadSceneAsync("Candy");
    }

    private IEnumerator ShowAndUnloadDoSServerScene()
    {
        BlockedIPMyServer = (string)PlayerPrefs.GetString("BlockedIPMyServer","");
        if (!(BlockedIPMyServer.Contains(AttackIP)))
        {
            int Battery = int.Parse(PlayerPrefs.GetString("BatteryMyServer","0"));
            Battery -= 25;
            PlayerPrefs.SetString("BatteryMyServer",Battery.ToString());
            PlayerPrefs.Save();
            // "DoS" シーンを現在のシーンに重ねる
            SceneManager.LoadScene("DoSNotification", LoadSceneMode.Additive);
            
            // 2秒待つ
            yield return new WaitForSeconds(2);
            
            // "DoS" シーンを削除
            SceneManager.UnloadSceneAsync("DoSNotification");
        }
    }
    private IEnumerator ShowAndUnloadDoSPlayerScene()
    {   
        BlockedIP = (string)PlayerPrefs.GetString("BlockedIP","");
        if (!(BlockedIP.Contains(AttackIP)))
        {
            int Battery = int.Parse(PlayerPrefs.GetString("Battery","0"));
            Battery -= 25;
            PlayerPrefs.SetString("Battery",Battery.ToString());
            PlayerPrefs.Save();
            // "DoS" シーンを現在のシーンに重ねる
            SceneManager.LoadScene("DoSNotification", LoadSceneMode.Additive);
            
            // 2秒待つ
            yield return new WaitForSeconds(2);
            
            // "DoS" シーンを削除
            SceneManager.UnloadSceneAsync("DoSNotification");
        }
        
    }
}
