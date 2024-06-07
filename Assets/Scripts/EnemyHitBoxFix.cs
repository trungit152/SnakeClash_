using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBoxFix : MonoBehaviour
{
    private EnemyCollide enemyCollide;
    private EnemyCollide EnemyCollide
    {
        get
        {
            if (enemyCollide == null)
            {
                enemyCollide = gameObject.transform.parent.GetComponent<EnemyCollide>();
            }
            return enemyCollide;
        }
        set
        {
            enabled = value;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            EnemyCollide.level -= 1;
        }
    }
}
