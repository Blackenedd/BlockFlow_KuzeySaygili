using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Block> blocks = new List<Block>();
    private List<Wall> walls = new List<Wall>();

    public Level level;

    private string BLOCK_DATA = "blocks/";

    [SerializeField] private Transform blockContainer;

    private void Start()
    {
        ConstructLevel(level);
    }

    private void ConstructLevel(Level level)
    {
        GridManager.instance.GenerateGrid(level.information.gridWidth, level.information.gridHeight);

        walls = GridManager.instance.GetWalls();

        level.information.blocks.ForEach(x =>
        {
            SpawnBlock(x.blockIndex, x.blockColor, x.worldPosition);
        });

        level.information.walls.ForEach(x =>
        {
            walls[x.orderIndex].Construct(x.lenght, x.color);

            if (x.lenght > 1) walls[x.orderIndex + 1].Close(walls[x.orderIndex].GetSideInformation());
            if (x.lenght > 2) walls[x.orderIndex + 2].Close(walls[x.orderIndex].GetSideInformation());
        });

    }

    private void SpawnBlock(int index, int color,Vector2 worldPosition)
    {
        Block block = Instantiate(Resources.Load<Block>(BLOCK_DATA + index),blockContainer);
        block.Construct(color, worldPosition);
        blocks.Add(block);
    }
}
