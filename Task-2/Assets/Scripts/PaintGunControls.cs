using UnityEngine;
using UnityEngine.InputSystem;

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
    private void Update()
    {
        fireCountdown-= Time.deltaTime;
        if (fireCountdown > 0) return;

        if (attackAction.IsPressed())
        {
            fireCountdown = FireDelay;
            controller.Fire();
        }
    }

}
