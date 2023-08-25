using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class tabScript : MonoBehaviour
{
    int tab_indexes;
    int currentIndex;
    public List<TMP_InputField> fieldList = new List<TMP_InputField>();

    void Start()
    {
        tab_indexes = fieldList.Count;
        currentIndex = 0;
        fieldList[currentIndex].ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(isShiftKeyDown)
            {
                currentIndex--;
                if (currentIndex == -1)
                {
                    currentIndex = (fieldList.Count-1);
                }
            }
            else
            {
                currentIndex++;
                if (currentIndex == fieldList.Count)
                {
                    currentIndex = 0;
                }
            }
            fieldList[currentIndex].ActivateInputField();
        }
    }
}
