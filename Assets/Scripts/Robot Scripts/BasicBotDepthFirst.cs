using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BasicBotDepthFirst : MonoBehaviour
{

    //2d array of the maze in terms of wall statese
    WallState[,] maze;

    //2d array for holding information at each possible node in the maze
    private class MazeCell{

        //variables
        //has this cell been visted
        bool visited = false;

        //vairble to know if this is already a parent cell or not
        bool isParent = false;

        //this cells children
        //children can only be in the increasing position/
        //so pos(0,0) will either have children (1,0) or (0,1)
        //children in up
        bool childUP = false;
        //child right
        bool childRight = false;
        //child left
        bool childLeft = false;
        //child down
        bool childDOWN = false;

        //keeps track of what cell this is an offshoot of
        //0 = no parent (only the root node)
        //1 = parent is below
        //2 = parent is right
        //3 = parent is left
        //4 = parent is above
        int parent;

        //constructor
        //child up and child right 
        public MazeCell(int parentDir){

            //parent side
            parent = parentDir;
        }

        //set the children nodes
        public void ChildUp_set(){
            childUP = true;
        }
        public void ChildRight_set(){
            childRight = true;
        }
        public void ChildLeft_set(){
            childLeft = true;
        }
        public void ChildDOWN_set(){
            childDOWN = true;
        }



        public bool Visited(){ //get
            return visited;
        }
        public void NowVisited(){ //set
            visited = true;
        }

        //read
        public bool ChildUP(){
            return childUP;
        }
        public bool ChildRight(){
            return childRight;
        }
        public bool ChildLeft(){
            return childLeft;
        }
        public bool ChildDOWN(){
            return childDOWN;
        }

        //get the parent direction
        public int Parent(){
            return parent;
        }


        //get if this is a parent of not
        public bool IsParent(){
            return isParent;
        }
        //now is parent
        public void NowParent(){
            isParent = true;
        }
    }

    MazeCell[,] mazeCells;

    //this state holds if the bot should start moving or not
    bool startMove = false;

    //initial bot position
    //x,z position
    int[] position = {0,0};

    //scaled size of the maze
    float size = 0f;


    //depth first seach stack
    //list of the child nodes, we should head to next
    public class Pos{

        //our i and j coor
        int i = 0;
        int j = 0;

        //init
        public Pos(int i, int j){
            this.i = i;
            this.j = j;
        }

        //
        public int GetI(){
            return i;
        }
        public int GetJ(){
            return j;
        }
    }

    List<Pos> stack = new List<Pos>();

    //cooldown durration for our bot till next move
    [SerializeField]
    float CooldownDurationInSec = 5;
    
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

        //initialize our maze info array
        initMazeInfo();


    }

    //inits the maze info array with mazeCells
    private void initMazeInfo(){

        //init maze cells array
        mazeCells = new MazeCell[maze.GetLength(0), maze.GetLength(1)];

        //just do the root node, then recurse the rest
        int i = 0;
        int j = 0;
        //paretn is 0, or in this case, root
        mazeCells[i,j] = new MazeCell(0);

        //is now a parent
        mazeCells[i,j].NowParent();


        //parent direction (non right now)
        int parent = 0;

        //assume this is root, what are the childs of this cell

        //does up child exsist
        if(!maze[i,j].HasFlag(WallState.UP)){

            //that means up child exsists
            //set current node with child now
            mazeCells[i,j].ChildUp_set();

            //parent comes from below then
            parent = 1;

            //init the child node
            //set the current node (i,j) as parent node
            mazeCells[i,j+1] = new MazeCell(parent);
            recursivePart(i,j+1);
        }
        //does down child exsist
        if(!maze[i,j].HasFlag(WallState.DOWN)){

            //that means up child exsists
            //set current node with child now
            mazeCells[i,j].ChildDOWN_set();

            //parent comes from above then
            parent = 4;

            //init the child node
            //set the current node (i,j) as parent node
            mazeCells[i,j-1] = new MazeCell(parent);
            recursivePart(i,j-1);
        }
        //does right child exsist
        if(!maze[i,j].HasFlag(WallState.RIGHT)){

            //that means right child exsists
            //set current node with child now
            mazeCells[i,j].ChildRight_set();

            //parent comes from left (backwards) then
            parent = 3;

            //init the child node
            //set the current node (i,j) as parent node
            mazeCells[i+1,j] = new MazeCell(parent);
            recursivePart(i+1,j);
        }
        //does left child exsist
        if(!maze[i,j].HasFlag(WallState.LEFT)){

            //that means left child exsists
            //set current node with child now
            mazeCells[i,j].ChildLeft_set();

            //parent comes from right (backwards) then
            parent = 2;

            //init the child node
            //set the current node (i,j) as parent node
            mazeCells[i-1,j] = new MazeCell(parent);
            recursivePart(i-1,j);
        }


            

    }


    //function that finishes initing the rest of the maze nodes
    void recursivePart(int i, int j){

        //
        //parent direction (non right now)
        int parent = 0;

        //now assume this is child node, what are the childs of this cell

        //this node is now a parent
        mazeCells[i,j].NowParent();

        //does up child exsist
        if(!maze[i,j].HasFlag(WallState.UP)){

            //is this 'child' node, a child?
            try{
                if(!mazeCells[i,j+1].IsParent()){

                    //then parent, dont do nothing
                }
            }
            catch(Exception e){
                //that mean maze cell child dont exist

                //that means up child exsists
                //set current node with child now
                mazeCells[i,j].ChildUp_set();

                //parent comes from below then
                parent = 1;

                //init the child node
                //set the current node (i,j) as parent node
                mazeCells[i,j+1] = new MazeCell(parent);
                recursivePart(i,j+1);

            }
            
            
        }
        //does down child exsist
        if(!maze[i,j].HasFlag(WallState.DOWN)){

            //is this 'child' node, a child?
            try{
                if(!mazeCells[i,j-1].IsParent()){
                    //then parent, do nothing
                }
            }
            catch(Exception e){
                //that mean maze cell child dont exist

                //that means up child exsists
                //set current node with child now
                mazeCells[i,j].ChildDOWN_set();

                //parent comes from above then
                parent = 4;

                //init the child node
                //set the current node (i,j) as parent node
                mazeCells[i,j-1] = new MazeCell(parent);
                recursivePart(i,j-1);
            }

            
        }
        //does right child exsist
        if(!maze[i,j].HasFlag(WallState.RIGHT)){

            //is this 'child' node, a child?
            try{
                if(!mazeCells[i+1,j].IsParent()){
                    //then parent, do nthing
                }
            }
            catch(Exception e){
                //then child

                //that means right child exsists
                //set current node with child now
                mazeCells[i,j].ChildRight_set();

                //parent comes from left (backwards) then
                parent = 3;

                //init the child node
                //set the current node (i,j) as parent node
                mazeCells[i+1,j] = new MazeCell(parent);
                recursivePart(i+1,j);
            }

            
        }
        //does left child exsist
        if(!maze[i,j].HasFlag(WallState.LEFT)){

            //is this 'child' node, a child?
            try{
                if(!mazeCells[i-1,j].IsParent()){
                    //then parent, donn't do nthing
                }
            }
            catch(Exception e){
                //then child

                //that means left child exsists
                //set current node with child now
                mazeCells[i,j].ChildLeft_set();

                //parent comes from right (backwards) then
                parent = 2;

                //init the child node
                //set the current node (i,j) as parent node
                mazeCells[i-1,j] = new MazeCell(parent);
                recursivePart(i-1,j);
            }

            
        }
    }



    // Update is called once per frame
    void Update()
    {

        

        //only move when we are ready
        if(startMove && Time.time >= lastMove+CooldownDurationInSec){

            //right
            if(!maze[position[0],position[1]].HasFlag(WallState.RIGHT) ){
                if( !mazeCells[position[0]+1,position[1]].Visited()){
                    Debug.Log("RIGHT is allowed");
                    stack.Insert(0, new Pos(position[0]+1, position[1]));
                }
                
            }
            //down
            if(!maze[position[0],position[1]].HasFlag(WallState.DOWN) ){
                if( !mazeCells[position[0],position[1]-1].Visited()){
                    Debug.Log("DOWN is allowed");
                    stack.Insert(0, new Pos(position[0], position[1]-1));
                }
                
            }
            //left
            if(!maze[position[0],position[1]].HasFlag(WallState.LEFT) ){
                if( !mazeCells[position[0]-1,position[1]].Visited()){
                    Debug.Log("LEFT is allowed");
                    stack.Insert(0, new Pos(position[0]-1, position[1]));
                }
                
            }
            //up
            if(!maze[position[0],position[1]].HasFlag(WallState.UP) ){
                if( !mazeCells[position[0],position[1]+1].Visited()){
                    Debug.Log("UP is allowed, "+ (!maze[0,0].HasFlag(WallState.UP)).ToString());
                    stack.Insert(0, new Pos(position[0], position[1]+1));
                }
                
            }
            
            
            //now we take one from the stack
            Debug.Log("list lenght: "+stack.Count.ToString());
            position[0] = stack.ElementAt(0).GetI();
            position[1] = stack.ElementAt(0).GetJ();
            stack.RemoveAt(0);

            
            //for returning to old nodes
            /*
            //else parent cel
            else{

                //from right
                if(mazeCells[position[0],position[1]].Parent() == 2){
                    Debug.Log("RIGHT is BACK");
                    position[0] = position[0] + 1;
                }
                //return to parent node
                //from bellow
                else if(mazeCells[position[0],position[1]].Parent() == 1){
                    Debug.Log("DOWN is BACK");
                    position[1] = position[1] - 1;
                }
                //from left
                else if(mazeCells[position[0],position[1]].Parent() == 3){
                    Debug.Log("LEFT is BACK");
                    position[0] = position[0] - 1;
                }
                //from above
                else if(mazeCells[position[0],position[1]].Parent() == 4){
                    Debug.Log("UP is BACK");
                    position[1] = position[1] + 1;
                }

                
            }
            */
            //new pos
            transform.position = new Vector3(position[0]*size, 0, position[1]*size);
            //visit this node
            mazeCells[position[0], position[1]].NowVisited();
            //countdown till next position check
            lastMove = Time.time;
        }
    }

    //this checks to see if any of the child nodes, in this direction
    //are not visited
    //0 = up
    //1 = right
    private bool shouldIVisit(int i, int j, int child){

        //does the current node even exsist
        try{
           //first off, is this node not visited
            if(!mazeCells[i,j].Visited()){
                return true; //we should visit
            }
        }
        catch(Exception e){

            //ignore message
            Debug.Log("no error, just root node check, "+ e);

            //return
            return false;

        }


        //if up
        if(child == 0){
            //does up child exist
            if(mazeCells[i,j].ChildUP()){
                if( shouldIVisit(i, j+1, 0) | shouldIVisit(i+1, j, 1) | shouldIVisit(i-1, j, 2)){
                    return true;
                }
            }
        }
        //right
        else if( child == 1){
            //does child exsist
            if(mazeCells[i,j].ChildRight()){
                if( shouldIVisit(i, j+1, 0) | shouldIVisit(i+1, j, 1) | shouldIVisit(i, j-1, 0)){
                    return true;
                }
            }
            
        }
        //left
        else if( child == 2){
            //does child exsist
            if(mazeCells[i,j].ChildLeft()){
                if( shouldIVisit(i, j+1, 0) | shouldIVisit(i-1, j, 2) | shouldIVisit(i, j-1, 0)){
                    return true;
                }
            }
            
        }
        //down
        else if(child == 4){
            //does up child exist
            if(mazeCells[i,j].ChildDOWN()){
                if( shouldIVisit(i+1, j, 1) | shouldIVisit(i-1, j, 2) | shouldIVisit(i, j-1, 0) ){
                    return true;
                }
            }
        }

        //no more to visit here
        return false;
    }


  
}

 /*
 //old position update
            //check which direction is safe to move i
            if(!maze[position[0],position[1]].HasFlag(WallState.UP)){
                Debug.Log("UP is allowed, "+ (!maze[0,0].HasFlag(WallState.UP)).ToString());
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





            //update bot position
            //child right cell
            if(shouldIVisit(position[0], position[1], 1)){
                Debug.Log("RIGHT is allowed, " + (!maze[position[0], position[1]].HasFlag(WallState.RIGHT)).ToString());
                stack.Insert(0, new Pos(position[0]+1, position[1]));
                //position[0] = position[0] + 1;
            }
            //chid down cell
            if(shouldIVisit(position[0], position[1], 3)){
                Debug.Log("DOWN is allowed, "+ (!maze[position[0], position[1]].HasFlag(WallState.DOWN)).ToString());
                stack.Insert(0, new Pos(position[0], position[1]-1));
                //position[1] = position[1] - 1;
            }
            //child left cell
            if(shouldIVisit(position[0], position[1], 2)){
                Debug.Log("LEFT is allowed, " + (!maze[position[0], position[1]].HasFlag(WallState.LEFT)).ToString());
                stack.Insert(0, new Pos(position[0]-1, position[1]));
                //position[0] = position[0] - 1;
            }
            //child up cell
            if(shouldIVisit(position[0], position[1], 0)){
                Debug.Log("UP is allowed, "+ (!maze[position[0], position[1]].HasFlag(WallState.UP)).ToString());
                stack.Insert(0, new Pos(position[0], position[1]+1));
                //position[1] = position[1] + 1;
            }
            */