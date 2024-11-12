using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update()
    {
        FaceMouse();
    }

    private void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction to the mouse from the current object's position
        Vector2 direction = mousePosition - transform.position;
        transform.right = direction;

        // Get the position of the parent object (Player)
        Vector3 parentPosition = transform.parent.position;
        if (mousePosition.x > parentPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z-45);
        }
        else
        {
            transform.rotation = Quaternion.Euler(-180, -180, transform.rotation.eulerAngles.z+45);
        }
    }

}
