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

        Mat low_blue;
        Mat up_blue;

        // Start is called before the first frame update
        void Start()
        {
            //init range matrixes
            low_blue = new Mat();
            up_blue = new Mat();

            low_blue.SetTo(Scalar.low_blue);
            up_blue.SetTo(Scalar.up_blue);

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
            Debug.Log("update frame texture");
            Mat mat = Unity.TextureToMat(updateTexture);

            Mat depthMap = getDisparity(mat);
            //Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);

            Texture2D texture = Unity.MatToTexture(depthMap);

            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;
        }

        private Mat getDisparity(Mat inputFrame)
        {
            return add_hsv(inputFrame);

        }

        private Mat add_hsv(Mat inputFrame)
        {

            //blur
            Mat blur = new Mat();
            Cv2.GaussianBlur(inputFrame, blur, Size.Zero, 1.84, 1.84);

            //hsv filter
            Mat hsv = new Mat();
            Cv2.CvtColor(blur, hsv, ColorConversionCodes.BGR2HSV);

            return hsv;

            //hsv-filter mask
            Mat mask = new Mat();
            Cv2.InRange(hsv, low_blue, up_blue, mask);

            return mask;
        }
    }
}