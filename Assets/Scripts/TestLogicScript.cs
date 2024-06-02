using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TestLogicScript : MonoBehaviour
{

    public string serialNumber;
    public const int SERIALNUMBERLENGTH = 10;
    public int numberOfModules = 5;
    public GameObject[] puzzlesPrefabArray;
    public List<GameObject> modules = new List<GameObject>();
    [SerializeField]
    private int currentIndex = 0;
    public float spawnDistance = 5f;


    // Start is called before the first frame update
    void Start()
    {
        serialNumber = GenerateSerialNumber();
        GameObject.FindGameObjectWithTag("SerialNumber").GetComponent<TextMeshProUGUI>().text = serialNumber;
        puzzlesPrefabArray = Resources.LoadAll<GameObject>("Prefabs");
        CreateModules();
        ShowCurrentModule();
    }

    private string GenerateSerialNumber() 
    {
        StringBuilder sb = new StringBuilder();
        const string alphanumericCharacters = "ABCDEFHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        for (int i = 0; i < SERIALNUMBERLENGTH; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, alphanumericCharacters.Length);
            sb.Append(alphanumericCharacters[randomIndex]);
        }
        return sb.ToString();
    }

    private void CreateModules()
    {
        for (int i = 0; i < numberOfModules; i++)
        {
            Vector3 spawnPosition = GetCameraSpawnPosition();
            int randomIndex = UnityEngine.Random.Range(0, puzzlesPrefabArray.Length);
            GameObject newModule = Instantiate(puzzlesPrefabArray[randomIndex], spawnPosition, quaternion.identity);
            newModule.name = puzzlesPrefabArray[randomIndex].name + "_" + i;
            newModule.transform.position = new Vector3(0, 0, 0);
            newModule.SetActive(false);
            modules.Add(newModule);
        }
    }

    private void ShowCurrentModule() 
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].SetActive(i == currentIndex);
        }
    }

    Vector3 GetCameraSpawnPosition() 
    {
        Camera mainCamera = Camera.main;
        return mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;
    }

    public void WhenLeftClicked()
    {
        currentIndex = (currentIndex - 1 + modules.Count) % modules.Count;
        ShowCurrentModule();
    }

    public void WhenRightClicked()
    {
        currentIndex = (currentIndex + 1) % modules.Count;
        ShowCurrentModule();
    }


    // Update is called once per frame
    void Update()
    {
    }
}
