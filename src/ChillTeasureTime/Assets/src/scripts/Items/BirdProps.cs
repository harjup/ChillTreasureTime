using UnityEngine;

public class BirdProps 
{
    public string DefaultAnim { get; set; }

    public RuntimeAnimatorController AnimatorController { get; set; }

    public bool FaceLeft { get; set; }

    public BirdProps(string defaultAnim, bool faceLeft, RuntimeAnimatorController animatorController)
    {
        DefaultAnim = defaultAnim;
        FaceLeft = faceLeft;
        AnimatorController = animatorController;
    }
}
