using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movementDirection;
    private float targetAngle;
    private float movementSpeed;
    private System.Random rand;
    private GameObject foodTarget;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rand = new System.Random();
        targetAngle = rand.Next(0, 360);
        movementDirection = new Vector3(0, 0, 1);
        movementSpeed = 10f;
    }
    void Update()
    {
        Move();
    }

    private void Move()
    {
        rb.transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        rb.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            targetAngle += 180;
        }
    }
    private void RandFood()
    {
        //rand.Next(0,SpawnFood.instance.foods.Count())
        if (SpawnFood.instance.foods != null)
        {
            if (SpawnFood.instance.foods.Count > 0)
            {
                int rand_ = Random.Range(0, SpawnFood.instance.foods.Count);
                foodTarget = SpawnFood.instance.foods[rand_];
            }
        }
    }
    public void RemoveTarget()
    {
        foodTarget = null;
    }
}
