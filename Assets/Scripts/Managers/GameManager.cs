using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Level Booleans")]
    public bool autoLevel;
    [Header("Level'S"), Space(5)]
    public int level = -1;
    public int levelCount = 10;
    public int levelLoopFrom = 3;

    [Header("CURRENCY"), Space(5)]
    public int money = 0;
    public Level testLevel;
    public LevelManager levelManager;

    #region Singleton
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GetDependencies();
        }
    }
    #endregion

    private void Start()
    {
        int levelIndex = level;
        while (levelIndex > levelCount) levelIndex = levelIndex - levelCount + (levelLoopFrom - 1);

        TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/level-"+levelIndex);
        if (jsonFile == null)
        {
            Debug.LogError($"Level bulunamadı!");
            return;
        }

        Level.LevelData data = JsonUtility.FromJson<Level.LevelData>(jsonFile.text);
        if (testLevel != null) levelManager.ConstructLevel(testLevel.information);
        else levelManager.ConstructLevel(data);

        Application.targetFrameRate = 60;
    }
    private void GetDependencies()
    {
        level = DataManager.instance.level;
        money = DataManager.instance.money;
    }

    #region DataOperations
    public void AddMoney(int amount)
    {
        money += amount;
        DataManager.instance.SetMoney(money);
    }

    public void LevelUp(bool complete)
    {
        if(complete) DataManager.instance.SetLevel(++level);
    }
    #endregion

    #region SceneOperations
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
