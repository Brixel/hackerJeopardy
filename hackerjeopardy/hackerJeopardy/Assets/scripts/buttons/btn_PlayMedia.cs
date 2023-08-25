using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using TMPro;

public class btn_PlayMedia : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
            if(GameObject.Find("keyScript").GetComponent<keylistener_questions>().answerRevealed == false)
            {
                GameObject.Find("qVideo").GetComponent<VideoPlayer>().Play();
            }
            else
            {
                GameObject.Find("aVideo").GetComponent<VideoPlayer>().Play();
            }
            
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

    public void hideButton()
    {
        transform.GetComponent<Image>().color = new Color(transform.GetComponent<Image>().color.r, transform.GetComponent<Image>().color.g, transform.GetComponent<Image>().color.b, 0);
        transform.Find("pnl_icon").GetComponent<Image>().color = new Color(transform.Find("pnl_icon").GetComponent<Image>().color.r, transform.Find("pnl_icon").GetComponent<Image>().color.g, transform.Find("pnl_icon").GetComponent<Image>().color.b, 0);
        transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color.r, transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color.g, transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color.b, 0);
        isClicked = true;
    }

}