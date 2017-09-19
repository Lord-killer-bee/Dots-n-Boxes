using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayMan : MonoBehaviour {

    public static GameplayMan instance;

    public bool player1;
    public bool player2;
    public bool grantTurn = false;

    [SerializeField]
    private GameObject box;

    [SerializeField]
    private GameObject line;

    [SerializeField]
    private Sprite red;

    [SerializeField]
    private Sprite blue;



    // Use this for initialization
    void Start () {

        if(instance == null)
        {
            instance = this;
        }

        player1 = true;
        player2 = false;

        line.GetComponent<Image>().sprite = red;
    }
	
    public void SwapPlayers()
    {
        if(player1 == true)
        {
            if (grantTurn == false)
            {
                player1 = false;
                player2 = true;

                line.GetComponent<Image>().sprite = blue;
                AImanager.instance.AImove();
            }
            else
            {
                player1 = true;
                player2 = false;

                line.GetComponent<Image>().sprite = red;
            }
        }

        else if (player2 == true)
        {
            if (grantTurn == false)
            {
                player1 = true;
                player2 = false;
                line.GetComponent<Image>().sprite = red;
            }
            else
            {
                player1 = false;
                player2 = true;

                line.GetComponent<Image>().sprite = blue;
                AImanager.instance.AImove();
            }
        }

    }

}
