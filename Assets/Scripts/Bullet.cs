using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 2;
    public float disableTime = 5;

    public IObjectPool<GameObject> pool;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    public void BulletSet(Vector3 firePosition, Vector3 targetPosition)
    {
        transform.position = firePosition;
        transform.LookAt(targetPosition);
    }    

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableTime);
        pool.Release(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().hp -= PlayerController.Damage;
        }

        pool.Release(this.gameObject);
    }
}
