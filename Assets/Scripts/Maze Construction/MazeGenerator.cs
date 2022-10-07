using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//these set the states
[Flags] //flags atribute  maze[i,j].HasFlag(WallState.RIGHT);
public enum WallState
{
    // 0000 -> no walls (all down)
    // 1111 -> Left, Right, UP, Down (walls standing)
    LEFT = 1,   //0001
    RIGHT = 2,  //0010
    UP = 4,     //0100
    DOWN = 8,   //1000

    VISITED = 128, // 1000 0000
}

//used to keep track of which node we are in the maze
public struct Position
{
    public int X;
    public int Y;
}

//position of the neighbor and their shared wall (of the current node)
public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}


public static class MazeGenerator
{

    //function that returns the opposite wall of the given wall
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }



    //recursive back-track algorithm
    //this function will return an acutall completed maze
    //video where i get this algorithm from
    //https://www.youtube.com/watch?v=ya1HyptE5uc 
    private static WallState[,] ApplyRecursiveBackTracker(WallState[,] maze, int width, int height)
    {

        //the recursive part
        
        //step one: pick a random position/direction
        var rng = new System.Random();
        var positionStack = new Stack<Position>();
        var position = new Position{X = rng.Next(0,width), Y = rng.Next(0,height)};

        //mark position in the maze
        maze[position.X, position.Y] |= WallState.VISITED; //1000 1111

        //add this position to the stack
        positionStack.Push(position);

        //itirate over the position stack until empty
        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbors = GetUnvisitedNeighbours(current, maze, width, height);

            //if the node we are currently on still has unvisited neightbours, we add currnt node to stack
            if(neighbors.Count > 0){
                //
                positionStack.Push(current);

                //randomly pick one of the unvisited neigbour nodes to enter to
                var randIndex = rng.Next(0,neighbors.Count);
                var randomNeighbour = neighbors[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall; //removes shared wall on current node
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall); //remove the wall from the neighbor node
                
                //mark the node as visited
                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;

                //push onto stack
                positionStack.Push(nPosition);

                //repeat
            }

        }


        return maze;
    }

    //function that returns the neighbors of a given node/position
    //that are unvisited
    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        //create a new list
        var list = new List<Neighbour>();

        //left check
        if(p.X > 0)
        {
            if(!maze[p.X -1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour 
                        {
                            Position = new Position
                            {
                                X = p.X -1,
                                Y = p.Y
                            },
                            SharedWall = WallState.LEFT
                        });
            }
        }

        //Down check
        if(p.Y > 0)
        {
            if(!maze[p.X, p.Y -1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour 
                        {
                            Position = new Position
                            {
                                X = p.X,
                                Y = p.Y -1
                            },
                            SharedWall = WallState.DOWN
                        });
            }
        }


        //Up check
        if(p.Y < height - 1)
        {
            if(!maze[p.X, p.Y +1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour 
                        {
                            Position = new Position
                            {
                                X = p.X,
                                Y = p.Y +1
                            },
                            SharedWall = WallState.UP
                        });
            }
        }


        //RIGHT check
        if(p.X < width -1)
        {
            if(!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour 
                        {
                            Position = new Position
                            {
                                X = p.X + 1,
                                Y = p.Y
                            },
                            SharedWall = WallState.RIGHT
                        });
            }
        }

        return list;

    }

    //this function will return a new init of a generated maze
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
        return ApplyRecursiveBackTracker(maze, width, height);

    }
}
