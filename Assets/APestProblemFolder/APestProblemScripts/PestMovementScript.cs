using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestMovementScript : MonoBehaviour
{
    public PestHealthScript pestHealthScript;

    public BoxCollider movementBounds;
    [SerializeField] float speed, secondsBetweenMovements;
    [SerializeField] float minMovementAreaZ, maxMovementAreaZ; //setting the variables for movement area range
    [SerializeField] float minMovementAreaX, maxMovementAreaX; //setting the variables for movement area range
    private Vector3 movementPosition;
    private float movementRandomRangeZ;
    private float movementRandomRangeX;

    public bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PestMovement());
    }

    IEnumerator PestMovement()
    {

        while (true)//infinite loop till broken
        {
            Bounds movementBoundsBox = movementBounds.bounds;
            movementRandomRangeZ = Random.Range(minMovementAreaZ, maxMovementAreaZ);//chooses a random movement value between the 2 variables for z
            movementRandomRangeX = Random.Range(minMovementAreaX, maxMovementAreaX);//chooses a random movement value between the 2 variables for x

            movementPosition = new Vector3(movementRandomRangeX, transform.position.y, movementRandomRangeZ);//finds a random point using the x and z random movement values
            yield return new WaitForSeconds(secondsBetweenMovements);

        }









    }


    // Update is called once per frame
    void Update()
    {
        if (pestHealthScript.dead == true && alive == true)
        {

            alive = false;

            Debug.Log("Pest deaded.");
        
        }

        if (alive)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementPosition, speed * Time.deltaTime);//moves towards the random position at a set speed within the inspector.

            Bounds bounds = movementBounds.bounds;
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, bounds.min.x, bounds.max.x);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, bounds.min.z, bounds.max.z);
            transform.position = clampedPosition;
        }
    }
}
