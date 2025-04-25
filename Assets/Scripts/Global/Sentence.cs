using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{

    public float time;
    public string sentence;
    public string name;
    public Sprite image;

    public Sentence(float _time, string _sentence, string _name, Sprite _image)
    {
        time = _time;
        sentence = _sentence;
        name = _name;
        image = _image;
    }
}
