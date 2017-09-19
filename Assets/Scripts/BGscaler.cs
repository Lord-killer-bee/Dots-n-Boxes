using UnityEngine;
using System.Collections;

public class BGscaler : MonoBehaviour {

    private Camera cam;

    // Use this for initialization
    void Start()
    {

        /*cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();// Gets the component
        Vector3 tempScale = transform.localScale;// transform.localscale gives the current scale

        float width = sr.sprite.bounds.size.x;// Bounds gives the size of max coordinates, x is put in width
        float height = sr.sprite.bounds.size.y;

        float worldHeight = cam.orthographicSize * 2f;// Orthographicsize is the size from center axis, so, (*2) gives the real height 
        float worldWidth = worldHeight / Screen.height * Screen.width;// Aspect ratio determines the worldwidth

        tempScale.x = worldWidth / width;// This gives the required scale
        tempScale.y = worldHeight / height;
        transform.localScale = tempScale;// And now the tempscale is reassigned back.*/

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 tempScale = transform.localScale;

        float width = sr.sprite.bounds.size.x;// Bounds gives the size of max coordinates, x is put in width
        float height = sr.sprite.bounds.size.y;

        float worldHeight = Screen.height;// Orthographicsize is the size from center axis, so, (*2) gives the real height 
        float worldWidth = Screen.width;// Aspect ratio determines the worldwidth

        tempScale.x = worldWidth / width;
        tempScale.y = worldHeight / height;
        transform.localScale = tempScale;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
