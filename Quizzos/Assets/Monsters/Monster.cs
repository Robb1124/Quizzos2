using System;
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
    Animator animator;

    public MonsterSheet MonsterSheet { get => monsterSheet; set => monsterSheet = value; }
    public int MonsterNumber { get => monsterNumber; set => monsterNumber = value; }

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
        }
    }

    public void AttackPlayerAnimation()
    {
        animator.SetTrigger("AttackTrigger");        
    }

    public void AttackPlayerDamage()
    {
        player.TakeDamage(monsterBaseDamage);
    }

    
}
