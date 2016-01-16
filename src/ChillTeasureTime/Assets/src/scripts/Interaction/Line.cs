using UnityEngine;
using System.Collections;


public class Line : Direction
{
    public Line(string name, string content)
    {
        Name = name;
        Content = content;
    }

    public string Name { get; set; }
    public string Content { get; set; }
    
}

public class GetWingFlap : Line
{
    public GetWingFlap() : base("", "Got Wing Flap!")
    {
    }
}

public class LoadFight : Direction
{
    
}
