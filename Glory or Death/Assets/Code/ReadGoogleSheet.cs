using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ReadGoogleSheet : MonoBehaviour
{
    /*public int[] order_of_tiles;
    string[] notes;
    public TextMeshProUGUI display;
    int i = 0;
    int levelno = 1;

    string rowsjson = "";
    string[] lines;
    List<string> eachrow;
    private GameObject sectionName;

    void Start()
    {
        
        StartCoroutine(ObtainSheetData());
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    /*public void takefromCSV()
    {
        StartCoroutine(ObtainSheetData());
    }*/
   
    /*IEnumerator ObtainSheetData()
    {
        //sectionName = GameObject.Find("SectionName");
        UnityWebRequest www = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1AoyJcY9NrOYcXUX8o7PZUKtxGfMTlk5R83yGexrb57Y/values/Consumables?key=AIzaSyDBRRL33WBuDwQHJ_j1vdMIM_QMmhi25SI");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError || www.timeout > 2)
        {
            Debug.Log("error" + www.error);
            Debug.Log("Offline");
        }
        else
        {
            //networkerror.SetActive(false);
            string json = www.downloadHandler.text;
            var o = JSON.Parse(json);
            foreach (var item in o["values"])
            {
                var itemo = JSON.Parse(item.ToString());
                eachrow = itemo[0].AsStringList;
                foreach (var bro in eachrow)
                {
                    if(bro == "Mana Potion")
                    {
                        Debug.Log("He encontrado Mana: " + bro);
                    }

                    rowsjson += bro + ",";
                }
                rowsjson += "\n";
            }
            lines = rowsjson.Split(new char[] { '\n' });
            notes = lines[levelno].Split(new char[] { ',' });
            display.text = notes[0];
        }
    }*/
}
