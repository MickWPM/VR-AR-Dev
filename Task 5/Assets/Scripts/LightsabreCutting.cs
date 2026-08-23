using UnityEngine;
using UnityEngine.Timeline;

public class LightsabreCutting : MonoBehaviour
{
    [SerializeField] private LightsabreDefinition lightsabreDefinition;
    [SerializeField] private GameObject objectToBeCut;

    private GameObject rayObject;
    private void Awake()
    {
        rayObject = this.gameObject;
    }

    private void Update()
    {
        RayCutGeometry();
    }

    public enum CuttingState { HitNothing, HitButOutOfRange, HitObjectInRange, HitObjectInRangeNoVertsInDistance, CUTTING };
    public CuttingState cuttingState;
    public event System.Action<Vector3> CutHappenedEvent;
    void RayCutGeometry()
    {
        Ray ray = new Ray(rayObject.transform.position, rayObject.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Vector3.Distance(hit.point, transform.position) > lightsabreDefinition.Range)
            {
                cuttingState = CuttingState.HitButOutOfRange;
                return;
            }
        } else
        {
            cuttingState = CuttingState.HitNothing;
            return;
        }

        cuttingState = CuttingState.HitObjectInRange;
        Mesh mesh = objectToBeCut.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        int[] faces = mesh.triangles;
        int[] newFaces = new int[faces.Length];
        int newFaceIndex = 0;

        for (int i = 0; i < faces.Length; i += 3)
        {
            bool hasHit = false;
            for (int j = 0; j < 3; j++)
            {
                int vert = faces[i + j];
                Vector3 V = objectToBeCut.transform.TransformPoint(verts[vert]);
                if (Vector3.Distance(V, transform.position) > lightsabreDefinition.Range) continue;

                Vector3 P = ray.origin;
                Vector3 v1 = V - P;
                Vector3 dhat = ray.direction.normalized;
                Vector3 v2 = Vector3.Dot(v1, dhat) * dhat;
                float distance = (v1 - v2).magnitude;

                if (distance < lightsabreDefinition.Radius)
                {
                    hasHit = true;
                    cuttingState = CuttingState.CUTTING;
                }
            }

            if (!hasHit)
            {
                cuttingState = CuttingState.HitObjectInRangeNoVertsInDistance;
                newFaces[newFaceIndex++] = faces[i];
                newFaces[newFaceIndex++] = faces[i + 1];
                newFaces[newFaceIndex++] = faces[i + 2];
            } else
            {
                CutHappenedEvent?.Invoke(hit.point);
            }
        }
        mesh.triangles = newFaces;
    }
}
