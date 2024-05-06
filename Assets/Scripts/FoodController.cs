using UnityEngine;
using DG.Tweening;
using ParadoxNotion.Design;
public class FoodController : MonoBehaviour
{
    public static FoodController instance;
    public float speed = 15f;
    private bool isBit;
    private Vector3 target;
    private void Awake()
    {
        instance = this;
        isBit = false;
    }
    private void Update()
    {
        if (isBit && gameObject != null)
        {
            Vector3 direction = target - transform.position;
            float distance = direction.magnitude;
            if (distance > 0.01f)
            {
                Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
                transform.Translate(moveDirection, Space.World);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHead"))
        {
            SpawnFood.instance.foods.Remove(gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("Head"))
        {
            NavigationScript nav = other.GetComponent<NavigationScript>();
            nav.RemoveTarget();
            SpawnFood.instance.foods.Remove(gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("HitBox"))
        {
            isBit = true;
            target = other.transform.position;
        }

    }
}
