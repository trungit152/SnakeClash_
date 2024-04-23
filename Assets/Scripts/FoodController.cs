using UnityEngine;

public class FoodController : MonoBehaviour
{
    public static FoodController instance;
    //private EnemyMove2 enemyMove2;
    //private EnemyMove2 EnemyMove2
    //{
    //    get
    //    {
    //        if (enemyMove2 == null)
    //        {
    //            enemyMove2 = GameObject.Find("Enemy").transform.GetChild(0).GetComponent<EnemyMove2>();
    //        }
    //        return enemyMove2;
    //    }
    //    set
    //    {
    //        enemyMove2 = value;
    //    }
    //}
    private void Awake()
    {
        instance = this;
        //data.foods.Clear();
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
            //EnemyMove2.nearFoods.Remove(gameObject);
            Destroy(gameObject);
        }

    }
}
