using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject snakeBodyPrefab;
    public Vector3 direction;
    public List<GameObject> snakeBody = new List<GameObject>();
    
    private SnakeMovement mySnakeMovement;
    private float rayLength = 1f;
    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    void Start()
    {
        mySnakeMovement = GetComponent<SnakeMovement>();
        snakeBody.Add(gameObject);
        direction = Vector3.right;
        AddSegment(); // Add the starting segment
    }

    void Update()
    {
        Vector3 currentDir = direction;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 && direction.x == 0)
        {
            direction = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
        }
        
        if (verticalInput != 0 && direction.y == 0)
        {
            direction = new Vector3(0, Mathf.Sign(verticalInput), 0);
        }

        // Normalize the direction to prevent faster diagonal movement
        direction.Normalize();

        if (currentDir != direction)
        {
            mySnakeMovement.direction = direction;
            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i].GetComponent<SnakeMovement>().GetInput(direction, transform.position);
            }
        }


        // Check for collision with body
        rayOrigin = transform.position;
        rayDirection = direction;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Body"))
            {
                Destroy(gameObject);
            }
        }
    }

    void AddSegment()
    {
        GameObject lastSegment = snakeBody[snakeBody.Count-1];
        Vector3 lastSegmentDir = lastSegment.GetComponent<SnakeMovement>().direction;
        Vector3 spawnLocation = lastSegment.transform.position - lastSegmentDir;
        List<Vector3> posList = lastSegment.GetComponent<SnakeMovement>().GetPositions();
        List<Vector3> dirList = lastSegment.GetComponent<SnakeMovement>().GetDirections();
        // Instantiate a new segment prefab and add it to the segments list
        GameObject newSegment = Instantiate(snakeBodyPrefab, spawnLocation, Quaternion.identity);
        newSegment.GetComponent<SnakeMovement>().direction = lastSegmentDir;
        if (snakeBody.Count > 1)
        {
            newSegment.GetComponent<SnakeMovement>().InitLists(posList, dirList);
        }
        snakeBody.Add(newSegment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            AddSegment();
            Destroy(other.gameObject);
        }
    }
}