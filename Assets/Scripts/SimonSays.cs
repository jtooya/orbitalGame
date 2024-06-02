using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class SimonSays : Puzzle
{
    public readonly int STARTINGDIFFICULTY = 2;
    private readonly char R = '0';
    private readonly char G = '1';
    private readonly char B = '2';
    private readonly char Y = '3';


    public int DifficultyModifier;
    public int DifficultyStage;
    public int preset;
    public GameObject Main;
    public string Answer;
    public string ExpectedInput;
    private SpriteRenderer redSpriteRenderer;
    private SpriteRenderer blueSpriteRenderer;
    private SpriteRenderer greenSpriteRenderer;
    private SpriteRenderer yellowSpriteRenderer;
    public SpriteRenderer greyLightRenderer;

    public Sprite redOriginal;
    public Sprite redBright;
    public Sprite blueOriginal;
    public Sprite blueBright;
    public Sprite greenOriginal;
    public Sprite greenBright;
    public Sprite yellowOriginal;
    public Sprite yellowBright;
    public Sprite greyLight;
    public Sprite greenLight;
    public Sprite redLight;

    [SerializeField]
    private string PlayerInput = "";
    private bool acceptingInput = true;

    // Start is called before the first frame update
    void Start()
    {
        //initialising variables
        DifficultyModifier = STARTINGDIFFICULTY;
        DifficultyStage = 1;
        redSpriteRenderer = transform.Find("DarkRed").GetComponent<SpriteRenderer>();
        blueSpriteRenderer = transform.Find("DarkBlue").GetComponent<SpriteRenderer>();
        greenSpriteRenderer = transform.Find("DarkGreen").GetComponent<SpriteRenderer>();
        yellowSpriteRenderer = transform.Find("DarkYellow").GetComponent<SpriteRenderer>();

        //Dynamically finding the main gameObject
        //If this fails, its because I forgot to add the main tag to the TestLogicScript
        if(Main == null) 
        {
            Main = GameObject.FindWithTag("Main");

        }

        //calculating the sum of the serial number, and finding
        //the ones digit which is important to solve the puzzle
        if (Main != null) 
        {
            TestLogicScript mainScript = Main.GetComponent<TestLogicScript>();
            if (mainScript != null) {
                string serialNumber = mainScript.serialNumber;
                int sum = SumOfDigits(serialNumber);

                //should find the ones digit unless I am dumb
                int SpecialDigit = sum % 10;

                setPreset(SpecialDigit);
            }
        } 
        else 
        {
            Debug.Log("Main gameObject not assigned");
        }

        /*
        Assuming we associate the colours to a certain value, we can generate a series of characters which is the
        "answer" to the simon says puzzle. We can then reference that array whenever we play the colours of the
         buttons or accept inputs from the player

         Let's say we associate red = 0, blue = 1, green = 2, yellow = 3, and our generated answer is 213102.
         We will flash the colours GBYBRG. The players will solve according to the stage number and preset
         which was determined by the ones digit of the sum of the S/N. Assume preset 1, stage 1. The player
         should then press the colours: GRYRBG to solve the puzzle

         Essentially its a decoding puzzle with a caesar cipher
        */
        Answer = GenerateRandomNumberString(DifficultyModifier * 2);
        ExpectedInput = findExpectedInput(Answer, preset);
        PlayAnswer();
    }

    int SumOfDigits(string serialNumber) 
    {
        Debug.Log(serialNumber);
        int sum = 0;
        foreach (char c in serialNumber) 
        {
            if (char.IsDigit(c)) {
                sum += (int)char.GetNumericValue(c);
            }
        }
        return sum;
    }

    string GenerateRandomNumberString(int length) 
    {
        StringBuilder stringBuilder= new StringBuilder();

        for (int i = 0; i < length; i++) 
        {
            int digit = UnityEngine.Random.Range(0, 4);
            stringBuilder.Append(digit.ToString());
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Sets the correct preset for the module
    /// </summary>
    /// <param name="SpecialDigit">Derived from the ones digit of the sum of all numerical chars in the S/N</param>
    void setPreset(int SpecialDigit) 
    {
        //setting the preset 1, 2 or 3 (which is determined by the ones digit)
        //I will write a better version later
        int[] type1 = {0, 1, 3, 7};
        int[] type2 = {2, 5, 8};
        int[] type3 = {4, 6, 9};


        if (type1.Contains(SpecialDigit)) 
        {
            preset = 1;
        }
        else if (type2.Contains(SpecialDigit))
        {
            preset = 2;
        }
        else if (type3.Contains(SpecialDigit))
        {
            preset = 3;
        } else 
        {
            Debug.Log("Special Digit is not contained within the presets which should never happen");
            preset = 1;
        }
    }


    string findExpectedInput(string answer, int preset) {
        StringBuilder sb = new StringBuilder();

        /* 
         TODO: Make this whole mess more efficient using switch case statements instead
        */

        if (answer == null) {
            Debug.LogWarning("Invalid answer argument to findExpectedInput function");
            return null;
        }

        if (preset == 1) 
        {
            foreach (char c in answer) 
            {
                if (c == R) {sb.Append(B);} //red -> blue
                else if (c == B) {sb.Append(R);} // blue -> red
                else if (c == G) {sb.Append(G);} // green -> green
                else if (c == Y) {sb.Append(Y);} // yellow -> yellow
            }
            Debug.Log("Expected input: " + sb.ToString());
            return sb.ToString();
        }
        else if (preset == 2)
        {
            foreach (char c in answer) 
            {
                if (c == R) {sb.Append(R);} //red -> red
                else if (c == B) {sb.Append(B);} // blue -> blue
                else if (c == G) {sb.Append(Y);} // green -> yellow
                else if (c == Y) {sb.Append(G);} // yellow -> green
            }
            Debug.Log("Expected input: " + sb.ToString());
            return sb.ToString();
        }
        else if (preset == 3) 
        {
            foreach (char c in answer) 
            {
                if (c == R) {sb.Append(G);} //red -> green
                else if (c == B) {sb.Append(R);} // blue -> red
                else if (c == G) {sb.Append(Y);} // green -> yellow
                else if (c == Y) {sb.Append(B);} // yellow -> blue
            }
            Debug.Log("Expected input: " + sb.ToString());
            return sb.ToString();
        }
        else
        {
            Debug.Log("Invalid input argument for findExpectedInput function");
            return null;
        }
    }

    public void PlayAnswer() 
    {
        if (acceptingInput)
        {
            StartCoroutine(PlayAnswerCoroutine(Answer));
        }
    }

    private IEnumerator PlayAnswerCoroutine(string Answer) 
    {
        acceptingInput = false;
        foreach (char c in Answer) 
        {
            switch (c)
            {
                case '0':
                    yield return ChangeSprite(redSpriteRenderer, redBright, redOriginal);
                    break;
                case '1':
                    yield return ChangeSprite(greenSpriteRenderer, greenBright, greenOriginal);
                    break;
                case '2':
                    yield return ChangeSprite(blueSpriteRenderer, blueBright, blueOriginal);
                    break;
                case '3' :
                    yield return ChangeSprite(yellowSpriteRenderer, yellowBright, yellowOriginal);
                    break;
            }
        }
        acceptingInput = true;
    }

    private IEnumerator ChangeSprite(SpriteRenderer spriteRenderer, Sprite brightSprite, Sprite originalSprite)
    {
        spriteRenderer.sprite = brightSprite;
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = originalSprite;
        yield return new WaitForSeconds(0.5f);
    }

    public void HandleSpriteClick(char input) 
    {
        if (!acceptingInput) return;

        StartCoroutine(HandleInput(input));
    }

    private IEnumerator HandleInput(char input) {
        if (!acceptingInput) {
            yield break;
        }

        SpriteRenderer spriteRenderer = null;
        Sprite brightSprite = null;

        switch (input) 
        {
            case '0' :
                spriteRenderer = redSpriteRenderer;
                brightSprite = redBright;
                break;
            case '1' :
                spriteRenderer = greenSpriteRenderer;
                brightSprite = greenBright;
                break;
            case '2' :
                spriteRenderer = blueSpriteRenderer;
                brightSprite = blueBright;
                break;
            case '3' :
                spriteRenderer = yellowSpriteRenderer;
                brightSprite = yellowBright;
                break;
        }

        if (spriteRenderer != null && brightSprite != null) 
        {
            Sprite originalSprite = spriteRenderer.sprite;
            spriteRenderer.sprite = brightSprite;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.sprite = originalSprite;

            PlayerInput += input;

            if (ExpectedInput.StartsWith(PlayerInput))
            {
                if (PlayerInput.Length == ExpectedInput.Length) 
                {
                    acceptingInput = false;
                    greyLightRenderer.sprite = greenLight;
                }
            } 
            else 
            {
                //player input is wrong
                StartCoroutine(HandleFailure());
            }
        }
    }

    private IEnumerator HandleFailure() 
    {
        acceptingInput = false;
        greyLightRenderer.sprite = redLight;
        yield return new WaitForSeconds(2f);
        greyLightRenderer.sprite = greyLight;
        PlayerInput = "";
        acceptingInput = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
