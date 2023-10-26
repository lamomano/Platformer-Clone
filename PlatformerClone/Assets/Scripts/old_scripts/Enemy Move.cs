using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the movement of enemy cubes.]
 */
public class EnemyMove : MonoBehaviour
{

    public GameObject leftPoint;
    public GameObject rightPoint;
    public Vector3 leftPos;
    public Vector3 rightPos;
    public int speed;
    public bool goingLeft;



    // Start is called before the first frame update
    void Start()
    {
        leftPos = leftPoint.transform.position;
        rightPos = rightPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }



    /// <summary>
    /// moves the enemy cube left or right.
    /// </summary>
    public void Move()
    { 
        if (goingLeft)
        {
            if (transform.position.x <= leftPos.x) 
            { 
                goingLeft = false;
            }
            else
            {
                transform.position += Vector3.left * Time.deltaTime * speed;
            }
        }
        else
        {
            if (transform.position.x >= rightPos.x) 
            {
                goingLeft = true;
            }
            else
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }
        }
    }




}
