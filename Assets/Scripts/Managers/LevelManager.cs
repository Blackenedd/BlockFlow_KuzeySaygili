using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    private List<Block> blocks = new List<Block>();
    private List<Wall> walls = new List<Wall>();

    public Level level;

    private string BLOCK_DATA = "blocks/";

    private int targetCount = 0;
    private int currentCount = 0;

    private float targetTime;
    private float currentTime;

    [SerializeField] private Transform blockContainer;
    [HideInInspector] public UnityEvent<bool> endGameEvent = new UnityEvent<bool>();
    private bool started = false;
    private bool finished = false;

    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ConstructLevel(level);
        StartLevel();
    }

    private void StartLevel()
    {
        if (level.information.hasTime)
        {
            targetTime = level.information.time;
            currentTime = targetTime;
            DOTween.To(() => currentTime, x => currentTime = x, 0f, targetTime).OnUpdate(() =>
            {
                //uimanager updates the time
            }).SetEase(Ease.Linear).OnComplete(OnTimerOut);
        }
    }
    private void OnTimerOut()
    {
        if (finished) return; finished = true;
        endGameEvent.Invoke(false);
    }
    public void OnProgress(int count = 1)
    {
        if (finished) return;

        currentCount += count;
        if (currentCount >= targetCount) { finished = true; endGameEvent.Invoke(true); }
    }
    private void ConstructLevel(Level level)
    {
        GridManager.instance.GenerateGrid(level.information.gridWidth, level.information.gridHeight);

        walls = GridManager.instance.GetWalls();
        targetCount = level.information.blocks.Count;

        level.information.blocks.ForEach(x =>
        {
            SpawnBlock(x.blockIndex, x.blockColor, x.worldPosition,x.Lock);
        });

        level.information.walls.ForEach(x =>
        {
            walls[x.orderIndex].Construct(x.lenght, x.color);

            if (x.lenght > 1) walls[x.orderIndex + 1].Close(walls[x.orderIndex].GetSideInformation());
            if (x.lenght > 2) walls[x.orderIndex + 2].Close(walls[x.orderIndex].GetSideInformation());
        });

        CameraManager.instance.SetOffset(level.information.gridWidth / 2, 0, level.information.gridHeight  / 2);

    }

    private void SpawnBlock(int index, int color,Vector2 worldPosition,bool l)
    {
        Block block = Instantiate(Resources.Load<Block>(BLOCK_DATA + index),blockContainer);
        block.Construct(color, worldPosition,l);
        blocks.Add(block);
    }
}
