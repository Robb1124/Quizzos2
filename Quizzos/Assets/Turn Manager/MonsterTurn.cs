using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTurn : TurnState
{
    [SerializeField] Monster[] monsters;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] TurnManager turnManager;

    
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
        print("monstaa");
        StartCoroutine(MonsterAttacks());
    }

    IEnumerator MonsterAttacks()
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
                yield return new WaitForSeconds(1f);
                monsters[i].AttackPlayerAnimation(); //Trigger event on the animation to Take Damage method from player
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
        }
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PrePlayerTurn>());
        monsterManager.AllDead = false;
    }
}
