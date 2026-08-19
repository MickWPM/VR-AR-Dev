using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishType type;
    public FishType Type { get { return type; } }

    public enum FishType { Fish, Shark, FoodOnly}
}
