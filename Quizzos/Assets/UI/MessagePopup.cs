using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messagePopupText;
    [SerializeField] float animationDelay = 2f;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveStringAndShowPopup(string messageString)
    {
        messagePopupText.text = messageString;
        gameObject.SetActive(true);        
        StartCoroutine(AnimationDelay());

    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(animationDelay);
        animator.SetTrigger("MessagePopup"); //gameobject is set to inactive in animation event.               
    }

    public void DesactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
