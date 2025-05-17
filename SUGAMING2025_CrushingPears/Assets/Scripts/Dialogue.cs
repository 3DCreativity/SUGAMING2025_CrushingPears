using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialogue
{
    public string key;
    [TextArea(2, 10)]
    public string[] sentences;

    public bool hasOnEndEvent;
    public UnityEvent onEnd;
}
