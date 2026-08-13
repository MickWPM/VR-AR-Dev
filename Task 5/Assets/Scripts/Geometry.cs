using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Geometry : MonoBehaviour
{
    public enum TestType { PlaneCut, RayCut}
    public TestType testType = TestType.RayCut;

    public GameObject rayObject;
    public GameObject markerPrefab;
    private GameObject marker;

    public GameObject meshGeometry;
    public GameObject planeGeometry;

    private int[] originalFaces;

    private void Start()
    {
        Mesh mesh = meshGeometry.GetComponent<MeshFilter>().mesh;
        originalFaces = mesh.triangles;

        marker = Instantiate(markerPrefab);
    }


    void Update()
    {
        switch (testType)
        {
            case TestType.PlaneCut:
                PlaneCutGeometry();
                break;
            case TestType.RayCut:
                RayCutGeometry();
                break;
            default:
                break;
        }
    }

    void RayCutGeometry()
    {
        Ray ray = new Ray(rayObject.transform.position, rayObject.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            marker.transform.position = hit.point;  
        }

        Mesh mesh = meshGeometry.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        int[] faces = mesh.triangles;
        int[] newFaces = new int[faces.Length];
        int newFaceIndex = 0;

        for (int i = 0; i < faces.Length; i+=3)
        {
            bool hasHit = false;
            for (int j = 0; j < 3; j++)
            {
                int vert = faces[i+j];
                Vector3 V = meshGeometry.transform.TransformPoint(verts[vert]);
                Vector3 P = ray.origin;
                Vector3 v1 = V - P;
                Vector3 dhat = ray.direction.normalized;
                Vector3 v2 = Vector3.Dot(v1, dhat) * dhat;
                float distance = (v1 - v2).magnitude;

                if (distance < 0.02f)
                {
                    hasHit = true;  
                }
            }

            if (!hasHit)
            {
                newFaces[newFaceIndex++] = faces[i];
                newFaces[newFaceIndex++] = faces[i + 1];
                newFaces[newFaceIndex++] = faces[i + 2];
            }
        }
        mesh.triangles = newFaces;
    }

    void PlaneCutGeometry()
    {
        Plane plane = new Plane(planeGeometry.transform.up, planeGeometry.transform.position);
        Mesh mesh = meshGeometry.GetComponent<MeshFilter>().mesh;

        

        Vector3[] verts = mesh.vertices;
        Vector2[] uvs = mesh.uv;
        int[] faces = originalFaces;
        int[] newFaces = new int[faces.Length];
        int newFaceIndex = 0;

        for (int i = 0; i < faces.Length; i+=3)
        {
            int v1=faces[i];
            int v2=faces[i+1];
            int v3=faces[i+2];
            if ((plane.GetSide(meshGeometry.transform.TransformPoint(verts[v1]))) &&
                (plane.GetSide(meshGeometry.transform.TransformPoint(verts[v2]))) &&
                (plane.GetSide(meshGeometry.transform.TransformPoint(verts[v3]))))
            {
                newFaces[newFaceIndex++] = v1;
                newFaces[newFaceIndex++] = v2;
                newFaces[newFaceIndex++] = v3;
                //Workshop tests...
                //uvs[i] = new Vector2(0.1f, 0.5f);
            } else
            {
                //Workshop tests...
                //uvs[i] = new Vector2(0.9f, 0.5f);
            }
        }
        //Workshop tests...
        //mesh.vertices = verts;
        //mesh.uv = uvs;  
        mesh.triangles = newFaces;
    }
}
