using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton

    private static BrickManager _instance;

    public static BrickManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject brickContainer;

    public List<Brick> RemainingBricks { get; set; }

    public Sprite[] Sprites;

    public Color[] Colors;

    public List<int[,]> LevelsData { get; set; }
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;
    private int InitialBricksCount;

    public Brick brickPrefab;

    public int CurrentLevel;

    private void Start()
    {
        this.brickContainer = new GameObject("BricksContainer");
        this.RemainingBricks = new List<Brick>();
        this.LevelsData = LoadLevelData();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Debug.Log($"{currentSpawnX}, {currentSpawnY}, {0.0f - zShift}");
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(this.brickContainer.transform, this.Sprites[brickType - 1], this.Colors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;

                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
    }

    private List<int[,]> LoadLevelData()
    {
        TextAsset textFile = Resources.Load("levels") as TextAsset;

        string[] rows = textFile.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }
}
