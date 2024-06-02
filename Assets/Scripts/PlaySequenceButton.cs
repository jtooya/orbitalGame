using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySequenceButton : MonoBehaviour
{


    public SimonSays simonSays;
    // Start is called before the first frame update
    void Start()
    {
        simonSays = transform.parent.GetComponent<SimonSays>();
    }

    public void OnMouseDown() 
    {
        Debug.Log("Playing sequence");
        simonSays.PlayAnswer();
    }
}
