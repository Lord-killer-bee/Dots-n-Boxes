using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelSetter : MonoBehaviour {

    public Button pref;
    public ButtonMan buttonMan;
    public GameObject panel;
    public GameObject box;

    private float tileHeight;
    private float tileWidth;

    [HideInInspector]
    public float dotHeight = 10;
    [HideInInspector]
    public float dotWidth = 10;

    public float n;
    private float noOfLines;

    [HideInInspector]
    public float lineHeight, lineWidth;

    private int rotationCode;
    private int boxname = 0;

    public List<GameObject> buttonsHorizontal = new List<GameObject>();
    public List<GameObject> buttonsVertical = new List<GameObject>();

    // Use this for initialization
    void Start () {
        tileHeight = (panel.GetComponent<RectTransform>().rect.height) * 0.75f;
        tileWidth = (panel.GetComponent<RectTransform>().rect.width) * 0.75f;

        lineHeight = (tileHeight - (n * dotHeight)) / (n - 1);
        lineWidth = (tileWidth - (n * dotWidth)) / (n - 1);
        noOfLines = 2 * n * (n - 1);
        ArrangeButtons();
        ArrangeBoxes();        
    }

    void ArrangeButtons()
    {
        int j = 0;
        int k = 0;

        for (int i = 0; i < noOfLines / 2; i++)
        {
            if(j < n - 1)
            {
                rotationCode = 1;
                PlaceButtons(new Vector3((j + 0.5f) * (lineWidth + dotWidth),
                             -k * (lineHeight + dotHeight), 1),
                             new Vector2(lineWidth, dotHeight),
                             i);
                j++;
                if(j == n - 1)
                {
                    j = 0;
                    k++;
                }
            }
        }

       j = 0;
       k = 0;
        for (int i = 0; i < noOfLines / 2; i++)
        {
            if (j < n - 1)
            {
                rotationCode = -1;
                PlaceButtons(new Vector3( k * (lineWidth + dotWidth),
                             -(j + 0.5f) * (lineHeight + dotHeight), 1),
                             new Vector2(lineHeight, dotWidth),
                             i);
                j++;
                if (j == n - 1)
                {
                    j = 0;
                    k++;
                }
            }
        }

    }


    void PlaceButtons(Vector3 position, Vector3 size, int index)
    {
        Button temp = GameObject.Instantiate(pref, new Vector3(0, 0, 1), Quaternion.identity) as Button;
        temp.onClick.AddListener(() => buttonMan.ButtonPress(temp.gameObject));
        temp.transform.SetParent(panel.transform);
        temp.name = index.ToString();

        temp.GetComponent<RectTransform>().anchoredPosition = position;
        temp.GetComponent<RectTransform>().sizeDelta = size;
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if(rotationCode == 1)
        {
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.tag = "horizontal";
            buttonsHorizontal.Add(temp.gameObject);
        }
        else if(rotationCode == -1)
        {
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 90);
            temp.tag = "vertical";
            buttonsVertical.Add(temp.gameObject);
        }
    }

    void ArrangeBoxes()
    {       
        int j = 0;
        int k = 0;
        for (int i = 0; i < (n - 1) * (n - 1); i++)
        {
            AImanager.instance.boxScores.Add(0);

            PlaceBoxes(new Vector3((lineWidth + dotWidth) * ( j + 0.5f ), (-lineHeight - dotHeight) * (k + 0.5f), 0), boxname);
            j++;
            boxname++;
            if(j > n - 2)
            {
                j = 0;
                k++;
            }
        }

    }

    void PlaceBoxes(Vector3 position, int boxname)
    {
        GameObject temp = Instantiate(box, panel.transform.position, Quaternion.identity) as GameObject;
        temp.transform.SetParent(GameObject.FindGameObjectWithTag("Base").transform);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        temp.GetComponent<RectTransform>().anchoredPosition = position;
        temp.GetComponent<RectTransform>().sizeDelta = new Vector3(lineWidth + 2 * dotWidth, lineHeight + 2 * dotHeight, 1);
        temp.name = boxname.ToString();
        temp.SetActive(false);
        buttonMan.boxes.Add(temp);
    }
}
