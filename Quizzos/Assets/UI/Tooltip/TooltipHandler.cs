using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public enum TooltipType { BasicAttack, SpecialAbility1, SpecialAbility2, Passive, SimpleText };

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    GameObject tooltip;
    [SerializeField] TooltipType tooltipsType;
    [TextArea(2, 5)]
    [SerializeField] string simpleTextField;
    TextMeshProUGUI tooltipText;
    Player player;
    int classIndex;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        classIndex = player.ClassIndex;
        tooltip = FindObjectOfType<Tooltip>().gameObject;
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.transform.position = transform.position;
        switch (tooltipsType)
        {
            case TooltipType.BasicAttack:
                tooltipText.text = player.CharacterClass.GetAbilityToolTip(0);
                break;
            case TooltipType.SpecialAbility1:
                tooltipText.text = player.CharacterClass.GetAbilityToolTip(1);
                break;
            case TooltipType.SpecialAbility2:
                tooltipText.text = player.CharacterClass.GetAbilityToolTip(2);
                break;
            case TooltipType.Passive:
                tooltipText.text = player.CharacterClass.GetAbilityToolTip(3);
                break;
            case TooltipType.SimpleText:
                tooltipText.text = simpleTextField;
                break;


        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            tooltip.transform.localPosition = new Vector3(1100, 0, 0);
    }
}
