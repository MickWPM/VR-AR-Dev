using UnityEngine;

public enum ControlSystem { Windows, VR }
public class SceneReferences : MonoBehaviour
{
    public ControlSystem controlSystem;

    public GameObject XROriginGO, flatscreenControlGO;

    private void Awake()
    {
        switch (controlSystem)
        {
            case ControlSystem.Windows:
                Destroy(XROriginGO);
                break;
            case ControlSystem.VR:
                Destroy(flatscreenControlGO);
                break;
            default:
                break;
        }
    }
}
