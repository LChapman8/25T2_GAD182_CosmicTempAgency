using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatterMovementMechanicsScript : MonoBehaviour
{
    public BoxCollider movementBounds;

    [SerializeField] private Camera mainCamera;

    [SerializeField]//allows the variables to be visible within the unity inspector.
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;//sets the mainCamera variable to the main camera within the scene.
    }

    // Update is called once per frame
    void Update()
    {
        FollowMousePosition(speed);
        float input = Input.GetAxis("Vertical");

        Bounds bounds = movementBounds.bounds;
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bounds.min.x, bounds.max.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, bounds.min.z, bounds.max.z);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bounds.min.y, bounds.max.y);
        transform.position = clampedPosition;
    }

    private void FollowMousePosition(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, GetWorldPositionFromMouse(), speed * Time.deltaTime);//moves towards the cursor position at speed set by the speed variable in realtime.


    }



    private Vector3 GetWorldPositionFromMouse()
    {

        Plane plane = new Plane(Vector3.up, new Vector3(0, 2f, 0)); // Plane at Y = 2
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Debug.DrawLine(ray.origin, hitPoint, Color.red);
            return hitPoint;
        }

        return transform.position;
    }
}