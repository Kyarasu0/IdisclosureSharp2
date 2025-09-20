using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class ShowProfiles : MonoBehaviour
{
    public TMP_Text Profiles;
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Profiles"))
        {
            Profiles.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["Profiles"];
        }

    }
}
