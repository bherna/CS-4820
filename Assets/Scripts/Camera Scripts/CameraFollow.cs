using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //credit https://www.youtube.com/watch?v=MFQhpwc6cKE


    //reference to what to follow
    public Transform target;

    //used for camera smooth movement, (less teleporty movement)
    public float smoothSpeed = 0.125f;

    //how far for the camera to be from the origin of target
    public Vector3 offset;

    void LateUpdate()
    {
        //update camera orientation
        Vector3 desiredPosition = target.position + offset; 
        //lerp (origin, destination, from 0-1 where in between do we want to be from these two points)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //camera look at player
        transform.LookAt(target);

    }
}
