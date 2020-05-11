using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FoodCollectorAgent_End : Agent
{
    FoodCollectorAcademy_End m_MyAcademy;
    // Whether to shoot laser or not
    bool m_Shoot;
    // to store the rigid body component attached on the agent
    Rigidbody m_AgentRb;
    // Speed of agent rotation.
    public float turnSpeed = 300;
    // Speed of agent movement.
    public float moveSpeed = 2;

    float m_LaserLength = 1.0f;

    // Game objects I should be aware of
    public GameObject myLaser;

    // Materials representing different states
    public Material normalMaterial;
    public Material badMaterial;
    public Material goodMaterial;
    public Material frozenMaterial;

    // Agent state tracking
    bool m_Frozen;
    bool m_Poisoned;
    bool m_Satiated;

    float m_FrozenTime;
    float m_EffectTime;

    // Area
    public GameObject area;
    FoodCollectorArea_End m_MyArea;

    public override void InitializeAgent()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_MyAcademy = FindObjectOfType<FoodCollectorAcademy_End>();
        m_MyArea = area.GetComponent<FoodCollectorArea_End>();
    }
    public override void AgentReset()
    {
        Unfreeze();
        Unpoison();
        Unsatiate();
        m_Shoot = false;
        m_AgentRb.velocity = Vector3.zero;
        myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(Random.Range(-m_MyArea.range, m_MyArea.range),
            2f, Random.Range(-m_MyArea.range, m_MyArea.range))
            + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }
    public override void CollectObservations()
    {
        var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
        AddVectorObs(System.Convert.ToInt32(m_Frozen));
        AddVectorObs(System.Convert.ToInt32(m_Shoot));
    }
    public override float[] Heuristic()
    {
        var action = new float[3];
        if (Input.GetKey(KeyCode.D))
        {
            action[1] = 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action[1] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2f;
        }
        action[2] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
        return action;
    }
    public override void AgentAction(float[] vectorAction)
    {
        MoveAgent(vectorAction);
    }

    public void MoveAgent(float[] act)
    {
        // State tracker
        if (Time.time > m_FrozenTime + 4f && m_Frozen)
        {
            Unfreeze();
        }
        if (Time.time > m_EffectTime + 0.5f)
        {
            if (m_Poisoned)
            {
                Unpoison();
            }
            if (m_Satiated)
            {
                Unsatiate();
            }
        }

        // Zero things out so if no command
        // The agent won't move or rotate
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_Shoot = false;
        var shootCommand = false;

        // Get data from action array
        var forwardAxis = (int)act[0];
        var rotateAxis = (int)act[1];
        var shootAxis = (int)act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward;
                break;
            case 2:
                dirToGo = -transform.forward;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = -transform.up;
                break;
            case 2:
                rotateDir = transform.up;
                break;
        }
        switch (shootAxis)
        {
            case 1:
                shootCommand = true;
                break;
        }
        if (shootCommand)
        {
            m_Shoot = true;
            dirToGo *= 0.5f;
            m_AgentRb.velocity *= 0.75f;
        }
        m_AgentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        // slow it down
        if (m_AgentRb.velocity.magnitude > 5f)
        {
            m_AgentRb.velocity *= 0.95f;
        }

        if (m_Shoot)
        {
            var myTransform = transform;
            myLaser.transform.localScale = new Vector3(1f, 1f, m_LaserLength);
            var rayDir = myTransform.forward;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 2f, rayDir, out hit, 25f))
            {
                if (hit.collider.gameObject.CompareTag("agent"))
                {
                    hit.collider.gameObject.GetComponent<FoodCollectorAgent_End>().Freeze();
                }
            }
        }
        else
        {
            myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        Monitor.Log("Score", GetReward());

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            Satiate();
            collision.gameObject.GetComponent<FoodLogic_End>().OnEaten();
            AddReward(1f);

        }
        if (collision.gameObject.CompareTag("badFood"))
        {
            Poison();
            collision.gameObject.GetComponent<FoodLogic_End>().OnEaten();
            AddReward(-1f);
        }
    }
    void Freeze()
    {
        gameObject.tag = "frozenAgent";
        m_Frozen = true;
        m_FrozenTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = frozenMaterial;
    }

    void Unfreeze()
    {
        m_Frozen = false;
        gameObject.tag = "agent";
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }

    void Poison()
    {
        m_Poisoned = true;
        m_EffectTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = badMaterial;
    }

    void Unpoison()
    {
        m_Poisoned = false;
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }

    void Satiate()
    {
        m_Satiated = true;
        m_EffectTime = Time.time;
        gameObject.GetComponentInChildren<Renderer>().material = goodMaterial;
    }

    void Unsatiate()
    {
        m_Satiated = false;
        gameObject.GetComponentInChildren<Renderer>().material = normalMaterial;
    }
}
