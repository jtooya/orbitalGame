using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EquipmentType
    {
        Helmet,
        Sword,
        Bow, 
        Pickaxe
    }

public class Enchantment : Puzzle
{
    
    // Start is called before the first frame update
    private readonly Dictionary<String, EquipmentType> enchantmentsDict = new Dictionary<string, EquipmentType>
    {
        {"Fire Protection", EquipmentType.Helmet},
        {"Protection", EquipmentType.Helmet},
        {"Blast Protection", EquipmentType.Helmet},
        {"Thorns", EquipmentType.Helmet},
        {"Infinity", EquipmentType.Bow},
        {"Flame", EquipmentType.Bow},
        {"Power", EquipmentType.Bow},
        {"Efficiency", EquipmentType.Pickaxe},
        {"Fortune", EquipmentType.Pickaxe},
        {"Silk Touch", EquipmentType.Pickaxe},
        {"Fire Aspect", EquipmentType.Sword},
        {"Knockback", EquipmentType.Sword},
        {"Looting", EquipmentType.Sword}
    };

    private SpriteRenderer greyLightRenderer;
    public Sprite greyLight;
    public Sprite greenLight;
    public Sprite redLight;

    private bool acceptingInput = true;
    [SerializeField]
    private EquipmentType expectedAnswer;
    public int currCorrectCount;
    public int requiredCorrectCount = 3; 
    private String[] enchantments;
    void Start()
    {
        enchantments = enchantmentsDict.Keys.ToArray();
        greyLightRenderer = transform.Find("LightRenderer").GetComponent<SpriteRenderer>();
        StartTest();
    }

    private void StartTest()
    {
        if (currCorrectCount >= requiredCorrectCount) {
            acceptingInput = false;
            greyLightRenderer.sprite = greenLight;
            GameObject.FindGameObjectWithTag("EnchantText").GetComponent<TextMeshProUGUI>().text = "Done!";
            return;
        }

        string enchant = GenerateRandomEnchant();
        if (enchantmentsDict.TryGetValue(enchant, out EquipmentType answer))
        {
            // Debugging lines added
            Debug.Log("Generated enchant: " + enchant);
            GameObject texter = GameObject.FindGameObjectWithTag("EnchantText");
            if (texter != null) {
                TextMeshProUGUI textComponent = texter.GetComponent<TextMeshProUGUI>();
                if (textComponent != null) {
                    textComponent.text = enchant;
                    Debug.Log("Updated text to: " + enchant);
                } else {
                    Debug.LogError("TextMeshProUGUI component not found on EnchantText GameObject");
                }
            } else {
                Debug.LogError("GameObject with tag 'EnchantText' not found");
            }
            expectedAnswer = answer;
            Debug.Log("Changed expectedAnswer to " + answer);
        }
        else 
        {
            Debug.LogWarning("Unknown enchant tried " + enchant);
        }

    }

    public void HandleButtonClick(EquipmentType equipment) {
        if (!acceptingInput) 
        {
            return;
        }

        StartCoroutine(HandleInput(equipment));
    }

    private IEnumerator HandleInput(EquipmentType equipment)
    {
        if (expectedAnswer == equipment) {
            currCorrectCount++;
            StartTest();
        } else {
            acceptingInput = false;
            greyLightRenderer.sprite = redLight;
            yield return new WaitForSeconds(1f);
            greyLightRenderer.sprite = greyLight;
            acceptingInput = true;
        }
    }

    private string GenerateRandomEnchant()
    {
        int randomIndex = UnityEngine.Random.Range(0, enchantments.Length);
        return enchantments[randomIndex];
    }
}
