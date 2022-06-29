using UnityEngine;

[CreateAssetMenu(fileName ="EnemySO", menuName ="ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject  {
    [field: SerializeField] public float height {get; set;} = 1.0f;
    [field: SerializeField] public GameObject prefab {get; set;}
    [field: SerializeField] public float speed {get; set;} = 3.0f; 
    [field: SerializeField] public GameObject currencyPrefab {get; set;}
    
    [field: SerializeField] public int currencyAward {get; set;}
}
