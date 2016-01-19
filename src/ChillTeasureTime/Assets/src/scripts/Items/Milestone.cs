using System.Collections.Generic;
using UnityEngine;

public enum BirdType
{
    Blue, 
    Green, 
    Red
}

public class Milestone 
{
    public int Amount { get; private set; }

    public string Id { get; private set; }

    public BirdType BirdType { get; set; }

    public List<Direction> Directions { get; private set; }

    public Milestone(int amount, string id = null, BirdType birdType = BirdType.Blue, List<Direction> directions = null)
    {
        Amount = amount;
        Id = id;
        
        Directions = directions;
        if (directions == null)
        {
            Directions = new List<Direction>();
        }

        BirdType = birdType;
    }

}
