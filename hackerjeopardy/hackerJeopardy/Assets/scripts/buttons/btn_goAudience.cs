using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class btn_goAudience : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{

    Color originalColor;
    bool isClicked;

    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a / 2);
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
            transform.Find("pnl_icon").GetComponent<Image>().color = new Color(0, 255, 0, 255);
            transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);

            //logic

            //play timeout sound
            GameObject.Find("scriptHolder").GetComponent<soundScript>().playTimeOut();

            //stop incomming answers
            GameObject.Find("keyScript").GetComponent<keylistener_questions>().canAnswer = false;

            //stop music
            GameObject.Find("scriptHolder").GetComponent<soundScript>().stopMusic();


            //show operator panel
            GameObject.Find("pnl_audience").GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);// left, bottom
            GameObject.Find("pnl_audience").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);  // right,top

            //set answer
            GameObject.Find("tmp_audPlAnswerCorrectAnswer").GetComponent<TextMeshProUGUI>().text = GameObject.Find("txt_aTxt").GetComponent<TextMeshProUGUI>().text;

            Invoke("resetColor", 1f);
        }
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("pnl_icon").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}
