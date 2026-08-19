using UnityEngine;

[RequireComponent(typeof(FishMotor))]
public class Fish : MonoBehaviour
{
    [SerializeField] private FishType type;
    public FishType Type { get { return type; } }

    public enum FishType { Fish, Shark}
}
