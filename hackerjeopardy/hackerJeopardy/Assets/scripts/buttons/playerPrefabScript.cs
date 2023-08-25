using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class playerPrefabScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setName(string theName)
    {
        transform.Find("pnl_playerName").Find("txt_playerName").GetComponent<TextMeshProUGUI>().text = theName;
    }

    public void goAway()
    {
        Destroy(this.gameObject);
    }
}
