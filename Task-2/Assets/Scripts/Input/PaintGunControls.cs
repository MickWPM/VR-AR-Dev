using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PaintGunControls : MonoBehaviour
{
    private InputAction attackAction, clearAction;
    private PaintGunController controller;
    private PainterCanvas[] canvases;
    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        clearAction = InputSystem.actions.FindAction("Interact");
        controller = GetComponent<PaintGunController>();

        canvases = GameObject.FindObjectsByType<PainterCanvas>(FindObjectsSortMode.None);
    }

    [SerializeField] private float FireDelay = 0.5f;
    private float fireCountdown = 0;
    private bool holdingGun = false;
    private void Update()
    {
        fireCountdown-= Time.deltaTime;
        if (fireCountdown > 0) return;

        if (clearAction.WasPressedThisFrame())
        {
            foreach (var c in canvases)
            {
                c.ResetCanvas();
            }
        }
        if (holdingGun) HoldingGunUpdate();
    }


    private void HoldingGunUpdate()
    {
        if (attackAction.IsPressed())
        {
            fireCountdown = FireDelay;
            controller.Fire();
        }
    }


    public void GrabbedGun()
    {
        holdingGun = true;
    }

    public void ReleasedGun()
    {
        holdingGun = false;
    }

}
