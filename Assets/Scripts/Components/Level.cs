using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    public LevelData information;

    [Serializable]
    public struct LevelData
    {
        public int gridWidth;
        public int gridHeight;
        public List<WallData> walls;
        public List<BlockData> blocks;
    }
    [Serializable]
    public struct BlockData
    {
        public int blockIndex;
        public int blockColor;
        public Vector2 worldPosition;
    }
    [Serializable]
    public struct WallData
    {
        public int orderIndex;
        public int lenght;
        public int color;
    }
}
