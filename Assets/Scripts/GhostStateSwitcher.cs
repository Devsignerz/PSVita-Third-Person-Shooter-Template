using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    Scatter,
    Chase,
    Frigntened,
    Eaten,
}

public class GhostStateSwitcher : MonoBehaviour
{
    public GhostState state = GhostState.Chase;
    public List<float> stateTimes1 = new List<float> { 7f, 20f, 7f, 20f, 5f, 20f, 5.00f };
    public List<float> stateTimes2 = new List<float> { 7f, 20f, 7f, 20f, 5f, 1033.14f, .01f };
    public List<float> stateTimes5 = new List<float> { 5f, 20f, 5f, 20f, 5f, 1037.14f, .01f };
    int stateRound = 0;
    float stateTimer = 0f;
    bool stateTimerRun = true;


    void Update()
    {
        StateScatterChaseTimerSwitcher();
    }

    void StateScatterChaseTimerSwitcher()
    {
        if (stateTimerRun)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
            {
                if (stateRound % 2 == 0)
                    state = GhostState.Scatter;
                else
                    state = GhostState.Chase;
                stateTimer = stateTimes1[stateRound];
                ++stateRound;

                if (stateRound >= 7)
                {
                    state = GhostState.Chase;
                    stateTimerRun = false;
                }
            }
        }
    }
}
