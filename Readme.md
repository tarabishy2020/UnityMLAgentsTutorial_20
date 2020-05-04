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

## Reward Signals

[Check this link.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Reward-Signals.md)

## Config File

[Read this.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Training-PPO.md)

## Examples

[Check the list.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Learning-Environment-Examples.md)

## Challenge

[Check this link.](https://connect.unity.com/challenges/ml-agents-1)
