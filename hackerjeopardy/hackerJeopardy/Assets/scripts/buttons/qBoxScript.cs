using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class qBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    gameSettings gs;
    int QID;
    int catID;
    public Sprite bgSprite;


    void Start()
    {
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();

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
            transform.Find("txt_qValue").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);

            //logic
            //find box and animate
            foreach (GameObject box in GameObject.Find("boardScripts").GetComponent<questionLoader>().allBoxes)
            {
                if (box.GetComponent<boxScript>().catID == catID && box.GetComponent<boxScript>().qID == QID)
                {
                    box.GetComponent<boxScript>().clickLogic();
                }
            }
            Invoke("resetColor", 1f);
        }
    }

   

    public void destroyMe()
    {
        Destroy(this.gameObject);
    }

    public void setQID(int theID)
    {
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        QID = theID;
        //update the color to reflect the colors on screen
        if(gs.categoryList[catID].questions[QID].isAvailable == true)
        {
            //transform.GetComponent<Image>().color = new Color(0, 255, 0, 255);
            transform.GetComponent<Image>().color = new Color32((byte)gs.categoryList[catID].questions[QID].questionColorR, (byte)gs.categoryList[catID].questions[QID].questionColorG, (byte)gs.categoryList[catID].questions[QID].questionColorB, 255);
        }
        else
        {
            //color red and disable
            transform.GetComponent<Image>().color = new Color(255, 0, 0, 255);
            isClicked = true;
        }
        
    }
    public void setCatID(int theID)
    {
        catID = theID;
    }
    public void setCatVal(int catVal)
    {
        transform.Find("txt_qValue").GetComponent<TextMeshProUGUI>().text = catVal.ToString();
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("txt_qValue").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}