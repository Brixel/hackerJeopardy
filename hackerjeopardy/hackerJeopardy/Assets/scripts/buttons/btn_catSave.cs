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

public class btn_catSave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
        lastIndex = GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value;
        isActive = true;
    }

    public void setInActive()
    {
        isActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(isActive == true)
        {
            //check if the index has changed
            if(GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value != lastIndex)
            {
                //clear error message
                GameObject.Find("txt_catNameError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);

                if (GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value >= GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Count)
                {
                    // size is bigger, we must be adding a new one
                    // do nothing
                }
                else
                {
                    //changed
                    lastIndex = GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value;
                    //update cat name and color boxes
                    GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[lastIndex].categoryName;
                    GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[lastIndex].categoryColorR.ToString();
                    GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[lastIndex].categoryColorG.ToString();
                    GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[lastIndex].categoryColorB.ToString();
                    //update hidden edit value
                    GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text = lastIndex.ToString();
                    //update questions section
                    GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().updateQdrp();
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
            if (GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text == "-1")
            {
                //we're creating a new cat. Is the name taken?
                bool isTaken = false;
                foreach (Category thisCat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
                {
                    if (thisCat.categoryName == GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text)
                    {
                        isTaken = true;
                    }

                }
                if (isTaken == true)
                {
                    //name is taken, we can't continue
                    //show error
                    GameObject.Find("txt_catNameError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
                else
                {
                    //name is not taken, is the color safe?
                    //R
                    if (int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text) < 256)
                    {
                        //R is safe
                        //G
                        if (int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text) < 256)
                        {
                            //G is safe
                            //B
                            if (int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text) < 256)
                            {
                                //B is also safe

                                // insert cat!
                                Category thisCat = new Category();
                                thisCat.categoryName = GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text;
                                thisCat.categoryColorR = int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text);
                                thisCat.categoryColorG = int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text);
                                thisCat.categoryColorB = int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text);


                                thisCat.questions = new List<Question>();
                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Add(thisCat);

                                //clear fields and update dropdown
                                GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().updateCdrp();
                            }
                            else
                            {
                                GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                            }
                        }
                        else
                        {
                            GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                        }
                    }
                    else
                    {
                        GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                    }
                }
            }
            else
            {
                //update cat
                //Is the name taken? if so, is it only in this ID?
                bool isTaken = false;
                foreach (Category thisCat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
                {
                    if (thisCat.categoryName == GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text)
                    {
                        //we found one, is it the same ID?
                        if (GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text == GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.IndexOf(thisCat).ToString())
                        {
                            //no problem, it's this ID, don't change anything
                        }
                        else
                        {
                            //problem! this name is used in another caegory
                            isTaken = true;
                        }
                    }

                }
                if (isTaken == true)
                {
                    //name is taken, we can't continue
                    //show error
                    GameObject.Find("txt_catNameError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
                else
                {
                    //is the color safe?
                    //R
                    if (int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text) < 256)
                    {
                        //R is safe
                        //G
                        if (int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text) < 256)
                        {
                            //G is safe
                            //B
                            if (int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text) > -1 && int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text) < 256)
                            {
                                //B is also safe

                                // update cat!
                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].categoryName = GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text;
                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].categoryColorR = int.Parse(GameObject.Find("inp_catColorR").GetComponent<TMP_InputField>().text);
                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].categoryColorG = int.Parse(GameObject.Find("inp_catColorG").GetComponent<TMP_InputField>().text);
                                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].categoryColorB = int.Parse(GameObject.Find("inp_catColorB").GetComponent<TMP_InputField>().text);

                                //clear fields and update dropdown
                                GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text = "";
                                GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().updateCdrp();
                            }
                            else
                            {
                                GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                            }
                        }
                        else
                        {
                            GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                        }
                    }
                    else
                    {
                        GameObject.Find("txt_catColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                    }
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