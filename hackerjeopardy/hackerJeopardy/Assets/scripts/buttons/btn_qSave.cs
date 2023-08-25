using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Unity.Mathematics;
using TMPro.Examples;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class btn_qSave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
        lastIndex = GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().value;
        isActive = true;
    }

    public void setInActive()
    {
        isActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            //check if the index has changed
            if (GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().value != lastIndex)
            {
                //clear errors
                GameObject.Find("txt_qValError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);
                GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);

                if (GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().value >= GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].questions.Count)
                {
                    // size is bigger, we must be adding a new one
                    // do nothing
                }
                else
                {
                    //changed
                    lastIndex = GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().value;
                    //update input fields
                    Category thisCat = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)];

                    GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].value.ToString();
                    GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value = thisCat.questions[lastIndex].PresentationType;
                    GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().RefreshShownValue();
                    GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionText;
                    GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].answer;
                    if (thisCat.questions[lastIndex].PresentationType == 1)
                    {
                        GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionImage;
                        GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].answerImage;
                    }
                    if (thisCat.questions[lastIndex].PresentationType == 2)
                    {
                        GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionVideo;
                        GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].answerVideo;
                    }

                    GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionNote;

                    //update colors
                    GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionColorR.ToString();
                    GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionColorG.ToString();
                    GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text = thisCat.questions[lastIndex].questionColorB.ToString();

                    //update hidden edit value
                    GameObject.Find("txt_qID").GetComponent<TextMeshProUGUI>().text = lastIndex.ToString();
                }
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
            //check if ID > -1 and if name exists
            if (GameObject.Find("txt_qID").GetComponent<TextMeshProUGUI>().text == "-1")
            {
                //we're creating a new q.
                //Is the input valid?
                //value numeric?
                int result;
                if(int.TryParse(GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text, out result)) 
                {
                    //value is numeric
                    //are the colors sane?
                    //R
                    if (int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text) < 256)
                    {
                        //R is safe
                        //G
                        if (int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text) < 256)
                        {
                            //G is safe
                            //B
                            if (int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text) < 256)
                            {
                                //B is also safe

                                // insert Q!
                                Question thisQ = new Question();
                                thisQ.value = int.Parse(GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text);
                                thisQ.PresentationType = GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value;
                                thisQ.questionText = GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text;
                                thisQ.answer = GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text;
                                if(thisQ.PresentationType == 1)
                                {
                                    thisQ.questionImage = GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text;
                                    thisQ.answerImage = GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text;
                                }
                                if (thisQ.PresentationType == 2)
                                {
                                    thisQ.questionVideo = GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text;
                                    thisQ.answerVideo = GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text;
                                }
                                thisQ.questionNote = GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text;
                                thisQ.questionColorR = int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text);
                                thisQ.questionColorG = int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text);
                                thisQ.questionColorB = int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text);
                                thisQ.isAvailable = true;

                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].questions.Add(thisQ);

                                //clear fields and update dropdown
                                GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value = 0;
                                GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().updateQdrp();
                            }
                            else
                            {
                                GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                            }
                        }
                        else
                        {
                            GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                        }
                    }
                    else
                    {
                        GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                    }
                }
                else
                {
                    //qval is not numeric
                    //show error
                    GameObject.Find("txt_qValError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
                
            }
            else
            {
                //update question
                // is the value safe?
                int result;
                if (int.TryParse(GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text, out result))
                {
                    //is the color safe?
                    //R
                    if (int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text) < 256)
                    {
                        //R is safe
                        //G
                        if (int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text) < 256)
                        {
                            //G is safe
                            //B
                            if (int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text) < 256)
                            {
                                //B is also safe

                                // update Q!
                                Question thisQ = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].questions[int.Parse(GameObject.Find("txt_qID").GetComponent<TextMeshProUGUI>().text)];
                                thisQ.value = int.Parse(GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text);
                                thisQ.PresentationType = GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value;
                                thisQ.questionText = GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text;
                                thisQ.answer = GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text;
                                if (thisQ.PresentationType == 1)
                                {
                                    thisQ.questionImage = GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text;
                                    thisQ.answerImage = GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text;
                                }
                                if (thisQ.PresentationType == 2)
                                {
                                    thisQ.questionVideo = GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text;
                                    thisQ.answerVideo = GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text;
                                }
                                thisQ.questionNote = GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text;
                                thisQ.questionColorR = int.Parse(GameObject.Find("inp_qColorR").GetComponent<TMP_InputField>().text);
                                thisQ.questionColorG = int.Parse(GameObject.Find("inp_qColorG").GetComponent<TMP_InputField>().text);
                                thisQ.questionColorB = int.Parse(GameObject.Find("inp_qColorB").GetComponent<TMP_InputField>().text);

                                //clear fields and update dropdown
                                GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value = 0;
                                GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().updateQdrp();

                            }
                            else
                            {
                                GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                            }
                        }
                        else
                        {
                            GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                        }
                    }
                    else
                    {
                        GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                    }
                }
                else
                {
                    //qval is not numeric
                    //show error
                    GameObject.Find("txt_qValError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
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
}
