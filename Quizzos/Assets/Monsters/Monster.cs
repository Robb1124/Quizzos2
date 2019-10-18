using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterAnimations { StandardAttack, ChargeAttack}
public class Monster : MonoBehaviour
{
    [SerializeField] MonsterSheet monsterSheet;
    MonsterManager monsterManager;
    [SerializeField] int monsterCurrentHp;
    [SerializeField] int monsterMaxHp;
    [SerializeField] float monsterBaseDamage;
    [SerializeField] Text monsterName;
    [SerializeField] Image monsterHpBar;
    [SerializeField] Image monsterImage;
    [SerializeField] int monsterNumber;
    [SerializeField] Player player;
    [SerializeField] Image stunImage;
    [SerializeField] Text damagePopup;
    [SerializeField] float dmgAndHpLevelMultiplier = 1.1f;
    [SerializeField] MonsterTurn monsterTurn;
    bool isDead = false;
    int monsterLevel;
    Animator animator;
    bool isStunned;

    public MonsterSheet MonsterSheet { get => monsterSheet; set => monsterSheet = value; }
    public int MonsterNumber { get => monsterNumber; set => monsterNumber = value; }
    public bool IsStunned { get => isStunned; set => isStunned = value; }
    public Image MonsterImage { get => monsterImage; set => monsterImage = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        monsterManager = FindObjectOfType<MonsterManager>();        
    }

    public void OnSpawn(int monsterLevel)
    {
        IsDead = false;
        this.monsterLevel = monsterLevel;
        monsterMaxHp = Mathf.RoundToInt(monsterSheet.GetMonsterHp() * Mathf.Pow(dmgAndHpLevelMultiplier, monsterLevel - 1));
        monsterBaseDamage = Mathf.RoundToInt(monsterSheet.GetMonsterBaseDamage() * Mathf.Pow(dmgAndHpLevelMultiplier, monsterLevel - 1));
        monsterName.text = monsterSheet.GetMonsterName();
        MonsterImage.sprite = monsterSheet.GetMonsterImage();
        monsterCurrentHp = monsterMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        monsterHpBar.fillAmount = (float)monsterCurrentHp / monsterMaxHp;
    }

    public void TakeDamage(float damageDone, bool criticalHit)
    {
        //Randomness in the dmg +- 10% TODO make this customizable with different weapons
        float randomnDmgMultiplier = UnityEngine.Random.Range(0.9f, 1.1f);
        damageDone *= randomnDmgMultiplier;
        monsterCurrentHp -= Mathf.RoundToInt(damageDone);
        ActiveDamagePopup(damageDone, criticalHit);                
        animator.SetTrigger("TakeDamageTrigger");
        if(monsterCurrentHp <= 0)
        {
            //Monster death
            IsDead = true;
            monsterManager.MonsterDeath(monsterNumber);
            //Unstunning
            IsStunned = false;
            stunImage.gameObject.SetActive(false);
        }
    }

    public void HealDamage(int damageToHeal)
    {
        monsterCurrentHp += damageToHeal;
        if(monsterCurrentHp > monsterMaxHp)
        {
            monsterCurrentHp = monsterMaxHp;
        }
    }
    private void ActiveDamagePopup(float damageDone, bool criticalHit)
    {        
        damagePopup.gameObject.SetActive(true);
        damagePopup.GetComponent<DamagePopup>().ActivateDamagePopupAnimation(transform, damageDone, criticalHit);
    }

    public void AttackPlayerAnimation(MonsterAnimations monsterAnimation = MonsterAnimations.StandardAttack)
    {
        switch (monsterAnimation)
        {
            case MonsterAnimations.StandardAttack:
                animator.SetTrigger("AttackTrigger");
                break;
            case MonsterAnimations.ChargeAttack:
                animator.SetTrigger("ChargeAttackTrigger");
                break;
        }
    }

    public void AttackPlayer()
    {
        if (!IsDead)
        {
            monsterTurn.TheMonsterAttack();
        }
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

    public float GetMonsterBaseDamage()
    {
        return monsterBaseDamage;
    }

    public float GetHealthPercentage()
    {
        return monsterCurrentHp / monsterMaxHp;
    }
}
