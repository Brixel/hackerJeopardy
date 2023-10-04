using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_pickTeam : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    int stepsToFinish;
    int stepsSinceStart;
    int blinksCounter;
    int ChosenTeam;

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
            stepsToFinish = Random.Range(25, 50);
            stepsSinceStart = 0;
            GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().text = "";
            GameObject.Find("txt_currentSelection").GetComponent<TextMeshProUGUI>().text = "";
            GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            Invoke("ChooseNewOne", 1f);

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

    void ChooseNewOne()
    {
        ChosenTeam = Random.Range(0, GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList.Count);
        GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList[ChosenTeam];
        stepsSinceStart++;
        if(stepsSinceStart < stepsToFinish)
        {
            Invoke("ChooseNewOne", 0.3f);
        }
        else
        {
            GameObject.Find("txt_currentSelection").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList[ChosenTeam];
            blinksCounter = 0;
            Invoke("Blink", 1f);
        }
    }

    void Blink()
    {
        if(blinksCounter < 5)
        {
            if(GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().color.a > 0)
            {
                GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 0);
            }
            else
            {
                GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            }
            blinksCounter++;
            Invoke("Blink",0.6f);
        }
        else
        {
            GameObject.Find("txt_teamName").GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 0, 255);
            GameObject.Find("txt_selectedTeams").GetComponent<TextMeshProUGUI>().text = GameObject.Find("txt_selectedTeams").GetComponent<TextMeshProUGUI>().text + System.Environment.NewLine + GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList[ChosenTeam];
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().addPlayer(GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList[ChosenTeam]);
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().TeamsList.RemoveAt(ChosenTeam);
        }
    }
}
