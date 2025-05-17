using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;


public class DialogueManager : Singleton<DialogueManager>
{

    private Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
    [SerializeField] private Dialogue[] dialogueTransition; //Dictionaries cannot be visualized in the Inspector, so we use an array to fill it in
    [SerializeField] private Queue<string> sentences = new Queue<string>();
    [SerializeField] private float printingSpeed = 0.1f;
    [SerializeField] private Animator dialogueBoxAnimator;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private UnityEvent onDialogueEnd;
    private Dialogue currentDialogue;


    void Awake()
    {
        //Filling te Dictionary
        foreach (Dialogue dialogue in dialogueTransition)
        {
            dialogues.Add(dialogue.key, dialogue);
        }
    }

    public void StartDialogue(string key)
    {
        dialogueBoxAnimator.SetBool("isOpen", true);
        if (sentences.Count != 0)
        {
            sentences.Clear();
        }

        if (!dialogues.TryGetValue(key, out currentDialogue))
        {
            Console.WriteLine("Unable to get dialogue specified by trigger (key: " + key + ")");
        }
        foreach (string sentence in currentDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            onDialogueEnd.Invoke();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        dialogueBoxAnimator.SetBool("isOpen", false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(printingSpeed);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (sentences.Count != 0 || !currentDialogue.hasOnEndEvent)
                DisplayNextSentence();
            else
                currentDialogue.onEnd.Invoke();
        }
    }
}
