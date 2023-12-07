using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ZB.UsingTweening.Guage
{
    public class TweeningGuage : MonoBehaviour
    {
        public Image Img_mainBar;
        public Image Img_followBar;

        [SerializeField] private RectTransform rtf_mainBar;
        [SerializeField] private RectTransform rtf_followBar;

        [Space,Header("FillHear")]
        [SerializeField] private float duration_mainBar;
        [SerializeField] private float duration_followBar;
        [SerializeField] private float delay_followBar;
        [SerializeField] private Ease ease_mainBar;
        [SerializeField] private Ease ease_followBar;

        [Space, Header("TestValues")]
        [SerializeField] private float test_ratio;

        float firstSizeX;
        float firstSizeY;

        /// <summary>
        /// 0~1 사이 숫자 입력
        /// </summary>
        /// <param name="ratio"></param>
        public void Change(float ratio)
        {
            if (ratio < 0) ratio = 0;
            if (ratio > 1) ratio = 1;
            float targetSizeX = firstSizeX * ratio;

            rtf_mainBar.DOKill();
            rtf_followBar.DOKill();
            rtf_mainBar.DOSizeDelta(new Vector2(targetSizeX, firstSizeY), duration_mainBar).SetEase(ease_mainBar);
            rtf_followBar.DOSizeDelta(new Vector2(targetSizeX, firstSizeY), duration_mainBar).SetEase(ease_followBar).SetDelay(delay_followBar);
        }

        public void ChangeNotTweening(float ratio)
        {
            if (ratio < 0) ratio = 0;
            if (ratio > 0) ratio = 1;
            float targetSizeX = firstSizeX * ratio;

            rtf_mainBar.DOKill();
            rtf_followBar.DOKill();
            rtf_mainBar.sizeDelta = new Vector2(targetSizeX, firstSizeY);
            rtf_mainBar.sizeDelta = new Vector2(targetSizeX, firstSizeY);
        }

        [ContextMenu("TestChange")]
        public void TestChange()
        {
            Change(test_ratio);
        }

        private void Awake()
        {
            firstSizeX = rtf_mainBar.sizeDelta.x;
            firstSizeY = rtf_mainBar.sizeDelta.y;
        }
    }
}