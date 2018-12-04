using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMechanic : MonoBehaviour
{
    //private static readonly System.Random getrandom = new System.Random(123);


    //For testing and debug:
    bool[] alltrue = new bool[5] { true, true, true, true, true };
    
    //States
    public State startingState;

    private State actualState;
    private int passedStates;
    private bool wait, overrideTextComponent;
    private bool infoOn;
    private bool SadChildLeft;
    private bool HappyChildRight;


    //Loading scene
    public GameObject loadSceneObject;

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
    public Sprite SadChildRightIMG;
    public Image SadChildLeftIMG;
    public Image HappyChildRightIMG;
    public Image HappyChildLeftIMG;
    
    private string overrideText;

    //Story relevant
    private bool fillBasketAtOnce = false;
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
        textIntroComponent.text   = actualState.GetStateStory();
        textComponentChoices.text = actualState.GetStateStoryMenue();

        ResetValues();

        //statesUntilRescue = 30;
        wait = false;
        Debug.Log("Enter");

        SetupIntroUI();

        bool SadChildLeft = boolGen.NextBoolean();
        Debug.Log("SadChildLeft is "+SadChildLeft);
    }

    private bool containsBadApple(bool[] appleBasket)
    {
        foreach (bool goodApple in appleBasket)
        {
            if (!goodApple)
            {
                //Sobald der erste schlechte Apfel gefunden wird gehe aus der Methode raus
                //und return true, d.h. es wurde mind. ein schlechter Apfel gefunden. 
                return true;
            }
        }

        //alle Äpfel sind gut, darum wird false returned
        return false; 
    }

    private bool[] getFiveApples()
    {
        //Gibt ein bool array mit 5 Werten zurück.
        //True der Apfel ist gut
        //False der Apfel ist schlecht
        bool[] appleBasket = new bool[5];

        for (int i = 0; i < 5; i++)
        {
            appleBasket[i] = boolGen.NextBoolean();
        }
        return appleBasket;
    }



    // Update is called once per frame
    void Update()
    {
        ManageState();

        if(SadChildLeft == true)
        {
            HappyChildRight = true;
        }

        if (SadChildLeft == false)
        {
            HappyChildRight = false;
        }





    }

    private void ResetValues()
    {
        passedStates = 0;
        hunger = 0.0;
    }

    private State doTransition(State currentState, State nextState)
    {

        passedStates += 1;  



        if (currentState.name == "ApfelSuchen" && nextState.name == "RandomKinderHelfen")
        {
            Debug.Log("War in ApfelSuchen und habe nur gute Äpfel");

        }

        if (currentState.name == "ApfelSuchen" && nextState.name == "RandomApfelSchlecht")
        {
            Debug.Log("War in ApfelSuchen und habe schlechte Äpfel");

        }

        return nextState;
    }

    private void ManageState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            State[] nextStates = actualState.GetNextStates();
            Debug.Log("Key Press 1 - States size: " + nextStates.Length);
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
            Debug.Log("Key Press 2 - States size: " + nextStates.Length);
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
            Debug.Log("Key Press 3 - States size: " + nextStates.Length);
            if (nextStates.Length < 3)
            {
                return;
            }
            State nextState = nextStates[2];
            actualState = doTransition(actualState, nextState);
        }
        else
        {
            //Debug.Log("bin am leben");
        }

        if (wait || overrideTextComponent)
        {
            //Debug.Log("in wait " + infoOn);
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
            //Debug.Log("in wait else" + infoOn);
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

