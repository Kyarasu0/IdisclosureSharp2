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

                if (guessedSecretID == playerSecretID && (bool)player.CustomProperties["Exist"])
                {
                    // Tokens加算
                    int Tokens = PlayerPrefs.GetInt("Tokens", 0);
                    Tokens += income;
                    PlayerPrefs.SetInt("Tokens", Tokens);
                    PlayerPrefs.Save();

                    // 成功シーンへ遷移
                    SceneManager.LoadScene("Success");

                    // NotificationGlobalを更新
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("NotificationDisplayGlobal"))
                    {
                        notificationDisplayGlobal = (string)PhotonNetwork.CurrentRoom.CustomProperties["NotificationDisplayGlobal"];
                    }

                    notificationDisplayGlobal = "[" + Name + "] I crack " + player.NickName + "\'s SecretID!\n\n";

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
