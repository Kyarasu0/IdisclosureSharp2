using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class StartXm : MonoBehaviourPunCallbacks
{
    private int totalPlayers;
    public TMP_Text TimeLimitText; // 制限時間表示用テキスト
    public Slider TimerSlider; // スライダー
    public Button StartButton; // スタートボタン
    private float seconds; // 制限時間（秒）
    private double startTime; // ゲーム開始時間（秒）

    void Start()
    {
        UpdateValueText(TimerSlider.value); // 初期値を表示
        TimerSlider.onValueChanged.AddListener(UpdateValueText);   // スライダーの値が変わったときに呼ばれる
        StartButton.onClick.AddListener(StartByMaster); // ボタンにリスナーを追加
    }
    private void UpdateValueText(float value)
    {
        TimeLimitText.text = value.ToString() + "m";
        seconds = value * 60;
        Debug.Log("Time limit set to: " + seconds + " seconds.");
    }
     private void OnDestroy() // クリーンアップ
    {
        TimerSlider.onValueChanged.RemoveListener(UpdateValueText);
    }

    void Awake()
    {
        // シーン同期を有効にする
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void StartByMaster()
    {
        // 判別用プロパティをResetする
        Hashtable Webs = new Hashtable 
        { 
            { "VirusOO", false },
            {"FishingVirusOO", false},

            { "SpareBatteryPC", false },
            {"FishingSpareBatteryPC", false},

            { "SpareBatteryMyServer", false },
            {"FishingSpareBatteryMyServer", false},

            { "SmallBatteryPC", false },
            {"FishingSmallBatteryPC", false},

            { "SmallBatteryMyServer", false },
            {"FishingSmallBatteryMyServer", false},

            { "IPBST1", false },
            {"FishingIPBST1", false},

            { "IPBST2", false },
            {"FishingIPBST2", false},

            { "IPBST3", false },
            {"FishingIPBST3", false},

            { "DoSTool", false },
            {"FishingDoSTool", false},

            { "CrackTool", false },
            {"FishingCrackTool", false},
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(Webs);

        Hashtable props = new Hashtable
        {
            { "FishingNow", false},
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        //Masterが押しているかどうかのチェック
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("スタートボタンはマスタークライアントのみが押せます。");
            return;
        }

        // 一応Playerの人数を把握
        totalPlayers = PhotonNetwork.PlayerList.Length;

        // 1ゲーム何分のモードかと
        SaveTime();

        // 全員で一気にシーン遷移
        PhotonNetwork.LoadLevel("06_GameScreen");

        // IPListを生成
        MakeIPList();

    }

    private void SaveTime()
    {
        startTime = Time.time;
        // プレイ時間関連を保存
        Hashtable time = new Hashtable
        {
            { "RemainingTime", seconds },
            { "StartTime", startTime }
        };

        // 保存
        PhotonNetwork.CurrentRoom.SetCustomProperties(time);
        Debug.Log(seconds + ":" + startTime + "is saved!");
    }

    /*----------IPアドレスを生成----------*/
    private const byte SendIPs = 2;
    public void MakeIPList()
    {
        List<string> IPList = new List<string>();
        int NumberOfPlayer = PhotonNetwork.PlayerList.Length;

        while (IPList.Count < NumberOfPlayer * 2 + 1)
        {
            // IPを生成
            int first = UnityEngine.Random.Range(0, 256);
            int second = UnityEngine.Random.Range(0, 256);
            int third = UnityEngine.Random.Range(0, 256);
            int fourth = UnityEngine.Random.Range(0, 256);

            string NewIP = $"{first}.{second}.{third}.{fourth}";
            
            // 一意性が保証されたら追加
            if (!(IPList.Contains(NewIP)))
            {
                IPList.Add(NewIP);
            }
        }
            // サーバー上に保存
            Hashtable props = new Hashtable
            {
                { "IPList", string.Join(",", IPList) },
                { "SNSServerIP", IPList[0] }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            Debug.Log("IPList is saved! : " + string.Join(",", IPList));

        // プレイヤーに配布
        int i = 1;
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            object[] data = new object[] { IPList[i], IPList[i + 1] };

            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { player.ActorNumber }
            };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(SendIPs, data, options, sendOptions);
            i += 2;
        }
    }
}