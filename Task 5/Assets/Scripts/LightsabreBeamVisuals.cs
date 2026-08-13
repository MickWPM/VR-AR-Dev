using UnityEngine;

[ExecuteAlways]
public class LightsabreBeamVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LightsabreDefinition sabreDefinition;

    private void OnEnable()
    {
        if (sabreDefinition != null)
        {
            sabreDefinition.DefinitionUpdated += DefinitionUpdated;
        }
        lineRenderer.positionCount = 2;
    }

    private void OnDisable()
    {
        if (sabreDefinition != null)
        {
            sabreDefinition.DefinitionUpdated -= DefinitionUpdated;
        }
    }


    public void DefinitionUpdated()
    {
        Debug.Log("Definition updated");
        lineRenderer.startColor = sabreDefinition.Colour;
        lineRenderer.endColor = sabreDefinition.Colour;

        Vector3 localTip = Vector3.forward * sabreDefinition.Range;

        Vector3[] positions = new Vector3[] { Vector3.zero, localTip };
        lineRenderer.SetPositions(positions);


        lineRenderer.startWidth = sabreDefinition.Radius;
        lineRenderer.endWidth = sabreDefinition.Radius;
    }
}
