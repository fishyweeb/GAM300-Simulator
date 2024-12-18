using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    Entity theEntity;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Sprite green;
    [SerializeField]
    Sprite yellow;
    [SerializeField]
    Sprite red;
    [SerializeField]
    Image theID;
    [SerializeField]
    Sprite idGreen;
    [SerializeField]
    Sprite idYellow;
    [SerializeField]
    Sprite idRed;
    [SerializeField]
    TMPro.TextMeshProUGUI xpCount;

    private void Update()
    {
        if (healthBar != null )
        {
            healthBar.fillAmount = theEntity.GetHealthFraction();
            if (healthBar.fillAmount >= 0.6666666666666667f)
            {
                GetComponent<Animator>().SetBool("LowHealth", false);
                theID.sprite = idGreen;
                healthBar.sprite = green;
            }
            else if (healthBar.fillAmount >= 0.3333333333333333f)
            {
                GetComponent<Animator>().SetBool("LowHealth", false);
                theID.sprite = idYellow;
                healthBar.sprite = yellow;
            }
            else
            {
                GetComponent<Animator>().SetBool("LowHealth", true);
                theID.sprite = idRed;
                healthBar.sprite = red;
            }
        }
        if (xpCount)
        {
            xpCount.text = GameManager.Instance.GetXP().ToString();
        }
    }
}
