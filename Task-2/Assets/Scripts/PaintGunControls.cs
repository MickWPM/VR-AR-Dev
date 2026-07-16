using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PaintGunControls : MonoBehaviour
{
    private InputAction attackAction;
    private PaintGunController controller;
    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        controller = GetComponent<PaintGunController>();
    }

    [SerializeField] private float FireDelay = 0.5f;
    private float fireCountdown = 0;
    private bool holdingGun = false;
    private void Update()
    {
        fireCountdown-= Time.deltaTime;
        if (fireCountdown > 0) return;

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
