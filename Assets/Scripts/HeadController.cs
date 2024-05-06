using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class HeadController : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefabs;
    [SerializeField] private float moveSpeed;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject fullBody;
    [SerializeField] private GameObject startBody;
    [SerializeField] private GameObject firstBody;
    [SerializeField] GameObject foodPrefabs;
    [SerializeField] private DataSO data;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI top1Text;
    [SerializeField] private TextMeshProUGUI top2Text;
    [SerializeField] private TextMeshProUGUI top3Text;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject inGameRankingPanel;

    public static HeadController instance;
    public List<Vector3> bodyFoods;
    
    private List<GameObject> bodyParts;
    private List<Vector3> positionHistory;
    private int gap = 10;
    private float gapf = 10f;
    private float speedUpAdd = 5f;
    private float itemTime;
    private bool isSpeedUp;
    private bool isMagnite;
    public int level = 5;
    private float levelf = 5;
    private MovementController movementController;
    private MovementController MovementController
    {
        get
        {
            if (movementController == null)
            {
                movementController = GameObject.Find("MovementController").GetComponent<MovementController>();
            }
            return movementController;
        }
        set
        {
            movementController = value;
        }
    }
    private CameraController cameraController;
    private CameraController CameraController
    {
        get
        {
            if (cameraController == null)
            {
                cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }
            return cameraController;
        }
        set
        {
            cameraController = value;
        }
    }
    MinimapController minimapController;
    MinimapController MinimapController
    {
        get
        {
            if (minimapController == null)
            {
                minimapController = GameObject.Find("MinimapCamera").GetComponent<MinimapController>();
            }
            return minimapController;
        }
        set
        {
            minimapController = value;
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
    private void Awake()
    {
        isSpeedUp = false;
        isMagnite = false;
        instance = this;
        Time.fixedDeltaTime = 0.008f;
    }
    private void Start()
    {
        inGameRankingPanel.SetActive(false);
        level = data.startLevel;
        Debug.Log("Head " + level);
        bodyParts = new List<GameObject>();
        positionHistory = new List<Vector3>();
        for (int i = 0; i < startBody.transform.childCount; i++)
        {
            bodyParts.Insert(0, startBody.transform.GetChild(i).gameObject);
        }
    }
    public void SetStat()
    {
        moveSpeed = data.startSpeed;
        inGameRankingPanel.SetActive(true);
        MovementController.movementSpeed = data.startSpeed;
        level = data.startLevel;
        itemTime = data.itemTime;
        levelText.text = "Level " + level.ToString();
        for (int i = 0; i < level / 5; i++)
        {
            GrowSnake();
            SizeGrow();
        }
        if(level >= 50 && level < 200)
        {
            CameraController.CameraUp();
        }
        else if (level >= 200 && level < 450)
        {
            CameraController.CameraUp();
            CameraController.CameraUp();
        }
        else if (level >= 450 && level < 600)
        {
            CameraController.CameraUp();
            CameraController.CameraUp();
            CameraController.CameraUp();
        }
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        MoveBody();
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
                Vector3 point = positionHistory[Mathf.Clamp(i * gap, 0, positionHistory.Count() - 1)];
                Vector3 moveDirection = point - body.transform.position;
                body.transform.LookAt(point);
                body.transform.Translate(moveDirection.normalized * Time.deltaTime * moveSpeed, Space.World);
                i++;
            }
        }
    }
    private void GrowSnake()
    {
        if(bodyParts.Count < 60) 
        {
            GameObject body = Instantiate(bodyPrefabs);
            if (bodyParts.Count() != 0)
            {
                body.transform.localScale = bodyParts[bodyParts.Count-1].transform.localScale;
                body.transform.position = bodyParts[bodyParts.Count - 1].transform.position;
                bodyParts.Insert(bodyParts.Count - 1, body);
            }
            else
            {
                body.transform.position = firstBody.transform.position;
                bodyParts.Insert(0, body);
            }
            body.transform.SetParent(fullBody.transform);
            gapf += 0.05f;
            gap = (int)gapf;
        }
        if(moveSpeed < 25f)
        {
            moveSpeed += 0.04f;
            MovementController.IncreseSpeed();
        }
        Time.fixedDeltaTime = 0.008f + (float)bodyParts.Count() * 0.01f / 120f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            levelf += 0.5f;
            level = (int)levelf;
            if (level == 50 || level == 200 || level == 450 || level == 600)
            {
                CameraController.CameraUp();
                MinimapController.MiniCameraUp();
            }
            levelText.text = "Level " + level.ToString();
            if (level % 5 == 0)
            {
                GrowSnake();
                SizeGrow();
            }
        }
        if (other.CompareTag("Body"))
        {
            EnemyBodyController bodyCtr = other.GetComponent<EnemyBodyController>();
            if (level >= bodyCtr.GetLevel())
            {
                bodyCtr.Bit();
            }
            else
            {
                MovementController.Stun();
            }
        }
        if (other.CompareTag("SpeedUp"))
        {
            SpeedUp();
        }
        if (other.CompareTag("Magnite"))
        {
            Magnite();
        }
    }
    private void SpeedUp()
    {
        if(!isSpeedUp)
        {
            moveSpeed += speedUpAdd;
            MovementController.SpeedUp(speedUpAdd);
            gap -= (int)speedUpAdd / 3;
            gapf -= speedUpAdd / 3;
            isSpeedUp = true;
            StartCoroutine(Wait3s());
        }
    } 
    IEnumerator Wait3s()
    {
        yield return new WaitForSeconds(itemTime);
        gap += (int)speedUpAdd / 3;
        gapf += speedUpAdd / 3;
        moveSpeed -= speedUpAdd;
        MovementController.SpeedDown(speedUpAdd);
        isSpeedUp = false;
    }
    private void Magnite()
    {
        if (!isMagnite)
        {
            CapsuleCollider hitBoxCol = GameObject.Find("HitBox").GetComponent<CapsuleCollider>();
            hitBoxCol.radius *= 2;
            StartCoroutine(Wait2s());
        }
    }
    IEnumerator Wait2s()
    {
        yield return new WaitForSeconds(1f);
        CapsuleCollider hitBoxCol = GameObject.Find("HitBox").GetComponent<CapsuleCollider>();
        hitBoxCol.radius /= 2;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            MovementController.instance.Collide();
        }
        if (collision.gameObject.CompareTag("Head"))
        {
            EnemyCollide enemyHead = collision.gameObject.GetComponent<EnemyCollide>();
            if (level >= enemyHead.level)
            {
                enemyHead.Die();
            }
        }
    }
    private void SizeGrow()
    {
        if(level < 600)
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
        for (int i = 0;i < bodyParts.Count(); i++)
        {
            BodyController bdCtr = bodyParts[i].GetComponent<BodyController>();
            if (bdCtr.isBit)
            {
                for (int j = bodyParts.Count - 1; j >= i; j--)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        GameObject food = Instantiate(foodPrefabs, RandomPos(bodyParts[j]), Quaternion.identity);
                        food.transform.localScale = RandomScale(bodyParts[j]);
                    }
                    bodyFoods.Insert(0, bodyParts[j].transform.position);
                    Destroy(bodyParts[j]);
                    bodyParts.RemoveAt(j);
                }
                i = bodyParts.Count();
            }
        }
    }

    public void TurnOffLevelText()
    {
        levelText.text = null;
    }
    public void Die()
    {
        for (int i = bodyParts.Count() - 1; i >= 0; i--)
        {
            Destroy(bodyParts[i]);
            bodyParts.RemoveAt(i);
        }
        data.coin += level;
        Destroy(gameObject);
        losePanel.SetActive(true);
        MinimapController.HideMinimap();
        top1Text.text = RankingController.top1Text.text;
        top2Text.text = RankingController.top2Text.text;
        top3Text.text = RankingController.top3Text.text;
        playerRankText.text = RankingController.playerRankText.text;
        scoreText.text = "Your Score: " + level;
        rankingPanel.SetActive(true);
        RankingController.TurnOffText();
        levelText.text = null;
        Time.timeScale = 0f;
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
