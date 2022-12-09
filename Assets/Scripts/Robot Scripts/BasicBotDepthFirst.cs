using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBotDepthFirst : MonoBehaviour
{

    //2d array of the maze in terms of wall statese
    WallState[,] maze;

    //this state holds if the bot should start moving or not
    bool startMove = false;

    //initial bot position
    //x,z position
    int[] position = {0,0};

    //scaled size of the maze
    float size = 0f;

    //cooldown durration for our bot till next move
    [SerializeField]
    int CooldownDurationInSec = 5;
    
    float lastMove = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set initial state
        transform.position = new Vector3(0,0,0);
    }


    //function to take in the statemap of the maze
    public void GetWallStates(WallState[,] newMaze, float newSize){

        //set the maze variable
        maze = newMaze;

        //maze scale
        size = newSize;

        //set our boolean to true, we can start moving now
        startMove = true;


    }



    // Update is called once per frame
    void Update()
    {

        //only move when we are ready
        if(startMove && Time.time >= lastMove+CooldownDurationInSec){

            //check which direction is safe to move i
            if(!maze[position[0],position[1]].HasFlag(WallState.UP)){
                Debug.Log("UP is allowed, "+ (!maze[0,0].HasFlag(WallState.UP)).ToString());
                //update bot position
                position[1] = position[1] + 1;
            }
            else if(!maze[position[0],position[1]].HasFlag(WallState.DOWN)){
                Debug.Log("DOWN is allowed");
                position[1] = position[1] - 1;
            }
            else if(!maze[position[0],position[1]].HasFlag(WallState.LEFT)){
                Debug.Log("LEFT is allowed");
                position[0] = position[0] - 1;
            }
            else if(!maze[position[0],position[1]].HasFlag(WallState.RIGHT)){
                Debug.Log("RIGHT is allowed");
                position[0] = position[0] + 1;
            }

            //new pos
            transform.position = new Vector3(position[0]*size, 0, position[1]*size);
            //countdown till next position check
            lastMove = Time.time;
        }
    }


  
}
