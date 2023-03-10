using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CypherPuzzle.onSuccess += Success;
        CypherPuzzle.onAttempt += Attempt;
    }

    void Attempt(float accuracyOfAttempt)
    {
        Debug.Log($"Attempt accuracy :{accuracyOfAttempt}");
        this.gameObject.transform.GetComponent<Renderer>().material.color = new Color(1-Mathf.Abs(accuracyOfAttempt),0,Mathf.Abs(accuracyOfAttempt),1);
    }

    void Success()
    {
        this.gameObject.transform.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
    }
}
