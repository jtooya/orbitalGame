using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScripts : MonoBehaviour
{
    public GameObject Panel;

    public void TogglePanel() 
    {
        if (Panel != null) {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive); 
        }
    }
}
