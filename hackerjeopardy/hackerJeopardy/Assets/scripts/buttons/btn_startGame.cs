using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;


public class btn_startGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
            gameSettings gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
            isClicked = true;
            transform.Find("pnl_icon").GetComponent<Image>().color = new Color(0, 255, 0, 255);
            transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);
            Invoke("resetColor", 1f);

            //check if there is a file loaded (or at least categories)
            if (gs.categoryList.Count > 0)
            {
                //are there players?
                if (gs.players.Count > 0)
                {
                    //attach answer buttons to the players
                    foreach (Player thisPlayer in gs.players)
                    {
                        thisPlayer.answerButton = gs.playerCodes[GameObject.Find("scriptHolder").GetComponent<gameSettings>().players.IndexOf(thisPlayer)];
                    }
                    //set game variables
                    //make sure we present the categories
                    gs.firstCat = true;
                    //make sure the game isn't finished
                    gs.finishedGame = false;
                    //make all questions available again
                    int catCounter = 0;
                    foreach (Category thisCat in gs.categoryList)
                    {
                        foreach (Question thisQ in gs.categoryList[catCounter].questions)
                        {
                            thisQ.isAvailable = true;
                        }
                        catCounter++;
                    }
                    //reset all player scores
                    foreach (Player thisPlayer in gs.players)
                    {
                        thisPlayer.playerScore = 0;
                    }
                    //reset chosing player
                    gs.player_chosing = -1;

                    //stop the music
                    GameObject.Find("scriptHolder").GetComponent<soundScript>().stopMusic();




                    //let's go!
                    SceneManager.LoadScene("board");
                }
                else
                {
                    // no players
                    GameObject.Find("txt_noPlayersError").GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, 255);
                    Invoke("resetPlayerError", 3f);

                }
            }
            else
            {
                //no categories
                GameObject.Find("txt_noCatError").GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, 255);
                Invoke("resetCatError", 3f);
            }
        }
    }

    void resetPlayerError()
    {
        GameObject.Find("txt_noPlayersError").GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, 0);
    }
    void resetCatError()
    {
        GameObject.Find("txt_noCatError").GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, 0);
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        transform.Find("pnl_icon").GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        isClicked = false;
    }

}
