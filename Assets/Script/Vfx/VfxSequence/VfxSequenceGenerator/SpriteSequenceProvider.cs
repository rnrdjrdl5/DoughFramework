using UnityEngine;

public class SpriteSequenceProvider : MonoBehaviour
{
    public Sprite Sprite => sprite;
    
    [SerializeField] Sprite sprite;
}
