using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace HULTemplate
{
    public class TutorialPanel : Panel
    {
        public TextMeshProUGUI tutorialText;
        public RectTransform hand;
        public RectTransform ui;
        private bool closed = false;
        public void TapTutorial(string instruction, Vector3 worldPosition)
        {
            if (closed) return;

            ActiveSmooth(true, 0.5f);
            tutorialText.text = instruction;

            Vector2 adjustedPosition = Camera.main.WorldToScreenPoint(worldPosition);

            adjustedPosition.x *= ui.rect.width / (float)Camera.main.pixelWidth;
            adjustedPosition.y *= ui.rect.height / (float)Camera.main.pixelHeight;

            // set it

            //hand.anchoredPosition = adjustedPosition - ui.sizeDelta / 2f;

            hand.anchoredPosition = adjustedPosition - ui.sizeDelta / 2f;
            hand.gameObject.SetActive(true);

            hand.DOScale(1.25f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        }
        public void CloseTapTutorial()
        {
            closed = true;
            tutorialText.transform.DOKill();

            tutorialText.transform.DOScale(0f, 0.6f);
            hand.transform.DOScale(0f, 0.6f);

            hand.gameObject.SetActive(false); ActiveSmooth(false, 0.5f);
        }
    }
}