using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantmentButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public EquipmentType equipmentType;
    private Enchantment enchantment;
    void Start()
    {
        enchantment = transform.parent.GetComponent<Enchantment>(); 
    }

    // Update is called once per frame
    void OnMouseDown() {
        enchantment.HandleButtonClick(equipmentType);
        Debug.Log(transform.name + " pressed");
    }
}
