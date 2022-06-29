using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour {
    
    [SerializeField] private float angularVelocity;

    private float lifespan = 2.0f;
    
    void Start(){
        StartCoroutine(spin());
    }

    public IEnumerator spin(){
        while(lifespan > 0){
            transform.Rotate(Vector3.up, 360 * angularVelocity * Time.deltaTime); 
            lifespan -= Time.deltaTime;
            yield return null;
        }


        Destroy(this.gameObject, 0.1f);
    } 
}
