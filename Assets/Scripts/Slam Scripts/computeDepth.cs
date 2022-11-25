namespace OpenCvSharp.Demo
{

    using UnityEngine;
    using System.Collections;
    using OpenCvSharp;
    using UnityEngine.UI;

    public class computeDepth : MonoBehaviour
    {
        public Texture2D texture;

        // Start is called before the first frame update
        void Start()
        {
            Mat mat = Unity.TextureToMat(this.texture);
            Mat grayMat = new Mat();
            Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);
            Texture2D texture = Unity.MatToTexture(grayMat);

            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateFrame(Texture2D updateTexture)
        {
            Debug.Log("update frame texture");
            Mat mat = Unity.TextureToMat(updateTexture);
            Mat grayMat = new Mat();
            Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);
            Texture2D texture = Unity.MatToTexture(grayMat);

            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;
        }
    }
}