using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    public int myID;
    public void updateName(string name)
    {
        transform.Find("innerPanel").Find("Name").GetComponent<TextMeshProUGUI>().text = name;
    }
    public void updateScore(int score)
    {
        transform.Find("innerPanel").Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
    public void updateColor(Color32 theColor)
    {
        transform.Find("innerPanel").GetComponent<Image>().color = theColor;
    }
    public void goAway()
    {
        Destroy(this.gameObject);
    }
    public void ANSWERED()
    {
            //whoop! I answered first!
            transform.Find("innerPanel").GetComponent<Image>().color = new Color32(0, 255, 0, 255);
    }
    public void ANSWERS_OPEN()
    {
        //can answer
        transform.Find("innerPanel").GetComponent<Image>().color = new Color32(128, 128, 128, 255);
    }
    public void ANSWERS_CLOSED()
    {
        //can't answer
        transform.Find("innerPanel").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
    }

    private void Start()
    {
        Invoke("updateScore", 1f);
    }

    void updateScore()
    {
        updateScore(GameObject.Find("scriptHolder").GetComponent<gameSettings>().players[myID].playerScore);
        Invoke("updateScore", 1f);
    }
}
