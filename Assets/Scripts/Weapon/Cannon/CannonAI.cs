using System.Collections;
using UnityEngine;

public class CannonAI : MonoBehaviour {

    private GameObject m_target;
    private IEnumerator m_fireCo;

    [SerializeField] private Transform m_cannonNozzle;
    [SerializeField] private CannonRange m_cannonRange;
    [SerializeField] public CannonSO cannonStats;
    [SerializeField] private GameObject cannonBall;
    private AudioSource audioSource;



    void Start() {
        // set the range on the collider
        m_cannonRange.GetComponent<SphereCollider>().radius = cannonStats.m_rangeRadius;
        m_fireCo = null;
        audioSource = GetComponent<AudioSource>();
    }
    
    private IEnumerator cannonFire(){
        EnemyAI eai = m_target.GetComponent<EnemyAI>();

        while(eai.Health > 0){
            //Debug.Log("Fire! Fire! Fire! " + m_target.name);

            // start coroutine for cannonball
            audioSource.PlayOneShot(audioSource.clip, 0.1f);
            GameObject cBall = Instantiate(cannonBall, transform.position, Quaternion.identity);
            
            cBall.GetComponent<CannonBall>().moveCannonBall(m_target);

            // wait for delay
            yield return new WaitForSeconds(cannonStats.m_fireDelay / 1000f);

            //Debug.Log("before reduce health");
            // take away the health of m_target
            
            eai.reduceHealth(cannonStats.m_damage);
        }
    }



    // Update is called once per frame
    void Update() {
        // find target
        
        // if has no target
        if(m_target == null){
            // stop coroutine cannonFire
            m_target = m_cannonRange.getNextTarget();

            // if enemy dead and m_target = null
            // but m_fireCo != null
            if(m_fireCo != null){
                StopCoroutine(m_fireCo);
                m_fireCo = null;
            }

            // if no target is available
            if(m_target == null){               
                return;
            }
        }
        
        // if target moves out of the range
        if(!m_cannonRange.isTargetInRange(m_target)){
            m_target = null;
            // stop the coroutine
            if(m_fireCo != null){
                StopCoroutine(m_fireCo);
                m_fireCo = null;
            }
            return;
        }
    
        // if cannon has a target 
        /// direction to the target 
        Vector3 direction = m_target.transform.position - m_cannonNozzle.position;
        /// angle to rotate to face the target
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        /// rotate about axis up through the angle "angle" 
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        /// rotate
        m_cannonNozzle.rotation = Quaternion.Slerp(m_cannonNozzle.rotation, rotation, cannonStats.m_angVelocity * Time.deltaTime);
        
        // if cannon not firing at someone else then start firing on m_target
        if(m_fireCo == null){
            m_fireCo = cannonFire();
            StartCoroutine(m_fireCo);
        }
    }
}
