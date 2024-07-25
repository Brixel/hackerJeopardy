using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_importAfterLostMedia : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;

    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
    }



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

            //questions are loaded, so find and replace source with destination
            int changedMade = 0;
            foreach(Category cat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
            {
                foreach(Question q in cat.questions)
                {
                    if(q.answerImage != "" && q.answerImage != null && q.answerImage != "null")
                    {
                        if (q.answerImage.Substring(0, 3).ToLower() == GameObject.Find("imp_lostMediaDrive").GetComponent<TMP_InputField>().text.ToLower())
                        {
                            q.answerImage = q.answerImage.Replace(q.answerImage.Substring(0,3), GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().value].text);
                            changedMade++;
                        }
                    }
                    if (q.answerVideo != "" && q.answerVideo != null && q.answerVideo != "null")
                    {
                        if (q.answerVideo.Substring(0, 3).ToLower() == GameObject.Find("imp_lostMediaDrive").GetComponent<TMP_InputField>().text.ToLower())
                        {
                            q.answerVideo = q.answerVideo.Replace(q.answerVideo.Substring(0, 3), GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().value].text);
                            changedMade++;
                        }
                    }
                    if (q.questionImage != "" && q.questionImage != null && q.questionImage != "null")
                    {
                        if (q.questionImage.Substring(0, 3).ToLower() == GameObject.Find("imp_lostMediaDrive").GetComponent<TMP_InputField>().text.ToLower())
                        {
                            q.questionImage = q.questionImage.Replace(q.questionImage.Substring(0, 3), GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().value].text);
                            changedMade++;
                        }
                    }
                    if (q.questionVideo != "" && q.questionVideo != null && q.questionVideo != "null")
                    {
                        if (q.questionVideo.Substring(0, 3).ToLower() == GameObject.Find("imp_lostMediaDrive").GetComponent<TMP_InputField>().text.ToLower())
                        {
                            q.questionVideo = q.questionVideo.Replace(q.questionVideo.Substring(0, 3), GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_lostMediaReplacementDrive").GetComponent<TMP_Dropdown>().value].text);
                            changedMade++;
                        }
                    }
                }
            }
            //Debug.Log("Changed made: " + changedMade.ToString());

            GameObject.Find("scriptHolder").GetComponent<gameSettings>().saveFile();

            GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().hideLostMediaWindow();
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().loadFile();
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
        //hide error
        GameObject.Find("txt_importFileError").GetComponent<TextMeshProUGUI>().color = new Color(GameObject.Find("txt_importFileError").GetComponent<TextMeshProUGUI>().color.r, GameObject.Find("txt_importFileError").GetComponent<TextMeshProUGUI>().color.g, GameObject.Find("txt_importFileError").GetComponent<TextMeshProUGUI>().color.b, 0);
        isClicked = false;
    }

}