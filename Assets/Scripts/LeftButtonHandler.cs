using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftButtonHandler : MonoBehaviour
{
    public TestLogicScript testLogicScript;
    
    void Start()
    {
        testLogicScript = transform.parent.GetComponent<TestLogicScript>();
    }
    
    public void OnMouseDown()
    {
        testLogicScript.WhenLeftClicked();
        Debug.Log("Right button clicked");
    }
}
