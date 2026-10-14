using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private VirtualJoystick joystick;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector2 input = joystick.Direction;
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            _characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        _characterController.Move(Vector3.down * (9.81f * Time.deltaTime));
    }
}