using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{

    private int[,] grid;

     [SerializeField]private int blockSize;
     [SerializeField]private int blockgap;

    [SerializeField] private int gridSizeX,gridSizeY;

    [SerializeField] private RectTransform panel;

    [SerializeField]private BlockScript blockPrefab;

    [SerializeField] private Transform parentTransform;

    [SerializeField] private GridLayoutGroup gridLayout;


    //2d points into 3DGrid
    [SerializeField] private BlockScript[] allGrid;

    //[SerializeField] private GameObject[] enabledGrid;

    [SerializeField] private BlockScript[,] enabledGrid;



    [Header("SelectGrid")]
    [SerializeField] private int minSelectGrid = 3;
    [SerializeField] private int maxSelectGrid = 9;

    enum CardinalDirections
    {
        Top,
        Down,
        Left,
        Right
    }




    // Start is called before the first frame update
    void Start()
    {
       // GenerateGrid(gridSizeX,gridSizeY);
    }

    public void InitGridGenerator(int maximumGrids)
    {
        //Instantiate maximum amount of images possible

        allGrid = new BlockScript[maximumGrids];

        for (int i = 0; i < maximumGrids; i++)
        {
             allGrid[i] = Instantiate(blockPrefab, transform);
        }
    }

    public void DisableAllGrids()
    {
        if (enabledGrid == null) return;

        for (int i = 0; i < enabledGrid.GetLength(0); i++)
        {
            for (int j = 0; j < enabledGrid.GetLength(1); j++)
            {
                enabledGrid[i, j].DisableBlock();
                enabledGrid[i, j].gameObject.SetActive(false);
            }
        }
    }

    public void GenerateGrid(int gridSizeX, int gridSizeY)
    {
        Debug.Log("x :" + gridSizeX+" y :" + gridSizeY);
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;

        grid = new int[gridSizeX, gridSizeY];

        //position it at the center of the screen, just in case0Screen.height / 2);

        panel.sizeDelta = GetGridScale() * 10f;

        gridLayout.cellSize = Vector2.one * (blockSize * 10);

        gridLayout.spacing = Vector2.one * (blockgap * 10);

        gridLayout.padding = new RectOffset(10, 0, 10, 0);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        gridLayout.constraintCount = gridSizeX;

        enabledGrid = new BlockScript[gridSizeX, gridSizeY];

        int count = 0;

        for (int i = 0; i < enabledGrid.GetLength(0); i++)
        {
            for (int j = 0; j < enabledGrid.GetLength(1); j++)
            {
                //allGrid[i].gameObject.SetActive(true);
                enabledGrid[i, j] = allGrid[count];
                enabledGrid[i, j].gameObject.SetActive(true);
                ++count;
            }
        }

        ChooseSelectGrid();

    }

    private void ChooseSelectGrid()
    {
        int totalGrid = enabledGrid.Length;
        int selectPointsCount = totalGrid / 2;

        selectPointsCount = selectPointsCount > maxSelectGrid ? maxSelectGrid : selectPointsCount;

        int count = 0;
        int randomIndexX = 0;
        int randomIndexY = 0;

        int pathIncrementorX = 0;
        int pathIncrementorY = 0;


        Action selectGrid = null;


        List<Vector2> randomIndex = new List<Vector2>();

        Func<Vector2[], Vector2, bool> HasValue = delegate (Vector2[] selectedArray, Vector2 hasValue)
        {
            for (int i = 0; i < selectedArray.Length; i++)
            {
                if (selectedArray[i] == hasValue)
                {
                    return true;
                }
            }

            return false;
        };

        Vector2 indexToAdd = new Vector2();


        selectGrid = () =>
        {
            if (count == 0)
            {
                randomIndexX = UnityEngine.Random.Range(0, gridSizeX);
                randomIndexY = UnityEngine.Random.Range(0, gridSizeY);
                randomIndex.Add(new Vector2(randomIndexX, randomIndexY));
                enabledGrid[randomIndexX, randomIndexY].EnabledBlock();
                pathIncrementorX++;
            }
            else
            {

                CardinalDirections direction = (CardinalDirections)UnityEngine.Random.Range((float)CardinalDirections.Top, Enum.GetNames(typeof(CardinalDirections)).Length);

                if (direction == CardinalDirections.Top)
                {
                    if (randomIndex[0].y - 1 >= 0)
                    {
                        indexToAdd = new Vector2(randomIndex[0].x, randomIndex[0].y - 1);
                        if (!HasValue(randomIndex.ToArray(), indexToAdd))
                        {
                            enabledGrid[(int)indexToAdd.x, (int)indexToAdd.y].EnabledBlock();

                            randomIndex.Add(indexToAdd);
                        }
                    }
                }
                else if (direction == CardinalDirections.Down)
                {
                    if (randomIndex[0].y + 1 <= gridSizeY)
                    {

                        indexToAdd = new Vector2(randomIndex[0].x, randomIndex[0].y + 1);


                        if (!HasValue(randomIndex.ToArray(), indexToAdd))
                        {
                            enabledGrid[(int)indexToAdd.x, (int)indexToAdd.y].EnabledBlock();

                            randomIndex.Add(indexToAdd);
                        }
                    }
                }
                else if (direction == CardinalDirections.Left)
                {
                    if (randomIndex[0].x - 1 >= 0)
                    {
                        indexToAdd = new Vector2(randomIndex[0].x - 1, randomIndex[0].y);

                        if (!HasValue(randomIndex.ToArray(), indexToAdd))
                        {
                            enabledGrid[(int)indexToAdd.x, (int)indexToAdd.y].EnabledBlock();

                            randomIndex.Add(indexToAdd);
                        }
                        return;
                    }
                }
                else if (direction == CardinalDirections.Right)
                {
                    if (randomIndex[0].x + 1 <= gridSizeX)
                    {
                        indexToAdd = new Vector2(randomIndex[0].x + 1, randomIndex[0].y);

                        if (!HasValue(randomIndex.ToArray(), indexToAdd))
                        {
                            enabledGrid[(int)indexToAdd.x, (int)indexToAdd.y].EnabledBlock();

                            randomIndex.Add(indexToAdd);
                        }
                    }
                }
            }

            if (randomIndex.Count != selectPointsCount)
            {
                selectGrid();
            }
        };
        
        selectGrid();


        
    }

    




  
    
    private Vector2 GetGridScale()
    {
        return new((blockSize + blockgap) * gridSizeX, (blockSize + blockgap) * gridSizeY);
    }

    private Vector2 FirstPoint()
    {
        float y = GetGridScale().y / 2;
        float x = (GetGridScale().x / 2)-y;

        return new Vector2(x,y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
