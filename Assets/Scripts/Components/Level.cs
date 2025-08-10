using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Level : MonoBehaviour
{
    public LevelData information;

    [Serializable]
    public struct LevelData
    {
        public int gridWidth;
        public int gridHeight;

        public bool hasTime;
        public int time;

        public List<WallData> walls;
        public List<BlockData> blocks;

        public Vector3 cameraSettings;
        public Vector3 cameraCenter;
    }
    [Serializable]
    public struct BlockData
    {
        public int blockIndex;
        public int blockColor;
        public Vector2 worldPosition;
        public bool Lock;
        public bool ice;
    }
    [Serializable]
    public struct WallData
    {
        public int orderIndex;
        public int lenght;
        public int color;
    }

    [ContextMenu("Export Level to JSON")]
    public void ExportLevel()
    {
        string json = JsonUtility.ToJson(information, true);

        // Levels klasörü (Assets/Resources/Levels)
        string folderPath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = folderPath + $"/" + gameObject.name + ".json";
        File.WriteAllText(filePath, json);

        Debug.Log($"Level JSON olarak kaydedildi: {filePath}");
    }
}
