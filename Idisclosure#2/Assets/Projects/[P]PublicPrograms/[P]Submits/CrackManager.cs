using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using TMPro;

public class CrackSender : MonoBehaviourPunCallbacks
{
    public TMP_Text GuessSecretID;

    private const byte CrackedSecretID = 1;
    string notificationDisplayGlobal = "";
    string Name = "";

    void Start()
    {
        Name = PlayerPrefs.GetString("Name", "");
    }

    public void CrackFunction()
    {
        // IDが入力されていなければ中止
        if (GuessSecretID == null)
        {
            Debug.LogError("GuessSecretID is nothing!");
            return;
        }

        string guessedSecretID = GuessSecretID.text.Replace("\u200B", "");
        int income = 500;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("SecretID"))
            {
                string playerSecretID = ((string)player.CustomProperties["SecretID"]).Replace("\u200B", "");

                if (guessedSecretID == playerSecretID)
                {
                    // BuhiCoin加算
                    int BuhiCoin = int.Parse(PlayerPrefs.GetString("BuhiCoin", "0").Replace("\u200B", ""));
                    BuhiCoin += income;
                    PlayerPrefs.SetString("BuhiCoin", BuhiCoin.ToString());
                    PlayerPrefs.Save();

                    // 成功シーンへ遷移
                    SceneManager.LoadScene("Success");

                    // NotificationGlobalを更新
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NotificationDisplayGlobal"))
                    {
                        notificationDisplayGlobal = (string)PhotonNetwork.CurrentRoom.CustomProperties["NotificationDisplayGlobal"];
                    }
                    notificationDisplayGlobal = Name + " crack " + player.NickName + "\'s SecretID!";

                    Hashtable notification = new Hashtable
                    {
                        { "NotificationDisplayGlobal", notificationDisplayGlobal }
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(notification);

                    // 相手に通知を送信
                    // messageの定義
                    string message = "Your SecretID is cracked!";
                    // 送信先の定義
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { player.ActorNumber } };
                    // オプションの定義
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    // 送信する
                    PhotonNetwork.RaiseEvent(CrackedSecretID, message, raiseEventOptions, sendOptions);
                    return;
                }
            }
        }
    }
}
