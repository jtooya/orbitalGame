using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public char inputChar;
    private SimonSays simonSays;
    // Start is called before the first frame update
    void Start()
    {
        simonSays = FindObjectOfType<SimonSays>();
    }

    void OnMouseDown()
    {
        if (simonSays != null)
        {
            simonSays.HandleSpriteClick(inputChar);
        }
    }
}
