namespace OpenCvSharp.Demo
{

    using UnityEngine;
    using System.Collections;
    using OpenCvSharp;
    using UnityEngine.UI;

    public class computeDepth : MonoBehaviour
    {

        //variables
        public Texture2D texture;


        // Start is called before the first frame update
        void Start()
        {

            //update frame
            Mat mat = Unity.TextureToMat(this.texture);
            Mat grayMat = new Mat();
            Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);
            Texture2D texture = Unity.MatToTexture(grayMat);

            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;

        }


        public void UpdateFrame(Texture2D updateTexture)
        {
            Debug.Log("update frame texture - HSV filter");
            Mat mat = Unity.TextureToMat(updateTexture);

            Mat depthMap = getDisparity(mat);

            Texture2D texture = Unity.MatToTexture(depthMap);

            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;
        }

        private Mat getDisparity(Mat inputFrame)
        {
            //make a copy of the frame, and apply hsv filter on it
            Mat mask = add_hsv(inputFrame);
            
            //now bitwise and the frame, to get an outline
            Mat result = new Mat();
            Cv2.BitwiseAnd(inputFrame, inputFrame, result, mask);

            //apply shape recognition, find any rectanlges on screen 
            //Point[] rect_frame = find_Rectangles(inputFrame, mask);  //doesnt work

            return result;

        }

        
        //rectangle filter finder
        //takes in the input frame and mask from hsv, to find all possible rectangles
        private Point[] find_Rectangles(Mat inputFrame, Mat mask){

            Point[] center = new Point[2]; 
            //first we find all contours in the frame
/*            
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            

            //did we find any red conyours
            if (contours.Length > 0){

            }
  */          
            //return 
            return center;
        }



        //hsv filtering function
        //takes in an input frame, and pulls out all the pixels that are red
        private Mat add_hsv(Mat inputFrame)
        {

            //blur the frame to remove noise first
            Mat blur = new Mat();
            Cv2.GaussianBlur(inputFrame, blur, new Size (5,5), 0);

            //hsv filter
            Mat hsv = new Mat();
            Cv2.CvtColor(blur, hsv, ColorConversionCodes.RGB2HSV);

            //levels at which we track
            //detect for color red
            Scalar l_b_r = new Scalar(100, 0, 0);
            Scalar u_b_r = new Scalar(255, 255, 255);

            //hsv-filter mask
            Mat mask = new Mat();
            Cv2.InRange(hsv, l_b_r, u_b_r, mask);
            
            //remove more noise from the hsv filter
            Cv2.Erode(mask, mask, new Mat(), new Point(-1,1), 2);
            Cv2.Dilate(mask, mask, new Mat(), new Point(-1,1), 2);

            return mask;
        }
    }
}