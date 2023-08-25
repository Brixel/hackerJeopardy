using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_importFile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    bool isActive;
    public int lastIndex;

    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
        isActive = false;
        lastIndex = -1;
    }

    public void setActive()
    {
        lastIndex = GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().value;
        isActive = true;
    }
    public void setInActive()
    {
        isActive = false;
    }


    void Update()
    {
        if (isActive == true)
        {
            //check if the index has changed
            if (GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().value != lastIndex)
            {
                //value has changed, update files

                //first clear the file names
                GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().options.Clear();

                //now find all files on drive and filter, then make new options for the file list
                string[] theFiles = Directory.GetFiles(GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().value].text);
                foreach(string fileName in theFiles)
                {
                    if(fileName.EndsWith(".jeopardy"))
                    {
                        TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
                        thisOption.text = fileName;
                        GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().options.Add(thisOption);

                    }
                }
                GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().RefreshShownValue();

                lastIndex = GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().value;
            }
        }
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
            if(GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().options.Count > 0)
            {
                string theFile = GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_importFileList").GetComponent<TMP_Dropdown>().value].text;
                if(File.Exists(theFile))
                {
                    GameObject.Find("scriptHolder").GetComponent<gameSettings>().fileName = theFile;
                    GameObject.Find("scriptHolder").GetComponent<gameSettings>().loadFile();

                    Invoke("updateStats", 1f);
                    
                }
                
            }

            Invoke("resetColor", 1f);
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