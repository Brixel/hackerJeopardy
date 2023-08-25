using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class op_questionScripts : MonoBehaviour
{
    int catID;
    int qID;
    gameSettings gs;
    void Start()
    {
        //find gs
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        //set variables
        catID = gs.selectedC;
        qID = gs.selectedQ;
        //populate question fields
        GameObject.Find("txt_catName").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catID].categoryName;
        GameObject.Find("txt_QVal").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catID].questions[qID].value.ToString();
        switch(gs.categoryList[catID].questions[qID].PresentationType)
        {
            case 0:
                GameObject.Find("txt_QType").GetComponent<TextMeshProUGUI>().text = "Text question";
                //hide media buttons
                GameObject.Find("btn_playMedia").GetComponent<btn_PlayMedia>().hideButton();
                GameObject.Find("btn_stopMedia").GetComponent<btn_stopMedia>().hideButton();
                break;
            case 1:
                GameObject.Find("txt_QType").GetComponent<TextMeshProUGUI>().text = "Picture question";
                //hide media buttons
                GameObject.Find("btn_playMedia").GetComponent<btn_PlayMedia>().hideButton();
                GameObject.Find("btn_stopMedia").GetComponent<btn_stopMedia>().hideButton();
                break;
            case 2:
                GameObject.Find("txt_QType").GetComponent<TextMeshProUGUI>().text = "Video question";
                //show media button (not needed, button is there
                break;
        }
        GameObject.Find("txt_qTxt").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catID].questions[qID].questionText;
        GameObject.Find("txt_aTxt").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catID].questions[qID].answer;
        GameObject.Find("txt_nTxt").GetComponent<TextMeshProUGUI>().text = gs.categoryList[catID].questions[qID].questionNote;
    }


    void Update()
    {
        
    }
}
