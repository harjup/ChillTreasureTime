using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdPropsService : Singleton<BirdPropsService>
{

    public Dictionary<string, BirdProps> BirdPropses = new Dictionary<string, BirdProps>
    {
        {"Main-Observer", new BirdProps("RestPose", false, null)},
        {"Main-JumpHelper", new BirdProps("RestPoseFront", false, null)},
    };

    public BirdProps GetBirdPropsById(string id)
    {
        if (BirdPropses.ContainsKey(id))
        {
            return BirdPropses[id];
        }

        return null;
    }

}
