using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float horisontalSpeed = 2f;
    public float verticalSpeed = 1f;
    public Vector3 targetRotation;


    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            targetRotation.x = Mathf.Clamp(targetRotation.x + Input.GetAxis("Mouse Y") * verticalSpeed,-45f,45f);
            targetRotation.y += Input.GetAxis("Mouse X") * horisontalSpeed;
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * 10f);
    }

    public float ApplyRotationToBody()
    {
        return targetRotation.y;
    }
}
