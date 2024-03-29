using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    public float hp;
    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Die();
    }

    private void Die()
    {
        if(hp <= 0)
        {
            Debug.Log("Á×À½");
            Destroy(gameObject);
        }
    }
}
