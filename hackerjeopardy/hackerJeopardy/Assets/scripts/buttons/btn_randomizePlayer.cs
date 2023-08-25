using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_randomizePlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    Color originalColor;
    bool isClicked;
    gameSettings gs;
    float startSpeed;
    float stopSpeed;
    float speedHop;
    float currentSpeed;
    int currentHop;
    int minHops;
    int plTarget;
    int currentTarget;
    int blinkCount;
    float blinkSpeed = 0.5f;
    int blinkTarget = 10;
    bool blinkOn = false;

    void Start()
    {
        //find game settings
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();

        blinkCount = 0;
        originalColor = transform.GetComponent<Image>().color;
        isClicked = false;
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
            //Do a little animation to choose players
            plTarget = Random.Range(0, gs.players.Count);
            startSpeed = 0.1f;
            stopSpeed = 0.3f;
            minHops = Random.Range(10, 20);
            speedHop = (stopSpeed * 10) / minHops;
            speedHop = speedHop / 10;
            currentSpeed = startSpeed;
            currentHop = 0;
            currentTarget = 0;

            //set name in operator view
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().text = "Randomzing...";
            //change color of name in operator view
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 255, 255);

            //hide the randomize button
            transform.GetComponent<RectTransform>().offsetMin = new Vector2(0 - 378, 0); // left, bottom
            transform.GetComponent<RectTransform>().offsetMax = new Vector2(-1737, 0);// right,top

            Invoke("doAnim", currentSpeed);

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

    void doAnim()
    {
        //go again?
        if (currentHop <= minHops)
        {
            //clear all statusses
            foreach(GameObject player in gs.playerObjects)
            {
                player.GetComponent<playerScript>().ANSWERS_CLOSED();
            }

            foreach (GameObject player in gs.playerObjects)
            {
                if(player.GetComponent<playerScript>().myID == currentTarget)
                {
                    //light up current player
                    player.GetComponent<playerScript>().ANSWERED();
                    //play sound
                    GameObject.Find("scriptHolder").GetComponent<soundScript>().playBeep();
                }
            }
                
                

            // minhops not reached yet
            if (currentTarget < (gs.players.Count -1))
            {
                currentTarget++;
            }
            else
            {
                currentTarget = 0;
            }


            currentSpeed = currentSpeed + speedHop;
            currentHop++;
            Invoke("doAnim", currentSpeed);
        }
        else
        {
            //minhops reached
            //continue until target player
            //clear all statusses
            foreach (GameObject player in gs.playerObjects)
            {
                player.GetComponent<playerScript>().ANSWERS_CLOSED();
            }

            foreach (GameObject player in gs.playerObjects)
            {
                if (player.GetComponent<playerScript>().myID == currentTarget)
                {
                    //light up current player
                    player.GetComponent<playerScript>().ANSWERED();
                }
            }
            if (currentTarget != plTarget)
            {
                //play sound
                GameObject.Find("scriptHolder").GetComponent<soundScript>().playBeep();

                if (currentTarget < (gs.players.Count - 1))
                {
                    currentTarget++;
                }
                else
                {
                    currentTarget = 0;
                }
                currentHop++;
                Invoke("doAnim", currentSpeed);
            }
            else
            {
                //target reached, blink
                if(blinkCount == 0)
                {
                    //set target in gamesettings
                    gs.player_chosing = plTarget;
                    //set name in operator view
                    GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().text = gs.players[plTarget].playerName;
                    //change color of name in operator view
                    GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 0, 255);
                    //play tada sound
                    GameObject.Find("scriptHolder").GetComponent<soundScript>().playTada();

                }

                if(blinkCount < blinkTarget)
                {
                    //clear all statusses
                    foreach (GameObject player in gs.playerObjects)
                    {
                        player.GetComponent<playerScript>().ANSWERS_CLOSED();
                    }
                    if (blinkOn == false)
                    {
                        blinkOn = true;
                        foreach (GameObject player in gs.playerObjects)
                        {
                            if (player.GetComponent<playerScript>().myID == currentTarget)
                            {
                                //light up current player
                                player.GetComponent<playerScript>().ANSWERED();
                            }
                        }
                    }
                    else
                    {
                        blinkOn = false;
                    }
                    //go again
                    blinkCount++;
                    Invoke("doAnim", blinkSpeed);
                }
                else
                {
                    //target is reached and shown. Stop animating
                    //clear all statusses
                    foreach (GameObject player in gs.playerObjects)
                    {
                        player.GetComponent<playerScript>().ANSWERS_CLOSED();
                    }
                    //light up current player
                    foreach (GameObject player in gs.playerObjects)
                    {
                        if (player.GetComponent<playerScript>().myID == currentTarget)
                        {
                            //light up current player
                            player.GetComponent<playerScript>().ANSWERED();
                        }
                    }
                }
            }
        }
        
    }

}