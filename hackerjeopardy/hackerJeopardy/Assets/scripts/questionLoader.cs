using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class questionLoader : MonoBehaviour
{
    public GameObject boxObj;
    GameObject mainCanvasObj;

    gameSettings gameSettings;

    public List<GameObject> allBoxes;



    void Start()
    {
        gameSettings = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        allBoxes = new List<GameObject>();
        loadBoxes();       
    }

    public void Awake()
    {

    }

    

    public void loadBoxes()
    {
        //set the main canvas
        mainCanvasObj = GameObject.Find("board-bg");

        gameSettings.loadPlayers();

        //get the highest number of questions
        int qRowCount = 0;
        foreach (Category cat in gameSettings.categoryList)
        {
            if (cat.questions.Count > qRowCount)
            {
                qRowCount = cat.questions.Count;
            }
        }

        qRowCount = qRowCount + 1; // row 0 is always the category;


        for (int q = 0; q < qRowCount; q++)
        {

            for (int c = 0; c < gameSettings.categoryList.Count; c++)
            {
                GameObject newBox = Instantiate(boxObj, mainCanvasObj.transform);
                allBoxes.Add(newBox);
                int desiredWidth = Camera.main.pixelWidth / gameSettings.categoryList.Count;
                int desiredHeight = (Camera.main.pixelHeight - 100) / qRowCount; // 100 is the size of the player panel at the bottom
                
                //make sure the height is never bigger then the width (makes it look ugly)
                if (desiredHeight > desiredWidth)
                {
                    desiredHeight = desiredWidth;
                }

                int left = 0 + (desiredWidth * c);
                int bottom = (Camera.main.pixelHeight - 100) - (desiredHeight * (q + 1));
                int right = 0 - (Camera.main.pixelWidth - (desiredWidth * (c + 1)));
                int top = 0 - (desiredHeight * q);

                newBox.GetComponent<RectTransform>().offsetMin = new Vector2(left, bottom); // left, bottom
                newBox.GetComponent<RectTransform>().offsetMax = new Vector2(right, top); // right,top
                newBox.GetComponent<boxScript>().catID = c;
                newBox.GetComponent<boxScript>().qID = q - 1;

                //style the box
                    if (q == 0)
                    {
                        //box is a category
                        newBox.GetComponent<boxScript>().UpdateStyle(0);
                        newBox.GetComponent<boxScript>().isCat = true;
                        newBox.transform.Find("innerBox").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[c].categoryColorR, (byte)gameSettings.categoryList[c].categoryColorG, (byte)gameSettings.categoryList[c].categoryColorB, 255);
                        newBox.GetComponent<boxScript>().setText(gameSettings.categoryList[c].categoryName);
                    }
                    else
                    {
                        if (q - 1 < gameSettings.categoryList[c].questions.Count)
                        {
                            if (gameSettings.categoryList[c].questions[q - 1].isAvailable == true)
                            {
                                //box is a question and is available
                                newBox.GetComponent<boxScript>().UpdateStyle(1);
                                newBox.GetComponent<boxScript>().isCat = false;
                                newBox.transform.Find("innerBox").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[c].questions[q - 1].questionColorR, (byte)gameSettings.categoryList[c].questions[q - 1].questionColorG, (byte)gameSettings.categoryList[c].questions[q - 1].questionColorB, 255);
                                newBox.GetComponent<boxScript>().setText(gameSettings.categoryList[c].questions[q - 1].value.ToString());
                            }
                            else
                            {
                                //box is a question and is unavailable
                                newBox.GetComponent<boxScript>().UpdateStyle(2);
                                newBox.GetComponent<boxScript>().isCat = false;
                                //newBox.GetComponent<boxScript>().setText("");
                            }
                        }
                        else
                        {
                            //draw a empty box
                            newBox.GetComponent<boxScript>().UpdateStyle(3);
                            newBox.GetComponent<boxScript>().qID = -1;
                            newBox.GetComponent<boxScript>().setText("");
                        }
                }
            }
        }

        if(gameSettings.finishedGame == true)
        {
            //overlay gameover panel
            GameObject.Find("pnl_gameFinished").GetComponent<RectTransform>().offsetMin = new Vector2(0, 100);
            GameObject.Find("pnl_gameFinished").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            //find winning player
            int winning = 0;
            int highestScore = -999999999;
            int plCounter = 0;
            foreach(Player thisPl in gameSettings.players)
            {
                if(thisPl.playerScore > highestScore)
                {
                    winning = plCounter;
                    highestScore = thisPl.playerScore;
                }
                plCounter++;
            }
            //update textfield
            GameObject.Find("txt_gameOverPlayer").GetComponent<TextMeshProUGUI>().text = gameSettings.players[winning].playerName + "\n" + gameSettings.players[winning].playerScore.ToString();
            //light up player
            foreach(GameObject plObj in gameSettings.playerObjects)
            {
                if(plObj.GetComponent<playerScript>().myID == winning)
                {
                    plObj.GetComponent<playerScript>().ANSWERED();
                }
            }
            //hide presenting overlay panel
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMin = new Vector2(0, 1234);  // left, bottom
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMax = new Vector2(-374, 1187);  // right,top
            //show operator finished panel
            GameObject.Find("pnl_op_gameFinished").GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);  // left, bottom
            GameObject.Find("pnl_op_gameFinished").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);  // right,top
            //update operator fields
            GameObject.Find("txt_playerWinner").GetComponent<TextMeshProUGUI>().text = gameSettings.players[winning].playerName;
            GameObject.Find("txt_playerWinnerScore").GetComponent<TextMeshProUGUI>().text = gameSettings.players[winning].playerScore.ToString(); ;
        }
    }

    void Update()
    {
        

    }


}



