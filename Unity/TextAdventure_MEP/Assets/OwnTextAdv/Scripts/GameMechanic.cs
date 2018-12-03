using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMechanic : MonoBehaviour
{
    //private static readonly System.Random getrandom = new System.Random(123);

    //States
    public State startingState;

    private State actualState;
    private int passedStates;
    private bool wait, overrideTextComponent;
    private bool infoOn;


    //Loading scene
    private SceneLoader sceneLoader;

    //UI elements
    public Text textIntroComponent;
    public Text textStoryComponent;
    public Text textComponentChoices;
    public Image introBG;
    public Image storyBG;
    public Image storyMenueBG;
    public Image hungerStateBG;
    public Image appleStateBG;
    public Text appleStateTxt;
    public Text hungerStateTxt;
    public GameObject loadSceneObject;

    private string overrideText;

    //Story relevant
    private bool RandomApfelGut = false;
    private double hunger;
    private BoolGenerator boolGen = new BoolGenerator();



    private void SetupIntroUI()
    {
        introBG.enabled = textIntroComponent.enabled = true;
        storyMenueBG.enabled = textComponentChoices.enabled = true;

        storyBG.enabled = textStoryComponent.enabled = false;
        hungerStateBG.enabled = hungerStateTxt.enabled = false;
        appleStateBG.enabled = appleStateTxt.enabled = false;
        infoOn = false;
    }

    private void SetupInfoUI()
    {
        introBG.enabled = textIntroComponent.enabled = false;
        storyMenueBG.enabled = textComponentChoices.enabled = true;

        storyBG.enabled = textStoryComponent.enabled = true;
        hungerStateBG.enabled = hungerStateTxt.enabled = true;
        appleStateBG.enabled = appleStateTxt.enabled = true;
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


        bool RandomApfelGut = boolGen.NextBoolean();
        if (RandomApfelGut == true)
        {
            Debug.Log("RandomApfelGut is true");
        }

        if (RandomApfelGut == false)
        {
            Debug.Log("RandomApfelGut is false");
        }


    }



    // Update is called once per frame
    void Update()
    {
        ManageState();

    }

    private void ResetValues()
    {
        passedStates = 0;
        hunger = 0.0;
    }

    private State doTransition(State currentState, State nextState)
    {

        passedStates += 1;



        if (currentState.name == "RandomApfel" && RandomApfelGut)
        {
            //anzeigen dass nur gute und zu kinder Helgen gehen

        }

        if (currentState.name == "RandomApfel" && !RandomApfelGut)
        {


        }

        return nextState;
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
            Debug.Log("Pressed 1");
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

    }
}

