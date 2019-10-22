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
    [SerializeField] MonsterFX monsterFXHolder;
    MonsterAttacksHolder[] monsterAttacksHolders;
    SpecialEffect monsterAttackEffect;
    MonsterAttacks monsterAttack;
    float monsterAttackDamage;
    [Header("Monster Attacks Settings")]
    [SerializeField] float basicAttackMultiplier = 1f;
    [SerializeField] float headButtAttackMultiplier = 1.3f;
    [SerializeField] float venomousStingMultiplier = 0.8f;
    [SerializeField] float heavyHitMultiplier = 1.5f;
    [SerializeField] float punchMultiplier = 0.7f;
    [SerializeField] float cureMultiplier = 4f;
    [SerializeField] float flameSlashMultiplier = 1.4f;
    [SerializeField] float shieldBashMultiplier = 1.2f;

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
            if (monsters[i].isActiveAndEnabled && !monsters[i].IsDead)
            {
                monstersThatWillAttack++;
            }
        }

        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].isActiveAndEnabled && !monsters[i].IsDead && !monsterManager.AllDead && !monsters[i].IsStunned)
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
                        monsterAttackEffect = monsterAttacksHolders[j].specialEffectOnHit;
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
                        monsters[i].AttackPlayerAnimation(MonsterAnimations.ChargeAttack); //Trigger event on the animation to OnMonsterAttack method from player
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
                    case MonsterAttacks.Cure:
                        Monster targetMonster = monsters[i];
                        float percentageOfHealth = 1;
                        for (int j = 0; j < monsters.Length; j++)
                        {
                            if (monsters[j].isActiveAndEnabled && percentageOfHealth >= monsters[j].GetHealthPercentage())
                            {
                                targetMonster = monsters[j];
                                percentageOfHealth = monsters[j].GetHealthPercentage();
                            }
                        }                                              
                        monsterFXHolder.PlayMonsterFX(MonsterFXs.Cure, targetMonster.transform);
                        yield return new WaitForSeconds(0.5f);
                        targetMonster.HealDamage(Mathf.RoundToInt(monsterAttackDamage * cureMultiplier)); //TODO when refactoring abilities, make sure cure doesnt scale with damage anymore. so a weak character could heal regardless of its damage.
                        break;
                    case MonsterAttacks.FlameSlash:
                        monsterAttackDamage *= flameSlashMultiplier;
                        monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to OnMonsterAttack method from player
                        break;
                    case MonsterAttacks.ShieldBash:
                        monsterAttackDamage *= shieldBashMultiplier;
                        monsters[i].AttackPlayerAnimation(MonsterAnimations.ChargeAttack); //Trigger event on the animation to OnMonsterAttack method from player
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
                player.TakeDamage(monsterAttackDamage, monsterAttackEffect);
                break;
            case MonsterAttacks.VenomousSting:
                player.TakeDamage(monsterAttackDamage, monsterAttackEffect);
                break;
            case MonsterAttacks.FlameSlash:
                player.TakeDamage(monsterAttackDamage, monsterAttackEffect);
                break;
            case MonsterAttacks.ShieldBash:
                player.TakeDamage(monsterAttackDamage, monsterAttackEffect);
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
