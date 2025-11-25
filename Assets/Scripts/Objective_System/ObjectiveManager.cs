using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class ObjectiveManager : MonoBehaviour
{
    public Objective objective;

    public static ObjectiveManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
