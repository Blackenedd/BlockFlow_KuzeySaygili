using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject cornerPrefab;
    public GameObject whitePrefab;

    

    [Header("Parents")]
    public Transform cellParent;
    public Transform wallParent;
    public Transform cornerParent;
    public Transform colliderParent;
    public Transform whiteParent;

    public static GridManager instance;

    private float cellSize = 1f;
    private List<Wall> walls = new List<Wall>();

    private void Awake()
    {
        instance = this;
    }

    private int _width;
    private int _height;

    public void GenerateGrid(int gridWidth,int gridHeight)
    {
        if (!cellPrefab || !wallPrefab || !cornerPrefab)
        {
            Debug.LogWarning("Prefab atamaları eksik!");
            return;
        }

        _width = gridWidth;
        _height = gridHeight;

        ClearChildren(cellParent);
        ClearChildren(wallParent);
        ClearChildren(cornerParent);
        ClearChildren(colliderParent);

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, z * cellSize);
                Instantiate(cellPrefab, pos, Quaternion.identity, cellParent);
            }
        }

        SpawnWalls();

        SpawnCorners();

        BuildFrameColliders();
    }

    public List<Wall> GetWalls()
    {
        return walls;
    }

    void SpawnWalls()
    {
        float maxX = (_width - 1) * cellSize;
        float maxZ = (_height - 1) * cellSize;
        float half = cellSize / 2f;

        Wall wall;

        // Alt kenar (yatay)
        for (int x = 0; x < _width; x++)
        {
            wall = Instantiate(wallPrefab, new Vector3(x * cellSize - half, 0, -half), Quaternion.Euler(0, 90, 0), wallParent).GetComponent<Wall>();
            wall.UpdateSideInformation(0);
            walls.Add(wall);
        }

        // Üst kenar (yatay)
        for (int x = 0; x < _width; x++)
        {
            wall = Instantiate(wallPrefab,new Vector3(x * cellSize - half, 0, maxZ + 0.8f),Quaternion.Euler(0, 90, 0),wallParent).GetComponent<Wall>();
            wall.UpdateSideInformation(1);
            walls.Add(wall);
        }

        // Sol kenar (dikey)
        for (int z = 0; z < _height; z++)
        {
            wall = Instantiate(wallPrefab, new Vector3(-0.8f, 0, z * cellSize - half), Quaternion.identity, wallParent).GetComponent<Wall>();
            wall.UpdateSideInformation(2);
            walls.Add(wall);
        }

        // Sağ kenar (dikey)
        for (int z = 0; z < _height; z++)
        {
            wall = Instantiate(wallPrefab, new Vector3(maxX + half, 0, z * cellSize - half), Quaternion.identity, wallParent).GetComponent<Wall>();
            wall.UpdateSideInformation(3);
            walls.Add(wall);
        }
        
    }

    void SpawnCorners()
    {
        float maxX = (_width - 1) * cellSize;
        float maxZ = (_height - 1) * cellSize;
        float half = cellSize / 2f;

        Instantiate(cornerPrefab, new Vector3(-half, 0, -half), Quaternion.identity, cornerParent); // Sol alt
        Instantiate(cornerPrefab, new Vector3(maxX + half, 0, -half), Quaternion.Euler(0,-90,0), cornerParent); // Sağ alt
        Instantiate(cornerPrefab, new Vector3(-half, 0, maxZ + half), Quaternion.Euler(0, 90, 0),cornerParent); // Sol üst
        Instantiate(cornerPrefab, new Vector3(maxX + half, 0, maxZ + half), Quaternion.Euler(0, 180, 0), cornerParent); // Sağ üst
    }

    void ClearChildren(Transform parent)
    {
        if (parent == null) return;
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }

    private void BuildFrameColliders()
    {
        float totalWidth = _width * cellSize;
        float totalHeight = _height * cellSize;
        float wallThickness = 0.5f;
        float wallHeight = 1f;

        // Sol
        CreateWallCollider(new Vector3(-wallThickness * 1.5f, wallHeight / 2f, totalHeight / 2f - cellSize / 2f),
                           new Vector3(wallThickness, wallHeight, totalHeight));

        // Sağ
        CreateWallCollider(new Vector3(totalWidth - wallThickness / 2f, wallHeight / 2f, totalHeight / 2f - cellSize / 2f),
                           new Vector3(wallThickness, wallHeight, totalHeight));

        // Alt
        CreateWallCollider(new Vector3(totalWidth / 2f - cellSize / 2f, wallHeight / 2f, -wallThickness * 1.5f),
                           new Vector3(totalWidth, wallHeight, wallThickness));

        // Üst
        CreateWallCollider(new Vector3(totalWidth / 2f - cellSize / 2f, wallHeight / 2f, totalHeight - wallThickness / 2f),
                           new Vector3(totalWidth, wallHeight, wallThickness));
    }

    private void CreateWallCollider(Vector3 position, Vector3 size)
    {
        GameObject wallObj = new GameObject("WallCollider");
        wallObj.transform.SetParent(colliderParent);
        wallObj.transform.position = position;
        BoxCollider col = wallObj.AddComponent<BoxCollider>();
        col.size = size;
    }
    private void CreateWhiteBorder(Vector3 position, Vector3 size)
    {
        GameObject whiteBorder = Instantiate(whitePrefab);
        whiteBorder.transform.SetParent(whiteParent);
        whiteBorder.transform.position = position;
        whiteBorder.transform.localScale = size;
    }
}
