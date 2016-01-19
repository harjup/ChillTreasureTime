using UnityEngine;
using System.Collections.Generic;

public class BirdPropsService : Singleton<BirdPropsService>
{

    public RuntimeAnimatorController RedBirdAnimatorController;
    public RuntimeAnimatorController GreenBirdAnimatorController;

    public static Dictionary<string, BirdProps> BirdPropses;

    private Dictionary<string, BirdProps> GetBirdPropses()
    {
        if (BirdPropses == null)
        {
            BirdPropses = new Dictionary<string, BirdProps>
            {
                {"Main-Observer", new BirdProps("RestPose", false, GreenBirdAnimatorController)},
                {"Main-JumpHelper", new BirdProps("RestPoseFront", false, null)},
                {"Lighthouse-Treasure", new BirdProps("RestPoseFront", false, GreenBirdAnimatorController)},
                //Flap
                {"Main-FlapHelp", new BirdProps("Flap", false, null)},
            };
        }

        return BirdPropses;
    }
    
    public BirdProps GetBirdPropsById(string id)
    {
        var bp = GetBirdPropses();
        if (bp.ContainsKey(id))
        {
            return bp[id];
        }

        return null;
    }

}
