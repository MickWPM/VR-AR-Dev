using Fusion;
using UnityEngine;


public class AvatarControl : NetworkBehaviour
{

    public GameObject XROriginGO, XRCameraGO, flatscreenControlGO;
    private SceneReferences sceneReference;

    private void Awake()
    {
        //Set control system - if VR is enabled we do the VR control, otherwise we do the Windows control
        //pass for now
        sceneReference = GameObject.FindFirstObjectByType<SceneReferences>();
        XROriginGO = sceneReference.XROriginGO;
        XRCameraGO = sceneReference.XRCameraGO;
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
        switch (sceneReference.controlSystem)
        {
            case ControlSystem.Windows:
                UpdateNonVR();
                break;
            case ControlSystem.VR:
                UpdateVR();
                break;
            default:
                break;
        }
    }

    private void UpdateNonVR()
    {
        transform.position = flatscreenControlGO.transform.position;
        transform.rotation = flatscreenControlGO.transform.rotation;
    }

    private void UpdateVR()
    {
        
        transform.position = XROriginGO.transform.position;
        Vector3 lookRot = new Vector3(0, XRCameraGO.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(lookRot);
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
