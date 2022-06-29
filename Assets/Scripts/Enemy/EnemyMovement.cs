using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private EnemySO enemyData;
    // if deathState is 1 then it means the coins will be dropped
    // if the deathState is 0 then the coins will not be dropped
    private int deathState = 1;
    private AudioSource audioSource;
    
    private BezierPath path;
    public BezierPath Path {
        set {
            path = value;
        }
    }

    //void OnTriggerStay(Collider collider){
    //    if(collider.gameObject.tag == "Path"){
    //        Debug.Log(gameObject.name + " was triggered by " + collider.gameObject.name);

    //        // transform.position += Vector3.up * 0.2f;
    //    }
    //}

    void Start(){
        //audioSource = GetComponent<AudioSource>();
    }
 
    public void startFollow() {
        StartCoroutine(startFollowCour());
    } 

    private IEnumerator startFollowCour(){
        foreach (Vector3 pos in path.Positions){
            yield return move(enemyData.speed, new Vector3(pos.x, transform.position.y, pos.z));
        }
        
        setDeathState(0);
        FindObjectOfType<Manager>().DecrementLives();
        Destroy(this.gameObject, 0.2f);
    }

    public void setDeathState(int value){
        deathState = value;
    }

    public int getDeathState(){
        return deathState;
    }

    void OnDestroy(){

        if(deathState == 1){
            //audioSource.PlayOneShot(audioSource.clip, 0.3f);
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            GameObject g = Instantiate(enemyData.currencyPrefab, pos, Quaternion.identity);
            FindObjectOfType<Manager>().changeCurrency(enemyData.currencyAward);
        }        
    }


    private IEnumerator move(float speed, Vector3 dest){        
        while(transform.position != dest){
            dest.y = -1.8f;
            Vector3 direction = transform.position - dest;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis((angle+180f), Vector3.up);
            transform.rotation = rot;

            transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            yield return null;
        }
    }
}
