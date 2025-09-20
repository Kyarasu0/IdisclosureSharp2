using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class CrackedManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte CrackedSecretID = 1;

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == CrackedSecretID)
        {
            // DebugLogを表示
            string message = (string)photonEvent.CustomData;
            Debug.Log("[CrackedManager] Message received: " + message);

            // Tekensをリセット
            PlayerPrefs.SetInt("Tokens",0);
            Hashtable Reset  = new Hashtable
            {
                {"Tokens", 0},
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(Reset);

            // Scene遷移
            SceneManager.LoadScene("Cracked");
        }
    }
}
