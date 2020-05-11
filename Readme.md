# Unity ML-Agents <!-- omit in toc -->

- [Things to download](#things-to-download)
- [Create a Project and Import ML-Agents](#create-a-project-and-import-ml-agents)
- [Starting with the Basic Scene](#starting-with-the-basic-scene)
- [How to train our own model?](#how-to-train-our-own-model)
- [Visualize your training progress](#visualize-your-training-progress)
- [A step back](#a-step-back)
- [Is the agent learning?](#is-the-agent-learning)
- [Implement an academy](#implement-an-academy)
- [Create an agent](#create-an-agent)
- [Basic Example](#basic-example)
  - [Academy](#academy)
  - [Agent](#agent)
    - [Initialize the Agent](#initialize-the-agent)
    - [Agent Reset steps](#agent-reset-steps)
    - [Agent heuristics](#agent-heuristics)
    - [Agent Actions](#agent-actions)
    - [Test heuristics](#test-heuristics)
    - [Can we start training now?](#can-we-start-training-now)
    - [What about now?](#what-about-now)
    - [Setting Behavior Parameters](#setting-behavior-parameters)
    - [Why are things slow?](#why-are-things-slow)
    - [When should you stop?](#when-should-you-stop)
    - [Multiple training areas](#multiple-training-areas)
- [Food Collector Example](#food-collector-example)
  - [Agent Heuristics](#agent-heuristics-1)
  - [Create an academy](#create-an-academy)
  - [Agent Initialize](#agent-initialize)
  - [Agent Action](#agent-action)
  - [Area](#area)
  - [Back to the Academy](#back-to-the-academy)
  - [Make Agent Eat](#make-agent-eat)
    - [Activate and deactivate states](#activate-and-deactivate-states)
    - [Paint it blue!](#paint-it-blue)
    - [Freeze/Unfreeze](#freezeunfreeze)
    - [Agent Reset](#agent-reset)
    - [Agent observations](#agent-observations)
    - [More observations](#more-observations)
  - [Party](#party)
  - [Even bigger party](#even-bigger-party)
- [Reward Signals](#reward-signals)
- [Config File](#config-file)
- [Examples](#examples)
- [Challenge](#challenge)

## Things to download

- Create an account on [Unity](https://id.unity.com/en/conversations/d46c60c4-d0aa-4cc5-bc7b-ba0f8e70620b00af?view=register).
- Download [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe). After downloading install it.
- In Unity Hub, sign-in and got to Installs > Add > and install 2019.3.12f1 or any 2019.3.\*
- Download [Anaconda Python 3.7](https://www.anaconda.com/products/individual). We will be using windows, if you are on windows download the 64-bit version. After downloading, install it.
- Download [Unity ML-Agents 0.13](https://github.com/Unity-Technologies/ml-agents/archive/0.13.1.zip).
- Download [Visual Studio Community edition](https://visualstudio.microsoft.com/downloads/).

## Create a Project and Import ML-Agents

Create a unity project using the standard 3D project template.

After you open the project, go to Windows > Package Manager > Advanced > Show Preview packages > search for Barracuda and Install version [0.4.0], this is the version compatible with ML-Agent [0.13.1] that we are using.

For reference you can go to the [github repo](https://github.com/Unity-Technologies/ml-agents/tree/release-0.13.1) and make sure you are in the branch we are working on.

You can check the [docs folder](https://github.com/Unity-Technologies/ml-agents/tree/release-0.13.1/docs) for general help. We will start in the [examples help](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Learning-Environment-Examples.md).

To import Unity ML-Agents, after you created an empty project, and downloaded the ML-Agents repo, from the repo copy this folder

```
.\ml-agents-0.13.1\UnitySDK\Assets\ML-Agents
```

into your project's Assets folder.

## Starting with the Basic Scene

```
Assets\ML-Agents\Examples\Basic
```

Open the scene inside this folder.

Explore the objects in the scene a bit.

You will notice only two objects have custom components on them.

- Academy: With a script component called basic academy. Controls and facilitates learning within the scene.
- Basic/BasicAgent: With a behavior parameters script and a basic agent script. The agent is the link between the environment and the brain/neural network. It will take input from the scene, pass it to the network which will decide on what behavior should the agent respond with in the scene.

The behavior of an agent is encoded in a neural network, the example scene comes with a trained model.

To test it make sure `Basic/BasicAgent/Behavior Parameter/Model` has the trained model from `Ml-Agents/Examples/Basic/TFModels` referenced.

If you press play you will see the agent going to the big sphere continuously, which is the objective of this basic scene. What the brain/NN is doing now is called inference.

The brain/nn was already trained, so the model we just linked is a pre-trained model.

## How to train our own model?

To do that, we need to use python.

This is where Anaconda that we already downloaded comes in.

If you Press on windows Start > Anaconda Prompt, a command line interface will open up.

Right now we are using a specific version of ml-agents, what happens if we later download a new one and test it, will there be conflict between dependencies?

Thats why we use Anaconda, because it supports creating different environments, in each env you can have a different Python version, a list of diff plugins, and no conflicts will happen between environments.

To see available env

```cmd
conda env list
```

We will only see one `base` because we didn't create new ones yet.

To see env commands, [visit this link](https://docs.conda.io/projects/conda/en/latest/user-guide/tasks/manage-environments.html).

To create a new env

```cmd
conda create --name mlagents001301 python=3.7
conda env list
conda activate mlagents001301
```

You should see now the name of the env on the left of your command prompt.

Notice we are using python 3.7, as this is the last version tensorflow 1.15 that we will be using for training works on.

We can install ml-agents for python in [two ways](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Installation.md).

From pip, package repository, and we will get the latest version, which we may not be currently using.

Or just installing it from the folder we downloaded, which is what we will do.

```cmd
cd to/mlagents/location
cd ml-agents-envs
pip install -e ./
cd ..
cd ml-agents
pip install -e ./
```

If you get an error saying `pip is not recognized as an external or internal command` then you will need to install pip first.

```cmd
conda install pip
```

Before we start training we need to follow the steps [here](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Basic-Guide.md).
Basically go to Edit > Project Settings > Player > Other Settings > API Compatibility Settings = .NET 4.x

We also need a config file for the training, this is located in `.\ml-agents-0.13.1\config\trainer_config.yaml`

To train, in anaconda, after activating our env, we will use this command

```cmd
mlagents-learn <trainer-config-path> --run-id=<run-identifier> --train
```

So lets do this

```cmd
cd to\where\this\is\ml-agents-0.13.1\
mlagents-learn config\trainer_config.yaml --run-id=MyFirstRun --train
```

If all went well you will see this

```cmd
INFO:mlagents_envs:Listening on port 5004. Start training by pressing the Play button in the Unity Editor.
```

So go to Unity and press play.

## Visualize your training progress

If you open a new anaconda prompt, and activate our env

```cmd
conda activate mlagents001301
cd to\where\this\is\ml-agents-0.13.1\
tensorboard --logdir summaries
```

tensorboard is TensorFlow's visualization toolkit. The summaries folder is where ml-agents python toolkit dumps the training progress, you will find a folder inside named the same name as our run-identifier `MyFirstRun`. What we are usually interested in is Cumulative Reward.

If for some reason you training is interrupted or ended, you will find an `.nn` file saved in the models folder. This is your trained network that you can use in Unity.

If you want to resume training from were it stoppped or got interrupted

```cmd
mlagents-learn config\trainer_config.yaml --run-id=MyFirstRun --train --load
```

## A step back

What are we trying to do?

We are trying to use reinforcement learning "to learn a **policy**, which is essentially a mapping from **observations** to **actions**."

[An observation is what the robot can measure from its environment (in this case, all its sensory inputs) and an action, in its most raw form, is a change to the configuration of the robot (e.g. position of its base, position of its water hose and whether the hose is on or off).](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Background-Machine-Learning.md)

The one remaining piece is the reward signal. How do we keep the agent excited and interested in learning and exploring the environment?

We provide it with **rewards** (positive and negative) indicating how well it is doing on completing a task.

An agent learns about your objective because it receives a large positive reward when it accomplishes that objective and a small negative reward for every passing second.

"Similar to both **unsupervised** and **supervised learning**, **reinforcement learning** also involves two tasks: attribute selection and model selection. Attribute selection is defining the set of observations for the robot that best help it complete its objective, while model selection is defining the form of the policy (mapping from observations to actions) and its parameters. "

## Is the agent learning?

So how can we know?

Well from tensorboard! You can check [this](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Training-PPO.md) and [this](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Getting-Started-with-Balance-Ball.md) at the bottom, for explanation of what the graphs show.

## Implement an academy

In newer versions its there by default no need to implement.

```cs
using MLAgents;

public class RollerAcademy : Academy { }
```

## Create an agent

The Agent is the actor that observes and takes actions in the environment.

Behavior name should be same name as the config in the config file you will use during training.

Two important things:

- vector observation space
- Vector action space

Functions you need to override

- agent.AgentReset()
- agent.CollectObservations() must call AddVectorObs, so the total number of obs matches the vector action space size.
- agent.AgentAction(float[] vectorAction) changes agent and also SetReward() and can also set the agent to Done(); You can have the Agent reset immediately, by setting the Agent.ResetOnDone in the inspector to true, in this case it doesn't rely on the academy to reset it.
- agent.Heuristic() if the user is playing himself.

## Basic Example

Copy Basic_scratch, which is where we will start from.

### Academy

Create an empty GO called "Academy".

Create a script called "BasicAcademy_Scratch".

All this will do is inherit from Academy.

```cs
using MLAgents;

public class BasicAcademy_End : Academy
{

}
```

Add that component to your Academy GO.

### Agent

Open Basic_Scratch Prefab, create BasicAgent_Scratch cs script and add it to `Basic_Scratch/BasicAgent`.

Start the script by

```cs
using MLAgents;

public class BasicAgent_End : Agent
{
    public GameObject largeGoal;
    public GameObject smallGoal;

}
```

And add your LargeGoal and SmallGoal GOs to their respective parameters.

**Important.** Once you inherit from Agent and save, if you don't see a new component added automatically under you BasicAgent_Scratch component, then add manually a component named "Behavior Parameters".

#### Initialize the Agent

Similar to `void Setup`, anything we want to happen once, when the agent is enabled, we can put in `InitializeAgent`.

We should also store a reference to the academy, even though we will not use it, as mentioned in newer versions this step is not necessary.

**Notice.** I will try to have everything as a reference to the current parent position. You can use absolute world references, but by going with local references, things will be much easier if we want to create duplicates of our agent along with its training area/environment. Thats why am storing the parent origin here.

```cs
BasicAcademy_End m_Academy;

public override void InitializeAgent()
{
    m_Academy = FindObjectOfType(typeof(BasicAcademy_End)) as BasicAcademy_End;
    parentOrigin = gameObject.transform.parent.transform.position;
}
```

#### Agent Reset steps

Next, we need to create the logic of what should happen when we reset our agent. This happens in a method called AgentReset inside the Agent base class.

To define our own logic for our own agent, we will override that method.

We will set the agent to the position of the parent's origin, so it snaps back in the middle.

And we will also move the goals to their place.

```cs
public override void AgentReset()
{
    gameObject.transform.position = parentOrigin;
    var APos = gameObject.transform.localPosition;
    smallGoal.transform.localPosition = new Vector3(APos.x + m_SmallGoalPosition, 0f, 0f);
    largeGoal.transform.localPosition = new Vector3(APos.x + m_LargeGoalPosition, 0f, 0f);
}
```

#### Agent heuristics

Next step, each agent has a Heuristic method, that provides the agent with instructions on what to do. This is useful if we want to control the agent manually without the NN/brain. Its also helpful in making us think what kind of controls we want for our agent.

In this case we want to be able to move the agent to the left or to the right, but also stay in its place if it wants to.

This method should return an array of floats. In our case this array will include just one element, indicating the key the user pressed lets say to move the agent left or right.

```cs
public override float[] Heuristic ()
{
    if (Input.GetKey (KeyCode.D))
    {
        return new float[] { 2 };
    }
    else if (Input.GetKey (KeyCode.A))
    {
        return new float[] { 1 };
    }
    return new float[] { 0 };
}
```

#### Agent Actions

So from the previous, you can imagine that an array of floats is what the agent will need before it takes any actions in the env. You are correct!

We know what to expect as an input, since we decided on that in the previous step.

All we have to do, is select the values we are expecting our Agent to receive, and guide it to what it should do with those values.

```cs
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

    gameObject.transform.localPosition += new Vector3(direction, 0f, 0f);
}
```

#### Test heuristics

Now if you go to `Behavior Parameters` component and set the `Behavior Type` to `Heuristic Only` and play.

You will notice two things:

- Some times the agent in one step can pass the goals.
- Even when it reaches the goals nothing happens.

To deal with those two problems:

- We need to set bounds for the agent. If it passes a specific coordinates, we need to snap it back to place.
- And once it reaches the goal we need to let the agent know that it accomplished its goal.

So lets change the `AgentAction` method to look like this instead

```cs
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

    if (currentPosX == m_SmallGoalPosition)
    {
        Done();
    }

    if (currentPosX == m_LargeGoalPosition)
    {
        Done();
    }
}
```

**Notice.** In the `Basic Agent_scratch` component, we need to activate `Reset On Done`.

#### Can we start training now?

No!

We need to let the agent know when he is taking a good action or a bad action.

We also need to incentivize it to keep moving and exploring, instead of standing in place.

So we will use the `AddReward` method, to keep giving negative rewards, very small evey step in the world. A positive reward when it reaches the small goal, and a big positive reward when it reaches the big goal.

Add the rewards in the `AgentAction` so that it looks like this.

```cs
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
```

#### What about now?

Well you better not to!

Right now your agent is blind folded in the environment, taking actions and getting rewards, but not really aware of whats happening around it.

We need to record observations!

Observations help develop a better policy.

In this example an observation can just be the position of the agent.

```cs
public override void CollectObservations()
{
    AddVectorObs(gameObject.transform.localPosition.x);
}
```

#### Setting Behavior Parameters

Go to the `Behavior Parameters` component, and set Space Size to 1 (we only have one observation) and stacked Vectors to 1 (how far to the past should the agent see?).

Then set Vector Action Space type to Discrete, Branches to 1, and the size of that branch is 3, Since we have one decision to make, that decision can be one of three actions, stay out, go left or go right.

After training you might find things are a bit slow.

#### Why are things slow?

["With decision interval set to 5, the agent actually repeats the same action 5 times before making the next decision step. You could implement this behaviour using on-demand-decisions and request a new decision every 5th time the agent's FixedUpdate method executes. Setting the decision interval to greater than 1 is just a convenient way for spreading out decisions without having to write your own interval code. Longer decison intervals can be beneficial if an agent doesn't need updates on every frame, e.g. for deciding which direction to move in."](https://github.com/Unity-Technologies/ml-agents/issues/2132#issuecomment-502027295)

So we can go to the `BasicAgent_Scratch` component and change `Decision Interval` to 5. Now start the training again and you will see things going faster.

#### When should you stop?

Open tensorboard and check the graphs, once cumulative rewards converges, you can stop from unity.

Once you interrupt the training, inside the Models folder, with the name of your run_id, you will find a `.nn` file, copy that to your project, and reference it inside the Model parameter inside the Behavior Parameters component on your BasicAgent.

You can also decrease the decision interval back to 1 now during inference.

#### Multiple training areas

So remember at the begining when I mentioned lets use all coordinates relative to the origin of the parent GO in the hierarchy?

Well lets utilize that.

Make sure your `Behavior Type` is set to Default and your `Decision Interval` set to 5.

Now duplicate the whole prefab `Basic_Scratch` multiple times around the scene, in a grid like fashion.

And start your training again from python. They are all participating!!

## Food Collector Example

### Agent Heuristics

Lets start from here, how do we want to control the agent?
W - forward, S - backward, A - turn left, D - Turn Right and Space - Shoot Laser and slow down.

Create `FoodCollectorAgent_End` script. Add `Using MlAgents;`, inherit `Agent` instead of `Monobehaviour` and lets start with heuristics for this agent.

We have three actions, first one with three options, forward/backward/stand still, second one rotate left/right/stand still and lastly shoot laser pointer with two options on/off.

```cs
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
```

We can also now go to `Behavior Parameters` components, and set `Space Type` to Discrete. `Branch Sizes` to 3, the first two will be of size 3 and the last one of size 2.

### Create an academy

Create an empty go called Academy, and a script called `FoodCollectorAcademy_End`.

For now we will leave it empty.

```cs
using MLAgents;

public class FoodCollectorAcademy_End : Academy
{

}
```

### Agent Initialize

```cs
FoodCollectorAcademy_End m_MyAcademy;
// to store the rigid body component attached on the agent
Rigidbody m_AgentRb;

public override void InitializeAgent()
{
    // we will store here the agents's rigid body component
    m_AgentRb = GetComponent<Rigidbody>();
    m_MyAcademy = FindObjectOfType <FoodCollectorAcademy_End> ();
}
```

### Agent Action

Based on those inputs what should the agent do?

```cs
// Whether to shoot laser or not
bool m_Shoot;

// Speed of agent rotation.
public float turnSpeed = 300;
// Speed of agent movement.
public float moveSpeed = 2;

float m_LaserLength = 1.0f;

// Game objects I should be aware of
public GameObject myLaser;

public override void AgentAction(float[] vectorAction)
{
    MoveAgent(vectorAction);
}
public void MoveAgent(float[] act)
{
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
    }
    else
    {
        myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
    }
```

If you press play now your agent should be moving around.

**Notice** Since the `wallOuter` go under `Court` go has a mesh collider, and our `Agent` has a `Rigid body` component with `Collison` detection set to Discrete and Constraints/FreezeRotation in X, Y and Z activated, and a `Box collider` it will stop in place once it hits it.

### Area

We need to create something different this time, that we didn't do in the previous example.

A training area, imagine it like a classroom. It will take care of randomizing the food overtime a new session of training is started.

Create `FoodCollectorArea_End`, it will inherit from `Area` in `MLAgents` instead of Monobehaviour. And add it to `FoodCollectorArea` go.

Our area will need to know how represent bad food and how to represent bad food.

```cs
using MLAgents;

public class FoodCollectorArea_End : Area
{
    public GameObject food;
    public GameObject badFood;
}
```

You will notice in `/Prefabs` we have a `BadFood` and a `Food` Prefab, they are just spheres with a `Rigid Body` component on and different colors.

**Notice** another very important thing, each of them has a different tag. We will use the tags to know what type of object, in our game scenario, has an agent hit. The tags for each were added manually in Unity Editor, you can select a go or a prefab, and set a tag.

Reference each into the Area component we just created.
We will need some parameter to tell us where to place the food.

```cs
public int numFood = 25;
public int numBadFood = 25;
public bool respawnFood = true;
// this is the range where food will be spawned
// the court is 100 in width
// so we will generate food between -45 and 45
public float range = 45;
```

We will create a function that will add the food game objects

```cs
void CreateFood(int num, GameObject type)
{
    for (int i = 0; i < num; i++)
    {
        GameObject f = Instantiate(type, new Vector3(Random.Range(-range, range), 1f,
            Random.Range(-range, range)) + transform.position,
            Quaternion.Euler(Vector3.zero));
    }
}
```

And a function that will get called from our academy, once a session runs out of time, and we need to setup for a new session.

This function will move agents inside the classroom to random start positions, and take care of creating the food and the bad food.

```cs
public void ResetFoodArea(GameObject[] agents)
{
    foreach (GameObject agent in agents)
    {
        // Find the agents that are part of this area/"classroom"
        if (agent.transform.parent == gameObject.transform)
        {
            agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                Random.Range(-range, range))
                + transform.position; // So that its relative to the current area's position
            agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        }
    }

    CreateFood(numFood, food);
    CreateFood(numBadFood, badFood);
}
```

If you noticed, we did not do anything with the `public bool respawnFood = true;` field we created. This should control if food is eaten, whether it should respawn somewhere else or no.

This is a piece of logic that better be on the food itself.

So we will create a `FoodLogic_End` script.

The food object will be aware of the Area that created it, so that if its supposed to respawn it would do so within its boundaries. Notice again the use of `myArea.transform.position` so the values generate would be relative to the Area's position in the world.

```cs
public bool respawn;

public FoodCollectorArea_End myArea;
// Start is called before the first frame update
public void OnEaten()
{
    if (respawn)
    {
        transform.position = new Vector3(Random.Range(-myArea.range, myArea.range),
            3f,
            Random.Range(-myArea.range, myArea.range)) + myArea.transform.position;
    }
    else
    {
        Destroy(gameObject);
    }
}
```

We will add this Component to the `Food` and `BadFood` prefabs.

We also want the Area to pass to the food it creates whether it should be respawned on being eaten or not.

So back to the `CreateFood` method in the `FoodCollectorArea_End` component, we will add those two lines inside the for loop.
This will set the respawn property inside the food Go to the current respawn settings in the area component. And will also make sure that the food object knows that "this" is the Area that created it.

```cs
f.GetComponent<FoodLogic_End>().respawn = respawnFood;
f.GetComponent<FoodLogic_End>().myArea = this;
```

Two things should be unclear now, who is calling `OnEaten`? And also where do we call `ResetFoodArea` from?

### Back to the Academy

For the second question, lets go back to our academy.

We will edit it to look like this

```cs
public class FoodCollectorAcademy_End : Academy
{
    public override void AcademyReset()
    {
        // Claer all food objects
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));
        ClearObjects(GameObject.FindGameObjectsWithTag("badFood"));
        // Find all agents in the scene
        // notice if we didn't set them to "Agent" Tag
        // we will not be able to find them this way
        // There are other ways of doing this but finiding with tag is faster
        var agents = GameObject.FindGameObjectsWithTag("agent");
        // This will then do the same for all Areas
        var listArea = FindObjectsOfType<FoodCollectorArea_End>();
        // And feed each of them all the avaliable Agents
        // So that the area can reset its own agents' positions
        foreach (var fa in listArea)
        {
            fa.ResetFoodArea(agents);
        }
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }
}
```

Now if you play, you will find food appearing, and when the agent hits the food, food moves away.

So we need to make the Agent eat the food, which brings us back to the question, who is calling `OnEaten`?

### Make Agent Eat

In unity, a GO with Rigid Body component can use `OnCollisionEnter` method. Read more about it [here](https://docs.unity3d.com/ScriptReference/Collider.OnCollisionEnter.html).

Since we already added a rigid body component to our Agent, we just need to go and define the logic of what should happen, once the agent collides with something.

We need to define what will happen if it collides with `Food` or `BadFood`, and also this is where it would make sense to set the rewards.

**Notice.** We will know which type of food the Agent hits based on the tags we already assigned the two types of food, if your food prefabs don't have tags this will not work.

We to sum up we need to do couple of things here:

- Figure out which type of food the agent ate.
- Figure out rewards

So go to `FoodCollectorAgent_End` and add this method, now we can figure out which type of food we hit

```cs
void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("food"))
    {
    }
    if (collision.gameObject.CompareTag("badFood"))
    {
    }
}
```

We want to change the Agents color based on the actions its doing, so we will add those properties to the Agent

```cs
// Materials representing different states
public Material normalMaterial;
public Material badMaterial;
public Material goodMaterial;
public Material frozenMaterial;
```

In the starter files, you will find a material for each, if Agent state is normal we will use `AgentBlue` Material. If Agent ate `Food` we will use `Green` material, `badFood` `Red` material, and something we did not mention yet, but agents can shoot agents, like a game of tag, an agent that is shot by another agent will freeze, that will be visualized with `GrayMiddle` material.

Reference the respective materials for each property in the editor after you save.

#### Activate and deactivate states

So for all the states we mentioned we will create a method to activate or deactivate that state. And some properties to keep track whether a state is activated or not and what time that happened. If you look at the methods they are simple and almost do the same thing, change materials based on state that got activated or deactivated, keep track of active and disabled states in a property in our Agent's class and store time.

Add those properties

```cs
// Agent state tracking
bool m_Frozen;
bool m_Poisoned;
bool m_Satiated;

float m_FrozenTime;
float m_EffectTime;
```

Add those methods

```cs
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
```

Now that the logic of activating and deactivating different states is there, lets go use it, lets jump back to the `OnCollisionEnter` method.

We should update it to look like that

```cs
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
```

Finally we found who is calling `OnEaten`.

Now if you play, you will see food disappearing once you eat it, and appearing somewhere else, if `Respawn Food` was set to true like we did in the `FoodCollectorArea`.

One annoying thing though, the color of the agent doesn't change unless it eats something different than what it ate before! We should have the agent turn to blue back again after eating something.

#### Paint it blue!

So to do that, if you go back to your `MoveAgent` function, we will put some logic here to check how much time has passed since we ate something. So we need to check what time is it now, and if its more than the time we recorded, when a specific state was activated, then we will deactivate that state.

Add this at the very top of your `MoveAgent` method

```cs
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
```

Play now to test if this is working, it should! If it didn't start by checking if you saved what we just did or not!

You might have noticed we have the logic for freeze and unfreeze, but we did not use that anywhere!

#### Freeze/Unfreeze

In your `MoveAgent` method, towards the end, update the `m_shoot` if statement to look like this

```cs
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
```

Basically what we are doing is, if the agent shoots laser, check if the laser is shot in a direction that would hit another agent, if so, freeze that hit agent.

Two things we are missing now, what observations is our agent collecting from the environment, and what happens when our agent resets?

#### Agent Reset

Every time the agent resets, we want to move it to a new random position within the classroom/Area, so we need to be aware of our area.

Add a field to store the Area at the top of your Agent class, each agent, will reference its parent are, once you save, drag the `FoodCollectorArea` to the Area property that will appear in the component.

```cs
public GameObject area;
FoodCollectorArea_End m_MyArea;
```

So now in our `InitializeAgent` method, we can add this line

```cs
m_MyArea = area.GetComponent<FoodCollectorArea_End>();
```

And now lets override the `AgentReset` method, we will make sure all states are deactivated, and move the agent to a random position with a random rotation within the area.

```cs
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
```

#### Agent observations

We will use our current velocity, defined locally to our position. And also whether we are shooting laser or frozen.

```cs
public override void CollectObservations()
{
    var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
    AddVectorObs(localVelocity.x);
    AddVectorObs(localVelocity.z);
    AddVectorObs(System.Convert.ToInt32(m_Frozen));
    AddVectorObs(System.Convert.ToInt32(m_Shoot));
}
```

As you can see we are using 4 observations, so time to go and let the Agent's `Behaviour Parameters` know about that. So our `Vector Observation Space Size` will be 4, and we will set `Stack Vectors` to 1, since we are only interested in the current state of things, we don't want our model to look into the past states.

#### More observations

What we added as observations now is not enough for the agent to learn anything.

There is another way of adding observations in ML-Agents and thats using a `Ray Perception Sensor`. It gives the object more awareness about its surroundings. What it does is, it creates sphere casts, out from the agent position, you define it as if its the agent cone of vision, you define the angle of that cone, how many casts to perform within that angle, and what type of objects you are interested in. You define objects of interest by tags, so you should be using tags on everything you are interested in, which we have been doing i.e food, badFood, agent, frozenAgent and wall.

To read more about this check [here](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Learning-Environment-Design-Agents.md#raycast-observations).

### Party

Add more agents, now that all the settings are set, just duplicate the agent in the training area and start the training.

### Even bigger party

once you duplicated agents within the area, you can even start duplicating the area. and proceed with the training.

## Reward Signals

[Check this link.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Reward-Signals.md)

## Config File

[Read this.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Training-PPO.md)

## Examples

[Check the list.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Learning-Environment-Examples.md)

## Challenge

[Check this link.](https://connect.unity.com/challenges/ml-agents-1)
