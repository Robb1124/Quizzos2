using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemAndGoldSystem : MonoBehaviour
{
    [SerializeField] int gold;
    [SerializeField] TextMeshProUGUI worldMapGoldText;

    // Start is called before the first frame update
    void Start()
    {
        worldMapGoldText.text = gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
        worldMapGoldText.text = gold.ToString();
    }

    public void RemoveGold(int goldAmount)
    {
        gold -= goldAmount;
        worldMapGoldText.text = gold.ToString();
    }

    public int GetGold()
    {
        return gold;
    }

}
