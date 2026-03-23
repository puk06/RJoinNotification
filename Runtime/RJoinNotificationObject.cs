using System;
using net.puk06.CanvasAnimation;
using net.puk06.CanvasAnimation.Models;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace com.rurinya.joinnotification
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RJoinNotificationObject : UdonSharpBehaviour
    {
        private string joinNotificationText = "Join";
        private string exitNotificationText = "Exit";

        private float fadeInDuration = 0.4f;
        private float fadeOutDuration = 0.4f;
        private float durationBeforeRemoval = 1f;

        // [SerializeField] private bool allowTextAnimation = true;
        // [SerializeField] private float intervalPerChar = 0.1f;

        private int joinNotificationPopOffset = 120;
        private Color joinNotificationColor;
        private Color exitMessageColor;

        [Header("オブジェクトレファレンス")]
        [SerializeField] private Image background;
        [SerializeField] private Image statusBubble;

        [SerializeField]
        [FormerlySerializedAs("joinTextTMP")]
        private Text joinText;

        [SerializeField]
        [FormerlySerializedAs("exitTextTMP")]
        private Text exitText;

        [SerializeField]
        [FormerlySerializedAs("usernameTMP")]
        private Text usernameText;

        [SerializeField] private CanvasAnimationSystem canvasAnimationSystem;

        private Component[] notificationAnimationComponents;
        private Vector3 defaultScale;
        private Vector3 targetScale;

        // Default position
        private int defaultUsernamePosition;
        private int defaultBGPosition;
        private int defaultStatusPosition;

        // Text Animation

        // private float timeStamp;
        // private string tempUsername;
        // private int animIndex;
        public void Setup(string join, string exit, int offset, float transitionIn, float transitionOut, float transitionStay, Color joinColor, Color exitColor)
        {
            joinNotificationText = join;
            exitNotificationText = exit;
            joinNotificationPopOffset = offset;
            fadeInDuration = transitionIn;
            fadeOutDuration = transitionOut;
            durationBeforeRemoval = transitionStay;
            joinNotificationColor = joinColor;
            exitMessageColor = exitColor;

            if (background == null || statusBubble == null || joinText == null || exitText == null || usernameText == null)
            {
                Debug.LogError("RJoinNotification: すべてのコンポーネントが正しく設定されていません。");
                gameObject.SetActive(false);
                return;
            }

            notificationAnimationComponents = new Component[] { background, statusBubble, joinText, exitText, usernameText };

            defaultUsernamePosition = (int)usernameText.gameObject.GetComponent<RectTransform>().anchoredPosition.x - joinNotificationPopOffset;
            defaultBGPosition = (int)background.gameObject.GetComponent<RectTransform>().anchoredPosition.x - joinNotificationPopOffset;
            defaultStatusPosition = Math.Abs((int)statusBubble.gameObject.GetComponent<RectTransform>().anchoredPosition.x) + joinNotificationPopOffset;

            defaultScale = background.transform.localScale;
            canvasAnimationSystem
                .DefineTransform(notificationAnimationComponents, new Vector3(1, 1, 1), TransformType.Scale)
                .SaveTransform(notificationAnimationComponents, new TransformType[] { TransformType.Position })
                .Hide(background)
                .Hide(statusBubble)
                .Hide(joinText)
                .Hide(exitText)
                .Hide(usernameText);
        }

        public void StartAnimation(bool status, string username, bool hasBackground, int mode)
        {
            background.gameObject.SetActive(hasBackground);

            joinText.gameObject.SetActive(status);
            exitText.gameObject.SetActive(!status);

            if (status == true)
            {
                joinText.text = joinNotificationText;
                statusBubble.color = joinNotificationColor;
            }
            else
            {
                exitText.text = exitNotificationText;
                statusBubble.color = exitMessageColor;
            }

            usernameText.text = username;
            AnimationController(mode);
        }
        // void Update()
        // {
        //     if (allowTextAnimation)
        //     {
        //         if(Time.timeSinceLevelLoad > timeStamp + intervalPerChar)
        //         {
        //             if (animIndex <= tempUsername.Length)
        //             {
        //                 usernameTMP.text = tempUsername.Substring(0, animIndex);
        //                 timeStamp = Time.timeSinceLevelLoad;
        //             }
        //         }
        //     }
        // }
        private void AnimationController(int mode)
        {
            canvasAnimationSystem
                .Cancel(notificationAnimationComponents);
            // Not In Use
            // if(allowTextAnimation)
            // {
            //     timeStamp = Time.timeSinceLevelLoad;
            //     animIndex = 0;
            // }

            switch (mode)
            {
                case 0:
                    AnimFadeIn();
                    return;
                case 1:
                    AnimPop();
                    return;
                case 2:
                    AnimFadeInLeft();
                    return;
                case 3:
                    AnimFadeInRight();
                    return;
                case 4:
                    AnimFadeInDown();
                    return;
                default:
                    Debug.LogError("RJoinNotification: モードは0~4しか登録できません。");
                    return;
            }
        }

        #region CanvasAnimationSystem Animations
        private void AnimFadeIn()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(notificationAnimationComponents, new TransformType[] { TransformType.Position })
                .Fade(background, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut);
        }
        private void AnimPop()
        {
            // background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
            // usernameTMP.color = new Color(usernameTMP.color.r, usernameTMP.color.g, usernameTMP.color.b, 1);
            // statusBubble.color = new Color(statusBubble.color.r, statusBubble.color.g, statusBubble.color.b, 1);
            // joinTextTMP.color = new Color(joinTextTMP.color.r, joinTextTMP.color.g, joinTextTMP.color.b, 1);
            // exitTextTMP.color = new Color(exitTextTMP.color.r, exitTextTMP.color.g, exitTextTMP.color.b, 1);
            targetScale = new Vector3((float)1.2 * defaultScale.x, (float)1.2 * defaultScale.y, (float)1.2 * defaultScale.z);
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(notificationAnimationComponents, new TransformType[] { TransformType.Scale })
                .Fade(background, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Scale(notificationAnimationComponents, fadeInDuration, 0, AnimationDirection.From, new Vector3(0, 0, 0), TransitionType.EaseInOut)
                .Move(background, fadeInDuration, 0, defaultBGPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .Move(usernameText, fadeInDuration, 0, defaultUsernamePosition, MoveDirection.Right, TransitionType.EaseInOut)
                .Move(statusBubble, fadeInDuration, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .Move(joinText, fadeInDuration, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .Move(exitText, fadeInDuration, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                //.Scale(notificationAnimationComponents, transitionInTime*0.2f, transitionInTime, AnimationDirection.To, targetScale, TransitionType.Linear)
                //.Scale(notificationAnimationComponents, 0.1f, 0, AnimationDirection.From, targetScale, TransitionType.EaseInOut)
                .Fade(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Scale(notificationAnimationComponents, fadeOutDuration, durationBeforeRemoval + fadeInDuration, AnimationDirection.To, new Vector3(0, 0, 0), TransitionType.EaseInOut)
                .MoveTo(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, defaultBGPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .MoveTo(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, defaultUsernamePosition, MoveDirection.Left, TransitionType.EaseInOut)
                .MoveTo(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .MoveTo(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .MoveTo(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut);
        }
        private void AnimFadeInLeft()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(notificationAnimationComponents, new TransformType[] { TransformType.Position })
                .Fade(background, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(notificationAnimationComponents, fadeInDuration, 0, 200, MoveDirection.Right, TransitionType.EaseInOut)
                .Fade(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut);
        }
        private void AnimFadeInRight()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(notificationAnimationComponents, new TransformType[] { TransformType.Position })
                .Fade(background, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(notificationAnimationComponents, fadeInDuration, 0, 200, MoveDirection.Left, TransitionType.EaseInOut)
                .Fade(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut);
        }
        private void AnimFadeInDown()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(notificationAnimationComponents, new TransformType[] { TransformType.Position })
                .Fade(background, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameText, fadeInDuration, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(notificationAnimationComponents, fadeInDuration, 0, 50, MoveDirection.Up, TransitionType.EaseInOut)
                .Fade(background, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameText, fadeOutDuration, durationBeforeRemoval + fadeInDuration, FadeType.Out, TransitionType.EaseInOut);
        }
        #endregion
    }
}
