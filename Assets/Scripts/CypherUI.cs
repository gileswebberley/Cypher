using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Challenge to create a quick shift encoding mechanism to work with a GUI in Unity
public class CypherUI : MonoBehaviour
{
    readonly char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '!', '@', '?', '#', ',', ':', '£', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    protected int shiftAmount = 0;
    int shiftDirectionMultiplier = 1;//change to -1 to decode

    [SerializeField] TMP_InputField inputText;
    [SerializeField] int inputLengthLimit = 255;
    //making this public as I am about to inherit from it and SerializeField I don't think carries down - ah no, wrong
    [SerializeField] protected TMP_InputField outputText;
    protected Slider shiftSlider;//simply name the slider game object ""ShiftAmount"

    protected TextMeshProUGUI shiftText, charCountText;//name "ShiftAmountText" and "CharacterCount" if you want to use them
    protected Button encodeButton, decodeButton;//name "Encode" and "Decode"

    // Start is called before the first frame update
    //This finds which components are available for the gui and connects their event methods
    protected void Start()
    {
        //set a limit for the input message - 255 so it feels like an sms
        if(inputText != null) inputText.characterLimit = inputLengthLimit;
        //now see if there is a text area for counting the length of message
        //if it exists then attach an event to look after displaying the count
        if (SafeFind<TextMeshProUGUI>("CharacterCount",ref charCountText))
        {
            //ok, to use TryGetComponent simply create a temp local out variable and assign it -
            // it would have to be a ref to have worked as I expected
            Debug.Log("SafeSearch worked as expexted");
            //charCountText = c;
            CharCountChanged("");
            //onValueChange is a UnityEvent<string> so the entered string is sent to the listener
            inputText.onValueChanged.AddListener(CharCountChanged);
        }
        //grab the expected slider which sets the shift amount
        if(SafeFind<Slider>("ShiftAmount",ref shiftSlider)){
            //shiftSlider = ss;
            shiftSlider.maxValue = letters.Length-1;
        }
        //check to see if we can grab an associated textfield named ShiftAmountText
        //if the text area exists then set it and attach the event to update when value changes
        if (SafeFind<TextMeshProUGUI>("ShiftAmountText",ref shiftText))
        {
            //shiftText = s;
            //set the text in the shift amount textfield to the slider's start value
            SliderValueChange(shiftSlider.value);
            //had to add float parameter to the method to make it work as it is a UnityEvent<float>
            shiftSlider.onValueChanged.AddListener(SliderValueChange);
        }

        //Now find the buttons named Encode and Decode and automagically tie them together with their event methods
        if(SafeFind<Button>("Encode",ref encodeButton))
        {
            //encodeButton = e;
            encodeButton.onClick.AddListener(Encode);
        }
        if(SafeFind<Button>("Decode",ref decodeButton))
        {
            Debug.Log("Decode button found");
            //decodeButton = d;
            decodeButton.onClick.AddListener(Decode);
        }
    }
    //Safely try to Find(find) and GetComponent<T>
    protected bool SafeFind<T>(string find, ref T component)
    {
        //need this to only search in it's own heirachy to work as a prefab (GameObject.Find will always only find one of the named)
        GameObject returnGO = TraverseTransformFind(find, gameObject.transform);
        if(returnGO == null){
            Debug.Log($"Find({find}) returned null");
             component = default(T);
             return false;
        }else{
            Debug.Log($"Find({find}) returned a game object");
            if(returnGO.TryGetComponent<T>(out T tmpComponent)){
                component = tmpComponent;
                return true;
            }else{
                Debug.Log("But couldn't find the component");
                component = default(T);
                return false;
            }
        }
    }

//Gosh, this has had me twisting my brain - I want to be able to Find(name) but only within the parent
//game object so recursively search with the Transform.Find which only checks direct children
    protected GameObject TraverseTransformFind(string find, Transform parentT)
    {
        //THANK YOU Penjimon - I had ended up just getting more and more complicated when I knew it should be a simple solution!!
        //https://stackoverflow.com/questions/56410705/how-to-find-a-gameobject-lower-in-hierarchy-starting-at-root-gameobject
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(parentT);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == find) return c.gameObject;
            foreach (Transform t in c) queue.Enqueue(t);
        }
        return null;
        
        // Debug.Log($"Traversing...inside transform {parentT.name}");
        // //try to see if direct child..
        // Transform returnT = parentT.Find(find);
        // if (returnT != null) return returnT.gameObject;
        // //otherwise dig down..
        // if(parentT.childCount > 0){
        //     //check out the first level of children
        //     foreach(Transform t in parentT)
        //     {
        //         returnT = t.Find(find);
        //         //this doesn't feel right :/
        //         if(returnT == null){
        //             if(t.childCount > 0){
        //                 Debug.Log($"{t.name} has children");
        //                 return TraverseTransformFind(find, t);//CAREFULL GILO, RECURSION
        //             }else if(parentT.parent.GetChild(t.GetSiblingIndex()+1) != null){//trying to stop it getting stuck at the bottom
        //                 Debug.Log("Going back up the transform heirachy");
        //                 //return null;
        //                 //I think this means it only digs down the first
        //                 return TraverseTransformFind(find, parentT.parent.GetChild(t.GetSiblingIndex()+1));
        //             }
        //         } else {
        //             return returnT.gameObject;  
        //         }
        //     }
        // }else if(parentT.parent.GetChild(parentT.GetSiblingIndex()+1) != null) {
        //     Debug.Log($"We have no children in {parentT.name} so trying to get back above");
        //     return TraverseTransformFind(find, parentT.parent.GetChild(parentT.GetSiblingIndex()+1));
        // }
        // //it's found in it's parent
        // return returnT.gameObject;
    }

    //for setting the textfield to match how many characters have been entered 
    public virtual void CharCountChanged(string s)
    {
        charCountText.text = $"{s.Length}/{inputLengthLimit}";
    }

    //float parameter was added to make it allowable to be added as a listener to OnValueChanged
    public virtual void SliderValueChange(float f)
    {
        shiftText.text = $"Shift Amount: {f}";//{(int)shiftSlider.value}";
    }

    //attach to OnClick event for the gui encode button
    public virtual void Encode()
    {
        shiftAmount = (int)shiftSlider.value;
        outputText.text = $"{Encrypt(inputText.text)}\nTo decode shift by: {shiftAmount}";
    }

    //attach to OnClick event for the gui decode button
    public virtual void Decode()
    {
        shiftAmount = (int)shiftSlider.value;
        outputText.text = Decrypt(inputText.text);
    }

    //Original static class methods created for C# console version are below
    protected string Encrypt(string toEncode)
    {
        shiftDirectionMultiplier = 1;
        return ShiftString(toEncode);
    }

    protected string Decrypt(string toDecode)
    {
        shiftDirectionMultiplier = -1;
        return ShiftString(toDecode);
    }

    //string encoding function
    string ShiftString(string s)
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
    int ShiftLetterIndex(char letter)
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
