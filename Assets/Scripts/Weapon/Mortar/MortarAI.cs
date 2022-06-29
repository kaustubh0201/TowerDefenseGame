using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarAI : MonoBehaviour
{
    private GameObject m_target;
    private IEnumerator m_fireCo;

    [SerializeField] private Transform m_mortarNozzle;
    [SerializeField] private MortarRange m_mortarRange;

    [SerializeField] private MortarSO mortarStats;
    [SerializeField] private GameObject mortarBall;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // setting the radius for the collider
        m_mortarRange.GetComponent<SphereCollider>().radius = mortarStats.m_rangeRadius;
        
        // initally no coroutine assigned
        m_fireCo = null;

        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator mortarFire(){
        
        EnemyAI eai = m_target.GetComponent<EnemyAI>();

        while(eai.Health > 0) {

            // start Coroutine for Cannonball
            audioSource.PlayOneShot(audioSource.clip, 0.8f);
            GameObject mBall = Instantiate(mortarBall, transform.position, Quaternion.identity);
            mBall.GetComponent<MortarBall>().moveMortarBall(m_target, mortarStats.m_damage);

            // wait for delay
            yield return new WaitForSeconds(mortarStats.m_fireDelay / 1000f);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target == null){
            // if no target
            m_target = m_mortarRange.getNextTarget();

            // if enemy dead and m_target is null
            // but m_fireCo is not null
            if(m_fireCo != null){
                StopCoroutine(m_fireCo);
                m_fireCo = null;
            }

            // if no target is available
            if(m_target == null){
                return;
            }
        }

        // if target moves out of range
        if(!m_mortarRange.isTargetInRange(m_target)) {
            m_target = null;
            // stop the coroutine
            if(m_fireCo != null){
                StopCoroutine(m_fireCo);
                m_fireCo = null;
            }

            return;
        }

        // if there is target then rotate towards target
        Vector3 direction = m_target.transform.position - m_mortarNozzle.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        m_mortarNozzle.rotation = Quaternion.Slerp(m_mortarNozzle.rotation, rotation, mortarStats.m_angVelocity * Time.deltaTime);

        // if mortar not firing at someone
        if(m_fireCo == null){
            m_fireCo = mortarFire();
            StartCoroutine(m_fireCo);
        }
    }
}
