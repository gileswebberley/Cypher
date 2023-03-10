using UnityEngine;
using RDInterfaces;

public class CypherPuzzle : CypherUI, IPuzzleEvent
{
    [SerializeField, Tooltip("Identifying string to be sent with the OnSuccess event")] string puzzleId = "UId";
    [SerializeField] string clue = "?TcM";
    [SerializeField] int solution = 52;
    //set to 0 to make failure impossible
    [SerializeField, Tooltip("Set to zero for infinite attempts")] int maxAttempts = 0;
    //track how many attempts have occured so it can fail
    int attempts;
    float accuracy;
    bool isEnabled = true;
    //

    //put in some kind of basic event system
    public delegate void OnSuccess(string puzzleId);
    public event OnSuccess onSuccess;
    public delegate void OnFailure();
    public static event OnFailure onFailure;
    //so we can track how close you are to the solution
    public delegate void OnAttempt(float accuracyOfAttempt);//0 is correct
    public event OnAttempt onAttempt;
    // Start is called before the first frame update
    new void Start()
    {
      base.Start();
      attempts = 0;
      outputText.text = clue;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public bool AttemptSolution()
    {
        if(!isEnabled)return false;
        //here we check if we can/have failed
        if(++attempts > maxAttempts && maxAttempts != 0){ return false;}
        else{
            //find the float value [-1..1]
            accuracy = (solution-shiftAmount)/shiftSlider.maxValue;
            //fire the onAttempt event
            onAttempt?.Invoke(accuracy);
            return true;
            }
    }

    void SuccesfulAttempt()
    {
        //set the attempts to lock it when it's completed
        isEnabled = false;
        Debug.Log("Succesfully completed this puzzle");
        onSuccess?.Invoke(puzzleId);
    }

    //this is the event for Slider OnValueChanged set in base.Start() - override cos we want less info in the string
    public override void SliderValueChange(float f)
    {
        shiftText.text = $"{f}";
    }

    //this is the event added to the Decode button in base.Start() - override cos it's behaviour is different
    public override void Decode()
    {
        if(!isEnabled)return;
        //shiftAmount is refered to in AttemptSolution so update it first
        shiftAmount = (int)shiftSlider.value;
        if(AttemptSolution()){
        outputText.text = Decrypt(clue);
        if(shiftAmount == solution){
            SuccesfulAttempt();
        }
        }
    }
}
