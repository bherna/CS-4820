using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

    //variables to control how big maze is
    [SerializeField]
    [Range(1,50)]
    private int width = 10;

    [SerializeField]
    [Range(1,50)]
    private int height = 10;

    //used for linking the wall prefab
    [SerializeField]
    private Transform wallPrefab = null;


    // Start is called before the first frame update
    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }


    private void Draw(WallState[,] maze)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
