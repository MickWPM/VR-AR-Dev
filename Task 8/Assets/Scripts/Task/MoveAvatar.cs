using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class MoveAvatar : NetworkBehaviour
{
    [SerializeField] private InputActionReference moveActionRef;

    private void OnEnable()
    {
        moveActionRef.action.Enable();
    }

    private void OnDisable()
    {
        moveActionRef.action.Disable();
    }

    public float speed = 1, turnSpeed = 90;
    private void Update()
    {
        //if (Object.HasInputAuthority == false) return;

        Vector2 moveInput = moveActionRef.action.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0)
        {
            transform.position += speed * Time.deltaTime * transform.forward * moveInput.y;
            transform.rotation *= Quaternion.AngleAxis(turnSpeed * Time.deltaTime * moveInput.x, Vector3.up);
        }
    }
}