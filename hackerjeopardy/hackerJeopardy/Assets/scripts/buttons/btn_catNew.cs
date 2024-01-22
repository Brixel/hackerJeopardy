using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class btn_catNew : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
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
            isClicked = true;
            transform.Find("pnl_icon").GetComponent<Image>().color = new Color(0, 255, 0, 255);
            transform.Find("pnl_text").Find("btnTxt").GetComponent<TextMeshProUGUI>().color = new Color(0, 255, 0, 255);
            Invoke("resetColor", 1f);

            TMP_Dropdown.OptionData thisOption = new TMP_Dropdown.OptionData();
            thisOption.text = "";
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Add(thisOption);
            GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().value = GameObject.Find("drp_cats").GetComponent<TMP_Dropdown>().options.Count;

            GameObject.Find("inp_catName").GetComponent<TMP_InputField>().text = "";
            GameObject.Find("inp_catHint").GetComponent<TMP_InputField>().text = "";
            GameObject.Find("inp_catName").GetComponent<TMP_InputField>().ActivateInputField();
            GameObject.Find("txt_catID").GetComponent<TextMeshProUGUI>().text = "-1";
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