using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BasicAgent_End : Agent
{
    public GameObject largeGoal;
    public GameObject smallGoal;
    BasicAcademy_End m_Academy;

    Vector3 m_loc;
    Vector3 parentOrigin;

    float m_SmallGoalPosition = -3f;
    float m_LargeGoalPosition = 7f;
    float PlatformHalfLimit = 10f;


    public override void InitializeAgent()
    {
        m_Academy = FindObjectOfType(typeof(BasicAcademy_End)) as BasicAcademy_End;
        parentOrigin = gameObject.transform.parent.transform.position;
    }

    public override void AgentReset()
    {
        gameObject.transform.position = parentOrigin;
        var APos = gameObject.transform.localPosition;
        smallGoal.transform.localPosition = new Vector3(APos.x + m_SmallGoalPosition, 0f, 0f);
        largeGoal.transform.localPosition = new Vector3(APos.x + m_LargeGoalPosition, 0f, 0f);
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 2 };
        }
        else if (Input.GetKey(KeyCode.A))
        {
            return new float[] { 1 };
        }
        return new float[] { 0 };
    }

    public override void AgentAction(float[] vectorAction)
    {
        var movement = (int)vectorAction[0];

        var direction = 0;

        switch (movement)
        {
            case 1:
                direction = -1;
                break;
            case 2:
                direction = 1;
                break;
        }
        var currentPosX = gameObject.transform.localPosition.x;
        currentPosX += direction;

        if (currentPosX > PlatformHalfLimit) { currentPosX = PlatformHalfLimit; }
        if (currentPosX < PlatformHalfLimit * -1f) { currentPosX = PlatformHalfLimit * -1f; }

        gameObject.transform.localPosition = new Vector3(currentPosX, 0f, 0f);
        AddReward(-0.01f);
        if (currentPosX == m_SmallGoalPosition)
        {
            Done();
            AddReward(0.1f);
        }

        if (currentPosX == m_LargeGoalPosition)
        {
            Done();
            AddReward(1f);
        }
    }
    public override void CollectObservations()
    {
        AddVectorObs(gameObject.transform.localPosition.x);
    }
}
