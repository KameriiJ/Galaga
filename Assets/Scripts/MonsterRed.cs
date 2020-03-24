﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterRed : MonoBehaviour, MonsterStrategy
{
    private Text score;
    private int bulletAmounts;
    private bool attack, setEndTarget, goToRocket;
    private GameObject target, mbullet;
    private Vector2 targetPos, startPos;
    private float degreeRatio, monsterSpeed, bulletSpeed;

    public void Scoring()
    {
        score.text = (int.Parse(score.text) + 200).ToString();
    }

    public void ActiveMove()
    {
        transform.GetComponent<Rigidbody2D>().velocity = -transform.up * monsterSpeed;
    }

    public void AttackPlayer()
    {
        target = GameObject.FindGameObjectWithTag("rocket");
        targetPos = target.transform.position;
        attack = true;
        goToRocket = true;
    }

    public void BackToStartPoint()
    {
        transform.position = new Vector2(startPos.x - 2, 10);
        transform.GetComponent<Rigidbody2D>().velocity = -transform.up * 0;
        targetPos = startPos;
        setEndTarget = false;
    }

    private void Shoot()
    {
        //-45 to +45 on bottom of monster
        for(int i = 0; i < bulletAmounts; i++)
        {
            GameObject bul = GameObject.Instantiate(mbullet);
            bul.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
            bul.transform.rotation = Quaternion.AngleAxis(45-(degreeRatio*i), new Vector3(0, 0, 1));
            bul.GetComponent<Rigidbody2D>().velocity = -bul.transform.up * bulletSpeed;
        }
    }

    private void SetMonsterValue()
    {
        GameplayManager setting = transform.parent.GetComponent<GameplayManager>();
        score = setting.score;
        bulletAmounts = setting.redBulletAmounts;
        if (bulletAmounts > 10) bulletAmounts = 10;
        mbullet = setting.mbullet;
        monsterSpeed = setting.redSpeed;
        bulletSpeed = setting.redBulletSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMonsterValue();
        startPos = transform.position;
        attack = false;
        setEndTarget = false;
        degreeRatio = 90 / (bulletAmounts - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            if (transform.position.y < -3f)
            {
                if (!setEndTarget)
                {
                    Shoot();
                    if (transform.position.x > 0) targetPos = GameObject.Find("CheckPointRight").transform.position;
                    else targetPos = GameObject.Find("CheckPointLeft").transform.position;
                    setEndTarget = true;
                }
                goToRocket = false;
            }

            if (!goToRocket) transform.position = Vector2.MoveTowards(transform.position, targetPos, monsterSpeed * 2 * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - startPos.y) < 0.1 && Mathf.Abs(transform.position.x - startPos.x) < 0.1)
            {
                attack = false;
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, monsterSpeed * Time.deltaTime);
            }
        }
    }
}
