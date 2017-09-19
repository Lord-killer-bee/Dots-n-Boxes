using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChainDetector : MonoBehaviour {
    
    public static ChainDetector instance;
    public GameObject line;
    public GameObject panel;
    public GameObject chainBot;
    public ButtonMan bm;
    public LevelSetter ls;
    public int LessThan2Count;
    public List<int> LessThan2Boxes = new List<int>();
    public List<int> masterList = new List<int>();
    public List<GameObject> botsList = new List<GameObject>();
    public List<GameObject> usedBots = new List<GameObject>();
    public List<Chain> chainLists = new List<Chain>();

    private int column, row;
    private float raycastHorizontal, raycastVertical;

    // Use this for initialization
    void Start () {
        if (instance == null) instance = this;

        raycastHorizontal = ( ls.buttonsVertical[(int)ls.n - 1].transform.position - bm.boxes[0].transform.position ).x;
        raycastVertical = (ls.buttonsHorizontal[0].transform.position - bm.boxes[0].transform.position).y;
    }
	

    public void SearchForPivot()
    {
        masterList.Clear();

        LessThan2Count = 0;
        for (int i = 0; i < AImanager.instance.boxScores.Count; i++)
        {
            if(AImanager.instance.boxScores[i] >= 2 && AImanager.instance.boxScores[i] < 4)
            {
                masterList.Add(i);
            }
            else
            {
                LessThan2Count++;
            }
        }

        if (LessThan2Count <= ls.n)
        {
            AddRemainingBoxes();
        }

    }

    void AddRemainingBoxes()
    {
        LessThan2Boxes.Clear();

        AImanager.instance.stage1Done = true;
        for (int i = 0; i < AImanager.instance.boxScores.Count; i++)
        {
            if(AImanager.instance.boxScores[i] < 2)
            {
                int c = (i % ((int)ls.n - 1));
                int r = (i - c) / ((int)ls.n - 1);

                AImanager.instance.SetReferenceArray(i);

                for (int j = 0; j < AImanager.instance.referenceList.Count; j++)
                {
                    bool flag = false;

                    if (j == 0)
                    {
                        if (r != 0)
                        {
                            if (AImanager.instance.referenceList[j].activeSelf && AImanager.instance.boxScores[i - ((int)ls.n - 1)] < 2)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                        else
                        {
                            if (AImanager.instance.referenceList[j].activeSelf)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                    }

                    if (j == 1)
                    {
                        if (r != ((int)ls.n - 2))
                        {
                            if (AImanager.instance.referenceList[j].activeSelf && AImanager.instance.boxScores[i + ((int)ls.n - 1)] < 2)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                        else
                        {
                            if (AImanager.instance.referenceList[j].activeSelf)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                    }

                    if (j == 2)
                    {
                        if (c != 0)
                        {
                            if (AImanager.instance.referenceList[j].activeSelf && AImanager.instance.boxScores[i - 1] < 2)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                        else
                        {
                            if (AImanager.instance.referenceList[j].activeSelf)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                    }

                    if (j == 3)
                    {
                        if (c != ((int)ls.n - 2))
                        {
                            if (AImanager.instance.referenceList[j].activeSelf && AImanager.instance.boxScores[i + 1] < 2)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                        else
                        {
                            if (AImanager.instance.referenceList[j].activeSelf)
                            {
                                LessThan2Boxes.Add(i);
                                break;
                            }
                        }
                    }
                }

            }
        }

    }

    public void DeployBots()
    {
        foreach (GameObject bot in botsList)
        {
            Destroy(bot);    
        }
        botsList.Clear();

        for(int i = 0; i < masterList.Count; i++)
        {
            GameObject botClone = Instantiate(chainBot) as GameObject;
            botClone.transform.SetParent(panel.transform);
            botClone.GetComponent<RectTransform>().anchoredPosition = bm.boxes[masterList[i]].GetComponent<RectTransform>().anchoredPosition;
            botClone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            botClone.name = i.ToString();
            botsList.Add(botClone);
        }
        
    }

    public void StartSearching()
    {
        chainLists.Clear();
        usedBots.Clear();
        foreach (GameObject bot in botsList)
        {
            if (!CheckifBotisUsed(bot))
            {               
                Chain instance = new Chain();
                chainLists.Add(instance);
                SearchForNeighbours(masterList[int.Parse(bot.name)], instance);
            }               
        }
        InsertionSort(chainLists);
    }

    bool CheckifBotisUsed(GameObject bot)
    {
        int count = 0;
        foreach (GameObject item in usedBots)
        {
            if(item.name == bot.name)
            {
                count++;
            }    
        }
        if(count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SearchForNeighbours(int index, Chain instance)
    {
            List<int> neighboursList = new List<int>();

            column = (index % ((int)ls.n - 1));
            row = (index - column) / ((int)ls.n - 1);

            //top middle row
            if (row == 0 && column != 0 && column != ((int)ls.n - 2))
            {
                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                   
                }

                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            //bottom middle row
            if (row == ((int)ls.n - 2) && column != 0 && column != ((int)ls.n - 2))
            {
                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                }

                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }

            }

            //left middle column
            if (column == 0 && row != 0 && row != ((int)ls.n - 2))
            {
                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            //right middle column
            if (column == ((int)ls.n - 2) && row != 0 && row != ((int)ls.n - 2))
            {

                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            //top left corner
            if (row == 0 && column == 0)
            {
                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            //top right corner
            if (row == 0 && column == ((int)ls.n - 2))
            {
                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            //bottom left corner
            if (row == ((int)ls.n - 2) && column == 0)
            {
                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }
            }

            //bottom right corner
            if (row == ((int)ls.n - 2) && column == ((int)ls.n - 2))
            {
                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }
            }

            //middle boxes
            if (row != 0 && column != 0 && row != ((int)ls.n - 2) && column != ((int)ls.n - 2))
            {
                if (bm.boxes[index - 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - 1);
                }

                if (bm.boxes[index + 1].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + 1);
                }

                if (bm.boxes[index - ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index - ((int)ls.n - 1));
                }

                if (bm.boxes[index + ((int)ls.n - 1)].GetComponent<Box>().buttonScore >= 2)
                {
                    neighboursList.Add(index + ((int)ls.n - 1));
                }
            }

            BotRayCast(neighboursList, index, instance);
            neighboursList.Clear();

    }

    void BotRayCast(List<int> list, int index, Chain instance)
    {
        GameObject bot = null;
        for (int i = 0; i < masterList.Count; i++)
        {
            if(index == masterList[i])
            {
                 bot = botsList[i];
            }    
        }

        RaycastHit2D righthit = Physics2D.Raycast(bot.transform.position, Vector2.right, raycastHorizontal);
        RaycastHit2D lefthit = Physics2D.Raycast(bot.transform.position, Vector2.left, raycastHorizontal);
        RaycastHit2D tophit = Physics2D.Raycast(bot.transform.position, Vector2.up, raycastVertical);
        RaycastHit2D bottomhit = Physics2D.Raycast(bot.transform.position, Vector2.down, raycastVertical);

        usedBots.Add(bot);

        if (!instance.chain.Contains(index))
        {
            instance.chain.Add(index);
        }

        if (righthit.collider != null)
        {
            if (list.Contains(index + 1) && !instance.chain.Contains(index + 1))
            {
                instance.chain.Add(index + 1);
                SearchForNeighbours(index + 1, instance);
            }      
        }
        if (lefthit.collider != null)
        {
            if (list.Contains(index - 1) && !instance.chain.Contains(index - 1))
            {
                instance.chain.Add(index - 1);
                SearchForNeighbours(index - 1, instance);
            }

        }
        if (tophit.collider != null)
        {
            if (list.Contains(index - ((int)ls.n - 1)) && !instance.chain.Contains(index - ((int)ls.n - 1)))
            {
                instance.chain.Add(index - ((int)ls.n - 1));
                SearchForNeighbours(index - ((int)ls.n - 1), instance);
            }

        }
        if (bottomhit.collider != null)
        {
            if (list.Contains(index + ((int)ls.n - 1)) && !instance.chain.Contains(index + ((int)ls.n - 1)))
            {
                instance.chain.Add(index + ((int)ls.n - 1));
                SearchForNeighbours(index + ((int)ls.n - 1), instance);
            }

        }           
      
    }

    void InsertionSort(List<Chain> inputarray)
    {
        for (int i = 0; i < inputarray.Count - 1; i++)
        {
            int j = i + 1;

            while (j > 0)
            {
                if (inputarray[j - 1].chain.Count > inputarray[j].chain.Count)
                {
                    Chain temp = inputarray[j - 1];
                    inputarray[j - 1] = inputarray[j];
                    inputarray[j] = temp;

                }
                j--;
            }
        }
    }

}

[System.Serializable]
public class Chain
{
    public List<int> chain = new List<int>();
}
