using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class BirdProps 
{
    public string DefaultAnim { get; set; }

    public AnimatorController AnimatorController { get; set; }

    public bool FaceLeft { get; set; }

    public BirdProps(string defaultAnim, bool faceLeft, AnimatorController animatorController)
    {
        DefaultAnim = defaultAnim;
        FaceLeft = faceLeft;
        AnimatorController = animatorController;
    }
}
