using System.Collections.Generic;
using UnityEngine;

public class Milestone 
{
    public int Amount { get; private set; }

    public string Id { get; private set; }

    public List<Line> Lines { get; private set; }

    public Milestone(int amount, string id, List<Line> lines = null)
    {
        Amount = amount;
        Id = id;
        
        Lines = lines;
        if (lines == null)
        {
            Lines = new List<Line>();
        }
    }

}
