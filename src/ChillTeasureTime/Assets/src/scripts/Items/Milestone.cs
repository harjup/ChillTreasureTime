using System.Collections.Generic;
using UnityEngine;

public class Milestone 
{
    public int Amount { get; private set; }

    public string Id { get; private set; }

    public List<Direction> Directions { get; private set; }

    public Milestone(int amount, string id, List<Direction> directions = null)
    {
        Amount = amount;
        Id = id;
        
        Directions = directions;
        if (directions == null)
        {
            Directions = new List<Direction>();
        }
    }

}
