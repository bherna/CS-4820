using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchWall : MonoBehaviour
{

    //visited material
    [SerializeField]
    public Material visitedMat = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //when in collision with walls, turn them green, as in visited
    void OnCollisionEnter(Collision collision){

        //collision.gameObject.GetComponent<Renderer>().material.color = visitedMat.color;
    }
}
