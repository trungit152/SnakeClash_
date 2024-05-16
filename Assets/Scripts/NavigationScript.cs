using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementDirection;
    public float targetAngle;
    public float movementSpeed;
    private Vector3 res;
    private float stunned;

    GameObject foodTarget;
    
    public bool isTarget;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stunned = 0;
        movementDirection = new Vector3(0,0,1); 
        targetAngle = 0;
    }
    private void Update()
    {
        FindFood();
       
    }
    private void FindFood()
    {
        if(stunned == 0)
        {
            if (foodTarget != null)
            {
                targetAngle = Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg;
                if (Vector3.Distance(transform.position, foodTarget.transform.position) < 0.05f)
                {
                    foodTarget = null;
                }
            }
            else
            {
                RandFood();
            }
        }
        else
        {
            if (stunned > 0)
            {
                stunned -= Time.deltaTime;
            }
            else stunned = 0;
        }
        rb.transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 1f);

    }
    public void Stun()
    {
        targetAngle += 180f;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
        stunned = 0.5f;
        foodTarget = null;
    }
    public void Busy()
    {
        stunned = Random.Range(1f, 5f);
        foodTarget = null;
    }
    private void RandFood()
    {
        if(SpawnFood.instance.foods != null)
        {
            if (SpawnFood.instance.foods.Count > 0)
            {
                foreach (var food in SpawnFood.instance.foods)
                {
                    if (Vector3.Distance(food.transform.position, gameObject.transform.position) < 20f)
                    {
                        foodTarget = food;
                    }
                }
                if(foodTarget == null)
                {
                    int rand_ = Random.Range(0, SpawnFood.instance.foods.Count);
                    foodTarget = SpawnFood.instance.foods[rand_];
                }
                res = foodTarget.transform.position - transform.position;
            }
        }
    }
    public void RemoveTarget()
    {
        foodTarget = null;
    }
    public void IncreseSpeed()
    {
        movementSpeed += 0.035f;
    }
    public void SetAngle(float value)
    {
        targetAngle = value;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 120f);
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
