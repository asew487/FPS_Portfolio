using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    [Header("ObjectType")]
    [SerializeField] bool isGoldObject;
    [SerializeField] bool isPowerObject;

    [Header("Object")]
    [SerializeField] float hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            if (isGoldObject)
            {

            }

            if (isPowerObject)
            {

            }

            Destroy(gameObject);
        }
    }
}
