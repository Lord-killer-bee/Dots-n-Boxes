using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonMan : MonoBehaviour {

    public GameObject red;
    public LevelSetter ls;

    private GameObject nearestBox;

    public List<GameObject> boxes = new List<GameObject>();


    public void ButtonPress(GameObject button)
    {
        if(GameplayMan.instance.grantTurn == true)
        {
            GameplayMan.instance.grantTurn = false;
        }

        string _tag = button.tag;
        button.SetActive(false);
        GameObject temp = Instantiate(red, button.transform.position, button.transform.rotation) as GameObject;
        temp.transform.SetParent(GameObject.FindGameObjectWithTag("Base").transform);
        temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if (_tag ==  "horizontal")
        {
            temp.GetComponent<RectTransform>().sizeDelta = new Vector3(ls.lineWidth, ls.dotHeight, 0);
        }

        else if (_tag == "vertical")
        {
            temp.GetComponent<RectTransform>().sizeDelta = new Vector3(ls.lineHeight, ls.dotWidth, 0);
        }

        AddScore(button);
        //Destroy(button);
        button.SetActive(false);
        ChainDetector.instance.SearchForPivot();
        ChainDetector.instance.DeployBots();
        ChainDetector.instance.StartSearching();

        
        GameplayMan.instance.SwapPlayers();
    }



    void AddScore(GameObject button)
    {
        int index = int.Parse(button.name);
        if (button.tag == "horizontal")
        {
            if (index - ((int)ls.n - 1) < 0)
            {
                boxes[index].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(index);
            }

            else if(index - (((int)ls.n - 1) * ((int)ls.n - 1)) >= 0)
            {
                boxes[index - ((int)ls.n - 1)].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(index - ((int)ls.n - 1));
            }

            else 
            {
                boxes[index].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(index);
                boxes[index - ((int)ls.n - 1)].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(index - ((int)ls.n - 1));
            }

        }


        if (button.tag == "vertical")
        {
            int column = 0;
            int row = 0;

            column = (index - (index % ((int)ls.n - 1))) / ((int)ls.n - 1);
            row = index - (((int)ls.n - 1) * column);

            if(column == 0)
            {
                boxes[((int)ls.n - 1) * row].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(((int)ls.n - 1) * row);
            }

            else if (column == ((int)ls.n - 1))
            {
                boxes[column + (((int)ls.n - 1) * row) - 1].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(column + (((int)ls.n - 1) * row) - 1);
            }

            else
            {
                boxes[column + (((int)ls.n - 1) * row)].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(column + (((int)ls.n - 1) * row));
                boxes[column + (((int)ls.n - 1) * row) - 1].GetComponent<Box>().ScoreIncrement();
                AImanager.instance.UpdateScoreList(column + (((int)ls.n - 1) * row) - 1);
            }
        }
    }
}
