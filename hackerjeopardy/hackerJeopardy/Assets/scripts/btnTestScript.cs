using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class btnTestScript : MonoBehaviour
{
    public int playerNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && playerNumber == 1)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F2) && playerNumber == 2)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F3) && playerNumber == 3)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F4) && playerNumber == 4)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F5) && playerNumber == 5)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F6) && playerNumber == 6)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F7) && playerNumber == 7)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F8) && playerNumber == 8)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F9) && playerNumber == 9)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
        if (Input.GetKeyDown(KeyCode.F10) && playerNumber == 10)
        {
            GetComponent<Image>().color = Color.green;
            Invoke("resetColor", 1f);
        }
    }

    void resetColor()
    {
        GetComponent<Image>().color = Color.red;
    }

    public void goAway()
    {
        Destroy(this.gameObject);
    }
}
