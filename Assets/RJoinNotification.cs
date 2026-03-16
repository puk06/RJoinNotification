
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
        [SerializeField] private bool hasBackground = true;
        [SerializeField] private AnimMode animationMode = AnimMode.FadeIn;
        [SerializeField] private bool allowMultipleNotifications = true;
        [SerializeField] private GameObject[] notification;
        [SerializeField] private AudioClip joinSound;
        [SerializeField] private AudioClip exitSound;
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
                if (!notification[0].activeSelf) return notification[0];
                else return notification[1];
            }
            if(notificationIndex >= notification.Length) notificationIndex = 1;
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
            if(!gameObject.GetComponent<AudioSource>().isPlaying && !isMuted){
                gameObject.GetComponent<AudioSource>().clip = joinSound;
                gameObject.GetComponent<AudioSource>().Play();
            }
            GameObject notificationObject = NotificationManager();
            notificationObject.SetActive(true);
            notificationObject.transform.SetAsLastSibling();
            notificationObject.GetComponent<RJoinNotificationObject>().SetInfo(true, player.displayName, hasBackground, (int)animationMode);

            
        }
        public override void OnPlayerLeft(VRCPlayerApi player){

            if(!gameObject.GetComponent<AudioSource>().isPlaying && !isMuted){
                gameObject.GetComponent<AudioSource>().clip = exitSound;
                gameObject.GetComponent<AudioSource>().Play();
            }
            GameObject notificationObject = NotificationManager();
            notificationObject.SetActive(true);
            notificationObject.transform.SetAsLastSibling();
            notificationObject.GetComponent<RJoinNotificationObject>().SetInfo(false, player.displayName, hasBackground, (int)animationMode);
            
        }

        public void SetMuted(bool state){
            isMuted = state;
        }

        public void SetMuted()
        {
            isMuted = !isMuted;
        }

    }

}
