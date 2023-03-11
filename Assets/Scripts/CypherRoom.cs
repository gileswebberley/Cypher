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
        //if we're down to the last needed 
        if (puzzleIdSolution.Count == 1)
        {
            //if it's the one we're still waiting to be solved
            if (puzzleIdSolution[0] == puzzleId)
            {
                Success();
                return;
            }
        }
        else
        {
            //check whether this particular puzzle has already been solved - 
            //to avoid being able to solve just one puzzle puzzleIdSolution.length times
            foreach (string pId in puzzleIdSolution)
            {
                if (puzzleId == pId)
                {
                    //remove this from the list that needs solving
                    puzzleIdSolution.Remove(pId);
                    //and check if it's the final solution required to unlock
                    if (puzzleIdSolution.Count == 0){ Success();}
                    //then break out of foreach cos we've affected the enumerator
                    break;
                }
                

            }
        }
    }

    void Success()
    {
        Debug.Log("DOOR OPENS IN CYPHER ROOM");
        gameObject.transform.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1);
    }
}
