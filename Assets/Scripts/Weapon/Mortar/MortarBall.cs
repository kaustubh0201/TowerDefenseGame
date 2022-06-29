using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBall : MonoBehaviour
{
    
    private GameObject m_target;
    //private float t = 0f;

    [SerializeField] private MortarBallSO m_mortarBallStats;
    [SerializeField] private MortarBallRange m_mortarBallRange;
    [SerializeField] private GameObject pathStart;
    [SerializeField] private GameObject pathTop;
    [SerializeField] private GameObject pathEnd;

    private HashSet<GameObject> m_potTargets;
    private uint m_mortarDamage;
    private Vector3 m_fixedLastEnemyPosition;

    private float distanceEnemy;
    private Vector3 directionEnemy;

    void Start(){
        m_mortarBallRange.GetComponent<SphereCollider>().radius = m_mortarBallStats.m_damageRadius;

        // initializing the positions of the path
        pathStart.transform.position = transform.position;

        // set the speed of the mortar ball
        GetComponent<ParabolaController>().setSpeed(m_mortarBallStats.m_speed);
    }
    
    private IEnumerator mortarBallMovement(){
        
        // calculation for the height of parabola
        distanceEnemy = Vector3.Distance(m_fixedLastEnemyPosition, transform.position);
        directionEnemy = (m_fixedLastEnemyPosition - transform.position).normalized;

        // setting height of the parabola
        pathTop.transform.position = (transform.position + (directionEnemy * distanceEnemy / 2f)) + Vector3.up * distanceEnemy / 2f;
        pathEnd.transform.position = m_fixedLastEnemyPosition;
        
        //Debug.Log("enemy " + m_fixedLastEnemyPosition);

        
        while(Vector3.Distance(m_fixedLastEnemyPosition, transform.position) > 1) {

            GetComponent<ParabolaController>().enabled = true;

            //Debug.Log("object" + transform.position);

            yield return null;
        }

        //Debug.Log("object" + transform.position);

        GetComponent<ParabolaController>().enabled = false;

        //Debug.Log("parabola controller disabled");

        m_potTargets = m_mortarBallRange.getPotTargets();

        foreach (GameObject gb in m_potTargets){
            if(m_mortarBallRange.isTargetInRange(gb))
                gb.GetComponent<EnemyAI>().reduceHealth(m_mortarDamage);
                //Debug.Log("Damage done!");
        }

        Destroy(this.gameObject);
    }

    
    void OnDestroy(){
        // if(m_target != null)
        //     m_target.GetComponent<EnemyAI>().deathEvent -= destroyBall;
    }

    // private void destroyBall(GameObject g) {
    //     Destroy(this.gameObject, 0.2f);
    // }

    public void moveMortarBall(GameObject target, uint mortarDamage){
        m_target = target;
        m_mortarDamage = mortarDamage;
        m_fixedLastEnemyPosition = target.transform.position;

        //m_target.GetComponent<EnemyAI>().deathEvent += destroyBall;

        StartCoroutine(mortarBallMovement());
    }
}
