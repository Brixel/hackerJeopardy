using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreFloat : MonoBehaviour
{
    Vector3 targetPos;
    bool isMoving = false;
    void Start()
    {
        targetPos = new Vector3(transform.position.x, transform.position.y + 10f, 1f);
        isMoving = true;
        Invoke("killMe", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5f);
        }
    }

    void killMe()
    {
        Destroy(this.gameObject);
    }
}
