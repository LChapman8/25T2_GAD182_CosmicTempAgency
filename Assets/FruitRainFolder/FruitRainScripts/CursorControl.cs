using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]//allows the variables to be visible within the unity inspector.
    private float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;//sets the mainCamera variable to the main camera within the scene.
    }

    // Update is called once per frame
    void Update()
    {
        FollowMousePositionDelayed(maxSpeed);
        float input = Input.GetAxis("Vertical");

        float clampedY = Mathf.Clamp(transform.position.y, (float)-2.5, (float)-2.5);//sets the clamped y axis
        Vector2 pos = transform.position;//get the baskets current position
        pos.y = clampedY;//reassign the y value to the clamped position
        transform.position = pos;//reassign the clamped value onto the basket
    }

    private void FollowMousePositionDelayed(float maxSpeed)
    { 
        transform.position = Vector2.MoveTowards(transform.position, GetWorldPositionFromMouse  (), maxSpeed * Time.deltaTime);//moves towards the cursor position at speed set by the maxSpeed variable in realtime.
        
    
    }



    private Vector2 GetWorldPositionFromMouse()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);//mouse position in the world space is found through the position of the cursor in relation to the camera
    
    }
}
