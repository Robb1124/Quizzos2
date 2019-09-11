﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] MonsterSheet monsterSheet;
    MonsterManager monsterManager;
    [SerializeField] float monsterCurrentHp;
    [SerializeField] int monsterMaxHp;
    [SerializeField] float monsterBaseDamage;
    [SerializeField] Text monsterName;
    [SerializeField] Image monsterHpBar;
    [SerializeField] int monsterNumber;
    [SerializeField] Player player;
    [SerializeField] Image stunImage;
    Animator animator;
    bool isStunned;

    public MonsterSheet MonsterSheet { get => monsterSheet; set => monsterSheet = value; }
    public int MonsterNumber { get => monsterNumber; set => monsterNumber = value; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        monsterManager = FindObjectOfType<MonsterManager>();        
    }

    public void OnSpawn()
    {
        monsterMaxHp = monsterSheet.GetMonsterHp();
        monsterBaseDamage = monsterSheet.GetMonsterBaseDamage();
        monsterName.text = monsterSheet.GetMonsterName();
        GetComponent<Image>().sprite = monsterSheet.GetMonsterImage();
        monsterCurrentHp = monsterMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        monsterHpBar.fillAmount = monsterCurrentHp / monsterMaxHp;
    }

    public void TakeDamage(float damageDone)
    {
        monsterCurrentHp -= damageDone;
        animator.SetTrigger("TakeDamageTrigger");
        if(monsterCurrentHp <= 0)
        {
            //Monster death
            monsterManager.MonsterDeath(monsterNumber);
            //Unstunning
            IsStunned = false;
            stunImage.gameObject.SetActive(false);
        }
    }

    public void AttackPlayerAnimation()
    {
        animator.SetTrigger("AttackTrigger");        
    }

    public void AttackPlayerDamage()
    {
        player.TakeDamage(monsterBaseDamage, monsterSheet.GetSpecialEffectOnHit());
    }

    public void StunEnemy()
    {
        StartCoroutine(StunEnemyDelay());        
    }

    private IEnumerator StunEnemyDelay()
    {
        yield return new WaitForSeconds(0.7f);
        IsStunned = true;
        stunImage.gameObject.SetActive(true);
    }

    public void UnStunEnemy()
    {
        StartCoroutine(UnStunEnemyDelay());       
    }

    private IEnumerator UnStunEnemyDelay()
    {
        yield return new WaitForSeconds(0.7f);
        IsStunned = false;
        stunImage.gameObject.SetActive(false);
    }
}
