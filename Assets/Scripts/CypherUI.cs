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
    static char[] letters = new char[] {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
        static int shiftAmount = 0;
        static int shiftDirectionMultiplier = 1;//change to -1 to decode

        [SerializeField] TMP_InputField inputText;
        [SerializeField] TMP_InputField outputText;
        Slider shiftSlider;

        TextMeshProUGUI shiftText;
        Button encodeButton, decodeButton;

        //let's try to set up the delegates properly for the OnClick events
        delegate void encode();
        delegate void decode();

    // Start is called before the first frame update
    void Start()
    {   
        shiftSlider = GameObject.Find("ShiftAmount").GetComponent<Slider>();
        //check to see if we can grab an associated textfield named ShiftAmountText
        shiftText = GameObject.Find("ShiftAmountText").GetComponent<TextMeshProUGUI>();
        //if the text area exists then set it and attach the event to update when value changes
        if(shiftText != null){
            //set the text in the shift amount textfield to the slider's start value
            SliderValueChange(shiftSlider.value);
            //had to add float parameter to the method to make it work as it is a UnityEvent<float>
            shiftSlider.onValueChanged.AddListener(SliderValueChange);
        }

        //Now find the buttons named Encode and Decode
        encodeButton = GameObject.Find("Encode").GetComponent<Button>();
        decodeButton = GameObject.Find("Decode").GetComponent<Button>();
        //automagically tie them together with their event methods - hasn't worked, hmmm
        //found a possible solution which I assume is some kind of inline delegate definiton?
        // encodeButton.onClick.AddListener(delegate {Encode();});
        // decodeButton.onClick.AddListener(delegate {Decode();});
        //worked but going to try to set up the delegates properly
        encode EncodeDelegate = new encode(Encode);
        decode DecodeDelegate = new decode(Decode);
        encodeButton.onClick.AddListener(Encode);
        decodeButton.onClick.AddListener(Decode);
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
       outputText.text = $"{Encrypt(inputText.text)}\nto decode set shift amount to: {shiftAmount}";
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
        //work with only lowercase for simplicity
        s = s.ToLower();

        foreach(char c in s)
        {
            //Array.Exists(array,predicate) basically checks whether the 'element' c (which is our character) is in 'array'
            //if it does then shift the index of the character by shift amount
           if(Array.Exists(letters, element => element == c)) tmpStr += letters[ShiftLetterIndex(c)];
           //otherwise it is whitespace or a special character so leave it alone (again just to keep this simple)
           else tmpStr += c;
        }
        Console.WriteLine(tmpStr);
        return tmpStr;
    }
    //function to move backwards and forwards through a number range - 0..the length of letters[]
    static int ShiftLetterIndex(char letter)
    {
        int tmpIndex = Array.IndexOf(letters,letter);//does the same as the loop below
        /*
        for(int i = 0; i < letters.Length; i++)
        {
            if(letters[i] == letter)
            {
                tmpIndex = i;
                break;
            }
        }
        */
        tmpIndex = tmpIndex+(shiftAmount*shiftDirectionMultiplier);
        //catch the index if it goes below zero and if so loop it back round to the top of the scale
        if(tmpIndex < 0) tmpIndex = (letters.Length) + (tmpIndex);
        //else simple modulus to loop round in the positive direction
        else tmpIndex = tmpIndex % (letters.Length);
        return tmpIndex;
    }
    }
