using UnityEngine;
using System.Collections.Generic;

public class SetupGrabbers : MonoBehaviour
{
    public int boneLevelsFromFeet = 3;
    public Transform baseBones;
    [Tooltip("Leaf bones do not have any functionality in the spidosaurus. In other models this may not be needed")]
    public bool ignoreFeetManipulation = true;
    public List<Transform> transformsToIgnore;
    private List<Transform> feet;
    private List<Transform> manipulationTransforms;

    public TransformTracker boneManipulatorPrefab;
    private GameObject manipulators;

    private void Start()
    {
        feet = new List<Transform>();
        manipulationTransforms = new List<Transform>();
        manipulators = new GameObject("Bone Manipulators");
        FindFeet(baseBones);
        SetupManipulations();
        
    }

    private void FindFeet(Transform t)
    {
        if (t.childCount == 0)
        {
            //At the feet object; if this is an ingnore transform then we dont want to add it
            if (transformsToIgnore.Contains(t) == false)
            {
                feet.Add(t);
            }
            return;
        }

        for (int i = 0; i < t.childCount; i++)
        {
            FindFeet(t.GetChild(i));
        }
    }

    private void SetupManipulations()
    {
        foreach (var foot in feet)
        {
            SetupManipulation(foot);
        }
    }

    private float distanceThreshold = 0.001f;
    private void SetupManipulation(Transform foot)
    {
        int level = 0;
        bool complete = false;
        Transform currentTransform = foot;
        while (!complete)
        {
            if (level > 0 || ignoreFeetManipulation == false)
            {
                AddTransformToManipulate(currentTransform);
            }
    
            ++level;
            if (level >= boneLevelsFromFeet)
            {
                complete = true;
                break;
            }

            Transform parentInRange = GetParentWithinThreshold(currentTransform);
            if (parentInRange == null) break;
            currentTransform = parentInRange;
        }
        if (complete == false)
        {
            //We can note here that we did not find a parent at "level"
        }
    }

    private Transform GetParentWithinThreshold(Transform t)
    {
        Transform parent = t.parent;
        // If we are the top (or effective top) of the heirarchy then bail early
        if (parent == null || parent == baseBones) return null;
        if (transformsToIgnore.Contains(parent)) return null;

        while (true)
        {
            float distance = Vector3.Distance(t.position, parent.position);
            if (distance > distanceThreshold)
            {
                return parent;
            }
            //keep going up the heirarchy until we are far enough away from a "zero distance"
            Transform newParent = parent.parent;

            //I am not a fan of "while true" but this is our escape clause
            if (newParent == null || newParent == baseBones) return null;
            if (transformsToIgnore.Contains(newParent)) return null;
            parent = newParent;
        }
    }

    private void AddTransformToManipulate(Transform t)
    {
        manipulationTransforms.Add(t);

        TransformTracker manipulator = Instantiate(boneManipulatorPrefab, t.position, t.rotation);
        manipulator.transformToUpdate = t;
        manipulator.transform.SetParent(manipulators.transform);
    }



    public float gizmosSize = 0.1f;
    private void OnDrawGizmos()
    {
        if (manipulationTransforms != null && manipulationTransforms.Count > 0)
        {
            foreach (var t in manipulationTransforms)
            {
                Gizmos.DrawSphere(t.position, gizmosSize);
            }
        } else if (feet != null && feet.Count > 0)
        {
            foreach (var foot in feet)
            {
                Gizmos.DrawSphere(foot.position, gizmosSize);
            }
        }

    }

}
