using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {
    // Bots running skils are scaled via the players rank
    public int rank;

    // Values found through trial and error. found to give the most realistic bot running.
    private float baseAcceleration = 0.1f;
    private float minSpeed = 1f;
    private float maxSpeed = 4f;
    private int baseSpeed = 3;

    private float currentSpeed;
    private float targetSpeed;
    private float acceleration;

    public RunnerAbstract runner;

    private void Awake()
    {
        runner = gameObject.AddComponent<RunnerAbstract>();
    }

    void Start()
    {
        runner.AbsStart();
        rank = GameStats.loggedInUser != null ? GameStats.loggedInUser.rank : 1;
        // Generate a random target speed within the bot's speed range. the bots speed will gradually rise to around the target speed
        // and it will stay around that speed more or less.
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        // Calculate the acceleration based on the bot's rank
        float maxAcceleration = rank / 10f;
        acceleration = Mathf.Lerp(baseAcceleration, maxAcceleration, Random.value);
    }


    /**************************************************************
     * Advances the bot on the track in a somewhat random manner. made to feel like a humanoid opponant and to scale with rank.
     **************************************************************/
    private void run()
    {
        // Calculate the difference between the current speed and the target speed
        // Will slow the bot down in a smooth manner when the current speed surpasses the target speed.
          float speedDiff = targetSpeed - currentSpeed;

        // Adjust the current speed based on the acceleration, the rank and the speed difference
        currentSpeed += acceleration * Time.deltaTime * Mathf.Sign(speedDiff) * rank * baseSpeed;

        // Clamp the current speed to the range of the bot's speed
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Move the bot forward based on the current speed
        transform.position += transform.forward * currentSpeed * Time.deltaTime * runner.finishPosition / runner.trackLength;

    }

    void Update()
    {
        if (runner.runningAllowed)
        {
            run();
        }
        runner.animateMe(currentSpeed, runner.runningAllowed);
    }
}
