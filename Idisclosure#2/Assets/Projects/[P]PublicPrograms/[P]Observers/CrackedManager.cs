using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CrackReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte CrackedSecretID = 1;

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == CrackedSecretID)
        {
            string message = (string)photonEvent.CustomData;
            Debug.Log("[CrackReceiver] Message received: " + message);
        }
    }
}
