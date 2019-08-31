using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Abilities { BasicAttack, SpecialAbility1, SpecialAbility2, ItemPouch};

public class Player : MonoBehaviour
{
    [SerializeField] int playerMaxHp = 100;
    [SerializeField] float playerCurrentHp;
    [SerializeField] int playerBaseDmg = 25;
    [SerializeField] float dmgReduction = 0;
    [SerializeField] Text playerHpText;
    [SerializeField] Image playerHpBar;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] PrePlayerTurn prePlayerTurn;
    
    public CharacterClass CharacterClass { get => characterClass; set => characterClass = value; }
    public int PlayerBaseDmg { get => playerBaseDmg; set => playerBaseDmg = value; }
    public float DmgReduction { get => dmgReduction; set => dmgReduction = value; }

    public delegate void OnPlayerDeath(); // declare new delegate type
    public event OnPlayerDeath onPlayerDeath; // instantiate an observer set

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHp = playerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        playerHpText.text = playerCurrentHp.ToString();
        playerHpBar.fillAmount = playerCurrentHp / playerMaxHp;
    }

    public void TakeDamage(float amountOfDamage)
    {      
        playerCurrentHp -= (amountOfDamage * (1 - DmgReduction));
        if(playerCurrentHp <= 0)
        {
            GameOver();
        }
    }
   

    public void ReceiveClass(int choosenClassIndex)
    {
        switch (choosenClassIndex)
        {
            case 0:
                gameObject.AddComponent<Warrior>();
                CharacterClass = GetComponent<Warrior>();
                break;
        }
        CharacterClass.OnClassEquip();
    }

    public void SetPlayerMaxHpAndBaseDmg(int maxHp, int baseDmg)
    {
        playerMaxHp = maxHp;
        playerCurrentHp = playerMaxHp;
        PlayerBaseDmg = baseDmg;
    }

    private void GameOver()
    {
        
        onPlayerDeath();
    }

    public void AddMaxHpAndBaseDamage(int maxHpGain, int maxDmgGain)
    {
        playerMaxHp += maxHpGain;
        playerCurrentHp = playerMaxHp;
        playerBaseDmg += maxDmgGain;
    }

}
