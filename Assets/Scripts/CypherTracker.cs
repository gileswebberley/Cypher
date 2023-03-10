using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDInterfaces;

public class CypherTracker : MonoBehaviour
{
    [SerializeField, Tooltip("Attach the IPuzzleEvent to track")] CypherPuzzle myPuzzle;
    [SerializeField, Tooltip("The 'Room' that we are part of")] CypherRoom myRoom;
    //public List<string> puzzleIdSolution;
    // Start is called before the first frame update
    void Start()
    {
        myPuzzle.onSuccess += Success;
        myPuzzle.onAttempt += Attempt;
    }

    void Attempt(float accuracyOfAttempt)
    {
        Debug.Log($"Attempt accuracy :{accuracyOfAttempt}");
        this.gameObject.transform.GetComponent<Renderer>().material.color = new Color(1-Mathf.Abs(accuracyOfAttempt),0,Mathf.Abs(accuracyOfAttempt),1);
    }

    void Success(string puzzleId)
    {
        this.gameObject.transform.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
        myRoom.RegisterSuccess(puzzleId);
    }
}
