using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_createQuestionFile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    List<char> allowedChars;
    List<char> allowedChars_gamename;
    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
        allowedChars = new List<char>();
        allowedChars.Add('a');
        allowedChars.Add('b');
        allowedChars.Add('c');
        allowedChars.Add('d');
        allowedChars.Add('e');
        allowedChars.Add('f');
        allowedChars.Add('g');
        allowedChars.Add('h');
        allowedChars.Add('i');
        allowedChars.Add('j');
        allowedChars.Add('k');
        allowedChars.Add('l');
        allowedChars.Add('m');
        allowedChars.Add('n');
        allowedChars.Add('o');
        allowedChars.Add('p');
        allowedChars.Add('q');
        allowedChars.Add('r');
        allowedChars.Add('s');
        allowedChars.Add('t');
        allowedChars.Add('u');
        allowedChars.Add('v');
        allowedChars.Add('w');
        allowedChars.Add('x');
        allowedChars.Add('y');
        allowedChars.Add('z');
        allowedChars.Add('A');
        allowedChars.Add('B');
        allowedChars.Add('C');
        allowedChars.Add('D');
        allowedChars.Add('E');
        allowedChars.Add('F');
        allowedChars.Add('G');
        allowedChars.Add('H');
        allowedChars.Add('I');
        allowedChars.Add('J');
        allowedChars.Add('K');
        allowedChars.Add('L');
        allowedChars.Add('M');
        allowedChars.Add('N');
        allowedChars.Add('O');
        allowedChars.Add('P');
        allowedChars.Add('Q');
        allowedChars.Add('R');
        allowedChars.Add('S');
        allowedChars.Add('T');
        allowedChars.Add('U');
        allowedChars.Add('V');
        allowedChars.Add('W');
        allowedChars.Add('1');
        allowedChars.Add('2');
        allowedChars.Add('3');
        allowedChars.Add('4');
        allowedChars.Add('5');
        allowedChars.Add('6');
        allowedChars.Add('7');
        allowedChars.Add('8');
        allowedChars.Add('9');
        allowedChars.Add('0');
        allowedChars.Add('_');
        allowedChars.Add('-');

        //game name has some extras, so lets add all the previous ones plus the extras
        allowedChars_gamename = new List<char>();
        foreach(char letter in allowedChars)
        {
            allowedChars_gamename.Add(letter);
        }
        allowedChars_gamename.Add(' ');
        allowedChars_gamename.Add('?');
        allowedChars_gamename.Add('!');
        allowedChars_gamename.Add('.');
        allowedChars_gamename.Add(',');
        allowedChars_gamename.Add('\'');
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
            //validate input
            if(GameObject.Find("inp_newQfileName").GetComponent<TMP_InputField>().text.Length > 0 || GameObject.Find("inp_newQFullPath").GetComponent<TMP_InputField>().text.Length > 0)
            {
                //there is a filename
                //is it dirty?

                bool filenameDirty = false;
                foreach (char letter in GameObject.Find("inp_newQfileName").GetComponent<TMP_InputField>().text)
                {
                    if(allowedChars.Contains(letter))
                    {
                        //do nothing
                    }
                    else
                    {
                        //char is dirty
                        filenameDirty = true;
                    }
                }
                if(filenameDirty == false)
                {
                    if (GameObject.Find("inp_newQgameName").GetComponent<TMP_InputField>().text != "")
                    {
                        //there is a game name
                        //is it dirty?
                        bool gamenameDirty = false;
                        foreach (char letter in GameObject.Find("inp_newQgameName").GetComponent<TMP_InputField>().text)
                        {
                            if (allowedChars_gamename.Contains(letter))
                            {
                                //do nothing
                            }
                            else
                            {
                                //char is dirty
                                gamenameDirty = true;
                            }
                        }
                        if(gamenameDirty == false)
                        {
                            //is there a tagline?
                            if (GameObject.Find("inp_newQgameTagline").GetComponent<TMP_InputField>().text != "")
                            {
                                //there is a tagline
                                //is it dirty?
                                bool taglineDirty = false;
                                foreach (char letter in GameObject.Find("inp_newQgameTagline").GetComponent<TMP_InputField>().text)
                                {
                                    if (allowedChars_gamename.Contains(letter))
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        //char is dirty
                                        taglineDirty = true;
                                    }
                                }
                                if (taglineDirty == false)
                                {
                                    string filePath = "";
                                    if(GameObject.Find("inp_newQFullPath").GetComponent<TMP_InputField>().text.Length == 0)
                                    {
                                        //create from dropdown and filename
                                        filePath = GameObject.Find("drp_driveList").GetComponent<TMP_Dropdown>().options[GameObject.Find("drp_driveList").GetComponent<TMP_Dropdown>().value].text + GameObject.Find("inp_newQfileName").GetComponent<TMP_InputField>().text + ".jeopardy";
                                    }
                                    else
                                    {
                                        //create from specified path
                                        filePath = GameObject.Find("inp_newQFullPath").GetComponent<TMP_InputField>().text;
                                    }
                                    //does the file exist already?
                                    if (File.Exists(filePath) == false)
                                    {
                                        //remove current categories
                                        GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Clear();

                                        //set game name
                                        GameObject.Find("scriptHolder").GetComponent<gameSettings>().gameName = GameObject.Find("inp_newQgameName").GetComponent<TMP_InputField>().text;

                                        //set tagline
                                        GameObject.Find("scriptHolder").GetComponent<gameSettings>().tagLine = GameObject.Find("inp_newQgameTagline").GetComponent<TMP_InputField>().text;

                                        //set file name
                                        GameObject.Find("scriptHolder").GetComponent<gameSettings>().fileName = filePath;

                                        //save the file empty for now
                                        GameObject.Find("scriptHolder").GetComponent<gameSettings>().saveFile();

                                        //set current file name
                                        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_filename").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().fileName;

                                        //set current game name
                                        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_gamename").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().gameName;

                                        //set current cat count
                                        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_ccount").GetComponent<TextMeshProUGUI>().text = "0";

                                        //set current q count
                                        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_qcount").GetComponent<TextMeshProUGUI>().text = "0";

                                        //remove window
                                        GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().hideNewQuestionsWindow();

                                        //show edit window
                                        GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().showEditQuestionsWindow();

                                    }
                                    else
                                    {
                                        GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "That file already exists!";
                                        GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                                    }
                                }
                                else
                                {
                                    //tagline is dirty
                                    GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Only a-z, A-Z and numbers allowed in tagline!";
                                    GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                                }                               
                            }
                            else
                            {
                                //tagline doesn't exist
                                GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Please provide a tagline!";
                                GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                            }
                        }
                        else
                        {
                            GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Only a-z, A-Z and numbers allowed in game name!";
                            GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                        }
                    }
                    else
                    {
                        GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Please provide a game name!";
                        GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                    }
                }
                else
                {
                    GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Only a-z, A-Z and numbers allowed in filenames!";
                    GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
            }
            else
            {
                GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().text = "Please provide a file name!";
                GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
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