using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the linear movement of laser projectiles]
 */
public class Laser : MonoBehaviour
{
    // projectile variables
    public float speed;
    public bool goingLeft;


    // Update is called once per frame
    void Update()
    {
        if (goingLeft == true)
        {
            transform.position += speed * Vector3.left * Time.deltaTime;           
        }
        else
        {
            transform.position += speed * Vector3.right * Time.deltaTime;
        }
    }
}
