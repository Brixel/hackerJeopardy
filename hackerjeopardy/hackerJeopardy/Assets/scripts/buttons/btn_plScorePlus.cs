using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_plScorePlus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    bool isClicked;
    public int plIndex;
    gameSettings GS;

    void Start()
    {
        isClicked = false;
        GS = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
    }

    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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

            //logic
            Debug.Log("adding score to pl index " + plIndex.ToString());
            GS.players[plIndex].playerScore = GS.players[plIndex].playerScore + 100;
            GS.updateCredits(plIndex, 100, true);
            transform.parent.Find("txt_playerScore").GetComponent<TextMeshProUGUI>().text = GS.players[plIndex].playerScore.ToString();
            Invoke("resetColor", 1f);
        }
    }

    void resetColor()
    {
        isClicked = false;
    }

}