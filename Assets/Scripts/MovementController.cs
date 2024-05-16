﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private GameObject snakeHead;
    [SerializeField] private Canvas inputCanvas;
    public float movementSpeed;
    [SerializeField] private DataSO data;
    [SerializeField] private GameObject firstBody;
    [SerializeField] Camera mainCamera;
    private float camSize;
    private Vector3 offset;

    private float canMove = 0;
    private float targetAngle;
    private List<Vector3> positionHistory;
    private Vector3 movementDirection;
    public int gap = 8;
    public List<GameObject> bodyParts;
    private bool isJoystick;
    private Rigidbody rb;

    public static MovementController instance;
    private void Awake()
    {
        instance = this;
        rb = snakeHead.GetComponent<Rigidbody>();
        Time.timeScale = 0;
    }
    private void Start()
    {
        positionHistory = new List<Vector3>();
        movementDirection = new Vector3(0, 0, 1);
        EnableJoystickInput();
        camSize = mainCamera.orthographicSize;
        if (offset == Vector3.zero)
        {
            offset = mainCamera.transform.position - snakeHead.transform.position;
        }
    }
    private void Update()
    {
        Move();
        mainCamera.transform.position = snakeHead.transform.position + offset;
    }
    public void AddToBodyParts(GameObject body)
    {
        bodyParts.Insert(0, body);
    }

    public void Stun()
    {
        targetAngle += 180f;
        canMove = 0.5f;
    }
    public void CameraUp()
    {
        StartCoroutine(ChangeCameraHeight(camSize, camSize + 4f, 0.5f));
    }
    public IEnumerator ChangeCameraHeight(float a, float b, float t)
    {
        float elapsedTime = 0f;
        while (elapsedTime < t)
        {
            camSize = Mathf.Lerp(a, b, elapsedTime / t);
            mainCamera.orthographicSize = camSize; 
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        camSize = b;
        mainCamera.orthographicSize = camSize;
    }
    private void MoveBody()
    {
        positionHistory.Insert(0, firstBody.transform.position);
        if (positionHistory.Count > 1000)
        {
            positionHistory.Remove(positionHistory[positionHistory.Count - 1]);
        }
        int i = 0;
        foreach (var body in bodyParts)
        {
            if (body != null)
            {
                Vector3 point = positionHistory[Mathf.Clamp(i * gap, 0, positionHistory.Count - 1)];
                Vector3 moveDirection = point - body.transform.position;
                body.transform.LookAt(point);
                body.transform.Translate(moveDirection.normalized * Time.deltaTime * movementSpeed, Space.World);
                i++;
            }
        }
    }
    private void Move()
    {
        if (isJoystick && snakeHead != null)
        {
            if (canMove > 0)
            {
                canMove -= Time.deltaTime;
            }
            else
            {
                canMove = 0;
            }
            if (joystick.Direction.sqrMagnitude > 0.01f && canMove == 0)
            {
                movementDirection = new Vector3(0, 0, 1);
                targetAngle = Mathf.Atan2(joystick.Direction.x, joystick.Direction.y) * Mathf.Rad2Deg;
            }
            rb.transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
            rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
        }
        MoveBody();
    }
    public void EnableJoystickInput()
    {
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }
    public void Collide()
    {
        canMove = 0.5f;
        targetAngle = Mathf.Atan2(rb.transform.position.x, rb.transform.position.z) * Mathf.Rad2Deg + 180f;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
    }
    public void IncreaseSpeed()
    {
        movementSpeed += 0.035f;
    }
    public void SpeedUp(float add)
    {
        movementSpeed += add;
    }
    public void SpeedDown(float add)
    {
        movementSpeed -= add;
    }
}