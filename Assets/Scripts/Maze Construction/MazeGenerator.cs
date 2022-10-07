using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Flags] //flags atribute  maze[i,j].HasFlag(WallState.RIGHT);
public enum WallState
{
    // 0000 -> no walls (all down)
    // 1111 -> Left, Right, UP, Down (walls standing)
    LEFT = 1,   //0001
    RIGHT = 2,  //0010
    UP = 4,     //0100
    DOWN = 8,   //1000
}



public static class MazeGenerator
{
    //this function will return a randomly generated maze
    //it takes in a width and height for the demensions of the maze
    public static WallState[,] Generate(int width, int height)
    {

        //create an empty maze
        WallState[,] maze = new WallState[width, height];

        //fill our empty maze with the initial state for the maze walls
        // i.e set to unexplored

        //the initla state for the walls
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

        //for loop through all nodes in the empty maze
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i,j] = initial; //1111
            }
        }

        //return the maze
        return maze;

    }
}
