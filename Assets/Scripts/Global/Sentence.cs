using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{

    public float time;
    public string sentence;
    public string name;

    public Sentence(float _time, string _sentence, string _name)
    {
        time = _time;
        sentence = _sentence;
        name = _name;
    }
}
