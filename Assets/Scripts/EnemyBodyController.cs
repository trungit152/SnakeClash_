using UnityEngine;

public class EnemyBodyController : MonoBehaviour
{
    public bool isBit;
    public EnemyCollide enemyCollide;
    public EnemyCollide EnemyCollide
    {
        get
        {
            if (enemyCollide == null)
            {
                enemyCollide = transform.parent.parent.GetChild(0).GetComponent<EnemyCollide>();
            }
            return enemyCollide;
        }
        set
        {
            enemyCollide = value;
        }
    }
    void Start()
    {
        isBit = false;
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public int GetLevel()
    {      
        return EnemyCollide.level;
    }
    public void Bit()
    {
        isBit = true;
        EnemyCollide.HeadBit(); 
    }
}
