using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro; 

public class EnemyCollide : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefabs;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject fullBody;
    [SerializeField] GameObject foodPrefabs;
    [SerializeField] GameObject kingState;
    [SerializeField] private GameObject fullFoods;
    [SerializeField] private TextMeshPro levelText;

    private List<Vector3> bodyFoods;
    private List<GameObject> bodyParts;
    private List<Vector3> positionHistory;
    private float speedUpAdd = 10f;
    private float itemTime = 3f;
    private NavigationScript nav;
    private NavigationScript Nav
    {
        get
        {
            if (nav == null)
            {
                nav = GetComponent<NavigationScript>();
            }
            return nav;
        }
        set { nav = value; }
    }
    private ItemSpawn itemSpawn;
    private ItemSpawn ItemSpawn
    {
        get
        {
            if(itemSpawn == null)
            {
                itemSpawn = GameObject.Find("FoodController").GetComponent<ItemSpawn>();
            }
            return itemSpawn;
        }
        set
        {
            itemSpawn = value;
        }
    }

    private int gap = 10;
    private float gapf = 10f;
    public int level;
    private void Start()
    {
        fullFoods = GameObject.Find("FullFood");
        bodyParts = new List<GameObject>();
        positionHistory = new List<Vector3>();
        kingState.SetActive(false);
        levelText.text = "Level " + level.ToString();
        for (int i = 0; i < 20 + level; i++)
        {
            if (i % 5 == 0)
            {
                GrowSnake();
                SizeGrow();
            }
        }
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        MoveBody();
    }

    //ham di chuyen body
    private void MoveBody()
    {
        positionHistory.Insert(0, transform.position);
        if(positionHistory.Count > 1000)
        {
            positionHistory.Remove(positionHistory[positionHistory.Count - 1]);
        }
        int i = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionHistory[Mathf.Clamp(i * gap, 0, positionHistory.Count() - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.LookAt(point);
            body.transform.Translate(moveDirection.normalized * Time.deltaTime * moveSpeed, Space.World);
            i++;
        }
    }
    private void GrowSnake()
    {
        //Gioi han so body
        if(bodyParts.Count < 60) 
        {
            GameObject body = Instantiate(bodyPrefabs);
            if (bodyParts.Count() != 0)
            {
                body.transform.localScale = bodyParts[0].transform.localScale;
                body.transform.position = bodyParts[0].transform.position;
            }
            else
            {
                body.transform.position = this.transform.position;
            }
            bodyParts.Insert(0, body);
            body.transform.SetParent(fullBody.transform);
            gapf += 0.04f;
            gap = (int)gapf;
        }
        moveSpeed += 0.05f;
        Nav.IncreseSpeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            level+=2;
            if(level > ItemSpawn.kingLevel)
            {
                ItemSpawn.kingLevel = level;
                ItemSpawn.SetKing2(gameObject);
                
            }
            levelText.text = "Level " + level.ToString();
            if (level % 6 == 0)
            {
                GrowSnake();
                SizeGrow();
            }
        }
        if (other.CompareTag("PlayerBody"))
        {
            BodyController bodyCtr = other.GetComponent<BodyController>();
            HeadController headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
            if (level > bodyCtr.GetLevel())
            {
                bodyCtr.Bit();
                MoveAfterBite(headController.bodyFoods);
            }
            else
            {
                Nav.Stun();
            }
        }
        if (other.CompareTag("Body") && (other.transform.parent.parent != gameObject.transform.parent))
        {
            EnemyBodyController bodyCtr = other.GetComponent<EnemyBodyController>();
            EnemyCollide enemyHead = other.transform.parent.parent.GetChild(0).GetComponent<EnemyCollide>();
            if (level >= bodyCtr.GetLevel())
            {
                bodyCtr.Bit();
                MoveAfterBite(enemyHead.bodyFoods);
            }
            else
            {
                Nav.Stun();
            }
        }
        if (other.CompareTag("SpeedUp"))
        {
            SpeedUp();
        }
        if (other.CompareTag("Magnite"))
        {
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            HeadController head = collision.gameObject.GetComponent<HeadController>();
            if (level > head.level)
            {
                head.Die();
            }
            else
            {
                Die();
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Head") && (collision.gameObject.transform.parent != gameObject.transform.parent))
        {
            EnemyCollide enemyHead = collision.gameObject.GetComponent<EnemyCollide>();
            if (level >= enemyHead.level)
            {
                enemyHead.Die();
                MoveAfterBite(enemyHead.bodyFoods);
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Nav.Stun();
        }
    }
    private void MoveAfterBite(List<Vector3> foods)
    {
        for (int i = 0; i < foods.Count; i++)
        {
            Vector3 res = foods[i] - gameObject.transform.position;
            Nav.Busy();
            Nav.SetAngle(Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg);
        }
    }
    private void SizeGrow()
    {
        if (level < 600)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            for (int i = 0; i < bodyParts.Count(); i++)
            {
                bodyParts[i].transform.localScale = transform.localScale / 2;
            }
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), 20f * Time.deltaTime);
        }
    }
    public void HeadBit()
    {
        bodyFoods = new List<Vector3>();
        for (int i = 0; i < bodyParts.Count(); i++)
        {
            EnemyBodyController bdCtr = bodyParts[i].GetComponent<EnemyBodyController>();
            if (bdCtr.isBit)
            {
                for (int j = bodyParts.Count - 1; j >= i; j--)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        GameObject food = Instantiate(foodPrefabs, RandomPos(bodyParts[j]), Quaternion.identity);
                        food.transform.localScale = RandomScale(bodyParts[j]);
                        food.transform.SetParent(fullFoods.transform);
                    }
                    bodyFoods.Insert(0, bodyParts[j].transform.position);
                    Destroy(bodyParts[j]);
                    bodyParts.RemoveAt(j);
                }
                i = bodyParts.Count();
            }
        }
    }
    private void SpeedUp()
    {
        moveSpeed += speedUpAdd;
        Nav.SpeedUp(speedUpAdd);
        gap -= (int)speedUpAdd / 3;
        gapf -= speedUpAdd / 3;
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(itemTime);
        gap += (int)speedUpAdd / 3;
        gapf += speedUpAdd / 3;
        moveSpeed -= speedUpAdd;
        Nav.SpeedDown(speedUpAdd);
    }
    public void Die()
    {
        bodyFoods = new List<Vector3>();
        for (int i = bodyParts.Count() - 1; i >= 0; i--)
        {
            for (int k = 0; k < 3; k++)
            {
                GameObject food = Instantiate(foodPrefabs, RandomPos(bodyParts[i]), Quaternion.identity);
                food.transform.localScale = RandomScale(bodyParts[i]);
                food.transform.SetParent(fullFoods.transform);
            }
            bodyFoods.Insert(0, bodyParts[i].transform.position);
            Destroy(bodyParts[i]);
            bodyParts.RemoveAt(i);
        }
        for (int k = 0; k < 5; k++)
        {
            GameObject food = Instantiate(foodPrefabs, RandomPos(gameObject), Quaternion.identity);
            food.transform.localScale = RandomScale(gameObject);
            food.transform.SetParent(fullFoods.transform);
        }
        ItemSpawn.SpawnEnemy(level/2);
        bodyFoods.Insert(0, gameObject.transform.position);
    }
    private Vector3 RandomPos(GameObject body)
    {
        Vector3 pos = new Vector3(body.transform.position.x + Random.Range(-50, 50) / 100f, body.transform.position.y,
            body.transform.position.z + Random.Range(-50, 50) / 100f);
        return pos;
    }
    private Vector3 RandomScale(GameObject body)
    {
        float rd = Random.Range(-400, 100) / 1000f;
        Vector3 scale = new Vector3(body.transform.localScale.x + rd, body.transform.localScale.y + rd, body.transform.localScale.z + rd);
        return scale;
    }
}
