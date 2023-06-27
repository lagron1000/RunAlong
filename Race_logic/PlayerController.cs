using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text nickname;
    public RunnerAbstract runner;

    private void Awake()
    {
        runner = gameObject.AddComponent<RunnerAbstract>();
    }

    void Start()
    {
        runner.AbsStart();

        if (GameStats.loggedInUser != null)
        {
            nickname.text = GameStats.loggedInUser.nickname;
        }
    }

    private void run()
    {
        // Get the player's real-life speed
        float realLifeSpeed = GetRealLifeSpeed();

        // Move the player model based on the current speed and on the amount of time left
        Vector3 moveBy = transform.forward * realLifeSpeed * Time.deltaTime * runner.finishPosition / runner.trackLength;
        transform.position += moveBy;

        runner.animateMe(realLifeSpeed, runner.runningAllowed);
    }

    private float GetRealLifeSpeed()
    {
        float speed = 0f;

        // Check if the device has an accelerometer
        if (SystemInfo.supportsAccelerometer)
        {
            Vector3 acc = Input.acceleration;

            // Get the current acceleration of the device in the X and Y directions
            Vector2 acceleration = new Vector2(acc.x, acc.y);
            // Ignore acceleration in the Z direction, which is affected by gravity

            speed = acceleration.magnitude;
        }
        else
        {
            // If the device doesn't have an accelerometer, Dont move.
        }

        return speed;

    }

    void Update()
    {
        if (runner.runningAllowed)
        {
            run();
        }
    }
}
