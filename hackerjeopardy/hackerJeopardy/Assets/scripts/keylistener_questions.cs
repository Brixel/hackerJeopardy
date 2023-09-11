using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

public class keylistener_questions : MonoBehaviour
{
    gameSettings gs;
    public bool canAnswer = false;
    public int answered = -1;
    public bool givenScore = false;
    public bool answerRevealed = false;
    List<int> playersAnswered;

    int plTimer = 0;
    int qTimer = 0;
    bool plIsCounting = false;
    bool qIsCounting = false;


    void Start()
    {
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        playersAnswered = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            //check player buttons
            for (int i = 0; i < gs.players.Count; i++)
            {
                if (Input.GetKeyDown(gs.playerCodes[i]))
                {
                    //this player is answering
                    if (canAnswer == true && playersAnswered.Contains(i) == false)
                    {
                        canAnswer = false;
                        answered = i;
                        foreach (GameObject player in gs.playerObjects)
                        {
                            if (player.GetComponent<playerScript>().myID == i)
                            {
                                //light up current player
                                player.GetComponent<playerScript>().ANSWERED();
                            }
                        }
                        //show operator panel
                        GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMin = new Vector2(0,0);// left, bottom
                        GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMax = new Vector2(0,0);  // right,top
                        //set name in operator panel
                        GameObject.Find("txt_aPlayerName").GetComponent<TextMeshProUGUI>().text = gs.players[answered].playerName;
                        //add player to answered players
                        playersAnswered.Add(answered);
                        //play button sound
                        GameObject.Find("scriptHolder").GetComponent<soundScript>().playAnswerButton();
                        //stop music
                        GameObject.Find("scriptHolder").GetComponent<soundScript>().stopMusic();
                        //stop video
                        GameObject.Find("qVideo").GetComponent<VideoPlayer>().Stop();
                        //start player timer
                        plTimer = 0;
                        plIsCounting = true;
                        GameObject.Find("txt_plTimer").GetComponent<TextMeshProUGUI>().text = "0";
                        GameObject.Find("txt_plTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                        Invoke("plCountDown", 1f);
                        //stop questions counter
                        qIsCounting = false;
                    }
                }
            }
        }
    }

    void plCountDown()
    {
        plTimer++;
        GameObject.Find("txt_plTimer").GetComponent<TextMeshProUGUI>().text = plTimer.ToString();
        if (plTimer >= 10)
        {
            GameObject.Find("txt_plTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            GameObject.Find("txt_plTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        }
        if(plIsCounting == true)
        {
            Invoke("plCountDown", 1f);
        }
    }
    void qCountDown()
    {
        qTimer++;
        GameObject.Find("txt_qTimer").GetComponent<TextMeshProUGUI>().text = qTimer.ToString();
        if (qTimer >= 30)
        {
            GameObject.Find("txt_qTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            GameObject.Find("txt_qTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        }
        if (qIsCounting == true)
        {
            Invoke("qCountDown", 1f);
        }
    }

    public void releaseAnswers()
    {
        foreach (GameObject player in gs.playerObjects)
        {
            if(playersAnswered.Contains(player.GetComponent<playerScript>().myID) == false)
            {
                player.GetComponent<playerScript>().ANSWERS_OPEN();
            }
        }
        canAnswer = true;
        givenScore = false;
        answered = -1;

        qTimer = 0;
        qIsCounting = true;
        GameObject.Find("txt_qTimer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        Invoke("qCountDown", 1f);

        //show question text
        GameObject.Find("design1").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = gs.categoryList[gs.selectedC].questions[gs.selectedQ].questionText;
        GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = gs.categoryList[gs.selectedC].questions[gs.selectedQ].questionText;
        GameObject.Find("design3").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = gs.categoryList[gs.selectedC].questions[gs.selectedQ].questionText;
        GameObject.Find("tmp_plAnswerCorrectAnswer").GetComponent<TextMeshProUGUI>().text = gs.categoryList[gs.selectedC].questions[gs.selectedQ].answer;

        //show image 
        GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color = new Color(GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.r, GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.g, GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.b, 255);


        //play music (unless video question)
        if (gs.categoryList[gs.selectedC].questions[gs.selectedQ].PresentationType != 2)
        {
            GameObject.Find("scriptHolder").GetComponent<soundScript>().playMusic();
        }
    }

    public void revealAnswer()
    {
        if (answerRevealed == false)
        {
            canAnswer = false;
            answerRevealed = true;
            GameObject.Find("qShow").GetComponent<qShower>().showAnswer();
            GameObject.Find("btn_showAnswer").GetComponent<btn_revealAnswer>().switchIcon();
            //stop music
            GameObject.Find("scriptHolder").GetComponent<soundScript>().stopMusic();
            //play timeout sound

        }
        else
        {
            if (answered == -1)
            {
                //keep last chosing player
            }
            else
            {
                gs.player_chosing = answered;
            }
            gs.categoryList[gs.selectedC].questions[gs.selectedQ].isAvailable = false;

            //sanitycheck to see if the game is finished
            int qsLeft = 0;
            for(int i = 0; i<gs.categoryList.Count;i++)
            {
                foreach(Question thisQ in gs.categoryList[i].questions)
                {
                    if(thisQ.isAvailable == true)
                    {
                        qsLeft++;
                    }
                }
            }
            if(qsLeft == 0)
            {
                //all done!
                gs.finishedGame = true;
                gs.firstCat = true;
            }

            //play board loading sound
            GameObject.Find("scriptHolder").GetComponent<soundScript>().playBoardLoad();

            SceneManager.LoadScene("board");
        }
    }

    public void addCredits()
    {
        if (answered > -1)
        {
            if (givenScore == false)
            {
                qIsCounting = false;
                plIsCounting = false;
                givenScore = true;
                gs.players[answered].playerScore = gs.players[answered].playerScore + gs.categoryList[gs.selectedC].questions[gs.selectedQ].value;
                gs.updateCredits(answered, gs.categoryList[gs.selectedC].questions[gs.selectedQ].value,true);
                //hide operator panel
                GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMin = new Vector2(0, 1304);// left, bottom
                GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMax = new Vector2(0, 1304);  // right,top
                //since the answer was correct, reveal it so no one can answer anymore
                revealAnswer();
                //play answer correct sound
                GameObject.Find("scriptHolder").GetComponent<soundScript>().playAnswerRight();
            }
        }
    }
    public void retractCredits()
    {
        if (answered > -1)
        {
            if (givenScore == false)
            {
                plIsCounting = false;
                givenScore = true;
                gs.players[answered].playerScore = gs.players[answered].playerScore - gs.categoryList[gs.selectedC].questions[gs.selectedQ].value;
                gs.updateCredits(answered, gs.categoryList[gs.selectedC].questions[gs.selectedQ].value,false);
                //hide operator panel
                GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMin = new Vector2(0, 1304);// left, bottom
                GameObject.Find("pnl_answer").GetComponent<RectTransform>().offsetMax = new Vector2(0, 1304);  // right,top
                //find player object and tell it it to turn red
                foreach(GameObject plObj in gs.playerObjects)
                {
                    if(plObj.GetComponent<playerScript>().myID == answered)
                    {
                        plObj.GetComponent<playerScript>().ANSWERS_CLOSED();
                    }
                }
                //play wrong answer sound
                GameObject.Find("scriptHolder").GetComponent<soundScript>().playAnswerWrong();
                //release players once again for another try
                Invoke("releaseAnswers",1f);

            }
        }
    }

}
