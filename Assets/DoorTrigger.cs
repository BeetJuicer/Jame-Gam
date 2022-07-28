using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorBody doorBody;

    [SerializeField]
    private bool cakeTrigger;

    private Vector3 startPosition;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("cake", cakeTrigger);
        Debug.Log("Active buttons: " + doorBody.activeButtons);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cakeTrigger && collision.GetComponent<PlayerHandler>().cake ||
                !cakeTrigger && !collision.GetComponent<PlayerHandler>().cake)
            {
                StepOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cakeTrigger && collision.GetComponent<PlayerHandler>().cake || 
                !cakeTrigger && !collision.GetComponent<PlayerHandler>().cake)
            {
                StepOff();
            }
        }
    }

    void StepOn()
    {
        doorBody.activeButtons++;
        transform.DOMoveY(startPosition.y - 0.25f, 0.2f).SetEase(Ease.InOutSine);
        if (doorBody.activeButtons > 0)
        {
            doorBody.OpenDoor();
        }
    }

    void StepOff()
    {
        doorBody.activeButtons--;
        transform.DOMoveY(startPosition.y, 0.2f).SetEase(Ease.InOutSine);
        if (doorBody.activeButtons == 0)
        {
            doorBody.CloseDoor();
        }
    }
}
