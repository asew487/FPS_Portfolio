using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void Attack(GameObject attackBullet, Vector3 firePosition, Vector3 targetPosition)
    {
        var obj = BulletPoolManager.Instance.attackPool.Get();
        obj.GetComponent<Bullet>().BulletSet(firePosition, targetPosition);
    }
}
