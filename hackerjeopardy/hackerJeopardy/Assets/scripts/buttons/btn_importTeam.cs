using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_importTeam : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
        lastIndex = GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().value;
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
            if (GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().value != lastIndex)
            {
                //value has changed, update files

                //first clear the file names
                GameObject.Find("drp_importTeamFileList").GetComponent<TMP_Dropdown>().options.Clear();

                //now find all files on drive and filter, then make new options for the file list
                string[] theFiles = Directory.GetFiles(GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().value].text);
                foreach (string fileName in theFiles)
                {
                    if (fileName.EndsWith(".teams"))
                    {
                        TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
                        thisOption.text = fileName;
                        GameObject.Find("drp_importTeamFileList").GetComponent<TMP_Dropdown>().options.Add(thisOption);

                    }
                }
                GameObject.Find("drp_importTeamFileList").GetComponent<TMP_Dropdown>().RefreshShownValue();

                lastIndex = GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().value;
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
            string theFile = "";
            if (GameObject.Find("inp_impTeamPath").GetComponent<TMP_InputField>().text.Length == 0)
            {
                //try to load from dropdown menu
                theFile = GameObject.Find("drp_importTeamFileList").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_importTeamFileList").GetComponent<TMP_Dropdown>().value].text;
            }
            else
            {
                //try to load from textfield
                theFile = GameObject.Find("inp_impTeamPath").GetComponent<TMP_InputField>().text;
            }
            if (File.Exists(theFile))
            {
                //TODO: IMP_TEAMS
                GameObject.Find("scriptHolder").GetComponent<gameSettings>().loadTeams(theFile);
            }
            else
            {
                //can't find or access file
                //show error
                GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color = new Color(GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.r, GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.g, GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.b, 255);
            }

            Invoke("resetColor", 1f);
        }
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("pnl_icon").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        //hide error
        GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color = new Color(GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.r, GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.g, GameObject.Find("txt_importTeamFileError").GetComponent<TextMeshProUGUI>().color.b, 0);
        isClicked = false;
    }

}
