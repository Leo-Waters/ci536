using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//leo waters 16/03/2023
//Guard AI 
//Attacks Nearby AI

[RequireComponent(typeof(GuardUpgrades))]
public class GuardAI : MonoBehaviour
{
    public GuardUpgrades upgradesSystem;
    public TargetSensor targetSensor;
    public Animator anim;

    bool coolingDown=false;

    public GameObject RangeDisplayobject;
    public void RangeDisplay(bool show)//shows the green range display sprite
    {
        if (show)
        {
            RangeDisplayobject.transform.localScale = new Vector3(upgradesSystem.current.Range*2, upgradesSystem.current.Range*2, 1);
        }
        RangeDisplayobject.SetActive(show);
        
    }

    void Awake()
    {
        upgradesSystem = GetComponent<GuardUpgrades>();
        int rand = Random.Range(0, 2);

        anim.SetBool("Change", rand == 1); // set skin to random 
    }

    public void Update()
    {
        if (!coolingDown && targetSensor.HasTarget())// attacks nearest target if not in cooldown
        {
            GameObject target = targetSensor.GetTarget();
            Vector3 direction = transform.position - target.transform.position;

            //transform.LookAt(target.transform);

            //set animations dir
            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);

            targetSensor.SetRange(upgradesSystem.current.Range);//set the range of the target sensor
            coolingDown = true;
            StartCoroutine(Fire(target));
        }
    }


    IEnumerator Fire(GameObject Target)
    {
        AINavigation ai = Target.GetComponent<AINavigation>();
        float Traveltime = BulletPoolingSystem.Instance.Fire(transform.position, Target, upgradesSystem.current.BulletPrefabName);//uses bullet pool to shoot a bullet and returns travel time

        StartCoroutine(WaitForBulletImpact(Target, Traveltime));

        yield return new WaitForSeconds(upgradesSystem.current.CoolDown);//waits for cooldown time before alowing the guard to fire again
        coolingDown = false;
    }

    IEnumerator WaitForBulletImpact(GameObject Target,float BulletTravelTime)
    {
        yield return new WaitForSeconds(BulletTravelTime);//waits for bullet travel before damaging and potentialy killing the AI so that the bullet alwasy hits first before the ai disapears
        if (Target.activeInHierarchy)
        {
            AINavigation ai = Target.GetComponent<AINavigation>();
            ai.Health -= upgradesSystem.current.Damage;//damage the AI
            if (ai.Health <= 0)
            {
                targetSensor.RemoveTarget(Target);//remove dead AI from the Targets
            }
            if (upgradesSystem.current.StunTime != 0)
            {
                ai.Stun(upgradesSystem.current.StunTime);//stun the AI
            }
            
        }
        else
        {
            targetSensor.RemoveTarget(Target);//remove dead AI from the Targets
        }

    }
}
