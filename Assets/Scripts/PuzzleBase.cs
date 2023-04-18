using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PuzzleBase : MonoBehaviour
{
    //unique ID for any puzzle object
    public virtual string PuzzleId {get;}
    //put in some kind of basic event system
    //public delegate void OnSuccess(string puzzleId);
    public event EventHandler<string> onSuccess;
    //public delegate void OnFailure();
    public event EventHandler onFailure;
    //so we can track how close you are to the solution
    //public delegate void OnAttempt(float accuracyOfAttempt);//0 is correct
    public event EventHandler<float> onAttempt;
    
}
