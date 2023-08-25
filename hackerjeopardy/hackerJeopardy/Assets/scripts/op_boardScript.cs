using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class op_boardScript : MonoBehaviour
{
    gameSettings gs;
    public GameObject catBoxPrefab;
    public GameObject qBoxPrefab;
    public GameObject playerPrefab;
    void Start()
    {
        //get the game settings
        gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        //do we need to show the randomize button for player choosing?
        if(gs.player_chosing == -1)
        {
            //yes
            //place the button
            GameObject.Find("btn_randomize").GetComponent<RectTransform>().offsetMin = new Vector2(1323, 27);// left, bottom
            GameObject.Find("btn_randomize").GetComponent<RectTransform>().offsetMax = new Vector2(-35,-27);// right,top
            //set the choosing text to 'nobody' and make the color red
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().text = "Nobody";
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            //no, show player name
            //set the choosing text to chosing player and make the color green
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().text = gs.players[gs.player_chosing].playerName;
            GameObject.Find("txt_choosingPlayer").GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 0, 255);
        }
        //if first time seeing the board, show category presenter, if not: hide it
        if(gs.firstCat == true)
        {
            //show it
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMin = new Vector2(0, 0); //left, bottom
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMax = new Vector2(-374, -47); //right, top

        }
        else
        {
            //hide it
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMin = new Vector2(0, 1234);  // left, bottom
            GameObject.Find("pnl_presentCategories").GetComponent<RectTransform>().offsetMax = new Vector2(-374, 1187);  // right,top
        }

        //populate categories list
        int catCounter = 0;
        foreach(Category thisCat in gs.categoryList)
        {
            GameObject thisCatObj = Instantiate(catBoxPrefab, GameObject.Find("catHolder").transform.Find("Viewport").Find("Content").transform);
            thisCatObj.GetComponent<catBoxScript>().setCatId(catCounter);
            thisCatObj.GetComponent<catBoxScript>().setCatName(thisCat.categoryName);

            catCounter++;
        }
        updatePlayers();

    }


    void updatePlayers()
    {
        List<Player> PLlist = new List<Player>();

        foreach(Player thisPL in gs.players.OrderByDescending(pl => pl.playerScore))
        {
            PLlist.Add(thisPL);
        }
        for(int i = 0; i<PLlist.Count; i++)
        {
            GameObject thisPLObj = Instantiate(playerPrefab, GameObject.Find("pnl_playerHolder").transform);
            thisPLObj.transform.Find("txt_playerName").GetComponent<TextMeshProUGUI>().text = PLlist[i].playerName;
            thisPLObj.transform.Find("txt_playerScore").GetComponent<TextMeshProUGUI>().text = PLlist[i].playerScore.ToString();
        }
    }

    
    void Update()
    {
        
    }
}
