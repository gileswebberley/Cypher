using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

//Challenge to create a quick shift encoding mechanism to work with a GUI in Unity
public class CypherUI : MonoBehaviour
{
    static readonly char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '!', '@', '?', '#', ',', ':', '£', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    static int shiftAmount = 0;
    static int shiftDirectionMultiplier = 1;//change to -1 to decode

    [SerializeField] TMP_InputField inputText;
    [SerializeField] int inputLengthLimit = 255;
    [SerializeField] TMP_InputField outputText;
    Slider shiftSlider;//simply name the slider game object ""ShiftAmount"

    TextMeshProUGUI shiftText, charCountText;//name "ShiftAmountText" and "CharacterCount" if you want to use them
    Button encodeButton, decodeButton;//name "Encode" and "Decode"

    // Start is called before the first frame update
    void Start()
    {
        //set a limit for the input message - 255 so it feels like an sms
        inputText.characterLimit = inputLengthLimit;
        //now see if there is a text area for counting the length of message
        //if it exists then attach an event to look after displaying the count
        if (GameObject.Find("CharacterCount").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI c))
        {
            //ok, to use TryGetComponent simply create a temp local out variable and assign it -
            // it would have to be a ref to have worked as I expected
            //Debug.Log("TryGetComponent worked as expexted");//no, it hasn't!
            charCountText = c;
            CharCountChanged("");
            //onValueChange is a UnityEvent<string> so the entered string is sent to the listener
            inputText.onValueChanged.AddListener(CharCountChanged);
        }
        //grab the expected slider which sets the shift amount
        shiftSlider = GameObject.Find("ShiftAmount").GetComponent<Slider>();
        //check to see if we can grab an associated textfield named ShiftAmountText
        //if the text area exists then set it and attach the event to update when value changes
        if (GameObject.Find("ShiftAmountText").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI s))
        {
            shiftText = s;
            //set the text in the shift amount textfield to the slider's start value
            SliderValueChange(shiftSlider.value);
            //had to add float parameter to the method to make it work as it is a UnityEvent<float>
            shiftSlider.onValueChanged.AddListener(SliderValueChange);
        }

        //Now find the buttons named Encode and Decode
        encodeButton = GameObject.Find("Encode").GetComponent<Button>();
        decodeButton = GameObject.Find("Decode").GetComponent<Button>();
        //automagically tie them together with their event methods
        encodeButton.onClick.AddListener(Encode);
        decodeButton.onClick.AddListener(Decode);
    }

    //for setting the textfield to match how many characters have been entered 
    public void CharCountChanged(string s)
    {
        charCountText.text = $"{s.Length}/{inputLengthLimit}";
    }

    //float parameter was added to make it allowable to be added as a listener to OnValueChanged
    public void SliderValueChange(float f)
    {
        shiftText.text = $"Shift Amount: {f}";//{(int)shiftSlider.value}";
    }

    //attach to OnClick event for the gui encode button
    public void Encode()
    {
        shiftAmount = (int)shiftSlider.value;
        outputText.text = $"{Encrypt(inputText.text)}\nTo decode shift by: {shiftAmount}";
    }

    //attach to OnClick event for the gui decode button
    public void Decode()
    {
        shiftAmount = (int)shiftSlider.value;
        outputText.text = Decrypt(inputText.text);
    }

    //Original static class methods created for C# console version are below
    static string Encrypt(string toEncode)
    {
        shiftDirectionMultiplier = 1;
        return ShiftString(toEncode);
    }

    static string Decrypt(string toDecode)
    {
        shiftDirectionMultiplier = -1;
        return ShiftString(toDecode);
    }

    //string encoding function
    static string ShiftString(string s)
    {
        string tmpStr = "";
        //work with only lowercase for simplicity - EXTENDED ARRAY TO INCLUDE CAPITALS, NUMBERS, AND A FEW SPECIAL CHARS
        //s = s.ToLower();
        foreach (char c in s)
        {
            //Array.Exists(array,predicate) basically checks whether the 'element' c (which is our character) is in 'array'
            //if it does then shift the index of the character by shift amount
            if (Array.Exists(letters, element => element == c)) tmpStr += letters[ShiftLetterIndex(c)];
            //otherwise it is not included in the letters array so don't change it (again just to keep this simple)
            else tmpStr += c;
        }
        Console.WriteLine(tmpStr);
        return tmpStr;
    }
    //function to move backwards and forwards through a number range - 0..the length of letters[]
    static int ShiftLetterIndex(char letter)
    {
        int tmpIndex = Array.IndexOf(letters, letter);
        //now add or remove the shift amount based on whether it is decoding or encoding
        tmpIndex = tmpIndex + (shiftAmount * shiftDirectionMultiplier);
        //catch the index if it goes below zero and if so loop it back round to the top of the scale
        if (tmpIndex < 0) tmpIndex = (letters.Length) + (tmpIndex);
        //else simple modulus to loop round in the positive direction
        else tmpIndex = tmpIndex % (letters.Length);
        return tmpIndex;
    }
}
