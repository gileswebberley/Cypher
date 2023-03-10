using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RDInterfaces{
    public interface ICollectable
{
    public enum CollectableTypes{
        CASH,
        HEALTH,
        CLUE
    };
    //the type of score or scale is affected
    [SerializeField] CollectableTypes type {get;}
    //any type of value eg strings and ints etc
    Object value {get;}
    //the game object that is the token itself
    GameObject collectableGO {get;}

    public void Collect();
    public void Discard();
    public void Use();
}
}
