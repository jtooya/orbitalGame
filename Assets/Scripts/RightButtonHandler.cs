using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightButtonHandler : MonoBehaviour
{
    public TestLogicScript testLogicScript;

    void Start()
    {
        testLogicScript = transform.parent.GetComponent<TestLogicScript>();
    }

    public void OnMouseDown()
    {
        testLogicScript.WhenRightClicked();
        Debug.Log("Right button clicked");
    }
}
