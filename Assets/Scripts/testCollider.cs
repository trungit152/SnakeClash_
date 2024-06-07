using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class testCollider : MonoBehaviour
{
    private HeadController headController;
    private HeadController HeadController
    {
        get
        {
            if (headController == null)
            {
                headController = gameObject.transform.parent.GetComponent<HeadController>();
            }
            return headController;
        }
        set
        {
            headController = value;
        }
    }

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            HeadController.LevelUp();
            RankingController.EnemyCheat(HeadController.level);
        }
        if (other.CompareTag("Body"))
        {
            EnemyBodyController bodyCtr = other.GetComponent<EnemyBodyController>();
            if (HeadController.level >= bodyCtr.GetLevel())
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
            HeadController.SpeedUp();
        }
        if (other.CompareTag("Magnite"))
        {
            HeadController.Magnite();
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            MovementController.instance.Collide();
        }
        if (other.gameObject.CompareTag("Head"))
        {
            EnemyCollide enemyHead = other.gameObject.GetComponent<EnemyCollide>();
            if (HeadController.level >= enemyHead.level)
            {
                enemyHead.Die();
            }
        }
    }
}
