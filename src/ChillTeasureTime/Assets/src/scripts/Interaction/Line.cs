using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

public class GreenBirdVisual : Direction
{
    
}

public class GetWingFlap : Line
{
    public GetWingFlap() : base("", "Got Wing Flap!")
    {
    }
}
public class CameraFadeIn: Direction
{
}

public class CameraFadeOut : Direction
{
}

public class LeaveChoicePrompt : Direction
{
}

public class LoadFight : Direction
{
    
}
