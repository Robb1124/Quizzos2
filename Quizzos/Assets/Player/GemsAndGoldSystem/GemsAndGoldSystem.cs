using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemsAndGoldSystem : MonoBehaviour
{
    [SerializeField] int gold;
    [SerializeField] int gems;
    [SerializeField] TextMeshProUGUI worldMapGemsText;
    [SerializeField] TextMeshProUGUI worldMapGoldText;

    // Start is called before the first frame update
    void Start()
    {
        worldMapGoldText.text = gold.ToString();
        worldMapGemsText.text = gems.ToString();

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

    public void AddGems(int gemsAmount)
    {
        gems += gemsAmount;
        worldMapGemsText.text = gems.ToString();
    }

    public void RemoveGems(int gemsAmount)
    {
        gems -= gemsAmount;
        worldMapGemsText.text = gems.ToString();
    }

    public int GetGems()
    {
        return gems;
    }

}
