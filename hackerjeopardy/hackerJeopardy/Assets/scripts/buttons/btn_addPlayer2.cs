using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_addPlayer2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    public bool isActive;
    void Start()
    {
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
        isActive = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        clickLogic();
    }

    // Update is called once per frame
    void Update()
    {

        //check for [enter] press, and proceed with the click logic
        if(isActive)
        {
            if(Input.anyKeyDown)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    clickLogic();
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


    void clickLogic()
    {
        if (isClicked == false)
        {
            isClicked = true;
            transform.Find("pnl_icon").GetComponent<Image>().color = new Color(0, 255, 0, 255);
            transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);

            //logic
            //check if player limit is reached
            if(GameObject.Find("scriptHolder").GetComponent<gameSettings>().players.Count < 10)
            {
                //check if name is taken
                bool found = false;
                foreach (Player findPlayer in GameObject.Find("scriptHolder").GetComponent<gameSettings>().players)
                {
                    if (findPlayer.playerName == GameObject.Find("inp_playerName").GetComponent<TMP_InputField>().text)
                    {
                        found = true;
                    }
                }
                if (found == false)
                {
                    //add this player
                    Player thisPlayer = new Player();
                    thisPlayer.playerName = GameObject.Find("inp_playerName").GetComponent<TMP_InputField>().text;
                    thisPlayer.playerScore = 0;
                    GameObject.Find("scriptHolder").GetComponent<gameSettings>().players.Add(thisPlayer);
                    GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().renderPlayers();
                    //remove window
                    GameObject.Find("operator_init_scripts").GetComponent<op_initScripts>().hideAddPlayerWindow();

                    //clear name
                    GameObject.Find("inp_playerName").GetComponent<TMP_InputField>().text = "";
                }
                else
                {
                    //show error
                    GameObject.Find("txt_playerNameError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
                }
            }
            else
            {
                //player limit is reached
                GameObject.Find("txt_playerLimitError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 255);
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