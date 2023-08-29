using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
using SFB;


public class btn_Import : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
            Invoke("resetColor", 1f);
            GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().showImportWindow();
            //var paths = StandaloneFileBrowser.OpenFilePanel("Select Jeopardy file", "", "jeopardy", false);
            //if(paths.Length > 0)
            //{
            //    GameObject.Find("scriptHolder").GetComponent<gameSettings>().fileName = paths[0];
            //    GameObject.Find("scriptHolder").GetComponent<gameSettings>().loadFile();

            //    Invoke("updateStats", 1f);
            //}


        }
    }

    public void updateStats()
    {
        gameSettings gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        //set filename
        GameObject.Find("txt_filename").GetComponent<TextMeshProUGUI>().text = gs.fileName;

        //set current game name
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_gamename").GetComponent<TextMeshProUGUI>().text = gs.gameName;

        //set current cat count
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_ccount").GetComponent<TextMeshProUGUI>().text = gs.categoryList.Count.ToString();

        //set current q count
        int totalQ = 0;
        foreach (Category thisCat in gs.categoryList)
        {
            totalQ = totalQ + thisCat.questions.Count;
        }
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_qcount").GetComponent<TextMeshProUGUI>().text = totalQ.ToString();
        GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().hideImportWindow();

        //set game title and tagline
        GameObject.Find("gameName").GetComponent<TextMeshProUGUI>().text = gs.gameName;
        GameObject.Find("gameTagline").GetComponent<TextMeshProUGUI>().text = gs.tagLine;
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("pnl_icon").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}