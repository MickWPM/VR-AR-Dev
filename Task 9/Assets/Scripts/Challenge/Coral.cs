using System;
using UnityEngine;

public class Coral : MonoBehaviour
{
    public Transform budParent;
    public CoralDefinitionSO coralDefinitionSO;
    private CoralDefinition coralDefinition;
    public CoralTube tube;
    public GameObject tubePart;
    private void Start()
    {
        coralDefinition = coralDefinitionSO.coralDefinition;
        if (budParent == null)
        {
            CreateTube(transform.position, transform.up, coralDefinition);
        } else
        {
            for (int i = 0; i < budParent.childCount; i++)
            {
                CreateTube(budParent.GetChild(i).position, budParent.GetChild(i).up, coralDefinition);
            }
        }
    }

    private void CreateTube(Vector3 spawnPos, Vector3 initialDirection, CoralDefinition coralDefinition)
    {
        Debug.Log("Starting create tube");
        int segments = coralDefinition.segments;
        tube = new CoralTube(segments+1);
        float size = coralDefinition.baseWidth;
        float length = coralDefinition.baseGrowth;
        Vector3 growDir = initialDirection;
        Vector3 pos = spawnPos;
        for(int i = 0; i < coralDefinition.segments + 1; i++)
        {
            tube.SetPoint(i, pos, size);

            size *= coralDefinition.segmentFalloff;
            length *= coralDefinition.segmentFalloff;
            growDir = GetNewGrowDir(growDir, coralDefinition);
            
            GameObject go = Instantiate(tubePart);
            go.transform.position = pos;
            go.transform.localScale = new Vector3 (size, length, size);
            go.transform.up = growDir;
            go.GetComponentInChildren<MeshRenderer>().material.color = coralDefinitionSO.tubeColour;

            pos += growDir * length;

        }

    }

    private Vector3 GetNewGrowDir(Vector3 growingDir, CoralDefinition coralDefinition)
    {
        return GetNewGrowDir(growingDir, coralDefinition, Vector3.up);
    }

    private Vector3 GetNewGrowDir(Vector3 growingDir, CoralDefinition coralDefinition, Vector3 sunDir)
    {
        Vector3 newGrowDir = Vector3.Lerp(growingDir, sunDir, coralDefinition.sunSeeking);
        
        return newGrowDir;
    }


    [System.Serializable]
    public class CoralDefinition
    {
        public int segments = 3;
        public float baseGrowth = 1;
        public float baseWidth = 0.3f;
        //rigidity - 0 goes straight to sun, 1 goes straight in growth direction
        public float sunSeeking = 0.7f;
        public float segmentFalloff = 0.75f;
    }

    public struct CoralTube
    {
        public Vector3[] positions;
        public float[] sizes;
        public CoralTube(int length)
        {
            positions = new Vector3[length];
            sizes = new float[length];
        }
        public void SetPoint(int index, Vector3 pos, float size)
        {
            positions[index] = pos;
            sizes[index] = size;
        }
    }
}
