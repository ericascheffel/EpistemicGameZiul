using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime; // For manipulating Ink files
using System.IO; // For using ReadFile() method

// This file is not an excecutable script, it is only a class that controlls the Global variables

// This file will be called by DialogMannager script
public class DVariablesChecker
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; } // Dictionary keeping all the global variables

    public DVariablesChecker(string GlobalsVarsPath) { // CONSTRUCT METODOT: (Requires a string path for global varibles Ink file) Reads the global variables file and add the values to the ditionary
        string globalContent = File.ReadAllText(GlobalsVarsPath); // Stores the content read from gobal variable's file
        Ink.Compiler InkCompiler = new Ink.Compiler(globalContent); // Instanciates a Ink compiler for globals variable read file
        Story globalVars = InkCompiler.Compile(); // Compiles the Ink file (Need to do through code)

        variables = new Dictionary<string, Ink.Runtime.Object>(); // Initialize the dictionary

        foreach (string name in globalVars.variablesState) { // Loops through all the variables name in global variables
            Ink.Runtime.Object value = globalVars.variablesState.GetVariableWithName(name); // gets the value for each variable name
            variables.Add(name, value); // Adds the variable to the dictionary (Key = name ; Value = value)
            Debug.Log("Initalized var: " + name + " = " + value);
        }
    
    }
    public void StartListening(Story currentgStory) { // LISTENER: Called through DialogMannager, starts Listening 
        VariablesToStory(currentgStory); 
        currentgStory.variablesState.variableChangedEvent += VariableChanged; // Calls VariableChanged metodoth everytime a variable change it's value
    }

    public void StopListening(Story currentStory) { // LISTENER: Called through DialogMannager, stops Listening 
        currentStory.variablesState.variableChangedEvent -= VariableChanged;

    }

    private void VariableChanged (string name, Ink.Runtime.Object value) {
        Debug.Log(name + " changed to: " + value);
        if (variables.ContainsKey(name)) { // Checks if the variable already exists in the dictionary, if yes, change it's value
            variables.Remove(name);
            variables.Add(name, value);

        }
    }

    private void VariablesToStory(Story currentStory) // Makes all the variables inside the dictionary global
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables) {
            currentStory.variablesState.SetGlobal(variable.Key, variable.Value); 

        }
    }


}
