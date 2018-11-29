using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AdventureGame : MonoBehaviour
{
    const int MAXSTATERESCUE = 30;
    const int MAXDEHYDRATION = 20;
    const double DEADDEHYDRATION = 100;
    const int MINWOOLKNIT = 2;
    const int COSTWOOL = 2;
    const double DRINKWATER = 1.5;
    const double DEHYDRATIONSTEP = 0.5;
    const int MINWOOLTOCOLLECT = 1;

    const string INFOALARM = "Info.Alarm";
    const string KNITDO = "Knit.Do";
    const string FIGHTATTACK = "Fight.Attack";
    const string COLLECTDO = "Collect.Do";
    const string INFODONE = "Info.Done";
    const string COLLECTINFO = "Collect.Info";
    const string KNITINFO = "Knit.Info";
    const string FIGHTDO = "Fight.Do";
    const string INFOHUMAN = "Info.Human";
    const string INFOACCIDENT = "Info.Accident";
    const string COLLECT = "Collect";
    const string END = "End";

    //private static readonly System.Random getrandom = new System.Random(123);

    public Text textIntroComponent;
    public Text textStoryComponent;
    public Text textComponentChoices;
    public State startingState;
    public Image introBG;
    public Image storyBG;
    public Image storyMenueBG;
    public Image humanStateBG;
    public Image woolStateBG;
    public Text woolStateTxt;
    public Text humanStateTxt;
    public GameObject loadSceneObject;


    private SceneLoader sceneLoader;
    private int passedStates;
    private int collectedWool;
    private double dehydration;
    private bool wait, overrideTextComponent;
    private bool infoOn;
    private string overrideText;


    private State actualState;

    private void SetupIntroUI()
    {
        introBG.enabled = textIntroComponent.enabled = true;
        storyMenueBG.enabled = textComponentChoices.enabled = true;

        storyBG.enabled = textStoryComponent.enabled = false;
        humanStateBG.enabled = humanStateTxt.enabled = false;
        woolStateBG.enabled = woolStateTxt.enabled = false;
        infoOn = false;
    }

    private void SetupInfoUI()
    {
        introBG.enabled = textIntroComponent.enabled = false;
        storyMenueBG.enabled = textComponentChoices.enabled = true;

        storyBG.enabled = textStoryComponent.enabled = true;
        humanStateBG.enabled = humanStateTxt.enabled = true;
        woolStateBG.enabled = woolStateTxt.enabled = true;
        infoOn = true;
    }

    // Use this for initialization
    void Start()
    {
        sceneLoader = loadSceneObject.GetComponent<SceneLoader>();
        actualState = startingState;
        textIntroComponent.text = actualState.GetStateStory();
        textComponentChoices.text = actualState.GetStateStoryMenue();

        ResetValues();

        //statesUntilRescue = 30;
        wait = false;
        Debug.Log("Enter");

        SetupIntroUI();
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private string GetDehydrationText()
    {
        string txt = "Human \n" +
                     "Name: Magda \n" +
                     "Age: 21 \n" +
                     "Dehydration: \n" +
                     dehydration + " %";
        return txt;
    }

    private string GetWoolText()
    {
        string txt = "Wool collected (kg): " + collectedWool;
        return txt;
    }

    private void ResetValues()
    {
        passedStates =  0;
        collectedWool = 0;
        dehydration = 0.0;
    }

    private State doTransition(State currentState, State nextState)
    {

        passedStates += 1;
        dehydration = (dehydration < MAXDEHYDRATION) ? dehydration += DEHYDRATIONSTEP : dehydration = MAXDEHYDRATION;

        if (nextState.name == END)
        {
            sceneLoader.LoadLeaveScene();
        }

        if (passedStates == MAXSTATERESCUE)
        {
            overrideTextComponent = wait = false;
            Debug.Log("reached passed state counts: " + passedStates);
            var rescue = Resources.Load<State>("States/Rescue");
            return rescue;
        }

        if (dehydration == MAXDEHYDRATION)
        {
            Debug.Log("Exit Dehydration " + dehydration);
            overrideTextComponent = wait = false;
            dehydration = DEADDEHYDRATION;

            //return (State)AssetDatabase.LoadAssetAtPath("Assets/MyGame/States/Dead.Dehydration.asset", typeof(State));
            var deadDehyd = Resources.Load<State>("States/Dead.Dehydration");
            return deadDehyd;

        }

        if (nextState.name == INFOALARM)
        {
            ResetValues();
            Debug.Log("Counters Reseted + " + passedStates + " " + collectedWool + " " + dehydration);
        }

        if (currentState.name != nextState.name)
        {
            wait = false;
            overrideText = "reset";
        }

        if (currentState.name == nextState.name)
        {
            if (nextState.name == KNITDO || nextState.name == FIGHTATTACK || nextState.name == COLLECTDO)
            {
                wait = false;
                overrideText = "reset in do|attack";
            }
            else
            {
                wait = true;
                overrideText = "Yes, waiting is the best option";
            }

        }

        if (nextState.name == INFODONE || nextState.name == COLLECTINFO)
        {
            SetupInfoUI();
            overrideTextComponent = false;
        }

        if (currentState.name == INFOHUMAN && nextState.name == INFODONE)
        {
            overrideTextComponent = true;
            overrideText = "Notification: Crime scene investigation revealed that robots destroyed all water inventories and water sponge warehouses. " + "\n \n" +
                           "Notification: All proper working service robots have to ensure that their godhumans stay alive and do not dry out." + "\n \n" +
                           "Notification: Collect wool and knit water sponges which are able to make water out of air. ";
       
        }

        if (currentState.name == INFOACCIDENT && nextState.name == INFODONE)
        {
            overrideTextComponent = true;
            overrideText = "Magda is a 21 year old woman. She loves salty food and is doing a lot of sports." + "\n" +
                           "Good news, Magda is alive and at this moment she isn't dehydrated." + "\n" +
                           "For knitting wool you visit her in her house. Collect wool and knit enough sponges so that she will " +
                           "survive until rescue is approaching.";

        }

        if (currentState.name == INFODONE && nextState.name == COLLECT)
        {
            overrideTextComponent = false;
        }

        if ((currentState.name == COLLECTINFO || currentState.name == COLLECTDO) && nextState.name == COLLECTDO)
        {
            int nbrWool = RandomState.getrandom.Next(MINWOOLTOCOLLECT, 3);
            collectedWool += nbrWool;
            collectedWool = Clamp(collectedWool, 0, 5);
            Debug.Log("Collected " + nbrWool + "kg wool: current wool count: " + collectedWool);
            return nextState;
        }


        if ((currentState.name == KNITINFO || nextState.name == KNITDO) && nextState.name == KNITDO)
        {
            if (collectedWool >= MINWOOLKNIT)
            {
                collectedWool -= COSTWOOL;
                dehydration -= DRINKWATER;
                Debug.Log("Wool Knitted -2kg + 1L water for magda, current dehydration" + dehydration);

            }
            else
            {
                overrideText = "Sorry, not enough wool for knitting. collect wool";
                overrideTextComponent = true;
                //nextState.SetKnitNotification(/*"Sorry, not enough wool for knitting. collect wool*/");
            }

            Debug.Log("Wolle -2, Wasser +1");
            return nextState;
        }

        if (currentState.name == KNITDO && currentState.name == COLLECTINFO)
        {
            overrideTextComponent = false;
        }

        if (currentState.name == FIGHTDO && (nextState.name == COLLECTINFO || nextState.name == FIGHTDO))
        {

            Debug.Log("wool before Fight in kg: " + collectedWool);
            collectedWool += RandomState.getrandom.Next(0, 3);
            collectedWool = Clamp(collectedWool, 0, 5);
            Debug.Log("wool after Fight in kg: " + collectedWool);

        }

        return nextState;
    }

    private int Clamp(int value, int cmin, int cmax)
    {
        return Math.Max(Math.Min(value, cmax), cmin);
    }


    private void ManageState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            State[] nextStates = actualState.GetNextStates();
            Debug.Log("States size 1");
            if (nextStates.Length < 1)
            {
                return;
            }
            State nextState = nextStates[0];
            actualState = doTransition(actualState, nextState);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            State[] nextStates = actualState.GetNextStates();
            Debug.Log("States size 2");
            if (nextStates.Length < 2)
            {
                return;
            }
            State nextState = nextStates[1];
            actualState = doTransition(actualState, nextState);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            State[] nextStates = actualState.GetNextStates();
            Debug.Log("States size 3");
            if (nextStates.Length < 3)
            {
                return;
            }
            State nextState = nextStates[2];
            actualState = doTransition(actualState, nextState);
        }
        else
        {
            Debug.Log("bin am leben");
        }

        if (wait || overrideTextComponent)
        {
            Debug.Log("in wait " + infoOn);
            if (infoOn)
            {
                textStoryComponent.text = overrideText;
            }
            else
            {
                textIntroComponent.text = overrideText;
            }

        }
        else
        {
            Debug.Log("in wait else" + infoOn);
            if (infoOn)
            {
                textStoryComponent.text = actualState.GetStateStory();
            }
            else
            {
                textIntroComponent.text = actualState.GetStateStory();
            }
        }


        textComponentChoices.text = actualState.GetStateStoryMenue();
        humanStateTxt.text = GetDehydrationText();
        woolStateTxt.text = GetWoolText();
    }
}
