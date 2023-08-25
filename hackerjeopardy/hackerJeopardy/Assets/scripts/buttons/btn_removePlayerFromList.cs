using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_removePlayerFromList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color(128, 0,0,255);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked == false)
        {
            transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clickLogic();

    }
    public void OnPointerDown(PointerEventData data)
    {
        clickLogic();
    }
    void clickLogic()
    {
        if (isClicked == false)
        {
            isClicked = true;
            transform.GetComponent<Image>().color = new Color(0, 255, 0, 255);

            //logic
            Player toRemove = new Player();
            foreach (Player thisPlayer in GameObject.Find("scriptHolder").GetComponent<gameSettings>().players)
            {
                if (thisPlayer.playerName == transform.parent.Find("hiddenPlayerName").GetComponent<TextMeshProUGUI>().text)
                {
                    toRemove = thisPlayer;
                }
            }
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().players.Remove(toRemove);

            //render new list
            GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().renderPlayers();

            //remove window
            GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().hideAddPlayerWindow();
        }
    }
    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("pnl_removePlayer").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}
