using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ReceiveIPs : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte SendIPs = 2;

    new void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    new void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendIPs)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            string PlayerIP = (string)receivedData[0];
            string ServerIP = (string)receivedData[1];
            Debug.Log("MyIP is " + PlayerIP);
            Debug.Log("MyServerIP is " + ServerIP);
            // ローカル保存
            PlayerPrefs.SetString("PlayerIP", PlayerIP);
            PlayerPrefs.SetString("ServerIP", ServerIP);
            PlayerPrefs.Save();
            // サーバー保存
            Hashtable props = new Hashtable
            {
                { "PlayerIP", PlayerIP },
                { "ServerIP", ServerIP }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            Debug.Log("IPs are saved! : " + PlayerIP + " : " + ServerIP);
        }
    }
}