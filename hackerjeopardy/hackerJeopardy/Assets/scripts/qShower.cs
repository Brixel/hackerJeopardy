using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class qShower : MonoBehaviour
{
    gameSettings gameSettings;
    int presentationType;
    void Start()
    {
        gameSettings = GameObject.Find("scriptHolder").GetComponent<gameSettings>();
        presentationType = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].PresentationType;
        //load players
        gameSettings.loadPlayers();
        //tell players there is no answering yet
        foreach (GameObject player in GameObject.Find("scriptHolder").GetComponent<gameSettings>().playerObjects)
        {
            player.GetComponent<playerScript>().ANSWERS_CLOSED();
        }

        //set the q panel to the right color
        GameObject.Find("pnl_q").GetComponent<Image>().color = new Color32((byte) gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionColorB,255);


        //update category panel colors
        GameObject.Find("design1").transform.Find("q").Find("catPanel").GetComponent<Image>().color = new Color32((byte) gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB,255);
        GameObject.Find("design1").transform.Find("a").Find("catPanel").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB, 255);
        GameObject.Find("design2").transform.Find("q").Find("catPanel").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB, 255);
        GameObject.Find("design2").transform.Find("a").Find("catPanel").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB, 255);
        GameObject.Find("design3").transform.Find("q").Find("catPanel").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB, 255);
        GameObject.Find("design3").transform.Find("a").Find("catPanel").GetComponent<Image>().color = new Color32((byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorR, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorG, (byte)gameSettings.categoryList[gameSettings.selectedC].categoryColorB, 255);

        //update category panel text
        GameObject.Find("design1").transform.Find("q").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;
        GameObject.Find("design1").transform.Find("a").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;
        GameObject.Find("design2").transform.Find("q").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;
        GameObject.Find("design2").transform.Find("a").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;
        GameObject.Find("design3").transform.Find("q").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;
        GameObject.Find("design3").transform.Find("a").Find("catPanel").Find("catName").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].categoryName;

        //update question text empty until answers are open
        GameObject.Find("design1").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("design3").transform.Find("q").Find("qPanel").Find("qName").GetComponent<TextMeshProUGUI>().text = "";

        //hide the picture
        GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color = new Color(GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.r, GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.g, GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().color.b, 0);




        //update answer text
        GameObject.Find("design1").transform.Find("a").Find("aPanel").Find("answer").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answer;
        GameObject.Find("design2").transform.Find("a").Find("aPanel").Find("answer").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answer;
        GameObject.Find("design3").transform.Find("a").Find("aPanel").Find("answer").GetComponent<TextMeshProUGUI>().text = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answer;

        //need to update images?
        if (presentationType == 1)
        {
            if (gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionImage != "")
            {
                string totalPath = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionImage;
                if (File.Exists(totalPath))
                {
                    Texture2D thisTexture = LoadTexture(totalPath);
                    Sprite NewSprite = Sprite.Create(thisTexture, new Rect(0, 0, thisTexture.width, thisTexture.height), new Vector2(0, 0), 100f, 0, SpriteMeshType.Tight);
                    GameObject.Find("design2").transform.Find("q").Find("qPanel").Find("qImg").GetComponent<Image>().sprite = NewSprite;
                }
                else
                {
                    Debug.Log("file does not exist");
                }
            }
        
            if (gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answerImage != "")
            {
                string totalPath = gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answerImage;
                if (File.Exists(totalPath))
                {
                    Texture2D thisTexture = LoadTexture(totalPath);
                    Sprite NewSprite = Sprite.Create(thisTexture, new Rect(0, 0, thisTexture.width, thisTexture.height), new Vector2(0, 0), 100f, 0, SpriteMeshType.Tight);
                    GameObject.Find("design2").transform.Find("a").Find("aPanel").Find("aImg").GetComponent<Image>().sprite = NewSprite;
                }
                else
                {
                    Debug.Log("file does not exist");
                }
            }
        }
        
        //need to update video?
        if(presentationType == 2)
        {
            if (gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionVideo != "")
            {
                GameObject.Find("design3").transform.Find("qVideoLoader").GetComponent<videoLoader>().preLoadVideo("file://" + gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].questionVideo);
                //GameObject.Find("qVideo").GetComponent<VideoPlayer>().SetTargetAudioSource(0, Camera.main.GetComponent<AudioSource>());

            }
            if (gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answerVideo != "")
            {
                GameObject.Find("design3").transform.Find("aVideoLoader").GetComponent<videoLoader>().preLoadVideo("file://" + gameSettings.categoryList[gameSettings.selectedC].questions[gameSettings.selectedQ].answerVideo);
                //GameObject.Find("aVideo").GetComponent<VideoPlayer>().SetTargetAudioSource(0, Camera.main.GetComponent<AudioSource>());
            }
        }
        



        //show the correct panel
        switch (presentationType)
        {
            case 0:
                GameObject.Find("design1").GetComponent<RectTransform>().offsetMin = new Vector2(0, 100);
                GameObject.Find("design1").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;
            case 1:
                GameObject.Find("design2").GetComponent<RectTransform>().offsetMin = new Vector2(0, 100);
                GameObject.Find("design2").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;
            case 2:
                GameObject.Find("design3").GetComponent<RectTransform>().offsetMin = new Vector2(0, 100);
                GameObject.Find("design3").GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;
        }


    }


    void Update()
    {
        
    }

    public void showAnswer()
    {
        switch(presentationType)
        {
            case 0:
                GameObject.Find("design1").transform.Find("q").localScale = new Vector3(0, 0, 0);
                GameObject.Find("design1").transform.Find("a").localScale = new Vector3(1, 1, 1);
                break;
            case 1:
                GameObject.Find("design2").transform.Find("q").localScale = new Vector3(0, 0, 0);
                GameObject.Find("design2").transform.Find("a").localScale = new Vector3(1, 1, 1);
                break;
            case 2:
                GameObject.Find("design3").transform.Find("q").localScale = new Vector3(0, 0, 0);
                GameObject.Find("design3").transform.Find("a").localScale = new Vector3(1, 1, 1);
                GameObject.Find("design3").transform.Find("qVideoLoader").GetComponent<videoLoader>().stopVideo();
                //GameObject.Find("design3").transform.Find("aVideoLoader").GetComponent<videoLoader>().startVideo();
                break;
        }
    }


    private Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Debug.Log(SpriteTexture.width);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
        return NewSprite;
    }
    public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference

        Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {
        FilePath= FilePath.Replace("/","\\");
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(0, 0);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
            {
                return Tex2D;                 // If data = readable -> return texture
            }
            else
            {
                Debug.Log("error");
                return Tex2D;
            }
                
        }
        return null;                     // Return null if load failed
    }
}
