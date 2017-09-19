using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Box : MonoBehaviour {

    [HideInInspector]
    public int buttonScore = 0;

    [SerializeField]
    private Sprite red;

    [SerializeField]
    private Sprite blue;

    public GameObject panel;

    public void ScoreIncrement()
    {
        buttonScore++;

        if(buttonScore == 4)
        {
            if (GameplayMan.instance.player1 == true)
            {
                gameObject.GetComponent<Image>().sprite = red;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = blue;
            }

            ReallocateBox();

            gameObject.SetActive(true);
            GameplayMan.instance.grantTurn = true;

        }

    }

    void ReallocateBox()
    {
        RectTransform temp = gameObject.GetComponent<RectTransform>();

        Vector3 offset = temp.anchoredPosition; 

        gameObject.transform.SetParent(null);
        panel = GameObject.FindGameObjectWithTag("Base");
        gameObject.transform.SetParent(panel.transform);

        gameObject.GetComponent<RectTransform>().anchoredPosition = offset;
    }

}
