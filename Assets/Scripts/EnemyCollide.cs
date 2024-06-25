using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using System.ComponentModel;
using UnityEngine.EventSystems;

[BurstCompile]  
public class EnemyCollide : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefabs;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject fullBody;
    [SerializeField] FoodController foodPrefabs;
    [SerializeField] GameObject kingState;
    [SerializeField] private GameObject fullFoods;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private DataSO data;
    [SerializeField] private GameObject firstBody;
    [SerializeField] private GameObject headSkin;
    [SerializeField] private Camera mainCamera;

    private List<Vector3> bodyFoods;
    private List<GameObject> bodyParts;
    private List<Vector3> positionHistory;
    private List<Quaternion> rotationHistory;
    private float speedUpAdd = 7f;
    private float itemTime = 3f;

    private float deltaTime = 0.0f;
    private float fps;
    /*******************/
    private bool isChasing = false;
    private float chaseTime = 4f;
    private bool canChase = true;
    private bool isRunningAway = false;
    private float runningAwayTime = 4f;
    private bool canRunAway = true;
    private Vector3 cameraPos;
    private int randSkin;
    private int randSkin_;
    public string enemyName;
    private int skinPath_;
    private int skinCounter = 0;
    private Rigidbody rb;
    private Collider col;
    private Vector3 movementDirection;
    public float targetAngle;
    //public float movementSpeed;
    private Vector3 res;
    private float stunned;
    private float isCollide = 0f;
    private float growTime = 0.8f;
    GameObject foodTarget;
    public bool isTarget;
    /********************/
    //for Job System
    private NativeArray<float3> bodyPartsNative;
    private NativeArray<float3> positionHistoryNative;
    private NativeArray<Quaternion> rotationHistoryNative;
    private NativeArray<Quaternion> bodyRotationNative;
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
    private HeadController headController;
    private HeadController HeadController
    {
        get
        {
            if (headController == null)
            {
                headController = GameObject.Find("PlayerHead").gameObject.GetComponent<HeadController>();
            }
            return headController;
        }
        set
        {
            headController = value;
        }
    }

    private int gap = 3;
    private float gapf = 3f;
    public int level;
    private void Awake()
    {
        kingState.SetActive(false);
    }
    private void Start()
    {
        positionHistory = new List<Vector3>();
        rotationHistory = new List<Quaternion>();
        /****************/
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        stunned = 0;
        movementDirection = new Vector3(0, 0, 1);
        targetAngle = 0;
        /****************/
        randSkin = UnityEngine.Random.Range(0, data.skins.Count);
        int skinPath = data.skins[randSkin].dataSprite.Count - 2;
        skinPath_ = skinPath;
        moveSpeed = 5f;
        //movementSpeed = 8f;

        name = enemyName;
        fullFoods = GameObject.Find("FullFoods");
        bodyParts = new List<GameObject>();
        GameObject body = Instantiate(bodyPrefabs);
        body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[data.skins[randSkin].dataSprite.Count - 1];
        body.transform.position = firstBody.transform.position - new Vector3(0, firstBody.transform.position.y-0.05f, 0);
        bodyParts.Insert(0, body);
        body.transform.SetParent(fullBody.transform);
        //
        levelText.text = enemyName + ": " + level.ToString();
        for (int i = 0; i < 20 + level; i++)
        {
            if (i % 5 == 0)
            {
                StartGrowSnake();
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
        UpdateIsColide();
        FPSCount();
    }
    private void FPSCount()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }
    private void FixedUpdate()
    {
        //Debug.Log(fps);
        if (CheckViewPos(gameObject.transform.position) || CheckViewPos(bodyParts[bodyParts.Count - 1].transform.position) 
            || CheckViewPos(bodyParts[bodyParts.Count/2].transform.position) || gameObject.transform.parent == RankingController.enemiesRank[0])
        {
            ChasePlayer(HeadController.transform.position);
            RunAwayPlayer(HeadController.transform.position);
            if ((!isChasing || !canChase) && (!isRunningAway || !canRunAway))
            {
                if (chaseTime < 4)
                {
                    chaseTime += Time.deltaTime;
                }
                else
                {
                    chaseTime = 4;
                    canChase = true;
                }

                if (runningAwayTime < 4)
                {
                    runningAwayTime += Time.deltaTime;
                }
                else
                {
                    runningAwayTime = 4;
                    canRunAway = true;
                }

                FindFood();
                MoveBodyUseJob();
                if (col.enabled == false)
                {
                    col.enabled = true;
                    for (int i = 0; i < bodyParts.Count - 1; i++)
                    {
                        bodyParts[i].SetActive(true);
                    }
                }
            }
        }
        else
        {
            col.enabled = false;
            for (int i = 0;i < bodyParts.Count - 1; i++)
            {
                bodyParts[i].SetActive(false);
            }
        }
    }

    private void UpdateIsColide()
    {
        if (isCollide > 0)
        {
            isCollide -= Time.deltaTime;
        }
        else isCollide = 0;

        if(growTime > 0)
        {
            growTime -= Time.deltaTime;
        }
        else
        {
            int temp = UnityEngine.Random.Range(1, 3);
            level+= temp;
            growTime = 5f;
        }
    }

    private void ChasePlayer(Vector3 pos)
    {
        if(pos!= null && CheckViewPos(gameObject.transform.position, -0.1f) && level > HeadController.level && canChase)
        {
            isChasing = true;
            chaseTime -= Time.deltaTime;
            if(chaseTime < 0)
            {
                canChase = false;
            }
            Debug.Log(gameObject + " dang duoi theo trong " + chaseTime +" giay nua");
            MoveBodyUseJob();
            res = HeadController.gameObject.transform.position - gameObject.transform.position;
            targetAngle = Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg;
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
            rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 1f);
        }
        else
        {
            isChasing = false;
        }
    }

    private void RunAwayPlayer(Vector3 pos)
    {
        if (pos != null && CheckViewPos(gameObject.transform.position, -0.1f) && level < HeadController.level && canRunAway)
        {
            isRunningAway = true;
            runningAwayTime -= Time.deltaTime;
            if (runningAwayTime < 0)
            {
                canRunAway = false;
            }
            Debug.Log(gameObject + " dang chay trong " + chaseTime + " giay nua");
            MoveBodyUseJob();
            res = -HeadController.gameObject.transform.position + gameObject.transform.position;
            targetAngle = Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg;
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
            rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 1f);
        }
        else
        {
            isRunningAway = false;
        }
    }
    private bool CheckViewPos(Vector3 pos, float buffer = 0.3f)
    {
        if (pos != null)
        {
            Vector3 viewportPoint = ItemSpawn.mainCamera.WorldToViewportPoint(pos);
            if (viewportPoint.x > -buffer && viewportPoint.x < 1 + buffer && viewportPoint.y > -buffer && viewportPoint.y < 1 + buffer && viewportPoint.z > 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    //ham di chuyen body
    private Vector3 FreezeYPos(Vector3 pos, float y)
    {
        Vector3 newPos = new Vector3(pos.x, y, pos.z);
        return newPos;
    }
    private void StartGrowSnake()
    {
        GameObject body = Instantiate(bodyPrefabs);
        body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[IndexCounter(skinPath_, skinCounter++)];
        if (bodyParts.Count() > 1)
        {
            body.transform.localScale = bodyParts[0].transform.localScale;
            body.transform.position = bodyParts[0].transform.position + new Vector3(0, 0.01f, 0);
            bodyParts.Insert(0, body);
        }
        else
        {
            body.transform.localScale = transform.localScale;
            body.transform.position = transform.position - new Vector3(0, transform.position.y - 0.06f, 0);
            bodyParts.Insert(0, body);
        }
        body.transform.SetParent(fullBody.transform);
        gapf += 0.025f;
        gap = (int)gapf;
    }
    private void GrowSnake()
    {
        //Gioi han so body
        if (bodyParts.Count < 60 && fps >= 35f)
        {
            GameObject body = Instantiate(bodyPrefabs);
            body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[IndexCounter(skinPath_, skinCounter++)];
            if (bodyParts.Count() > 1)
            {
                body.transform.localScale = bodyParts[0].transform.localScale;
                body.transform.position = bodyParts[0].transform.position + new Vector3(0, 0.01f, 0);
                bodyParts.Insert(0, body);
            }
            else
            {
                body.transform.localScale = transform.localScale;
                body.transform.position = transform.position - new Vector3(0, transform.position.y -0.06f, 0);
                bodyParts.Insert(0, body);
            }
            body.transform.SetParent(fullBody.transform);
            gapf += 0.025f;
            gap = (int)gapf;
            if (moveSpeed < 16f)
            {
                IncreseSpeed();
            }
        }
    }
    private void GrowSnakeUsePool()
    {
        GameObject body = BodyPool.instance.GetPooledObject();
        if (body != null)
        {
            body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[randSkin].dataSprite[IndexCounter(skinPath_, skinCounter++)];
            if (bodyParts.Count() > 1)
            {
                body.transform.localScale = bodyParts[0].transform.localScale;
                body.transform.position = bodyParts[0].transform.position + new Vector3(0, 0.01f, 0);
                bodyParts.Insert(0, body);
            }
            else
            {
                body.transform.localScale = transform.localScale;
                body.transform.position = transform.position - new Vector3(0, transform.position.y - 0.06f, 0);
                bodyParts.Insert(0, body);
            }
            body.transform.SetParent(fullBody.transform);
            gapf += 0.025f;
            gap = (int)gapf;
            body.SetActive(true);
        }
    }

    public void Eat()
    {
        level += 1;
        if (level > ItemSpawn.kingLevel)
        {
            ItemSpawn.kingLevel = level;
            ItemSpawn.SetKing2(gameObject);
        }
        levelText.text = enemyName + ": " + level.ToString();
        if (level % 7 == 0)
        {
            GrowSnake();
        }
        if(level % 7 == 0)
        {
            SizeGrow();
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("PlayerBody"))
    //    {
    //        stunned = 0;

    //        BodyController bodyCtr = other.GetComponent<BodyController>();
    //        HeadController headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
    //        if (level > bodyCtr.GetLevel())
    //        {
    //            bodyCtr.Bit();
    //            MoveAfterBite(headController.bodyFoods);
    //        }
    //        else
    //        {
    //            if (true)
    //            {
    //                Stun();
    //            }
    //        }
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Eat();
        }
        if (other.CompareTag("PlayerBody"))
        {
            stunned = 0;

            BodyController bodyCtr = other.GetComponent<BodyController>();
            HeadController headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
            if (level > bodyCtr.GetLevel())
            {
                bodyCtr.Bit();
                MoveAfterBite(headController.bodyFoods);
            }
            else
            {
                if (isCollide == 0)
                {
                    Stun();
                }
            }
        }
        if (other.CompareTag("Body") && (other.transform.parent.parent != gameObject.transform.parent))
        {
            stunned = 0;
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
            stunned = 0;
            HeadController head = other.gameObject.transform.parent.GetComponent<HeadController>();
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
            stunned = 0;
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
            stunned = 0;
            WallStun();
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
        if (level < 800)
        {
            transform.localScale += new Vector3(0.015f, 0.015f, 0.015f);
            levelText.transform.localScale = Vector3.one;
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
                        FoodController food = Instantiate(foodPrefabs, RandomPos(bodyParts[j]), Quaternion.identity);
                        foodPrefabs.SetSprite(data.foodSprites[randSkin]);
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
                FoodController food = Instantiate(foodPrefabs, RandomPos(bodyParts[i]), Quaternion.identity);
                foodPrefabs.SetSprite(data.foodSprites[randSkin]);
                food.transform.localScale = RandomScale(bodyParts[i]);
                food.transform.SetParent(fullFoods.transform);
            }
            bodyFoods.Insert(0, bodyParts[i].transform.position);
            Destroy(bodyParts[i]);
            bodyParts.RemoveAt(i);
        }
        for (int k = 0; k < 3; k++)
        {
            FoodController food = Instantiate(foodPrefabs, RandomPos(gameObject), Quaternion.identity);
            foodPrefabs.SetSprite(data.foodSprites[randSkin]);
            food.transform.localScale = RandomScale(gameObject);
            food.transform.SetParent(fullFoods.transform);
        }
        ItemSpawn.enemies.Remove(gameObject.transform.parent.gameObject);
        if (gameObject == ItemSpawn.king)
        {
            ItemSpawn.KingDie(gameObject);
        }

        RankingController.enemiesRank.Remove(gameObject.transform.parent.gameObject);
        if (fps >= 35)
        {
            ItemSpawn.SpawnEnemy(level / 2);
        }
        bodyFoods.Insert(0, gameObject.transform.position);
    }
    private Vector3 RandomPos(GameObject body)
    {
        Vector3 pos = new Vector3(body.transform.position.x + UnityEngine.Random.Range(-50, 50) / 100f, body.transform.position.y,
            body.transform.position.z + UnityEngine.Random.Range(-50, 50) / 100f);
        return pos;
    }
    private Vector3 RandomScale(GameObject body)
    {
        float rd = UnityEngine.Random.Range(500, 1000) / 1000f;
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
            else if (stunned < 0)
            {
                stunned = 0;
            }
        }
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
        //rb.velocity = movementDirection * movementSpeed * Time.deltaTime;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 1f);

    }
    public void Stun()
    {

        targetAngle += 180f;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
        isCollide = 0.1f;
        stunned = 0.5f;
        foodTarget = null;
    }
    public void WallStun()
    {
        targetAngle = Mathf.Atan2(rb.transform.position.x, rb.transform.position.z) * Mathf.Rad2Deg + 180f;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 180f);
        stunned = 0.5f;
        foodTarget = null;
    }
    public void Busy()
    {
        stunned = UnityEngine.Random.Range(1f, 5f);
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
                    int rand_ = UnityEngine.Random.Range(0, SpawnFood.instance.foods.Count);
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
        moveSpeed += 0.084f;
    }
    public void SetAngle(float value)
    {
        targetAngle = value;
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(0, targetAngle, 0), 120f);
    }
    public void SpeedUp(float add)
    {
        moveSpeed += add;
    }
    public void SpeedDown(float add)
    {
        moveSpeed -= add;
    }
    private void MoveBody()
    {
        positionHistory.Insert(0, transform.position);
        if (positionHistory.Count > bodyParts.Count * 6 + 20)
        {
            positionHistory.Remove(positionHistory[positionHistory.Count - 1]);
        }
        int i = 0;
        for (i = 0; i < bodyParts.Count; i++)
        {
            var body = bodyParts[i];
            Vector3 point = positionHistory[Mathf.Clamp(i * gap, 0, positionHistory.Count() - 1)];
            Vector3 moveDirection = FreezeYPos(point, body.transform.position.y) - body.transform.position;
            body.transform.LookAt(FreezeYPos(point, body.transform.position.y));
            body.transform.Translate(moveDirection.normalized * Time.fixedDeltaTime * moveSpeed, Space.World);
        }
    }

    [BurstCompile]
    private void MoveBodyUseJob()
    {
        positionHistory.Insert(0, transform.position);
        rotationHistory.Insert(0, transform.rotation);
        if (positionHistory.Count > bodyParts.Count * 6 + 20)
        {
            positionHistory.Remove(positionHistory[positionHistory.Count - 1]);
            rotationHistory.Remove(rotationHistory[rotationHistory.Count - 1]);
        }

        positionHistoryNative = new NativeArray<float3>(600, Allocator.TempJob);
        rotationHistoryNative = new NativeArray<Quaternion>(600, Allocator.TempJob);
        bodyPartsNative = new NativeArray<float3>(100, Allocator.TempJob);
        bodyRotationNative = new NativeArray<Quaternion>(100, Allocator.TempJob);
        for (int i = 0; i < positionHistory.Count; i++)
        {
            positionHistoryNative[i] = positionHistory[i];
            rotationHistoryNative[i] = rotationHistory[i];
        }
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyPartsNative[i] = bodyParts[i].transform.position;
            bodyRotationNative[i] = bodyParts[i].transform.rotation;
        }
        MoveBodyJob moveBodyJob = new MoveBodyJob()
        {
            bodyParts = bodyPartsNative,
            bodyRotation = bodyRotationNative,
            positionHistory = positionHistoryNative,
            rotationHistory = rotationHistoryNative,
            gap = gap,
            fixedDeltaTime = Time.fixedDeltaTime,
            moveSpeed = moveSpeed
        };

        JobHandle jobHandle = moveBodyJob.Schedule(bodyParts.Count, 1);
        jobHandle.Complete();

        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.position = bodyPartsNative[i];
            bodyParts[i].transform.rotation = bodyRotationNative[i];
        }

        positionHistoryNative.Dispose();
        rotationHistoryNative.Dispose();
        bodyRotationNative.Dispose();
        bodyPartsNative.Dispose();
    }

    public struct MoveBodyJob : IJobParallelFor
    {
        public NativeArray<Quaternion> bodyRotation;
        public NativeArray<float3> bodyParts;
        [NativeDisableParallelForRestriction] public NativeArray<float3> positionHistory;
        [NativeDisableParallelForRestriction] public NativeArray<Quaternion> rotationHistory;

        public int gap;
        public float moveSpeed;
        public float fixedDeltaTime;
        public float3 FreezeYPos(float3 a, float _y)
        {
            a.y = _y;
            return a;
        }
        public float3 Normalize(float3 vector)
        {
            float length = math.length(vector);
            if (length > float.Epsilon)
            {
                return vector / length;
            }
            return float3.zero;
        }
        public void Execute(int index)
        {
            float3 point = positionHistory[Mathf.Clamp(index * gap, 0, positionHistory.Count() - 1)];
            float3 moveDirection = FreezeYPos(point, bodyParts[index].y) - bodyParts[index];
            bodyRotation[index] = Quaternion.LookRotation(FreezeYPos(point, bodyParts[index].y) - bodyParts[index]);
            bodyParts[index] += Normalize(moveDirection) * fixedDeltaTime * moveSpeed;
        }
    }
}
