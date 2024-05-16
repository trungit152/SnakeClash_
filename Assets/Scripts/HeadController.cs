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
    [SerializeField] private TextMeshProUGUI top1Name;
    [SerializeField] private TextMeshProUGUI top1Score;
    [SerializeField] private TextMeshProUGUI top2Name;
    [SerializeField] private TextMeshProUGUI top2Score;
    [SerializeField] private TextMeshProUGUI top3Name;
    [SerializeField] private TextMeshProUGUI top3Score;
    //[SerializeField] private TextMeshProUGUI top4Name;
    //[SerializeField] private TextMeshProUGUI top4Score;
    //[SerializeField] private TextMeshProUGUI top5Name;
    //[SerializeField] private TextMeshProUGUI top5Score;
    [SerializeField] private TextMeshProUGUI playerRank;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject inGameRankingPanel;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject arrow;

    public static HeadController instance;
    public List<Vector3> bodyFoods;

    private int skinCounter = 0;
    private int skinPath_;
    private float gapf = 8f;
    private float speedUpAdd = 5f;
    private float itemTime;
    private bool isSpeedUp;
    private bool isMagnite;
    public int level = 5;
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
    }
    private void Start()
    {
        inGameRankingPanel.SetActive(false);
        level = data.startLevel;
        for (int i = 0; i < startBody.transform.childCount; i++)
        {
            MovementController.AddToBodyParts(startBody.transform.GetChild(i).gameObject);
        }
        int skinPath = data.skins[data.skinIndex].dataSprite.Count - 2;
        skinPath_ = skinPath;
        //gan skin cho ran
        SpriteRenderer headSkin = GameObject.Find("2DHead").GetComponent<SpriteRenderer>();
        headSkin.sprite = data.skins[data.skinIndex].dataSprite[0];
        SpriteRenderer tailSkin = GameObject.Find("2DTail").GetComponent<SpriteRenderer>();
        tailSkin.sprite = data.skins[data.skinIndex].dataSprite[data.skins[data.skinIndex].dataSprite.Count - 1];
        for (int i = 0; i < MovementController.bodyParts.Count - 1; i++)
        {
            MovementController.bodyParts[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[data.skinIndex].dataSprite[IndexCounter(skinPath_, skinCounter)];
            skinCounter++;
        }

    }
    private int IndexCounter(int nums, int i)
    {
        return (i % nums) + 1;
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
        if (level >= 50 && level < 200)
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
    public void GrowSnake()
    {
        if (MovementController.bodyParts.Count < 60)
        {
            GameObject body = Instantiate(bodyPrefabs);
            body.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = data.skins[data.skinIndex].dataSprite[IndexCounter(skinPath_, skinCounter++)];
            if (MovementController.bodyParts.Count() != 0)
            {
                body.transform.localScale = MovementController.bodyParts[MovementController.bodyParts.Count - 1].transform.localScale;
                body.transform.position = firstBody.transform.position;
                MovementController.bodyParts.Insert(0, body);
            }
            else
            {
                body.transform.position = firstBody.transform.position;
                MovementController.bodyParts.Insert(0, body);
            }
            body.transform.SetParent(fullBody.transform);
            gapf += 0.05f;
            MovementController.gap = (int)gapf;
        }
        if (moveSpeed < 22f)
        {
            moveSpeed += 0.035f;
            MovementController.IncreaseSpeed();
        }
    }
    public void LevelUp()
    {
        level++;
        if (level == 50 || level == 200 || level == 450 || level == 600)
        {
            MovementController.CameraUp();
            MinimapController.MiniCameraUp();
        }
        levelText.text = "Level " + level.ToString();
        if (level % 5 == 0)
        {
            GrowSnake();
            SizeGrow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            LevelUp();
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
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("va cham voi wall");
            MovementController.instance.Collide();
        }
        if (other.gameObject.CompareTag("Head"))
        {
            EnemyCollide enemyHead = other.gameObject.GetComponent<EnemyCollide>();
            if (level >= enemyHead.level)
            {
                enemyHead.Die();
            }
        }
    }
    private void SpeedUp()
    {
        if (!isSpeedUp)
        {
            moveSpeed += speedUpAdd;
            MovementController.SpeedUp(speedUpAdd);
            MovementController.gap -= (int)speedUpAdd / 3;
            gapf -= speedUpAdd / 3;
            isSpeedUp = true;
            StartCoroutine(Wait3s());
        }
    }
    IEnumerator Wait3s()
    {
        yield return new WaitForSeconds(itemTime);
        MovementController.gap += (int)speedUpAdd / 3;
        gapf += speedUpAdd / 3;
        moveSpeed -= speedUpAdd;
        MovementController.SpeedDown(speedUpAdd);
        isSpeedUp = false;
    }
    public void Magnite()
    {
        if (!isMagnite)
        {
            isMagnite = true;
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
        isMagnite = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("va cham voi wall");
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
        if (level < 600)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            for (int i = 0; i < MovementController.bodyParts.Count(); i++)
            {
                MovementController.bodyParts[i].transform.localScale = transform.localScale;
            }
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), 20f * Time.deltaTime);
        }
    }

    public void HeadBit()
    {
        bodyFoods = new List<Vector3>();
        for (int i = 0; i < MovementController.bodyParts.Count(); i++)
        {
            BodyController bdCtr = MovementController.bodyParts[i].GetComponent<BodyController>();
            if (bdCtr.isBit)
            {
                for (int j = MovementController.bodyParts.Count - 1; j >= i; j--)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        GameObject food = Instantiate(foodPrefabs, RandomPos(MovementController.bodyParts[j]), Quaternion.identity);
                        food.transform.localScale = RandomScale(MovementController.bodyParts[j]);
                    }
                    bodyFoods.Insert(0, MovementController.bodyParts[j].transform.position);
                    Destroy(MovementController.bodyParts[j]);
                    MovementController.bodyParts.RemoveAt(j);
                }
                i = MovementController.bodyParts.Count();
            }
        }
    }

    public void TurnOffLevelText()
    {
        levelText.text = null;
    }
    public void Die()
    {
        for (int i = MovementController.bodyParts.Count() - 1; i >= 0; i--)
        {
            Destroy(MovementController.bodyParts[i]);
            MovementController.bodyParts.RemoveAt(i);
        }
        data.coin += level;
        Destroy(GameObject.Find("MovementController").gameObject);
        Destroy(gameObject);
        MinimapController.HideMinimap();
        top1Name.text = RankingController.top1name.text;
        top1Score.text = RankingController.top1score.text;
        top2Name.text = RankingController.top2name.text;
        top2Score.text = RankingController.top2score.text;
        top3Name.text = RankingController.top3name.text;
        top3Score.text = RankingController.top3score.text;
        playerName.text = RankingController.playerName.text;
        playerRank.text = RankingController.playerRank.text;
        playerScore.text = RankingController.playerScore.text;
        scoreText.text = level.ToString();
        arrow.SetActive(false);
        rankingPanel.SetActive(true);
        inGameUI.SetActive(false);
        inGameRankingPanel.SetActive(false);
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