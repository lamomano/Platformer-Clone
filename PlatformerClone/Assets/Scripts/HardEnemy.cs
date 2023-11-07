using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemy : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;
    public Transform objectToFollow;
    public int contactDamage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "HardEnemy")
        {
            transform.position = objectToFollow.position;
        }
    }
}
