using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int minGridWidth;
    [SerializeField] private int maxGridWidth;
    [SerializeField] private int minGridHeight;
    [SerializeField] private int maxGridHeight;


    [Header("References")]
    [SerializeField] private GridGenerator gridGenerator;

    public void Start()
    {
        gridGenerator.InitGridGenerator(maxGridWidth * maxGridHeight);
    }

    public void GenerateRandomGrid()
    {

        gridGenerator.DisableAllGrids();
        gridGenerator.GenerateGrid(Random.Range(0, 2) == 0 ? minGridWidth : maxGridWidth, Random.Range(minGridHeight, maxGridHeight+1));

    }
}
