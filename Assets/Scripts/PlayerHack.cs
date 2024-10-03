using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHack : MonoBehaviour
{
    public KeyCode hackKey;
    public LayerMask enemyLayer;
    public Hackable currentSelectedEntity;
    public float sphereRadius = 0.5f;
    public float sphereCastDistance = 10f;
    private Animator animator;
    [SerializeField]
    GameObject hackBarGameObject;
    [SerializeField]
    Image hackBar;
    float hackBarAmount = 0;
    [Header("Hack Charge")]
    protected float chargeValue = 0;
    public float maxChargeValue = 40;
    public float chargeHitAmount = 10;
    [SerializeField]
    Image hackCharge;
    [SerializeField]
    GameObject hackChargeNotif;
    [SerializeField]
    GameObject crosshair;
    Coroutine hackNotifCoroutine = null;
    public void DisplayHackNotification()
    {
        if (hackChargeNotif)
        {
            if (hackNotifCoroutine != null)
            {
                StopCoroutine(hackNotifCoroutine);
            }
            hackChargeNotif.SetActive(true);
            hackNotifCoroutine = StartCoroutine(DisableHackChargeNotif(3.0f));
        }
    }
    public float GetChargeValue()
    {
        return chargeValue;
    }
    public void RemoveChargeValue(float amount)
    {
        chargeValue -= amount;
        if (chargeValue < 0)
        {
            chargeValue = 0;
        }
    }
    public void AddChargeValue(float amount)
    {
        chargeValue += amount;
        if (chargeValue > maxChargeValue)
        {
            chargeValue = maxChargeValue;
        }
    }
    private void Awake()
    {
        animator= GetComponent<Animator>();
        if (hackChargeNotif)
        {
            hackChargeNotif.SetActive(false);
        }
        if (crosshair)
        {
            crosshair.SetActive(false);
        }
    }
    void Update()
    {
        if (hackCharge)
        {
            hackCharge.fillAmount = chargeValue / maxChargeValue;
        }

        if (Input.GetKeyDown(hackKey))
        {
            if (GetComponent<ThirdPersonControllerRB>() && GetComponent<ThirdPersonControllerRB>().Grounded)
            {
                if (GameManager.Instance.GetHackMode() == false)
                {
                    if (chargeValue >= 10)
                    {
                        GameManager.Instance.ToggleHackMode(true);
                        animator.SetBool("Hacking", true);
                        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[1], 0.2f, transform.position);
                        if (crosshair)
                        {
                            crosshair.SetActive(true);
                        }
                    }
                    else
                    {
                        DisplayHackNotification();
                    }
                }
                else
                {
                    GameManager.Instance.ToggleHackMode(false);
                    animator.SetBool("Hacking", false);
                    AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[1], 0.2f, transform.position);
                    if (crosshair)
                    {
                        crosshair.SetActive(false);
                    }
                }
            }
        }
        if (chargeValue < 10 && GameManager.Instance.GetHackMode() == true)
        {
            GameManager.Instance.ToggleHackMode(false);
            animator.SetBool("Hacking", false);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[1], 0.2f, transform.position);

            DisplayHackNotification();
            if (crosshair)
            {
                crosshair.SetActive(false);
            }
        }

        if (!GameManager.Instance.GetHackMode())
        {
            hackBarGameObject.SetActive(false);
            hackBarAmount = 0;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (currentSelectedEntity != null && chargeValue >= 10)
            {
                hackBar.fillAmount = hackBarAmount;
                hackBarGameObject.SetActive(true);
                hackBarAmount += Time.deltaTime;
                if (hackBarAmount >= 1.0f)
                {
                    hackBarAmount = 0.0f;
                    currentSelectedEntity.Hack(GetComponent<Entity>());
                    if (currentSelectedEntity.GetComponent<EnemyAI>())
                    {
                        if (currentSelectedEntity.GetComponent<Animator>())
                        {
                            currentSelectedEntity.GetComponent<Animator>().SetTrigger("Stun");
                        }
                        currentSelectedEntity.GetComponent<EnemyAI>().Aggro();
                    }
                    currentSelectedEntity.hacked = true;
                    currentSelectedEntity.Selected = false;
                    currentSelectedEntity = null;
                    chargeValue -= 10.0f;
                }
            }
            else
            {
                hackBarGameObject.SetActive(false);
                hackBarAmount = 0;
            }
        }
        else
        {
            hackBarGameObject.SetActive(false);
            hackBarAmount = 0;

            CheckForEnemyHover();
        }
    }

    void CheckForEnemyHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a SphereCast in the direction of the ray
        if (Physics.SphereCast(ray, sphereRadius, out hit, Mathf.Infinity, enemyLayer))
        {
            // Get the Entity script on the object
            Hackable hackable = hit.collider.GetComponent<Hackable>();
            if (hackable && hackable.hacked)
            {
                hackable = null;
            }

            if (hackable != null)
            {
                // If there's a new entity under the mouse, deselect the old one
                if (currentSelectedEntity != null && currentSelectedEntity != hackable)
                {
                    currentSelectedEntity.Selected = false;
                }

                if (currentSelectedEntity != hackable && Vector3.Distance(transform.GetComponent<Entity>().neck.position, hackable.transform.position) <= sphereCastDistance)
                {
                    // Set the selected entity and mark it as selected
                    currentSelectedEntity = hackable;
                    currentSelectedEntity.Selected = true;
                }
            }
        }
        else
        {
            // If no enemy is hit by the SphereCast, deselect the current entity
            if (currentSelectedEntity != null)
            {
                currentSelectedEntity.Selected = false;
                currentSelectedEntity = null;
            }
        }
    }

    IEnumerator DisableHackChargeNotif(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        if (hackChargeNotif)
        {
            hackChargeNotif.SetActive(false);
        }
        yield return null;
    }
}
