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

## Reward Signals

[Check this link.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Reward-Signals.md)

## Config File

[Read this.](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Training-PPO.md)

## Examples

[Check the list](https://github.com/Unity-Technologies/ml-agents/blob/release-0.13.1/docs/Learning-Environment-Examples.md)
