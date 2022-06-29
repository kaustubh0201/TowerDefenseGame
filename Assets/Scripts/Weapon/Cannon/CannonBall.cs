using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private GameObject m_target;

    [SerializeField] private CannonBallSO cannonBallStats;

    private IEnumerator cannonBallMovement(){

        Vector3 destPos = m_target.transform.position; 

        while(Vector3.Distance(transform.position, destPos) > cannonBallStats.m_radius){
            transform.position = Vector3.MoveTowards(transform.position, destPos, cannonBallStats.m_speed);

            yield return null;


            try {
                destPos = m_target.transform.position;
            }catch(MissingReferenceException){}
        }

        Destroy(this.gameObject, 0.2f);
    }

    void OnDestroy(){
        if(m_target != null)
            m_target.GetComponent<EnemyAI>().deathEvent -= destroyBall;
    }

    private void destroyBall(GameObject g){
        Destroy(this.gameObject, 0.2f);
    }

    public void moveCannonBall(GameObject target){
        m_target = target;

        m_target.GetComponent<EnemyAI>().deathEvent += destroyBall;

        StartCoroutine(cannonBallMovement());
    }
}
