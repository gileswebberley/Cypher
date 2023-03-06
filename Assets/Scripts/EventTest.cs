using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CypherPuzzle.onSuccess += ChangeColour;
    }

    void ChangeColour()
    {
        this.gameObject.transform.GetComponent<Renderer>().material.color = new Color(1,0,0,1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
