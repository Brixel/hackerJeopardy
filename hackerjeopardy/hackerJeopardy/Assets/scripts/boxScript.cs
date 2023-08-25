using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class boxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public bool isCat = false;
    int myStyle = 0;
    bool isActive = true;
    public int catID;
    public int qID = -1;
    gameSettings gameSettings;
    Color origColor;
    float randStart;
    int catShowing;
    int catChosing;
    int qChosing;

    void Start()
    {
        //find gameSettings
        gameSettings = GameObject.Find("scriptHolder").GetComponent<gameSettings>();

        //set washer color
        transform.Find("innerBox").Find("washer").GetComponent<Image>().color = GameObject.Find("scriptHolder").GetComponent<gameSettings>().questionBoxWasher_normal;
        //start checking availability
        Invoke("checkAvailable", 1f);
        transform.GetComponent<Animator>().Play("doNothing");

        //if we're returning from a question, play a little animation
        if(gameSettings.firstCat == false)
        {
            hideButtonNow();
            Invoke("comeBack", Random.Range(0.2f, 2f));
        }
    }

    void comeBack()
    {
        isActive = true;
        transform.GetComponent<Animator>().Play("box_show");
        Invoke("checkAvailable", 1f);
    }

        // Update is called once per frame
    void Update()
    {

    }


    void loadQ()
    {
        SceneManager.LoadScene("question");
    }

    public void checkAvailable()
    {
        if(isCat == false && qID > -1)
        {
            if (GameObject.Find("scriptHolder").GetComponent<gameSettings>().categoryList[catID].questions[qID].isAvailable == true)
            {
                Invoke("checkAvailable", 1f);
            }
            else
            {
                isActive = false;
                setText("");
                UpdateStyle(2);
            }
        }
    }

    public void UpdateStyle(int style)
    {
        myStyle = style;
        switch (style)
        {
            case 0: //category
                if(GameObject.Find("scriptHolder").GetComponent<gameSettings>().firstCat == false)
                {
                    stopWasher();
                }
                else
                {
                    randStart = Random.Range(0f, 1f);
                    Invoke("startWasher", randStart);
                }
                break;
            case 1: // question available
                randStart = Random.Range(0f, 1f);
                Invoke("startWasher", randStart);
                break;
            case 2: // question unavailable
                transform.Find("innerBox").GetComponent<Image>().color = new Color(transform.Find("innerBox").GetComponent<Image>().color.r, transform.Find("innerBox").GetComponent<Image>().color.g, transform.Find("innerBox").GetComponent<Image>().color.b, 0.5f);                 stopWasher();
                randStart = Random.Range(0f, 1f);
                Invoke("startWasher", randStart);
                break;
            case 3: // empty space
                transform.Find("innerBox").GetComponent<Image>().color = new Color(transform.Find("innerBox").GetComponent<Image>().color.r, transform.Find("innerBox").GetComponent<Image>().color.g, transform.Find("innerBox").GetComponent<Image>().color.b, 0);
                if (GameObject.Find("scriptHolder").GetComponent<gameSettings>().firstCat == false)
                {
                    stopWasher();
                }
                else
                {
                    randStart = Random.Range(0f, 1f);
                    Invoke("startWasher", randStart);
                }
                break;
        }
        if (GameObject.Find("scriptHolder").GetComponent<gameSettings>().firstCat == true)
        {
            hideButtonNow();
        }
    }

    void startWasher()
    {
        transform.Find("innerBox").Find("washer").GetComponent<Animator>().enabled = true;
        transform.Find("innerBox").Find("washer").GetComponent<Animator>().Play("box_washer_anim");
    }

    public void stopWasher()
    {
        transform.Find("innerBox").Find("washer").GetComponent<Animator>().Play("box_washer_stopped");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isActive == true)
        {
            if (myStyle == 1)
            {
                stopWasher();
            }
        }
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(isActive == true)
        {
            if (myStyle == 1)
            {
                randStart = Random.Range(0f, 1f);
                Invoke("startWasher", randStart);
            }
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

    public void clickLogic()
    {
        if (isActive == true)
        {
            if (myStyle == 1)
            {
                transform.Find("innerBox").GetComponent<Image>().color = GameObject.Find("scriptHolder").GetComponent<gameSettings>().questionBoxColor_onClick;
                isActive = false;
                //move box to center and grow until screen size
                foreach (GameObject box in GameObject.Find("boardScripts").GetComponent<questionLoader>().allBoxes)
                {
                    box.GetComponent<boxScript>().hideButton();
                }

                GameObject.Find("scriptHolder").GetComponent<gameSettings>().selectedQ = qID;
                GameObject.Find("scriptHolder").GetComponent<gameSettings>().selectedC = catID;

                //set box in the middle
                transform.position = new Vector3(GameObject.Find("Canvas").transform.position.x, GameObject.Find("Canvas").transform.position.y);

                //disable washer
                stopWasher();

                //fix color?
                Color thisColor = new Color32((byte)gameSettings.categoryList[catID].questions[qID].questionColorR, (byte)gameSettings.categoryList[catID].questions[qID].questionColorG, (byte)gameSettings.categoryList[catID].questions[qID].questionColorB, 255);
                transform.Find("innerBox").GetComponent<Image>().color = thisColor;

                //make sure the text sizes too
                transform.Find("innerBox").Find("catText").GetComponent<TextMeshProUGUI>().enableAutoSizing = true;
                transform.Find("innerBox").Find("catText").GetComponent<TextMeshProUGUI>().fontSizeMax = 100;

                //play grow animation
                transform.GetComponent<Animator>().Play("box_chosen");

                //play downslide sound
                GameObject.Find("scriptHolder").GetComponent<soundScript>().playDownSlide();
                Invoke("playSoundSelected", 0.5f);

                //load Q
                Invoke("loadQ", 4.5f);
            }

        }
    }

    void playSoundSelected()
    {
        GameObject.Find("scriptHolder").GetComponent<soundScript>().playLoaded();
    }

    void disableButton()
    {
        stopWasher();
        isActive = false;
    }

    public void hideButton()
    {
        //enable animator
        transform.GetComponent<Animator>().enabled = true;
        //deactivate button
        isActive = false;
        //stop washer
        stopWasher();
        //play hidey animation
        transform.GetComponent<Animator>().Play("box_hide");
    }

    public void hideButtonNow()
    {
        //enable animator
        transform.GetComponent<Animator>().enabled = true;
        //deactivate button
        isActive = false;
        //play hidey animation
        transform.GetComponent<Animator>().Play("box_hideNow");
    }

    public void showCatBox()
    {
            //enable animator
            transform.GetComponent<Animator>().enabled = true;
            //deactivate button
            isActive = false;
            //stop washer
            stopWasher();
            //play show anim
            transform.GetComponent<Animator>().Play("box_show");
            //play sound
            GameObject.Find("scriptHolder").GetComponent<soundScript>().playDing();
    }
    public void showQBox()
    {
        //enable animator
        transform.GetComponent<Animator>().enabled = true;
        //activate button
        isActive = true;
        //play show anim
        transform.GetComponent<Animator>().Play("box_show");
        //start washer
        randStart = Random.Range(0f, 1f);
        Invoke("startWasher", randStart);
        //play sound
        GameObject.Find("scriptHolder").GetComponent<soundScript>().playWhoosh();
    }

    public void setChosingCatID(int theID)
    {
        catChosing = theID;
    }
    public void setChosingQID(int theID)
    {
        qChosing = theID;
        if(catChosing == catID && qChosing == qID)
        {
            //this is mine!
            //call click logic
            clickLogic();
        }
    }

    public void setText(string theText)
    {
        transform.Find("innerBox").Find("catText").GetComponent<TextMeshProUGUI>().text = theText;
    }

}
