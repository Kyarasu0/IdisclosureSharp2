using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using System;

public class ShowRoomMembers : MonoBehaviourPunCallbacks
{
    public TMP_Text ShowText;
    public TMP_Text PlayerCountText;
    public TMP_Text ClockText;

    int MaxPlayers = 6;


    void Start()
    {
        Debug.Log("WaitingRoom スクリプトが開始されました。");

        // ルームに入ったら1秒後にリスト更新を試みる
        StartCoroutine(WaitAndUpdateList());
    }

    IEnumerator WaitAndUpdateList()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("1秒後にプレイヤーリストを更新。");
        UpdatePlayerList();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"ルーム '{PhotonNetwork.CurrentRoom.Name}' に参加成功！現在のプレイヤー数: {PhotonNetwork.PlayerList.Length}");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"{player.NickName}");
        }

        // プレイヤーリストを遅延更新
        Invoke(nameof(UpdatePlayerList), 1f);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"新しいプレイヤーが参加: {newPlayer.NickName}, ID: {newPlayer.ActorNumber}");
        Debug.Log($"現在のプレイヤー数: {PhotonNetwork.PlayerList.Length}");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"[OnPlayerEnteredRoom] プレイヤー: {player.NickName}, ID: {player.ActorNumber}");
        }

        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"プレイヤーが退出: {otherPlayer.NickName}");
        Debug.Log($"現在のプレイヤー数: {PhotonNetwork.PlayerList.Length}");

        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        if (ShowText == null)
        {
            Debug.LogError("playerListText が NULL です！");
            return;
        }

        string playerList = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList += $"{player.NickName}\n";
        }
        int current = PhotonNetwork.PlayerList.Length;

        PlayerCountText.text = current + "/" + MaxPlayers;

        ShowText.text = playerList;
        Debug.Log("プレイヤーリスト更新:\n" + playerList);
    }
    void Update()
    {
        DateTime now = DateTime.Now;

        // 日付＋時刻を表示
        ClockText.text = now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}