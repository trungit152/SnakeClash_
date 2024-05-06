using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;
    private Vector3 movementDirection;

    public float targetAngle;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementDirection = new Vector3(0, 0, 1);
        moveSpeed = 12f;
        targetAngle = 0;
    }
    private void Update()
    {
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("-90deg"))
        {
            targetAngle = -90f;
        }
        else if (other.CompareTag("180deg"))
        {
            targetAngle = 180f;
        }
        else if (other.CompareTag("90deg"))
        {
            targetAngle = 90f;
        }
        else if (other.CompareTag("0deg"))
        {
            targetAngle = 0f;
        }
    }
    private void Move()
    {
        rb.transform.Translate(movementDirection * Time.deltaTime * moveSpeed);
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
    }
}
