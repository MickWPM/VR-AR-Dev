using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LightsabreDefinition : MonoBehaviour
{
    [SerializeField] private float range = 1f;
    [Range(0.01f, 0.1f)]
    [SerializeField] private float radius = 0.05f;
    [SerializeField] private Color colour;

    public Color Colour { get => colour; }
    public float Radius { get => radius; }
    public float Range { get => range; }
    public Vector3 WorldTipOffset { get => transform.forward * range; }

    public System.Action DefinitionUpdated;
    private void OnValidate()
    {
        Debug.Log("Definition validate");
        DefinitionUpdated?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        Gizmos.color = colour;
        Gizmos.DrawRay(transform.position, WorldTipOffset);
        Vector3 worldTipPosition = transform.position + WorldTipOffset;
        Gizmos.DrawWireSphere(worldTipPosition, Radius);
        Gizmos.color = c;
    }
}
