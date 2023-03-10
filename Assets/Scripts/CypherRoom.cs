using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CypherRoom : MonoBehaviour
{
    public List<string> puzzleIdSolution;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    
    public void RegisterSuccess(string puzzleId)
    {
        if(puzzleIdSolution.Count == 1 && puzzleIdSolution[0] == puzzleId)
        {
            Success();
            return;
        }
        //check whether this particular puzzle has already been solved - 
        //to avoid being able to solve just one puzzle puzzleIdSolution.length times
        foreach(string pId in puzzleIdSolution)
        {
            if(puzzleId == pId){
                puzzleIdSolution.Remove(pId);
            }
            if(puzzleIdSolution.Count == 0) Success();
        }
    }

    void Success()
    {
        gameObject.transform.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
    }
}
