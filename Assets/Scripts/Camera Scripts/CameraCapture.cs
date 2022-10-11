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
        
        // Encode texture into PNG
        byte[] bytes = imageOverview.EncodeToJPG();
        Destroy(imageOverview);

        // save in memory
        //make sure the path exsits first
        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + cameraAngle + fileCounter + ".jpg", bytes);
        fileCounter++;
     }
 }