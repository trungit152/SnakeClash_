using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class EnemyCollide : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefabs;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameObject fullBody;
    [SerializeField] GameObject foodPrefabs;
    [SerializeField] GameObject kingState;
    [SerializeField] private GameObject fullFoods;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private DataSO data;
    [SerializeField] private GameObject firstBody;
    [SerializeField] private GameObject headSkin;

    private List<Vector3> bodyFoods;
    private List<GameObject> bodyParts;
    private List<Vector3> positionHistory;
    private float speedUpAdd = 7f;
    private float itemTime = 3f;

    /*******************/
    private int randSkin;
    public string enemyName;
    private int skinPath_;
    private int skinCounter = 0;
    private Rigidbody rb;
    private Vector3 movementDirection;
    public float targetAngle;
    public float movementSpeed;
    private Vector3 res;
    private float stunned;
    GameObject foodTarget;
    public bool isTarget;
    /*******************/
    private ItemSpawn itemSpawn;
    private ItemSpawn ItemSpawn
    {
        get
        {
            if (itemSpawn == null)
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

    private RankingController rankingController;
    private RankingController RankingController
    {
        get
        {
            if (rankingController == null)
            {
                rankingController = GameObject.Find("RankingController").gameObject.GetComponent<RankingController>();
            }
            return rankingController;
        }
        set
        {
            rankingController = value;
        }
    }

    private int gap = 5;
    private float gapf = 5f;
    public int level;
    private void Awake()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0.1f;
        transform.position = currentPosition;
        kingState.SetActive(false);
    }
    private void Start()
    {
        /****************/
        rb = GetComponent<Rigidbody>();
        stunned = 0;
        movementDirection = new Vector3(0, 0, 1);
        targetAngle = 0;
        /****************/
        randSkin = Random.Range(0, data.skins.Count);
        int skinPath = data.skins[randSkin].dataSprite.Count - 2;
        skinPath_ = skinPath;
        moveSpeed = 10f;
        movementSpeed = 10f;
        name = enemyName;
        fullFoods = GameObject.Find("FullFoods");
        bodyParts = new List<GameObject>();
        GameObject body = Instantiate(bodyPrefabs);
        body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[data.skins[randSkin].dataSprite.Count - 1];
        body.transform.position = firstBody.transform.position;
        bodyParts.Insert(0, body);
        body.transform.SetParent(fullBody.transform);

        positionHistory = new List<Vector3>();
        levelText.text = enemyName + ": " + level.ToString();
        for (int i = 0; i < 20 + level; i++)
        {
            if (i % 5 == 0)
            {
                GrowSnake();
                SizeGrow();
            }
        }

        //gan skin cho ran
        SpriteRenderer headSkinSprite = headSkin.GetComponent<SpriteRenderer>();
        headSkinSprite.sprite = data.skins[randSkin].dataSprite[0];
        for (int i = 0; i < bodyParts.Count - 1; i++)
        {
            bodyParts[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[IndexCounter(skinPath_, skinCounter)];
            skinCounter++;
        }
    }
    private int IndexCounter(int nums, int i)
    {
        return (i % nums) + 1;
    }
    private void Update()
    {
        MoveBody();
        FindFood();
    }

    //ham di chuyen body
    private void MoveBody()
    {
        positionHistory.Insert(0, transform.position);
        if (positionHistory.Count > 1000)
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
        if (bodyParts.Count < 60)
        {
            GameObject body = Instantiate(bodyPrefabs);
            body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[IndexCounter(skinPath_, skinCounter++)];
            if (bodyParts.Count() != 0)
            {
                body.transform.localScale = bodyParts[0].transform.localScale;
                body.transform.position = firstBody.transform.position;
                bodyParts.Insert(0, body);
            }
            else
            {
                body.transform.position = this.transform.position;
                bodyParts.Insert(0, body);
            }
            body.transform.SetParent(fullBody.transform);
            gapf += 0.05f;
            gap = (int)gapf;
        }
        if (moveSpeed < 22f)
        {
            moveSpeed += 0.035f;
            IncreseSpeed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            level += 2;
            if (level > ItemSpawn.kingLevel)
            {
                ItemSpawn.kingLevel = level;
                ItemSpawn.SetKing2(gameObject);
            }
            levelText.text = enemyName +": " + level.ToString();
            if (level % 10 == 0 || (level + 1) % 10 == 0)
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
                Stun();
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
                Stun();
            }
        }
        if (other.CompareTag("SpeedUp"))
        {
            SpeedUp();
        }
        if (other.gameObject.CompareTag("PlayerHead"))
        {
            HeadController head = other.gameObject.GetComponent<HeadController>();
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
        if (other.gameObject.CompareTag("Head") && (other.gameObject.transform.parent != gameObject.transform.parent))
        {
            EnemyCollide enemyHead = other.gameObject.GetComponent<EnemyCollide>();
            if (level >= enemyHead.level)
            {
                enemyHead.Die();
                MoveAfterBite(enemyHead.bodyFoods);
                Destroy(other.gameObject.transform.parent.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            Stun();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
    private void MoveAfterBite(List<Vector3> foods)
    {
        if (foods.Count >= 5)
        {
            int i = foods.Count - 5;
            Vector3 res = foods[i] - gameObject.transform.position;
            Busy();
            SetAngle(Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg);
        }
    }
    private void SizeGrow()
    {
        if (level < 600)
        {
            transform.localScale += new Vector3(0.025f, 0.025f, 0.025f);
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
                    for (int k = 0; k < 2; k++)
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
        SpeedUp(speedUpAdd);
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
        SpeedDown(speedUpAdd);
    }
    public void Die()
    {
        bodyFoods = new List<Vector3>();
        for (int i = bodyParts.Count() - 1; i >= 0; i--)
        {
            for (int k = 0; k < 2; k++)
            {
                GameObject food = Instantiate(foodPrefabs, RandomPos(bodyParts[i]), Quaternion.identity);
                food.transform.localScale = RandomScale(bodyParts[i]);
                food.transform.SetParent(fullFoods.transform);
            }
            bodyFoods.Insert(0, bodyParts[i].transform.position);
            Destroy(bodyParts[i]);
            bodyParts.RemoveAt(i);
        }
        for (int k = 0; k < 3; k++)
        {
            GameObject food = Instantiate(foodPrefabs, RandomPos(gameObject), Quaternion.identity);
            food.transform.localScale = RandomScale(gameObject);
            food.transform.SetParent(fullFoods.transform);
        }
        ItemSpawn.enemies.Remove(gameObject.transform.parent.gameObject);
        if (gameObject == ItemSpawn.king)
        {
            ItemSpawn.KingDie(gameObject);
        }
        
        RankingController.enemiesRank.Remove(gameObject.transform.parent.gameObject);       
        ItemSpawn.SpawnEnemy(Mathf.Max(level / 2, level - 50));
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
        float rd = Random.Range(500, 1000) / 1000f;
        Vector3 scale = new Vector3(body.transform.localScale.x + rd, body.transform.localScale.y + rd, body.transform.localScale.z + rd);
        return scale;
    }

    /*******************************/
    private void FindFood()
    {
        if (stunned == 0)
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
        targetAngle = Mathf.Atan2(rb.transform.position.x, rb.transform.position.z) * Mathf.Rad2Deg + 180f;
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
        if (SpawnFood.instance.foods != null)
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
                if (foodTarget == null)
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
