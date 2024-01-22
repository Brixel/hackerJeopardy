using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_showNextCat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    gameSettings gs;
    int catShown;
    int qShown;

    public Sprite boardSprite;
    

    void Start()
    {
        //find game settings
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();

        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;

        catShown = 0;
        qShown = 0;
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
            if(catShown == 0)
            {
                //stop all washers
                foreach (GameObject box in GameObject.Find("boardScripts").GetComponent<questionLoader>().allBoxes)
                {
                    box.GetComponent<boxScript>().stopWasher();
                }               
            }
            
            if (catShown < gs.categoryList.Count)
            {
                //update text field
                GameObject.Find("txt_catNamePresenting").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catShown].categoryName;

                //update title
                GameObject.Find("cat_Title").GetComponent<TextMeshProUGUI>().text = "Present categories (" + (catShown + 1).ToString() + "/" + gs.categoryList.Count + ")";

                //update hint:
                GameObject.Find("txt_catHint").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catShown].categoryHint;                

                //update values
                GameObject.Find("txt_values").GetComponent<TextMeshProUGUI>().text = "";
                foreach(Question thisQ in gs.categoryList[catShown].questions)
                {
                    GameObject.Find("txt_values").GetComponent<TextMeshProUGUI>().text = GameObject.Find("txt_values").GetComponent<TextMeshProUGUI>().text + "[" + thisQ.value + "] ";
                }

                //find box and show it.
                foreach (GameObject box in GameObject.Find("boardScripts").GetComponent<questionLoader>().allBoxes)
                {
                    if(box.GetComponent<boxScript>().catID == catShown && box.GetComponent<boxScript>().isCat == true)
                    {
                        box.GetComponent<boxScript>().showCatBox();
                    }
                }
                qShown = 0;
                Invoke("showQs", 2f);
            }
            else
            {
                if (catShown == gs.categoryList.Count)
                {
                    //hide the panel
                    GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMin = new Vector2(0, 1234);  // left, bottom
                    GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMax = new Vector2(-374, 1187);  // right,top
                }
            }
        }
    }

    void showQs()
    {
        if(qShown <= gs.categoryList[catShown].questions.Count)
        {
            foreach (GameObject box in GameObject.Find("boardScripts").GetComponent<questionLoader>().allBoxes)
            {
                if(box.GetComponent<boxScript>().catID == catShown && box.GetComponent<boxScript>().qID == qShown && box.GetComponent<boxScript>().isCat == false)
                {
                    box.GetComponent<boxScript>().showQBox();
                }
            }
            qShown++;
            Invoke("showQs", 0.5f);
        }
        else
        {
            //all Q's shown
            catShown++;
            if(catShown == gs.categoryList.Count)           
            {
                gs.firstCat = false;
                GameObject.Find("btn_nextCat").transform.Find("pnl_text").transform.Find("btnTxt").GetComponent<TextMeshProUGUI>().text = "Show Board";
                GameObject.Find("btn_nextCat").transform.Find("pnl_icon").GetComponent<Image>().sprite = boardSprite;
                resetColor();
            }
            else
            {
                //reset next button
                resetColor();
            }
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