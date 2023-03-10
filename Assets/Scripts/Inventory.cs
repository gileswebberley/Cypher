using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDInterfaces;
public class Inventory : MonoBehaviour
{
    private List<ICollectable> CollectedInventory = new List<ICollectable>();
  
    void Test()
    {
        foreach(ICollectable token in CollectedInventory){
            token.collectableGO.SetActive(true);
        }
    }
}
