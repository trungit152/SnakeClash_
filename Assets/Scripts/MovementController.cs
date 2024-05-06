using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private GameObject snakeHead;
    [SerializeField] private Canvas inputCanvas;
    public float movementSpeed;
    [SerializeField] private DataSO data;

    private float canMove = 0;
    private float targetAngle;
    private Vector3 movementDirection;
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
        movementDirection = new Vector3(0, 0, 1);
        EnableJoystickInput();
    }
    private void Update()
    {
        Move();
    }
    public void Stun()
    {
        targetAngle += 180f;
        canMove = 0.5f;
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
    public void IncreseSpeed()
    {
        movementSpeed += 0.04f;
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
