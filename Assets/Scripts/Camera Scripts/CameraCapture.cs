 using System.IO;
 using UnityEngine;
 using System.Collections;
 using System;
 
 public class CameraCapture : MonoBehaviour
 {

    /*
    Things to make this script work:
        - need to attach this to a camera object
        - on the camera object, it needs to have a target texture attached to it
        - set the public variables    
    
    */
     public RenderTexture overviewTexture;
     GameObject OVcamera;
     public int fileCounter = 0;
     public string cameraAngle = "";
 

     void Start()
     {
         OVcamera = gameObject;
     }
 
     void LateUpdate()
     {           
         if (Input.GetKeyDown("f9"))
         {
             StartCoroutine(TakeScreenShot());
         }    
     }
 
     // return file name
     string fileName(int width, int height)
     {
        return string.Format("screen_{0}x{1}_{2}.png",
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }
 
     public IEnumerator TakeScreenShot()
     {
        yield return new WaitForEndOfFrame();

        Camera camOV = OVcamera.GetComponent<Camera>();  
        RenderTexture currentRT = RenderTexture.active;    
        RenderTexture.active = camOV.targetTexture;
        camOV.Render();
        Texture2D imageOverview = new Texture2D(camOV.targetTexture.width, camOV.targetTexture.height, TextureFormat.RGB24, false);
        imageOverview.ReadPixels(new Rect(0, 0, camOV.targetTexture.width, camOV.targetTexture.height), 0, 0);
        imageOverview.Apply();
        RenderTexture.active = currentRT;    
        
        //save copy in color
        saveImage(imageOverview);

        //convert to greyscale
        imageOverview = convertToGrey(imageOverview);

        //save copy in greyscale
        saveImage(imageOverview);


     }



    //save image to computer
    private void saveImage(Texture2D imageOverview){

        byte[] bytes = imageOverview.EncodeToJPG();
        //Destroy(imageOverview);
        String filename = cameraAngle + fileCounter + ".jpg";
        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + filename, bytes);
        fileCounter++;
    }



    //convet texture to greyscale
    private Texture2D convertToGrey(Texture2D graph){
        
        Texture2D grayImg;

        //convert texture
        grayImg = new Texture2D(graph.width, graph.height, graph.format, false);
        Graphics.CopyTexture(graph, grayImg);
        Color32[] pixels = grayImg.GetPixels32();
        Color32[] changedPixels = new Color32[grayImg.width*grayImg.height];
    
        for (int x = 0; x < grayImg.width; x++)
        {
            for (int y = 0; y < grayImg.height; y++)
            {
                Color32 pixel = pixels[x + y * grayImg.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                Color c = new Color(l, l, l, 1);
                changedPixels[x + y * grayImg.width] = c;
            }
        }
        grayImg.SetPixels32(changedPixels);
        grayImg.Apply(false);
        return grayImg;
    }






 }