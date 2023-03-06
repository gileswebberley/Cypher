using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CypherPuzzle : CypherUI
{
    [SerializeField] string clue = "?TcM";
    [SerializeField] int solution = 52;

    [SerializeField] int maxAttempts = 3;
    static int attempts;
    // Start is called before the first frame update
    new void Start()
    {
      base.Start();
      attempts = 0;
      outputText.text = clue;
    }

    public bool AttemptSolution()
    {
        if(++attempts > maxAttempts) return false;
        else return true;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public override void SliderValueChange(float f)
    {
        shiftText.text = $"{f}";//{(int)shiftSlider.value}";
    }
    //this is the event added to the Decode button in base.Start();
    public override void Decode()
    {
        if(AttemptSolution()){
        shiftAmount = (int)shiftSlider.value;
        outputText.text = Decrypt(clue);
        }
    }
}
