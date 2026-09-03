using Fusion;
using UnityEngine;


public class AvatarControl : NetworkBehaviour
{

    public GameObject XROriginGO, flatscreenControlGO;
    private SceneReferences sceneReference;

    private void Awake()
    {
        //Set control system - if VR is enabled we do the VR control, otherwise we do the Windows control
        //pass for now
        sceneReference = GameObject.FindFirstObjectByType<SceneReferences>();
        XROriginGO = sceneReference.XROriginGO;
        flatscreenControlGO = sceneReference.flatscreenControlGO;
    }

    public override void Spawned()
    {
        //This is only relevant for local control (to be replicated over the network)
        if (Object.HasInputAuthority == false)
        {
            this.enabled = false;
            return;
        }
        OnStartSetup();
    }

    public override void FixedUpdateNetwork()
    {
        //assume windows for nw
        Debug.Log("Setting position and rotation");
        transform.position = flatscreenControlGO.transform.position;
        transform.rotation = flatscreenControlGO.transform.rotation;
    }

    private void OnStartSetup()
    {
        switch (sceneReference.controlSystem)
        {
            case ControlSystem.Windows:
                NonVRSetup();
                break;
            case ControlSystem.VR:
                VRSetup();
                break;
            default:
                Debug.LogError("We added another control scheme that isnt handled.....");
                break;
        }
    }

    private void VRSetup()
    {
        Destroy(flatscreenControlGO);
    }
    private void NonVRSetup()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
