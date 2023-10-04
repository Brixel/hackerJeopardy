using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Linq;

public class op_initScripts : MonoBehaviour
{
    public GameObject playerPrefab;
    public int windowState;
    // Start is called before the first frame update
    void Start()
    {
        windowState = 0;

        //pop some dummy data
        //setDummyData();

        //check if we have some data in the system yet
        gameSettings gs = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        if(gs.players.Count > 0)
        {
            //render players
            renderPlayers();
        }
        if(gs.categoryList.Count > 0)
        {
            //update stats
            GameObject.Find("btn_importFile").GetComponent<btn_importFile>().updateStats();
        }
    }

    void setDummyData()
    {
        Category thisCat = new Category();
        thisCat.categoryName = "CAT1";
        thisCat.categoryColorR = 0;
        thisCat.categoryColorG = 0;
        thisCat.categoryColorB = 255;
        thisCat.questions = new List<Question>();
        Question thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 200;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 400;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 600;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Add(thisCat);
        thisCat = new Category();
        thisCat.categoryName = "CAT2";
        thisCat.categoryColorR = 0;
        thisCat.categoryColorG = 0;
        thisCat.categoryColorB = 255;
        thisCat.questions = new List<Question>();
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 200;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 400;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 600;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Add(thisCat);
        thisCat = new Category();
        thisCat.categoryName = "CAT3";
        thisCat.categoryColorR = 0;
        thisCat.categoryColorG = 0;
        thisCat.categoryColorB = 255;
        thisCat.questions = new List<Question>();
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 200;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 400;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        thisQ = new Question();
        thisQ.questionColorR = 0;
        thisQ.questionColorG = 200;
        thisQ.questionColorB = 88;
        thisQ.questionText = "que?";
        thisQ.value = 600;
        thisQ.answer = "que!";
        thisQ.PresentationType = 0;
        thisQ.isAvailable = true;
        thisCat.questions.Add(thisQ);
        GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Add(thisCat);

        Player thisPlayer = new Player();
        thisPlayer.playerName = "kefcom";
        thisPlayer.playerScore = 0;
        thisPlayer.answerButton = KeyCode.F1;
        GameObject.Find("scriptHolder").GetComponent<gameSettings>().players.Add(thisPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPlayer()
    {
        if(windowState == 0) // check if other windows are open
        {
            //show add user window
            GameObject.Find("window_addPlayer").transform.position = new Vector3(GameObject.Find("window_addPlayer").transform.position.x, 0, 0);
            //set focus to input field
            GameObject.Find("inp_playerName").GetComponent<TMP_InputField>().ActivateInputField();
            //set the button to add player active so we can trigger on enter
            GameObject.Find("window_addPlayer").transform.Find("pnl_overlay").Find("pnl_window").Find("btn_playerAdd").GetComponent<btn_addPlayer2>().isActive = true;
            //clear errors
            GameObject.Find("txt_playerNameError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);
            GameObject.Find("txt_playerLimitError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);
        }
    }

    public void hideAddPlayerWindow()
    {
        //hide window
        GameObject.Find("window_addPlayer").transform.position = new Vector3(GameObject.Find("window_addPlayer").transform.position.x, 1112, 0);
        //set button active to false
        GameObject.Find("window_addPlayer").transform.Find("pnl_overlay").Find("pnl_window").Find("btn_playerAdd").GetComponent<btn_addPlayer2>().isActive = false;
        //reset error message
        GameObject.Find("txt_playerNameError").GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, 0);
        //no more open windows
        windowState = 0;
    }

    public void addTeam()
    {
        if (windowState == 0) // check if other windows are open
        {
            //show import window
            GameObject.Find("window_importTeams").transform.position = new Vector3(GameObject.Find("window_importTeams").transform.position.x, 0, 0);

            //clear dropdown box
            GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().options.Clear();

            //populate dropdown box
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            int foundDrives = 0;
            foreach (DriveInfo thisDrive in allDrives)
            {
                if (thisDrive.DriveType == DriveType.Removable)
                {
                    foundDrives++;
                    TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
                    thisOption.text = thisDrive.Name;
                    GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().options.Add(thisOption);
                }
            }

            GameObject.Find("drp_importTeamDriveList").GetComponent<TMP_Dropdown>().RefreshShownValue();

            //enable monitor
            GameObject.Find("btn_importTeam").GetComponent<btn_importTeam>().setActive();

            //if there are files in the list, set the last index to a high number to trigger a file scan
            if (foundDrives > 0)
            {
                GameObject.Find("btn_importTeam").GetComponent<btn_importTeam>().lastIndex = 99999;
            }

        }
    }

    public void hideAddTeamWindow()
    {
        //hide window
        GameObject.Find("window_importTeams").transform.position = new Vector3(GameObject.Find("window_importTeams").transform.position.x, 1112, 0);

        //no more open windows
        windowState = 0;

        //deactivate monitor
        GameObject.Find("btn_importTeam").GetComponent<btn_importTeam>().setInActive();
    }

    public void renderPlayers()
    {
        //remove players (if needed)
        if(GameObject.Find("pnl_players").transform.Find("pnl_content").transform.childCount > 0)
        {
            foreach (GameObject player in GameObject.Find("scriptHolder").GetComponent<gameSettings>().playerObjects)
            {
                //remove player
                player.GetComponent<playerPrefabScript>().goAway();
            }
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().playerObjects.Clear();
        }

        int playerCounter = 0;
        //add players

        //window size: 550
        //object size: 55
        foreach(Player thisPlayer in GameObject.Find("scriptHolder").GetComponent<gameSettings>().players)
        {
            GameObject thisPlayerObject = Instantiate(playerPrefab, GameObject.Find("pnl_players").transform.Find("pnl_content").transform);
            thisPlayerObject.GetComponent<playerPrefabScript>().setName((playerCounter + 1).ToString() + " | " + thisPlayer.playerName);
            thisPlayerObject.transform.Find("hiddenPlayerName").GetComponent<TextMeshProUGUI>().text = thisPlayer.playerName;
            thisPlayerObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0 + (550 - (55*(playerCounter+1)))); // left, bottom
            thisPlayerObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0-(55 * (playerCounter))); // right,top
            playerCounter++;
            GameObject.Find("scriptHolder").GetComponent<gameSettings>().playerObjects.Add(thisPlayerObject);
        }
    }

    public void openNewQuestionsWindow()
    {
        if (windowState == 0) // check if other windows are open
        {
            //show new questions window
            GameObject.Find("window_newQuestionsFile").transform.position = new Vector3(GameObject.Find("window_newQuestionsFile").transform.position.x, 0, 0);

            //switch tab script
            disableTabScripts();
            GameObject.Find("window_newQuestionsFile").GetComponent<tabScript>().enabled = true;

            //clear dropdown box
            GameObject.Find("drp_driveList").GetComponent<TMP_Dropdown>().options.Clear();

            //populate dropdown box
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach(DriveInfo thisDrive in allDrives)
            {
                if(thisDrive.DriveType == DriveType.Removable)
                {
                    TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
                    thisOption.text = thisDrive.Name;
                    GameObject.Find("drp_driveList").GetComponent<TMP_Dropdown>().options.Add(thisOption);
                }
            }

            GameObject.Find("drp_driveList").GetComponent<TMP_Dropdown>().RefreshShownValue();


        }
    }

    public void hideNewQuestionsWindow()
    {
        //hide window
        GameObject.Find("window_newQuestionsFile").transform.position = new Vector3(GameObject.Find("window_newQuestionsFile").transform.position.x, 1112, 0);
        //remove the validation error
        GameObject.Find("txt_newFileError").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);

        disableTabScripts();
        //no more open windows
        windowState = 0;
    }

    public void showEditQuestionsWindow()
    {
        if (windowState == 0) // check if other windows are open
        {
            //show edit window
            GameObject.Find("window_editQuestions").transform.position = new Vector3(GameObject.Find("window_editQuestions").transform.position.x, 0, 0);

            //enable tab script
            disableTabScripts();
            GameObject.Find("window_editQuestions").GetComponent<tabScript>().enabled = true;

            //update the category dropdown
            updateCdrp();

            //update catID if needed
            if(GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Count > 0)
            {
                GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text = GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value.ToString();
            }
            else
            {
                GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text = "-1"; //indicating new cat
            }

            //set monitor active
            GameObject.Find("btn_catSave").GetComponent<btn_catSave>().setActive();
            GameObject.Find("btn_qSave").GetComponent<btn_qSave>().setActive();

        }
    }
    public void hideEditWindow()
    {
        //hide window
        GameObject.Find("window_editQuestions").transform.position = new Vector3(GameObject.Find("window_editQuestions").transform.position.x, 1112, 0);
        //remove the validation errors
        GameObject.Find("txt_qValError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);
        GameObject.Find("txt_qColorError").GetComponent<TextMeshProUGUI>().color = new Color(128, 0, 0, 0);

        //no more open windows
        windowState = 0;

        //disable tab script
        disableTabScripts();

        //save file and update counts
        GameObject.Find("scriptHolder").GetComponent<gameSettings>().saveFile();
        //set current game name
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_gamename").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().gameName;

        //set current cat count
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_ccount").GetComponent<TextMeshProUGUI>().text = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList.Count.ToString();

        //set current q count
        int totalQ = 0;
        foreach(Category thisCat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
        {
            totalQ = totalQ + thisCat.questions.Count;
        }
        GameObject.Find("pnl_currentQuestions").transform.Find("pnl_content").Find("txt_qcount").GetComponent<TextMeshProUGUI>().text = totalQ.ToString();

        //set monitor inactive
        GameObject.Find("btn_catSave").GetComponent<btn_catSave>().setInActive();
        GameObject.Find("btn_qSave").GetComponent<btn_qSave>().setInActive();
    }

        public void updateCdrp()
        {
        //update categories dropdown menu
        //first clear the existing items
        GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Clear();
        //populate new items
        foreach(Category thisCat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
        {
            TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
            thisOption.text = thisCat.categoryName;
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Add(thisOption);
        }

        //select last option
        if(GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Count > 0)
        {
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value = GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Count - 1;
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().RefreshShownValue();
            //set lastindex to something too big so the script triggers to update info
            GameObject.Find("btn_catSave").GetComponent<btn_catSave>().lastIndex = 999999;
        }
        else
        {
            //clear the list text
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().RefreshShownValue();
            //we are adding a new value
            GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text = "";
            GameObject.Find("inp_catName").GetComponent<TMP_InputField>().ActivateInputField();
            GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text = "-1";
            GameObject.Find("btn_catSave").GetComponent<btn_catSave>().lastIndex = -1;
        }
    }

    public void updateQdrp()
    {

        //update questions dropdown menu
        //first clear the existing items
        GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().options.Clear();
        GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().RefreshShownValue();
        GameObject.Find("inp_qVal").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("inp_qTxt").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("inp_aTxt").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("inp_mPath").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("inp_aMPath").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("inp_qNote").GetComponent<TMP_InputField>().text = "";
        GameObject.Find("drp_qType").GetComponent<TMP_Dropdown>().value = 0;
        GameObject.Find("txt_qID").GetComponent<TextMeshProUGUI>().text = "-1";

        //sort all the Q's in each C
        foreach (Category thisCat in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList)
        {
            thisCat.questions = thisCat.questions.OrderBy(w => w.value).ToList();
        }

        //populate new items (if possible)
        if (int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text) >= 0)
        {
            //there is a cat selected
            Category thisCat = GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)];
            if(thisCat.questions.Count > 0)
            {
                //sort the list according to value
                GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].questions.OrderBy(x => x.value);

                //the cat has questions
                foreach (Question thisQ in GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[int.Parse(GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text)].questions)
                {
                    TMP_Dropdown.OptionData thisData = new TMP_Dropdown.OptionData();
                    thisData.text = thisQ.value.ToString();
                    GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().options.Add(thisData);
                }

                GameObject.Find("drp_questions").GetComponent<TMP_Dropdown>().RefreshShownValue();
                //set lastindex to something too big so the script triggers to update info
                GameObject.Find("btn_qSave").GetComponent<btn_qSave>().lastIndex = 999999;
            }
        }
    }

    public void showImportWindow()
    {
        if (windowState == 0) // check if other windows are open
        {
            //show import window
            GameObject.Find("window_importFile").transform.position = new Vector3(GameObject.Find("window_importFile").transform.position.x, 0, 0);

            //clear dropdown box
            GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().options.Clear();

            //populate dropdown box
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            int foundDrives = 0;
            foreach (DriveInfo thisDrive in allDrives)
            {
                if (thisDrive.DriveType == DriveType.Removable)
                {
                    foundDrives++;
                    TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
                    thisOption.text = thisDrive.Name;
                    GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().options.Add(thisOption);
                }
            }

            GameObject.Find("drp_importDriveList").GetComponent<TMP_Dropdown>().RefreshShownValue();

            //enable monitor
            GameObject.Find("btn_importFile").GetComponent<btn_importFile>().setActive();

            //if there are files in the list, set the last index to a high number to trigger a file scan
            if(foundDrives > 0)
            {
                GameObject.Find("btn_importFile").GetComponent<btn_importFile>().lastIndex = 99999;
            }

        }
    }

    public void hideImportWindow()
    {
        //hide window
        GameObject.Find("window_importFile").transform.position = new Vector3(GameObject.Find("window_importFile").transform.position.x, 1112, 0);

        //no more open windows
        windowState = 0;

        //deactivate monitor
        GameObject.Find("btn_importFile").GetComponent<btn_importFile>().setInActive();

    }

    void disableTabScripts()
    {
        GameObject.Find("window_newQuestionsFile").GetComponent<tabScript>().enabled = false;
        GameObject.Find("window_editQuestions").GetComponent<tabScript>().enabled = false;
    }

}
