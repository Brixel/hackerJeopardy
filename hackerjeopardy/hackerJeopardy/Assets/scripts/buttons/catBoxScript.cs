using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class catBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    gameSettings gs;
    int catID;


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
            transform.Find("txt_catName").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);

            //logic
            //show questions for this cat
            //first clear all items that exist in the view
            foreach(Transform obj in GameObject.Find("qHolder").transform.Find("Viewport").Find("Content").transform)
            {
                obj.GetComponent<qBoxScript>().destroyMe();
            }
            //now load new questions
            int qCounter = 0;
            foreach(Question thisQ in gs.categoryList[catID].questions)
            {
                GameObject thisQGo = Instantiate(GameObject.Find("operatorCam").GetComponent<op_boardScript>().qBoxPrefab, GameObject.Find("qHolder").transform.Find("Viewport").Find("Content").transform);
                thisQGo.GetComponent<qBoxScript>().setCatID(catID);
                thisQGo.GetComponent<qBoxScript>().setCatVal(thisQ.value);
                thisQGo.GetComponent<qBoxScript>().setQID(qCounter);
                qCounter++;
            }

            Invoke("resetColor", 1f);

        }
    }

    public void setCatName(string catName)
    {
        transform.Find("txt_catName").GetComponent<TextMeshProUGUI>().text = catName;
    }
    public void setCatId(int theID)
    {
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        catID = theID;
        //update color to reflect the CAT button on screen
        transform.GetComponent<Image>().color = new Color(gs.categoryList[catID].categoryColorR, gs.categoryList[catID].categoryColorG, gs.categoryList[catID].categoryColorB, 255);
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("txt_catName").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}