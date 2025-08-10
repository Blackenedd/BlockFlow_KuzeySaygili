using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class GamePanel : Panel
{
    public RectTransform coinPanelRect;
    public RectTransform restartButtonRect;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;
    public CanvasGroup timerPanel;
    [HideInInspector] int inGameCurrency;

    private Tween tween;
    public TextMeshProUGUI moneyText;
    private Button restartButton { get { return restartButtonRect.GetComponent<Button>(); } }

    private void Start()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        levelText.text = "LEVEL " + GameManager.instance.level.ToString();
    }
    private void Update()
    {
        moneyText.text = GameManager.instance.money.ToString();
    }
    public void UpdateMoney()
    {
        moneyText.text = GameManager.instance.money.ToString();
    }
    public void OpenTimer()
    {
        timerPanel.DOFade(1f, 0.3f);
    }
    public void UpdateTimer(int count)
    {
        var ts = TimeSpan.FromSeconds(count);
        timerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
    public void SetMoney(float to, float duration = 0.3f)
    {
        if (tween != null) tween.Kill();

        coinPanelRect
        .DOScale(1.2f, duration * 0.5f)
        .SetEase(Ease.Linear)
        .SetLoops(2, LoopType.Yoyo);

        float startFrom = int.Parse(moneyText.text);
        tween = DOTween.To((x) => startFrom = x, startFrom, to, duration)
        .OnUpdate(() =>
        {
            moneyText.text = ((int)startFrom).ToString();
        })
        .OnComplete(() => moneyText.text = ((int)to).ToString());
    }

    public void AddMoney(int amount)
    {
        float startFrom = int.Parse(moneyText.text);
        SetMoney(inGameCurrency + amount);
    }

    private void OnClickRestartButton()
    {
        GameManager.instance.RestartScene();
    }
}
