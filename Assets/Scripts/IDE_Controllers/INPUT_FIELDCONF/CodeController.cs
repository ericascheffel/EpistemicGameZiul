using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeController : MonoBehaviour
{
    [SerializeField] private GameObject mainInput;
    [SerializeField] private TextMeshProUGUI codeLines;
    [SerializeField] private TextMeshProUGUI lineCounter;
    void Start()
    {
        
        mainInput.gameObject.GetComponent<TMPro.TMP_InputField>().lineType = TMP_InputField.LineType.MultiLineNewline;
        
        UpdateLines();
    }
    private void Update()
    {
        UpdateLines();
    }

    public void UpdateLines() {
        int LineCount = codeLines.textInfo.lineCount;
        
        codeLines.text = "1";
        string newCount = "1";
        for (int i = 2; i < LineCount+1; i++) {
            newCount += $"\n{i}";
        }
        
        lineCounter.text = newCount;

    
    }
}
