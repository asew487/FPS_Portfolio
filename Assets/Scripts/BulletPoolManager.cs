using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance;

    [Header("BulletPool")]
    public Queue<GameObject> attackPoolMMM = new Queue<GameObject>();
    public IObjectPool<GameObject> attackPool;
    public GameObject bulletPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackPool = new ObjectPool<GameObject>(CreateBullet, OnTakeFromPool, ReturnBullet, OnDestroyPoolObjcet, true, 10, 15);

        for (int i = 0; i < 10; i++)
        {
            Bullet bullet = CreateBullet().GetComponent<Bullet>();
            bullet.pool.Release(bullet.gameObject);
        }
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<Bullet>().pool = attackPool;
        return bullet;
    }

    private void OnTakeFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    public static void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void OnDestroyPoolObjcet(GameObject bullet)
    {
        Destroy(bullet);
    }
}
