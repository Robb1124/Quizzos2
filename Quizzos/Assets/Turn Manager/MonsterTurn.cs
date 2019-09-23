using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.ComponentModel;
using System.Text;

public class MonsterTurn : TurnState
{
    [SerializeField] Monster[] monsters;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameObject monsterAttackPopUp;
    [SerializeField] TextMeshProUGUI monsterAttackText;
    [SerializeField] Player player;

    MonsterAttacksHolder[] monsterAttacksHolders;
    MonsterAttacks monsterAttack;
    float monsterAttackDamage;
    [Header("Monster Attacks Settings")]
    [SerializeField] float basicAttackMultiplier = 1f;
    [SerializeField] float headButtAttackMultiplier = 1.3f;
    [SerializeField] float venomousStingMultiplier = 0.8f;
    [SerializeField] float heavyHitMultiplier = 1.5f;
    [SerializeField] float punchMultiplier = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStateChange()
    {
        StartCoroutine(MonsterAttackingCoroutine());
    }

    IEnumerator MonsterAttackingCoroutine()
    {
        int monstersThatWillAttack = 0;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].isActiveAndEnabled)
            {
                monstersThatWillAttack++;
            }
        }

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].isActiveAndEnabled && !monsterManager.AllDead && !monsters[i].IsStunned)
            {
                monstersThatWillAttack--;
                monsterAttacksHolders = monsters[i].MonsterSheet.GetMonsterAttacksHolders();
                float randAttackNum = Random.Range(0.00f, 1.00f);
                for (int j = 0; j < monsterAttacksHolders.Length; j++)
                {
                    randAttackNum -= monsterAttacksHolders[j].useProbability;
                    if(randAttackNum <= 0)
                    {
                        monsterAttack = monsterAttacksHolders[j].monsterAttack;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.35f);
                monsterAttackPopUp.SetActive(true);
                monsterAttackText.text = ToFormattedText(monsterAttack);
                yield return new WaitForSeconds(0.75f);
                monsterAttackDamage = monsters[i].GetMonsterBaseDamage();
                switch (monsterAttack)
                {
                    case MonsterAttacks.BasicAttack:
                        monsterAttackDamage *= basicAttackMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                    case MonsterAttacks.Headbutt:
                        monsterAttackDamage *= headButtAttackMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                    case MonsterAttacks.HeavyHit:
                        monsterAttackDamage *= heavyHitMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                    case MonsterAttacks.Punch:
                        monsterAttackDamage *= punchMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                    case MonsterAttacks.VenomousSting:
                        monsterAttackDamage *= venomousStingMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                }
                if (monstersThatWillAttack == 0)
                {
                    yield return new WaitForSeconds(1f);
                }
            }
            if (monsters[i].IsStunned)
            {
                yield return new WaitForSeconds(1f);
                monsters[i].UnStunEnemy();
            }
            monsterAttackPopUp.SetActive(false);
        }
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PrePlayerTurn>());
        monsterManager.AllDead = false;
    }

    public void TheMonsterAttack()
    {
        switch (monsterAttack)
        {
            case MonsterAttacks.BasicAttack:
            case MonsterAttacks.Headbutt:
            case MonsterAttacks.HeavyHit: //chance to stun ?
            case MonsterAttacks.Punch:
                player.TakeDamage(monsterAttackDamage, SpecialEffects.None);
                break;
            case MonsterAttacks.VenomousSting:
                player.TakeDamage(monsterAttackDamage, SpecialEffects.Poison);
                break;
        }
    }

    public static string ToFormattedText(MonsterAttacks monsterAttack)
    {
        var stringValue = monsterAttack.ToString();
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < stringValue.Length; i++)
        {
            if (char.IsUpper(stringValue[i]))
            {
                stringBuilder.Append(" ");
            }

            stringBuilder.Append(stringValue[i]);
        }

        return stringBuilder.ToString();
    }
}
