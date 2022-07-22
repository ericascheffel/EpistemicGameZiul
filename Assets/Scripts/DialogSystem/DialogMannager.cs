using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For manipulating Text Mesh Pro elements
using Ink.Runtime; // For manipulating Ink files
//using Ink.UnityIntegration; For manipulating Ink files

public class DialogMannager : MonoBehaviour
{
    [Header("Dialog box UI")]

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices; // array that has all the SCENE options buttons
    [SerializeField] private TextMeshProUGUI[] choicesTxt; // Array that keeps the options texts

    [Header("Global Variables")]
    [SerializeField] private InkFile globalsInkfile; // The scene's global variables file needs to be hooked up in Unity's inspector 

    private Story thisStory; // object from ink.Runtime, it keeps the dialog file
    private bool isTyping;
    private string currentScentence;
    public bool playingDialog { get; private set; } // Can be accessed through get method by other scripts

    [SerializeField] private float typingSpeed = 0.2f;

    // Creating a singleton class (A class that is static and needs to be unique in a scene)
    private static DialogMannager instance;

    private DVariablesChecker DVariables;

    private void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("There are more than one instace of Dialog managers on this scene!");
        }
        instance = this;

        DVariables = new DVariablesChecker(globalsInkfile.filePath); // Initializes a DvariablesChecker passing the hooked global vars file path to the constructor
    }

    public static DialogMannager GetInstance() { // Used by other scripts to accesses information from this singleton class
        return instance;
    }

    private void Start()
    {
        dialogBox.SetActive(false);
        playingDialog = false;
        isTyping = false;

        choicesTxt = new TextMeshProUGUI[choices.Length]; // starting the choice text array with the same lenght as the number of options avaible in the scene
        
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesTxt[index] = choice.GetComponentInChildren<TextMeshProUGUI>(); // getting all the text spaces from all options in the scene
            index++;
        }
        
    }

    private void Update()
    {
        if (!playingDialog)
        {
            return;
        }

        else {
            if (InteractionMannager.GetInstance().Interaction()) { // checks for the interaction button input
                InteractionMannager.GetInstance().CloseInteraction(); // closes the interaction event
                if (!isTyping)
                {
                    StartCoroutine(DialogUpdate());  // go to next dialog line, if possible
                }
                
                else if (isTyping)
                {
                    isTyping = false;
                    dialogText.text = currentScentence;
                }

            }
        }
    }

    public void TriggerDialog(TextAsset inkJson) { // starts the dialog

        thisStory = new Story(inkJson.text);
        dialogBox.SetActive(true);
        playingDialog = true;

        DVariables.StartListening(thisStory); // Calls the DVAriablesChecker changes lister 

        StartCoroutine(DialogUpdate());
        
    }

    public IEnumerator DialogUpdate() { // updates to the next dialog line or closes the intaration if it's over

        if (thisStory.canContinue)
        {
            
            isTyping = true;
            dialogText.text = "";
            currentScentence = thisStory.Continue();

            DisplayChoices();

            foreach (char letter in currentScentence)
            {
                if (isTyping)
                {
                    dialogText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            isTyping = false;
        }

        else {
            CloseDialog();
        }
    }

    private void CloseDialog() { // closes the dialog box and resets the dialog text

        dialogBox.SetActive(false);
        playingDialog = false;
        dialogText.text = "";

        DVariables.StopListening(thisStory); // Stops the DVAriablesChecker from listening to changes
    }

    private void DisplayChoices() { // shows all the options and sets their texts, also hides the unused obejects
        List<Choice> currentChoices = thisStory.currentChoices;

        if (currentChoices.Count > choices.Length) {
            Debug.LogError("There are more choices than UI spaces avaible: " + (currentChoices.Count - choices.Length));
        }

        int index = 0;
        foreach (Choice choice in currentChoices) {
            
            choices[index].gameObject.SetActive(true);
            choicesTxt[index].text = choice.text;

            index++;
        }

        for (int i = index; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
        
    }

    public void MakeChoice(int choiceIndex) { // event to make a choice and update the dialog
        thisStory.ChooseChoiceIndex(choiceIndex);
        StartCoroutine(DialogUpdate());
    }

    public Ink.Runtime.Object GetVarState(string varName) { // used by other scripts to get a global variable's current state
        Ink.Runtime.Object varValue = null;
        DVariables.variables.TryGetValue(varName, out varValue);
        if (varValue == null) {
            Debug.LogWarning("The variable " + varName + " was found to be null!");
        }

        return varValue;

    }

    public void SetVarState(string varName, Ink.Runtime.Object varValue) { // used by other scripts to set a global variable's state
        if (DVariables.variables.ContainsKey(varName))
        {
            DVariables.variables.Remove(varName);
            DVariables.variables.Add(varName, varValue);
        }
        else
        {
            Debug.LogWarning("Tried to update variable that wasn't initialized by globals.ink: " + varName);
        }
    }
}
