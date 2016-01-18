using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

public class BirdPropsService : Singleton<BirdPropsService>
{

    public AnimatorController RedBirdAnimatorController;
    public AnimatorController GreenBirdAnimatorController;

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
