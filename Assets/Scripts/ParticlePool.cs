using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public enum TypeParticle
    {
        Heal,
        Disappear,
        Magnet,
        Click,
        Eat,
        Move,
        Unlock
    }

    public TypeParticle typeParticle;

    private ParticleSystem particle;
    // Start is called before the first frame update

    public void OnParticleSystemStopped()
    {
        if (typeParticle == TypeParticle.Heal)
        {
            HeadController.instance.healPool.Release(particle);
            //LogicGame.Instance.clickParticlePool.Release(particle);
            //GameManager.Instance.donePlateParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Disappear)
        {
            testCollider.instance.disappearFoodPool.Release(particle);
            //LogicGame.Instance.eatParticlePool.Release(particle);
            //GameManager.Instance.putDownParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Magnet)
        {
            HeadController.instance.magnetPool.Release(particle);
            //GameManager.Instance.comboParticlePool.Release(particle);
        }
        else if (typeParticle == TypeParticle.Unlock)
        {
            //GameManager.Instance.comboParticlePool.Release(particle);
        }
    }
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
