using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AImanager : MonoBehaviour {

    public static AImanager instance;

    [SerializeField]
    private LevelSetter ls;

    [SerializeField]
    private ButtonMan bm;

    public List<int> boxScores = new List<int>();

    public List<GameObject> referenceList = new List<GameObject>();
    public List<int> listOfAll3s = new List<int>();
    public List<int> sizeof3chains = new List<int>();

    private int column, row;
    public bool stage1Done = false;
    public bool stage2Done = false;

    // Use this for initialization
    void Awake () {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetReferenceArray(int index)
    {
        referenceList.Clear();

        column = (index % ((int)ls.n - 1));
        row = (index - column) / ((int)ls.n - 1);

        referenceList.Add(ls.buttonsHorizontal[index]);
        referenceList.Add(ls.buttonsHorizontal[index + ((int)ls.n - 1)]);
        referenceList.Add(ls.buttonsVertical[row + (((int)ls.n - 1) * column)]);
        referenceList.Add(ls.buttonsVertical[row + ((int)ls.n - 1) + (((int)ls.n - 1) * column)]);

    }


    public void UpdateScoreList(int index)
    {
        boxScores[index]++;
    }

    public void AImove()
    {
        StartCoroutine(AIcoroutine());
    }

    IEnumerator AIcoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        listOfAll3s.Clear();
        sizeof3chains.Clear();
        foreach (Chain item in ChainDetector.instance.chainLists)
        {
            foreach (int element in item.chain)
            {
                if(boxScores[element] == 3)
                {
                    listOfAll3s.Add(element);
                    sizeof3chains.Add(item.chain.Count);
                }    
            }   
        }

        if (stage1Done == false)
        {
            Stage_1_MoveChecker();
        }
        else if(stage2Done == false)
        {
            Stage_2_MoveChecker();
        }
        else
        {
            Stage_3_MoveChecker();
        }
    }

    void Stage_1_MoveChecker()
    {
        if (GameplayMan.instance.player2 == true)
        {
            if (listOfAll3s.Count > 0)
            {
                SetReferenceArray(listOfAll3s[0]);
                MakeMoveStage2();
            }

            else
            {
                int temp = Random.Range(0, boxScores.Count);

                if (boxScores[temp] < 2)
                {
                    SetReferenceArray(temp);
                    MakeMove();
                }
                else
                {
                    AImove();
                }

            }
        }
    }
    
    void Stage_2_MoveChecker()
    {
        if (GameplayMan.instance.player2 == true)
        {
            if (listOfAll3s.Count > 0)
            {
                SetReferenceArray(listOfAll3s[0]);
                MakeMoveStage2();
            }
            else if(ChainDetector.instance.LessThan2Boxes.Count > 0)
            {
                ShuffleList(ChainDetector.instance.LessThan2Boxes);
                SetReferenceArray(ChainDetector.instance.LessThan2Boxes[0]);
                MakeMove();
            }
            else
            {
                AImove();
                stage2Done = true;
            }
        }

    }

    void Stage_3_MoveChecker()
    {
        if (GameplayMan.instance.player2 == true)
        {
            if (listOfAll3s.Count > 0)
            {
                SetReferenceArray(listOfAll3s[0]);
                MakeMoveStage2();
            }
            else
            {
                SetReferenceArray(ChainDetector.instance.chainLists[0].chain[0]);
                MakeMoveStage2();
            }
        }

    }

    void MakeMove()
    {
        bool buttonPlaced = false;

        GameObject top = referenceList[0];
        GameObject bottom = referenceList[1];
        GameObject left = referenceList[2];
        GameObject right = referenceList[3];

        int boxIndex = int.Parse(top.name);

        ShuffleRefList(referenceList);

        for (int i = 0; i < referenceList.Count; i++)
        {
            if(referenceList[i] == top)
            {
                if (row != 0)
                {
                    if (referenceList[i].activeSelf && boxScores[boxIndex - ((int)ls.n - 1)] < 2)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (referenceList[i].activeSelf)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if(referenceList[i] == bottom)
            {
                if (row != ((int)ls.n - 2))
                {
                    if (referenceList[i].activeSelf && boxScores[boxIndex + ((int)ls.n - 1)] < 2)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (referenceList[i].activeSelf)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if (referenceList[i] == left)
            {
                if (column != 0)
                {
                    if (referenceList[i].activeSelf && boxScores[boxIndex - 1] < 2)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (referenceList[i].activeSelf)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if (referenceList[i] == right)
            {
                if (column != ((int)ls.n - 2))
                {
                    if (referenceList[i].activeSelf && boxScores[boxIndex + 1] < 2)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (referenceList[i].activeSelf)
                    {
                        bm.ButtonPress(referenceList[i]);
                        buttonPlaced = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }
       

    }    
 
    void MakeMoveStage2()
    {
        bool buttonPlaced = false;

        GameObject top = referenceList[0];
        GameObject bottom = referenceList[1];
        GameObject left = referenceList[2];
        GameObject right = referenceList[3];

        int boxIndex = int.Parse(top.name);

        ShuffleRefList(referenceList);

        for (int i = 0; i < referenceList.Count; i++)
        {
            if (referenceList[i] == top)
            {
                if (referenceList[i].activeSelf)
                {
                    bm.ButtonPress(referenceList[i]);
                    buttonPlaced = true;
                    break;
                }
                else
                {
                    continue;
                }

            }
            else if (referenceList[i] == bottom)
            {
                if (referenceList[i].activeSelf)
                {
                    bm.ButtonPress(referenceList[i]);
                    buttonPlaced = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
            else if (referenceList[i] == left)
            {
                if (referenceList[i].activeSelf)
                {
                    bm.ButtonPress(referenceList[i]);
                    buttonPlaced = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
            else if (referenceList[i] == right)
            {              
                if (referenceList[i].activeSelf)
                {
                    bm.ButtonPress(referenceList[i]);
                    buttonPlaced = true;
                    break;
                }
                else
                {
                    continue;
                }
            }

        }
    }

    void ShuffleRefList(List<GameObject> listToShuffle) 
    {
            for (int i = 0; i < listToShuffle.Count; i++)
            {
                //Algorithm: Loop starts at i = 0 and 'random' is assigned to any element after i.
                // then these two objects are swapped. this goes on till the end of array.
                GameObject temp = listToShuffle[i];
                int random = Random.Range(i, listToShuffle.Count);
                listToShuffle[i] = listToShuffle[random];
                listToShuffle[random] = temp;
            }
            
     }

    void ShuffleList(List<int> listToShuffle)
    {
        for (int i = 0; i < listToShuffle.Count; i++)
        {
            //Algorithm: Loop starts at i = 0 and 'random' is assigned to any element after i.
            // then these two objects are swapped. this goes on till the end of array.
            int temp = listToShuffle[i];
            int random = Random.Range(i, listToShuffle.Count);
            listToShuffle[i] = listToShuffle[random];
            listToShuffle[random] = temp;
        }

    }
}
