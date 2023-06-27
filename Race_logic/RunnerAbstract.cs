using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/**************************************************************
 * Runner Composite, made in order to avoid duplicate code in runner classes.
 **************************************************************/
public class RunnerAbstract : MonoBehaviour
{
    public float trackLength;
    public Animator animator;
    private Transform finishLine;
    public float finishPosition;
    public bool runningAllowed = false;


    /**************************************************************
     * Initianating all neccesary fields
     **************************************************************/
    public void AbsStart()
    {
        finishLine = this.transform.parent.Find("Finish");
        finishPosition = finishLine.position.z - this.transform.position.z;

        trackLength = GameStats.trackLength > 0 ? GameStats.trackLength : 100;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("BotRest", true);

    }

    /**************************************************************
     * Animating the model based on the input speed
     **************************************************************/
    public void animateMe(float speed, bool running)
    {
        if (animator != null)
        {
            if (speed >= 1 && running)
            {
                animator.SetBool("BotRest", false);
                animator.SetBool("BotRun", true);
            }
            else
            {
                animator.SetBool("BotRun", false);
                animator.SetBool("BotRest", true);
            }
        }
    }

    /**************************************************************
    * Switching running state
     **************************************************************/
    public void switchRunning(bool run)
    {
        this.runningAllowed = run;
    }
}
