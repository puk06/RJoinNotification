
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace com.rurinya.joinnotification
{
    internal enum AnimMode
    {
        FadeIn,
        Pop,
        FadeInFromLeft,
        FadeInFromRight,
        FadeInFromBelow
    }
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RJoinNotification : UdonSharpBehaviour
    {
        [Header("入退室通知の内容")]
        [Header("スペースが文字数で変わらないため、文字数が多くなると表示がおかしくなる場合があります。")]
        [SerializeField] private string joinText = "Join";
        [SerializeField] private string exitText = "Exit";
        [Header("入退室通知の色")]
        [Header("Alphaを0のままにしてください。")]
        [Header("実際に表示されるAlpha値は1になります。")]
        [SerializeField] private Color joinInfoColor;
        [SerializeField] private Color exitInfoColor;
        [Header("アニメーションの長さ")]
        [SerializeField] private float transitionInTime = 0.4f;
        [SerializeField] private float transitionOutTime = 0.4f;
        [SerializeField] private float stayTime = 1f;

        [Header("バックグラウンド有効")]
        [SerializeField] private bool hasBackground = true;
        [Header("オーディオだけにする")]
        [SerializeField] private bool audioOnly;
        [Header("アニメーションのモード")]
        [SerializeField] private AnimMode animationMode = AnimMode.FadeIn;
        [Header("Popモードで右へのオフセット")]
        [SerializeField] private int popModeOffset = 120;
        [Header("複数のアニメーションが同時に動作できるようにする")]
        [SerializeField] private bool allowMultipleNotifications = true;

        [Header("入、退室音")]
        [SerializeField] private AudioClip joinSound;
        [SerializeField] private AudioClip exitSound;

        [Header("通知オブジェクト。通常では編集する必要がありません。")]
        [SerializeField] private GameObject[] notification;
        
        private bool isMuted = false;


        //Overflowed Index
        private int notificationIndex = 0;
        private int maxNotificationIndex;
        
        private Vector3 defaultScale;

        void Start()
        {
            defaultScale = gameObject.transform.localScale;
            maxNotificationIndex = notification.Length;
            
            if (maxNotificationIndex < 2)
            {
                Debug.LogError("RJoinNotification: Notificationオブジェクトがありません");
                gameObject.SetActive(false);
            }
            EyeHeightSetup();
            
        }
        void Update()
        {
            
            gameObject.transform.position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            gameObject.transform.rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
        }

        public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
        {
            if (!player.isLocal) return;
            EyeHeightSetup();
        }
        public override void OnAvatarChanged(VRCPlayerApi player)
        {
            if (!player.isLocal) return;
            EyeHeightSetup();
        }
        
        private void EyeHeightSetup()
        {
            float playerEyeHeight = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
            gameObject.transform.localScale = new Vector3(playerEyeHeight * defaultScale.x, playerEyeHeight * defaultScale.y, playerEyeHeight * defaultScale.z);
        }

        private GameObject NotificationManager()
        {
            if (!allowMultipleNotifications)
            {
                // if (!notification[0].activeSelf) 
                    return notification[0];
                // else return notification[1];
            }
            if(notificationIndex >= notification.Length) notificationIndex = 0;
            return notification[notificationIndex++];
            
            
        }
        // public void OverflowRecordCleanUp()
        // {
        //     for(int i = 0; i < maxNotificationIndex; i++)
        //     {
        //         if (notification[i].GetComponent<RJoinNotificationObject>().isActive){
        //             continue;
        //         }
        //         else return;
        //     }
        //     notificationIndex = 0;
        // }
        public override void OnPlayerJoined(VRCPlayerApi player){
            SendNotification(true, player.displayName);
            
        }
        public override void OnPlayerLeft(VRCPlayerApi player){
            SendNotification(false, player.displayName);
        }
        private void SendNotification(bool state, string username)
        {
            if(!gameObject.GetComponent<AudioSource>().isPlaying && !isMuted){
                gameObject.GetComponent<AudioSource>().clip = state ? joinSound : exitSound;
                gameObject.GetComponent<AudioSource>().Play();
            }
            GameObject notificationObject = NotificationManager();
            if (!audioOnly)
            {
                if(!notificationObject.activeSelf){
                    notificationObject.SetActive(true);
                    notificationObject.GetComponent<RJoinNotificationObject>().Setup(joinText, exitText, popModeOffset, transitionInTime, transitionOutTime, stayTime, joinInfoColor, exitInfoColor);
                }
                notificationObject.transform.SetAsLastSibling();
                notificationObject.GetComponent<RJoinNotificationObject>().StartAnimation(state, username, hasBackground, (int)animationMode);
            }
        }

        public void SetMuted(bool state){
            isMuted = state;
        }

        public void SetMuted()
        {
            isMuted = !isMuted;
            Debug.Log("RJoinNotification: Mute Mode: " + isMuted);
        }
        public void Test(bool state)
        {
            SendNotification(state, "テスト / Test Username " + UnityEngine.Random.Range(0,1000));
        }
        public void Test()
        {
            Test(UnityEngine.Random.Range(0,2) == 1);
        }

    }

}
