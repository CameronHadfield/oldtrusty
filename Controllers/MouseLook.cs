using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame

    void Rotate(Vector2 movement){
        playerBody.Rotate(Vector3.up * movement.x);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked){
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            Rotate(new Vector2(mouseX, mouseY));
        }
    }
}
