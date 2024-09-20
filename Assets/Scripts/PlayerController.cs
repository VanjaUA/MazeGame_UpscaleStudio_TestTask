using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform viewPoint;
    [SerializeField] private float mouseSensitivity = 1f;
    private float verticalRotationStore;
    private Vector2 mouseInput;

    [SerializeField] private bool invertLook;

    [SerializeField] private float walkSpeed, runSpeed;
    private float activeMoveSpeed;
    private Vector3 moveDirection, movement;

    private CharacterController characterController;

    private Camera mainCamera;

    private const string KEY_TAG = "Key";
    private int keyCount = 0;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        mainCamera = Camera.main;
    }

    void Update()
    {
        RotationHandle();

        MoveHandle();
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = viewPoint.position;
        mainCamera.transform.rotation = viewPoint.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(KEY_TAG))
        {
            Destroy(other.gameObject);
            keyCount++;
            UIController.Instance.UpdateKeysText(keyCount);
        }
    }

    private void RotationHandle()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            return;
        }
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        verticalRotationStore -= mouseInput.y;
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, -60f, 60f);

        viewPoint.rotation = Quaternion.Euler((invertLook ? -1f : 1f) * verticalRotationStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
    }

    private void MoveHandle()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = walkSpeed;
        }

        movement = ((transform.right * moveDirection.x) + (transform.forward * moveDirection.z)).normalized * activeMoveSpeed;


        characterController.Move(movement * Time.deltaTime);
    }
}
