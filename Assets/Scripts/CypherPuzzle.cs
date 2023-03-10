using UnityEngine;
using RDInterfaces;

public class CypherPuzzle : CypherUI, IPuzzleEvent
{
    [SerializeField] string clue = "?TcM";
    [SerializeField] int solution = 52;
    //track how many attempts have occured so it can fail
    [SerializeField] int maxAttempts = 3;
    static int attempts;
    float accuracy;

    //put in some kind of basic event system
    public delegate void OnSuccess();
    public static event OnSuccess onSuccess;
    public delegate void OnFailure();
    public static event OnFailure onFailure;
    //so we can track how close you are to the solution
    public delegate void OnAttempt(float accuracyOfAttempt);//0 is correct
    public static event OnAttempt onAttempt;
    // Start is called before the first frame update
    new void Start()
    {
      base.Start();
      attempts = 0;
      outputText.text = clue;
    }

    public bool AttemptSolution()
    {
        if(++attempts > maxAttempts){ return false;}
        else{
            accuracy = (solution-shiftAmount)/shiftSlider.maxValue;
            onAttempt?.Invoke(accuracy);
            return true;
            }
    }

    void SuccesfulAttempt()
    {
        //set the attempts to lock it when it's completed
        attempts = maxAttempts+1;
        Debug.Log("Succesfully completed this puzzle");
        onSuccess?.Invoke();
    }

    //override cos we want less info in the string
    public override void SliderValueChange(float f)
    {
        shiftText.text = $"{f}";//{(int)shiftSlider.value}";
    }

    //this is the event added to the Decode button in base.Start();
    public override void Decode()
    {
        shiftAmount = (int)shiftSlider.value;
        if(AttemptSolution()){
        outputText.text = Decrypt(clue);
        if(shiftAmount == solution){
            SuccesfulAttempt();
        }
        }
    }
}
