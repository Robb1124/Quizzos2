using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePlayerTurn : TurnState
{
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
        print("prepre");
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PlayerTurn>());
    }
}
