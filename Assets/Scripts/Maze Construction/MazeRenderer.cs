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

    //again for the wall /\
    [SerializeField]
    private Transform floorPrefab = null;

    //used for determining the size of a node
    [SerializeField]
    private float size = 1f;

    //prefab for the bot
    [SerializeField]
    private GameObject botObject = null;

    //prefab for the goal
    [SerializeField]
    private Transform goalPrefab = null;

    //prefab for the goal
    [SerializeField]
    private Transform nodePrefab = null;




    // Start is called before the first frame update
    void Start()
    {
        //generate a random maze
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);

        //place goal object
        var goal = Instantiate(goalPrefab);
        //place at the opposite side of bot
        goal.position = new Vector3(size*width -size, 0, size*height -size);

        //place it at the start of maze
        var bot = Instantiate(botObject);
        //give the bot the maze matrix, (idea for wall detection)
        bot.GetComponent<BasicBotDepthFirst>().GetWallStates(maze, size);


    }


    //used for actually rendering the maze, given
    private void Draw(WallState[,] maze)
    {
        //create the floor first, under the bot's height
        var floor = Instantiate(floorPrefab, transform);
        floor.position = new Vector3(0,-botObject.transform.localScale.y / 2 ,0);
        floor.localScale = new Vector3(width, 1, height); //increase floor size

        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //first get the maze node we are dealing with
                var cell = maze[i,j];
                var position = new Vector3(i*size, 0, j*size); //puts us at the start of the maze, then adds the offset

                //create a node prefab that the basic bot can travel to
                var node = Instantiate(nodePrefab);
                node.position = position + new Vector3(0,-botObject.transform.localScale.y / 2,0);

                //now we draw the walls of the maze, draw every up and left wall conditionally
                //      we dont draw every wall, since they will click
                if(cell.HasFlag(WallState.UP)){
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0,0,size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if(cell.HasFlag(WallState.LEFT)){
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size/2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                //now if we are the last row, or column we are missing a wall
                //so draw the down and right ones
                //bottom row
                if(i == width -1){

                    if(cell.HasFlag(WallState.RIGHT)){
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size/2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                //
                if(j == 0){
                    if(cell.HasFlag(WallState.DOWN)){
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0,0,-size/2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
