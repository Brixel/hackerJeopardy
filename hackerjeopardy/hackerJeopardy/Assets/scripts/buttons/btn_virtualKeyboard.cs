using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_virtualKeyboard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public string keybLetter;
    public TMP_InputField targetBox;

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
        transform.Find("txtKey").GetComponent<TextMeshProUGUI>().text = keybLetter;
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
        if(isClicked == false)
        {
            isClicked = true;
            switch(keybLetter)
            {
                case "<":
                    if(targetBox.text.Length > 0)
                    {
                        targetBox.text = targetBox.text.Remove(targetBox.text.Length - 1, 1);
                    }
                    break;
                default:
                    targetBox.text = targetBox.text + keybLetter;
                    break;
            }
            
            Invoke("resetColor", 0.5f);
        }
        
        
    }

    void resetColor()
    {
        transform.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a);
        isClicked = false;
    }
}
