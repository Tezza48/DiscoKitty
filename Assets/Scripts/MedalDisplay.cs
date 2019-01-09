using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalDisplay : MonoBehaviour 
{
    public GameObject MedalPrefab;
    public GameObject NoMedalPrefab;

    public void ShowMedals(int numMedals)
    {
        Mathf.Clamp(numMedals, 0, 4);
        if (numMedals == 0)
        {
            Instantiate(NoMedalPrefab, transform);
            Instantiate(NoMedalPrefab, transform);
            Instantiate(NoMedalPrefab, transform);
        }
        switch (numMedals)
        {
            case 0:
                Instantiate(NoMedalPrefab, transform);
                Instantiate(NoMedalPrefab, transform);
                Instantiate(NoMedalPrefab, transform);
                break;
            case 1:
                Instantiate(MedalPrefab, transform);
                Instantiate(NoMedalPrefab, transform);
                Instantiate(NoMedalPrefab, transform);
                break;
            case 2:
                Instantiate(MedalPrefab, transform);
                Instantiate(MedalPrefab, transform);
                Instantiate(NoMedalPrefab, transform);
                break;
            case 3:
                Instantiate(MedalPrefab, transform);
                Instantiate(MedalPrefab, transform);
                Instantiate(MedalPrefab, transform);
                break;
            default:
                break;
        }
    }

}
