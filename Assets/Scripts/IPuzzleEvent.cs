using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RDInterfaces{
public interface IPuzzleEvent
{
    //put in some kind of basic event system
    public delegate void OnSuccess(string puzzleId);
    public event OnSuccess onSuccess;
    public delegate void OnFailure();
    public static event OnFailure onFailure;
    //so we can track how close you are to the solution
    public delegate void OnAttempt(float accuracyOfAttempt);//0 is correct
    public event OnAttempt onAttempt;
    //when you wish to start interacting
    public void Enable();
    public void Disable();
}
}
