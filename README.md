# Fluid Dialogue
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-1-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->

A Unity dialogue system that features an easy to use drag and drop graph. ScriptableObject driven with the ability to write custom actions and conditions to create complex dialogue workflows.

* **Visual graph** editor
* Ability to create custom **actions** that execute as dialogue progresses
* Create custom **conditions** to determine how dialogue branches
* Built in **local database** to set and check variables for branching
* Allows you to **bring your own UI** or use a starter example (event driven dialogue presentation)
* Includes a simple [spell check](https://github.com/ashblue/unity-simple-spellcheck) tool that evaluates all dialogue nodes

![](docs/example-dialogue.png)

**Support**

Join the [Discord Community](https://discord.gg/8QHFfzn) if you have questions or need help.

See upcoming features and development progress on the [Trello Board](https://trello.com/b/wMrsWyxn/fluid-dialogue).

## Getting Started

First you'll need to [install](#installation) the package with Unity Package Manager. After that you can create a database as follows.

1. Right click on the project window
1. Create -> Fluid -> Dialogue -> Graph
1. Click "Edit Dialogue" to customize your graph

To create a custom UI to display your graph, you'll need to handle a series of events emitted by the dialogue controller. You can find a full example of this in the [Examples](#examples) folder.

### Playing Dialogue

To trigger dialogue playback you'll need the following snippet. This will turn your dialogue data into an ongoing conversation with events and variables.

```csharp
using System.Collections;
using System.Linq;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues;
using UnityEngine;

public class ExampleDialoguePlayback : MonoBehaviour {
  private DialogueController _ctrl;
  public DialogueGraph dialogue;
  
  private void Awake () {
    var database = new DatabaseInstanceExtended();
    _ctrl = new DialogueController(database);
    _ctrl.Events.Speak.AddListener((actor, text) => { Debug.Log($"{actor.DisplayName}: {text}"); });
    _ctrl.Play(dialogue);
  }
}
```

For more details see the [`ExampleDialoguePlayback.cs`](https://github.com/ashblue/fluid-dialogue/blob/develop/Assets/Examples/BasicConversation/Scripts/ExampleDialoguePlayback.cs) file for a complete solution on how to manage dialogue in your game.

### Node Types

There are a few different node types. Each with a specific focus in mind.

* Dialogue: Choices and text from a specific actor
* Hub: Used to route dialogue logic without triggering any dialogue or choices
* Choice Hub: Used to trigger a set of choices multiple times

### Actors

Every dialogue should have an actor assigned to it. Actors can be created with the following steps.

1. Right click on the project window
1. Create -> Fluid -> Dialogue -> Actor

### Actions

Actions can be added after selecting a node. They can be triggered before a node activates (enter actions) or after a node is complete (exit actions). Note that actions can activate with a delay.

### Conditions

Conditions are just like actions. Except if any of them return false the node will be skipped at runtime until it's evaluated again.

### Local Variables

To create a local variable and use it do the following.

1. Right click on the project window
1. Create -> Fluid -> Database -> Choose a variable type

To use the variable simply select a node on the graph. Then click the plus sign on a condition (variable checks) or action (change variable value) in the inspector. Please note you must assign the variable you just created to use it.

### Global Variables

Globals are mostly the same as local variables. The difference is these will be saved to a global database that persists after the conversation is over and can be referenced across multiple conversations.

The GlobalDatabaseManager will be marked DoNotDestroyOnLoad to persist between scene loads. If you want to save it and write the contents to a file run `Save()` to return a string. You can then restore the database simply by calling `Load(returned save string)`.

### Interacting With Runtime GameObject(s)

To interact with GameObject(s) at runtime use the "SendMessage" action suite. You cannot directly reference GameObjects in the dialogue graph as they are separately serialized. Instead, you can use the "SendMessage" action to send a message to a GameObject at runtime.

It's strongly recommended when sending messages you use a consistent object name like "DIALOGUE_DIRECTOR" to keep things simple and write your own custom scripts to handle the messages. This will make it easier to maintain and debug your game.

![send-message.png](docs/send-message.png)

You can also use the "SetActive" action to toggle GameObjects on and off. This is a great low overhead way to manage dialogue scripts easily.

If you need to do a lot of heavy lifting with tons of scripting on runtime GameObjects, it's recommended you write your own custom actions to handle this. See the APIs section for more details.

### Examples

To see the entire Fluid Dialogue workflow, it's highly recommended that you download this repo and run projects in the `Assets/Examples` folder.

## Installation

Fluid Dialogue is used through [Unity's Package Manager](https://docs.unity3d.com/Manual/CustomPackages.html). In order to use it you'll need to add the following lines to your `Packages/manifest.json` file. After that you'll be able to visually control what specific version of Fluid Dialogue you're using from the package manager window in Unity. This has to be done so your Unity editor can connect to NPM's package registry.

```json
{
  "scopedRegistries": [
    {
      "name": "NPM",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.fluid"
      ]
    }
  ],
  "dependencies": {
    "com.fluid.dialogue": "2.6.0"
  }
}
```

## APIs

The following APIs are available for customizing your dialogue experience.

### Custom Actions

You can create your own custom actions for the dialogue tree with the following API syntax.

```c#
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues;

[CreateMenu("Custom/My Action")]
public class CustomAction : ActionDataBase {
    public override void OnInit (IDialogueController dialogue) {
        // Run the first time the action is triggered
    }

    public override void OnStart () {
        // Runs when the action begins triggering
    }

    public override ActionStatus OnUpdate () {
        // Runs when the action begins triggering

        // Return continue to span multiple frames
        return ActionStatus.Success;
    }

    public override void OnExit () {
        // Runs when the actions `OnUpdate()` returns `ActionStatus.Success`
    }

    public override void OnReset () {
        // Runs after a node has fully run through the start, update, and exit cycle
    }
}
```

### Custom Conditions

Custom conditions can be crafted with the following API.

```c#
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;

public class CustomCondition : ConditionDataBase {
    public override void OnInit (IDialogueController dialogue) {
        // Triggered on first time setup
    }

    public override bool OnGetIsValid (INode parent) {
        // Place node condition logic here
        return true;
    }
}
```

### Resuming Graph Playback

Graph playback can be resumed from anywhere. This includes nested graphs and the exact node you want to resume from.

```C#
var ctrl = new DialogueController(...);

// Play your graph to generate runtime data and advance the graph to a specific node by hand
ctrl.Play(...);

// Record the required serialized properties
var graph = ctrl.RootGraph; // Finds the root graph that is playing
var parentHierarchy = dialogue.Ctrl.ParentHierarchy; // Gets the IDs of all nodes the nested graphs that are playing
var nextNodeId = ctrl.ActiveDialogue.Pointer.Next()?.UniqueId; // You probably want the next node in your graph, not the current one

// Create a new graph and resume playback
var newCtrl = new DialogueController(...);

if (newCtrl.CanPlay(graph, parentHierarchy, nextNodeId)) {
    // Failsafe that makes sure our graph playback data is valid    
    newCtrl.Play(graph, parentHierarchy, nextNodeId);
} else {
    // If the data is invalid, just play the graph from the beginning or implement your own fallback logic
    newCtrl.Play(graph);
}
```

### Custom Nodes (Experimental)

You can create your own custom nodes with the following API. Please note this is experimental and missing some features.

First you will need to setup the data layer. This is what will be converted to a nested ScriptableObject on demand for the graph.

```c#
using System.Linq;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace YourNamespaceHere {
    // Changing this will customize the name in the graph editor creation menu
    [CreateMenu("Example")]
    public class NodeExampleData : NodeDataBase {
        public string myMessage;
        
        // This is the default name the node will display in the graph editor
        protected override string DefaultName => "Example";
        
        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new NodeCombat(
                graphRuntime,
                UniqueId,
                myMessage,
                children.ToList<INodeData>(),
                conditions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList()
            );
        }
    }
}
```

Next you will need to create the runtime node. This is what will be generated at runtime for every corresponding data object.

```c#
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace YourNamespaceHere {
    public class NodeExample : NodeBase {
        readonly string _myMessage;

        public NodeCombat (
            IGraph runtime,
            string uniqueId,
            // We've added our new value here. You can pass in whatever values you need. Just add more parameters to the constructor
            string myMessage,
            System.Collections.Generic.List<INodeData> children,
            System.Collections.Generic.List<Fluid.Dialogues.Conditions.ICondition> conditions,
            System.Collections.Generic.List<Fluid.Dialogues.Actions.IAction> enterActions,
            System.Collections.Generic.List<Fluid.Dialogues.Actions.IAction> exitActions)
            // This is what handles all the messy setup we'd normally have to do
            : base(runtime, uniqueId, children, conditions, enterActions, exitActions) {
            
            _myMessage = myMessage;
        }
        
        // Handles what happens when the node is played
        protected override void OnPlay (IDialoguePlayback playback) {
            Debug.Log(_myMessage);
        }
    }
}
```

Lastly you will need to create a view layer. This is what will be displayed in the graph editor. Please note this must be nested in a folder called `Editor`.

```c#
using CleverCrow.Fluid.Dialogues.Editors;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using UnityEditor;
using UnityEngine;

namespace YourNamespaceHere {
    [NodeType(typeof(NodeExampleData))]
    public class NodeExampleEditor : NodeEditorBase {
        protected override Color NodeColor { get; } = new(0.75f, 0.52f, 0f);
        protected override float NodeWidth => 200;

        protected override void OnPrintBody (Event e) {
            serializedObject.Update();
            
            // This creates a dynamic input for the myMessage field
            EditorGUILayout.PropertyField(serializedObject.FindProperty("myMessage"), GUIContent.none);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
```

And that's it. The node will now be available in the graph editor and print our message at runtime. It even supports conditions, actions, and parent/child connections.

For more details please take a look at the source code for the different node types. This will give you a better idea of how to structure more complex custom nodes with multiple choices and other details.

## Releases

Please note that whatever node you're resuming from will play that exact node with enter actions and all previous nodes will not trigger anything. This is a direct reference and does not simulate the graph from the beginning.

```c#

## Releases

Archives of specific versions and release notes are available on the [releases page](https://github.com/ashblue/fluid-dialogue/releases).

## Nightly Builds

To access nightly builds of the `develop` branch that are package manager friendly, you'll need to manually edit your `Packages/manifest.json` as so. 

```json
{
    "dependencies": {
      "com.fluid.dialogue": "https://github.com/ashblue/fluid-dialogue.git#nightly"
    }
}
```

Note that to get a newer nightly build you must delete this line and any related lock data in the manifest, let Unity rebuild, then add it back. As Unity locks the commit hash for Git urls as packages.

## Development Environment

If you wish to run to run the development environment you'll need to install the latest [node.js](https://nodejs.org/en/). Then run the following from the root once.

`npm install`

If you wish to create a build run `npm run build` from the root and it will populate the `dist` folder.

### Making Commits

All commits should be made using [Commitizen](https://github.com/commitizen/cz-cli) (which is automatically installed when running `npm install`). Commits are automatically compiled to version numbers on release so this is very important. PRs that don't have Commitizen based commits will be rejected.

To make a commit type the following into a terminal from the root

```bash
npm run commit
```

---

This project was generated with [Oyster Package Generator](https://github.com/ashblue/oyster-package-generator).

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/bosaku"><img src="https://avatars.githubusercontent.com/u/2468343?v=4?s=100" width="100px;" alt="Richard Bryan Irwin"/><br /><sub><b>Richard Bryan Irwin</b></sub></a><br /><a href="https://github.com/ashblue/fluid-dialogue/commits?author=bosaku" title="Documentation">📖</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
