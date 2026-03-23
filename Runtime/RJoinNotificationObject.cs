
using System;
using net.puk06.CanvasAnimation;
using net.puk06.CanvasAnimation.Models;
using net.puk06.CanvasAnimation.Utils;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace com.rurinya.joinnotification
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RJoinNotificationObject : UdonSharpBehaviour
    {
        private string joinText = "Join";
        private string exitText = "Exit";
        private float transitionInTime = 0.4f;
        private float transitionOutTime = 0.4f;
        private float stayTime = 1f;
        // [SerializeField] private bool allowTextAnimation = true;
        // [SerializeField] private float intervalPerChar = 0.1f;
        private int popModeOffset = 120;
        private Color joinInfoColor;
        private Color exitInfoColor;
        [Header("オブジェクトレファレンス")]
        [SerializeField] private Image background;
        [SerializeField] private Image statusBubble;

        [Header("変数名にTMPが含まれていますが、TMProを利用しておりません。")]
        [SerializeField] private Text joinTextTMP;
        [SerializeField] private Text exitTextTMP;
        [SerializeField] private Text usernameTMP;
        

        [SerializeField] private CanvasAnimationSystem canvasAnimationSystem;

        private Component[] animatedComponents;
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
            joinText = join;
            exitText = exit;
            popModeOffset = offset;
            transitionInTime = transitionIn;
            transitionOutTime = transitionOut;
            stayTime = transitionStay;
            joinInfoColor = joinColor;
            exitInfoColor = exitColor;

            if (background!= null && statusBubble != null && joinTextTMP != null && exitTextTMP != null && usernameTMP != null)
                animatedComponents = new Component[] {background, statusBubble, joinTextTMP, exitTextTMP,  usernameTMP};
            else
            {
                Debug.LogError("RJoinNotification: アニメーションで使用されるコンポーネントが初期化されていません。");
                gameObject.SetActive(false);
                return;
            }
            
            defaultUsernamePosition = (int)usernameTMP.gameObject.GetComponent<RectTransform>().anchoredPosition.x - popModeOffset;
            defaultBGPosition = (int)background.gameObject.GetComponent<RectTransform>().anchoredPosition.x - popModeOffset;
            defaultStatusPosition = Math.Abs((int)statusBubble.gameObject.GetComponent<RectTransform>().anchoredPosition.x) + popModeOffset;

            
            defaultScale = background.transform.localScale;
            canvasAnimationSystem
                .DefineTransform(animatedComponents, new Vector3(1,1,1), TransformType.Scale)
                .SaveTransform(animatedComponents, new TransformType[]{TransformType.Position})
                .Hide(background)
                .Hide(statusBubble)
                .Hide(joinTextTMP)
                .Hide(exitTextTMP)
                .Hide(usernameTMP);
        }

        public void StartAnimation(bool status, string username, bool hasBackground, int mode)
        {
            background.gameObject.SetActive(hasBackground);
            if (status == true){
                joinTextTMP.text=joinText;
                joinTextTMP.gameObject.SetActive(true);
                exitTextTMP.gameObject.SetActive(false);

                statusBubble.color = joinInfoColor;
            }
            else {
                exitTextTMP.text=exitText;
                joinTextTMP.gameObject.SetActive(false);
                exitTextTMP.gameObject.SetActive(true);

                statusBubble.color = exitInfoColor;
            }
            usernameTMP.text = username;
            AnimationController(mode);
        }
        void Start()
        {
            
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
                .Cancel(animatedComponents);
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


        // CANVAS ANIMATION SYSTEM

        private void AnimFadeIn()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(animatedComponents, new TransformType[] {TransformType.Position})
                .Fade(background, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(background, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut);
        }
        private void AnimPop()
        {
            // background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
            // usernameTMP.color = new Color(usernameTMP.color.r, usernameTMP.color.g, usernameTMP.color.b, 1);
            // statusBubble.color = new Color(statusBubble.color.r, statusBubble.color.g, statusBubble.color.b, 1);
            // joinTextTMP.color = new Color(joinTextTMP.color.r, joinTextTMP.color.g, joinTextTMP.color.b, 1);
            // exitTextTMP.color = new Color(exitTextTMP.color.r, exitTextTMP.color.g, exitTextTMP.color.b, 1);
            targetScale = new Vector3((float)1.2*defaultScale.x, (float)1.2*defaultScale.y,(float)1.2*defaultScale.z);
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(animatedComponents, new TransformType[] {TransformType.Scale})
                .Fade(background, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Scale(animatedComponents, transitionInTime, 0, AnimationDirection.From, new Vector3(0,0,0), TransitionType.EaseInOut)
                .Move(background, transitionInTime, 0, defaultBGPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .Move(usernameTMP, transitionInTime, 0, defaultUsernamePosition, MoveDirection.Right, TransitionType.EaseInOut)
                .Move(statusBubble, transitionInTime, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .Move(joinTextTMP, transitionInTime, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .Move(exitTextTMP, transitionInTime, 0, defaultStatusPosition, MoveDirection.Left, TransitionType.EaseInOut)
                //.Scale(animatedComponents, transitionInTime*0.2f, transitionInTime, AnimationDirection.To, targetScale, TransitionType.Linear)
                //.Scale(animatedComponents, 0.1f, 0, AnimationDirection.From, targetScale, TransitionType.EaseInOut)
                .Fade(background, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Scale(animatedComponents, transitionOutTime, stayTime+transitionInTime, AnimationDirection.To, new Vector3(0,0,0), TransitionType.EaseInOut)
                .MoveTo(background, transitionOutTime, stayTime+transitionInTime, defaultBGPosition, MoveDirection.Left, TransitionType.EaseInOut)
                .MoveTo(usernameTMP, transitionOutTime, stayTime+transitionInTime, defaultUsernamePosition, MoveDirection.Left, TransitionType.EaseInOut)
                .MoveTo(statusBubble, transitionOutTime, stayTime+transitionInTime, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .MoveTo(joinTextTMP, transitionOutTime, stayTime+transitionInTime, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut)
                .MoveTo(exitTextTMP, transitionOutTime, stayTime+transitionInTime, defaultStatusPosition, MoveDirection.Right, TransitionType.EaseInOut);
        }
        private void AnimFadeInLeft()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(animatedComponents, new TransformType[] {TransformType.Position})
                .Fade(background, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(animatedComponents, transitionInTime, 0, 200, MoveDirection.Right, TransitionType.EaseInOut )
                .Fade(background, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut);
        }

        private void AnimFadeInRight()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(animatedComponents, new TransformType[] {TransformType.Position})
                .Fade(background, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(animatedComponents, transitionInTime, 0, 200, MoveDirection.Left, TransitionType.EaseInOut )
                .Fade(background, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut);
        }

        private void AnimFadeInDown()
        {
            canvasAnimationSystem
                // .Cancel(animatedComponents)
                .ResetTransform(animatedComponents, new TransformType[] {TransformType.Position})
                .Fade(background, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionInTime, 0, FadeType.In, TransitionType.EaseInOut)
                .Move(animatedComponents, transitionInTime, 0, 50, MoveDirection.Up, TransitionType.EaseInOut )
                .Fade(background, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(statusBubble, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(joinTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(exitTextTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut)
                .Fade(usernameTMP, transitionOutTime, stayTime+transitionInTime, FadeType.Out, TransitionType.EaseInOut);
        }
    }

}
