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

    public static HeadController instance;
    public List<Vector3> bodyFoods;
    
    private List<GameObject> bodyParts;
    private List<Vector3> positionHistory;
    private int gap = 10;
    private float gapf = 10f;
    private float speedUpAdd = 10f;
    private float itemTime;
    private bool isSpeedUp;
    public int level;
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
    private void Awake()
    {
        isSpeedUp = false;
        instance = this;
        Time.fixedDeltaTime = 0.008f;
    }
    private void Start()
    {
        level = 5;
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
        MovementController.movementSpeed = data.startSpeed;
        level = data.startLevel;
        itemTime = data.itemTime;
        levelText.text = "Level " + level.ToString();
        for (int i = 0; i < level / 5; i++)
        {
            GrowSnake();
            SizeGrow();
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
                body.transform.localScale = bodyParts[0].transform.localScale;
                body.transform.position = bodyParts[0].transform.position;
            }
            else
            {
                body.transform.position = firstBody.transform.position;
            }
            bodyParts.Insert(0, body);
            body.transform.SetParent(fullBody.transform);
            gapf += 0.04f;
            gap = (int)gapf;
        }
        moveSpeed += 0.05f;
        MovementController.IncreseSpeed(); 
        Time.fixedDeltaTime = 0.008f + (float)bodyParts.Count() * 0.01f / 120f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            level++;
            if (level == 50 || level == 200 || level == 450 || level == 600)
            {
                CameraController.CameraUp();
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
            StartCoroutine(Wait());
        }
    } 
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(itemTime);
        gap += (int)speedUpAdd / 3;
        gapf += speedUpAdd / 3;
        moveSpeed -= speedUpAdd;
        MovementController.SpeedDown(speedUpAdd);
        isSpeedUp = false;
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
