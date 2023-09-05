using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private float speed;
    public Vector3 direction = Vector3.right;
    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> directions = new List<Vector3>();
    
    void Start()
    {
        speed = FindObjectOfType<SnakeHeadController>().speed;
        // I NEED TO CODE HERE HOW A NEWLY CREATED BODY PART CAN RETRIEVE THE LAST PIECE OF THE BODIES SET OF INSTRUCTIONS
    }

    void Update()
    {
        if (positions.Count == 0) return;
        if (direction == Vector3.right && transform.position.x >= positions[0].x)
        {
            SetDirection();
        }

        if (direction == Vector3.left && transform.position.x <= positions[0].x)
        {
            SetDirection();
        }

        if (direction == Vector3.up && transform.position.y >= positions[0].y)
        {
            SetDirection();
        }


        if (direction == Vector3.down && transform.position.y <= positions[0].y)
        {
            SetDirection();
        }
    }

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection()
    {
        direction = directions[0];
        positions.RemoveAt(0);
        directions.RemoveAt(0);
    }

    public void GetInput(Vector3 inputDir, Vector3 inputPos)
    {
        directions.Add(inputDir);
        positions.Add(inputPos);
    }

    public List<Vector3> GetDirections()
    {
        return directions;
    }

    public List<Vector3> GetPositions()
    {
        return positions;
    }

    public void InitLists(List<Vector3> posList, List<Vector3> dirList)
    {
        positions = posList;
        directions = dirList;
    }
}