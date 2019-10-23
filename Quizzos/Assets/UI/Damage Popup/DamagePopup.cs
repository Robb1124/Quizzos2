using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] Text damagePopupText;
    [SerializeField] int xPosRandomness = 20;
    [SerializeField] int yPosRandomness = 5;
    [SerializeField] int normalFontSize = 24;
    [SerializeField] int criticalHitFontSize = 28;
    [SerializeField] Color normalFontColor = Color.white;
    [SerializeField] Color criticalHitFontColor = Color.yellow;
    [SerializeField] Color fadedColor;
    [SerializeField] int animationHeight = 50;
    [SerializeField] float animationHeightJumps = 2f;
    [SerializeField] float delayBetweenJumps = 0.1f;
    [SerializeField] float fadeSpeed = 4f;
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDamagePopupAnimation(Transform targetPosition, float damageDone, bool criticalHit)
    {
        if (criticalHit)
        {
            audioSource.Play();
        }

        damagePopupText.text = criticalHit ? "CRIT " + damageDone.ToString("0"): damageDone.ToString("0");
        damagePopupText.color = criticalHit ? criticalHitFontColor : normalFontColor;
        damagePopupText.fontSize = criticalHit ? criticalHitFontSize : normalFontSize;
        transform.position = new Vector3(targetPosition.transform.position.x + UnityEngine.Random.Range(-xPosRandomness, xPosRandomness), targetPosition.transform.position.y + UnityEngine.Random.Range(yPosRandomness, -yPosRandomness), 0);

        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        float currentY = transform.position.y;
        float alpha = damagePopupText.color.a;
        while(transform.position.y <= currentY + animationHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + animationHeightJumps, 0);
            alpha -= 0.01f;
            damagePopupText.color = Color.Lerp(damagePopupText.color, fadedColor, fadeSpeed * Time.deltaTime);
            yield return new WaitForSeconds(delayBetweenJumps);
        }
        gameObject.SetActive(false);
    }
}
