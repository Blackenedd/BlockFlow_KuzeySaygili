using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class EndPanel : Panel
{
    public EndPanelContainer success;
    public EndPanelContainer fail;
    private EndPanelContainer activePanel;

    public void Success()
    {
        win = true;
        activePanel = success;
        Appear(0.5f, GameManager.instance.level);
    }

    public void Fail()
    {
        win = false;
        activePanel = fail;
        Appear(0.5f, GameManager.instance.level);
    }

    private void Appear(float duration = 0.75f, int level = 0)
    {
        activePanel.levelText.text = "LEVEL " + level.ToString();
        activePanel.continueButton.localScale = Vector3.zero;
        activePanel.self.gameObject.SetActive(true);
        activePanel.continueButton.DOScale(1f, duration).SetEase(Ease.OutBack);
    }
    private bool win = false;
    private bool started = false;
    public void OnPressRestart()
    {
        if (!win)
        {
            GameManager.instance.RestartScene();
        }
        else
        {
            if (started) return; started = true;

            Delay(0.2f, GameManager.instance.RestartScene);
        }
    }
    #region Delay
    private IEnumerator DeleyCoroutine(float _waitTime = 1f, UnityAction _action = null)
    {
        yield return new WaitForSeconds(_waitTime);
        _action?.Invoke();
    }
    private void Delay(float waitTime = 1f, UnityAction onComplete = null)
    {
        StartCoroutine(DeleyCoroutine(waitTime, onComplete));
    }
    #endregion
}

[System.Serializable]
public struct EndPanelContainer
{
    public TextMeshProUGUI levelText;
    public RectTransform self;
    public RectTransform continueButton;
}
