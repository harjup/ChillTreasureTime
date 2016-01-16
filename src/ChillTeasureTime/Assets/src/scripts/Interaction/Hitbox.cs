using UnityEngine;
using System.Collections;

public enum HitboxType
{
    Unknown,
    PlayerWing
}

public class Hitbox : MonoBehaviour
{
    public HitboxType Type;

    private Player _player;
    void Start()
    {
        _player = transform.GetComponentInParent<Player>();
    }

    public void AddToInteractables(CanInteract canInteract)
    {
        _player.AddInteractable(canInteract);
    }

    public void RemoveToInteractables(CanInteract canInteract)
    {
        _player.RemoveInteractable(canInteract);
    }
}
