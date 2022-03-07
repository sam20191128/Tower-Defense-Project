using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<GameObject> enemys = new List<GameObject>(); //攻击范围内敌人列表

    //进入攻击范围
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    //离开攻击范围
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1; //多少秒攻击一次
    private float timer = 0;

    public GameObject bulletPrefab; //子弹
    public Transform firePosition;
    public Transform head;

    public bool useLaser = false;

    public float damageRate = 70;

    public LineRenderer laserRenderer;

    public GameObject laserEffect;

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        if (enemys.Count > 0 && enemys[0] != null) //有敌人存在
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }

        if (useLaser == false) //没有使用激光，普通攻击
        {
            timer += Time.deltaTime; //计时器
            if (enemys.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        else if (enemys.Count > 0) //使用激光
        {
            if (laserRenderer.enabled == false)
                laserRenderer.enabled = true;
            laserEffect.SetActive(true);
            if (enemys[0] == null) //攻击范围内敌人列表为空
            {
                UpdateEnemys();//更新一下
            }

            if (enemys.Count > 0)
            {
                laserRenderer.SetPositions(new Vector3[] {firePosition.position, enemys[0].transform.position}); //攻击第一个
                enemys[0].GetComponent<Enemy>().TakeDamage(damageRate * Time.deltaTime); //激光伤害
                laserEffect.transform.position = enemys[0].transform.position; //激光大众特效
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }
        else
        {
            laserEffect.SetActive(false);
            laserRenderer.enabled = false;
        }
    }

    //普通攻击
    void Attack()
    {
        if (enemys[0] == null)//攻击范围内敌人列表为空
        {
            UpdateEnemys();//更新一下
        }

        if (enemys.Count > 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }

    //更新敌人
    void UpdateEnemys()
    {
        //enemys.RemoveAll(null);
        List<int> emptyIndex = new List<int>();
        for (int index = 0; index < enemys.Count; index++)
        {
            if (enemys[index] == null)
            {
                emptyIndex.Add(index); //emptyIndex中加入
            }
        }

        for (int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i] - i); //攻击范围内敌人列表中移除
        }
    }
}