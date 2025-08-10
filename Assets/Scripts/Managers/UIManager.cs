using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MainPanel mainPanel;
    public GamePanel gamePanel;
    public EndPanel endPanel;

    public Transform hookButton;

    #region Singleton
    public static UIManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        gamePanel.Active(true);
        endPanel.Active(false);
        LevelManager.instance.endGameEvent.AddListener(EndGame);

    }

    public void EndGame(bool success)
    {
        endPanel.ActiveSmooth(true);
        gamePanel.ActiveSmooth(false);

        if (success) endPanel.Success();
        else endPanel.Fail();
    }
}