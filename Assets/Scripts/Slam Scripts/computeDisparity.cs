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


    //hsv filtering saved variables
    private int _width;
    private int _height;
    private int[] _blue;
    private readonly ParallelOptions _pOptions = new ParallelOptions { MaxDegreeOfParallelism = 16 };


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


  


    //computes a hsv-filter on the camera photos
    //this is to compare to a sobel filter
    private void computeHSV()
    {
        //compute image left hsv******
        //first blur the frame
        gausianBlur blurLeft = new gausianBlur(cameraLeft.ToString());
        blurLeft.Process(2).Save(Application.dataPath + "/Backgrounds/" + "left-blur-0.jpg");

        gausianBlur blurRight = new gausianBlur(cameraLeft.ToString());
        blurRight.Process(2).Save(Application.dataPath + "/Backgrounds/" + "right-blur-0.jpg");




        //our hsv works by just finding anything blue, and setting that as the max color value, everything else is 0
        Bitmap image = new Bitmap(Application.dataPath + "/Backgrounds/" + "left-blur-0.jpg");
        var rct = new Rectangle(0, 0, image.Width, image.Height);
        var source = new int[rct.Width * rct.Height];
        var bits = image.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        Marshal.Copy(bits.Scan0, source, 0, source.Length);
        image.UnlockBits(bits);

        _width = image.Width;
        _height = image.Height;

        
        _blue = new int[_width * _height];

        Parallel.For(0, source.Length, _pOptions, i =>
        {
            _blue[i] = (source[i] & 0x0000ff);
        });

        var dest = new int[_width * _height];


        //for each pixel in the image
        //if not blue , set as 0
        //else 1

        Parallel.For(0, dest.Length, _pOptions, i =>
        {
            if (_blue[i] > 10)
            {
                dest[i] = 2147483647;
            }
            else
            {
                dest[i] = 0;
            }

        });


        var newImage = new Bitmap(_width, _height);
        var bits2 = newImage.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
        newImage.UnlockBits(bits2);

        newImage.Save(Application.dataPath + "/Backgrounds/" + "left-blur-1.jpg");


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


