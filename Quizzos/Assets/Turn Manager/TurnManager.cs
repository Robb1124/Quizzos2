using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{    
    [SerializeField] TurnState turnState;

    public delegate void OnTurnChangeForPlayer(); // declare new delegate type
    public event OnTurnChangeForPlayer onTurnChangeForPlayer; // instantiate an observer set
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTurnState(TurnState turnState)
    {
        if (turnState.GetComponent<PrePlayerTurn>())
        {
            onTurnChangeForPlayer();
        }
        this.turnState = turnState;
        
        turnState.OnStateChange();
    }

    
}
