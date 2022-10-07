using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    //credit https://www.youtube.com/watch?v=ELz_EG-s0jU&t=22s
    
    [SerializeField]
    private float _speed = 3;

    //reference to the object rigid body, for collision detection
    [SerializeField]
    private  Rigidbody _rb;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //robot movement
        //first get the direction vector we want to move in
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //then update our velocity =  direction * magnitude
        _rb.velocity = dir *  _speed;
    }
}
