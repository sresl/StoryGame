using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject{

    //private static readonly System.Random getrandom = new System.Random(123);

    [TextArea(10,14)][SerializeField] string storyText;
    [TextArea(5, 14)] [SerializeField] string storyNextchoices;
    public State[] nextStates;
    public State[] randomStates;
    private readonly double probability = 0.3;

    public string GetStateStory()
    {
        return storyText;
    }

    public string GetStateStoryMenue()
    {
        return storyNextchoices;
    }

    //Fügt die Arrays nextStates und RandomStates zusammen:
    //Es wird die Auswahl (Random) aus RandomStates genommen und an den 
    //Anfang (d.h. Stelle 0) von Next States gestellt. 
    //Dieses neu zusammengestellte Array wird dann returned. 
    public State[] GetNextStates()
    {
        if (randomStates.Length == 0)
        {
            return nextStates; 
        }

        State[] extendedArray = new State[nextStates.Length + 1];

        int rnd = PickBiasedRandomState(0, 0, randomStates.Length, probability);
        extendedArray[0] = randomStates[rnd];
        for (int i = 0; i < nextStates.Length; i++)
        {
            extendedArray[i + 1] = nextStates[i];
        }

        return extendedArray;
    }

    //Wählt den prefered index (State) aus den Random States aus.
    int PickBiasedRandomState(int idxPrefered, int randomMin, int randomMax, double probabilityPrefered)
    {
        //return getrandom.NextDouble() < 0.8 ? idxPrefered : getrandom.Next(idxMin, idxMax);
     
        if (RandomState.getrandom.NextDouble() < probabilityPrefered)
        {
            Debug.Log("Current Probability < " + probabilityPrefered);
            return idxPrefered;
        }
        Debug.Log("Current Probabilty > " + probabilityPrefered);
        Debug.Log(randomMin +", "+ randomMax);
        return RandomState.getrandom.Next(randomMin, randomMax);
    }

}
