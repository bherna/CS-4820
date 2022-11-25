using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;



public class computeDisparity: MonoBehaviour
{


    //variable to hold what the frame rate of the simulator
    public float frame_rate = 60;
    //variable to know what the distance is between the two cameras
    public float B;
    //variable to know what the cameras focal lense length is
    public float F;
    //variable to know what the camera feild of view is in the horizontal plane
    public float alpha;


    //keeps track of the current captured photos from the camera
    public String cameraLeft; //files
    public string cameraRight;


    private Texture2D imageLeft; //image objects
    private Texture2D imageRight;


    //variable that keeps track of the number of cameras that finished photo taking
    int numberOfPhotos = 0;

    //function to wait untill both cameras take a photo
    public void doDisparity()
    {
        //increment first, then check if we are there 
        numberOfPhotos++;
        Debug.Log("Number of Photos: " + numberOfPhotos.ToString());

        if (numberOfPhotos >= 2)
        {
            Debug.Log("HSV");

            //actually run
            computeHSV();

            //and rest the variable
            numberOfPhotos = 0;
        }
    }


    //function to set the current captured photos
    private void setCameraPhotos()
    {
        //left image update
        imageLeft = new Texture2D(2, 2); //new texture object
        //imageLeft.LoadImage(cameraLeft); //get from file

        //right image update
        imageRight = new Texture2D(2, 2); //new texture object
        //imageRight.LoadImage(cameraRight); //get from file

    }


    //computes a hsv-filter on the camera photos
    //this is to compare to a sobel filter
    private void computeHSV()
    {
        //compute image left hsv******
        //first blur the frame
        gausianBlur blurLeft = new gausianBlur(cameraLeft.ToString());
        blurLeft.Process(2).Save(Application.dataPath + "/Backgrounds/" + "cameraLeft-blur.jpg"); 

    }


    //function that applies a shape recognition on the hsv filtered photo
    private void getRecog()
    {

    }


    //function that calculates the depth of an object
    private void computeDepth()
    {

    }




}


