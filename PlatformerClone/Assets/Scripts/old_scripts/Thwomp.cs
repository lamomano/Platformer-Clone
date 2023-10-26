using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the movement of the thwomp objects.]
 */
public class Thwomp : MonoBehaviour
{
    //game objects to determine how far top/bottom the Thwomp will go
    public GameObject topPoint;
    public GameObject bottomPoint;


    //boundary points for top/bottom
    private Vector3 topPos;
    private Vector3 bottomPos;

    //side to side movement speed
    private int speed = 4;

    //direction the Thwomp is going-up
    public bool goingUp;
    //check to see if the Thwomp is waiting
    public bool waiting;






    // Start is called before the first frame update
    void Start()
    {
        topPos = topPoint.transform.position;
        bottomPos = bottomPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ThwompMovement();
    }




    /// <summary>
    /// rests the thwomp at the bottom for 2 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToGoUp()
    {
        //Start the wait timer
        yield return new WaitForSeconds(2f);
        //After 2 seconds, now thwomp is going up and waiting is false
        goingUp = true;
        waiting = false;
    }






    /// <summary>
    /// makes the thwomp move up and down
    /// </summary>
    private void ThwompMovement()
    {
        if (goingUp)
        {
            //once the Thwomp reaches the topPos - goingUp is false
            if (transform.position.y >= topPos.y)
            {
                goingUp = false;
            }
            else
            {
                //translate the Thwomp up by speed using Time.deltaTime
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
        }
        else
        {
            if (transform.position.y <= bottomPos.y)
            {
                if (!waiting)
                {
                    waiting = true;
                    StartCoroutine(WaitToGoUp());
                }
            }
            else
            {
                //translate the Thwomp down by speed using Time.deltaTime
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
        }
    }

    

}
