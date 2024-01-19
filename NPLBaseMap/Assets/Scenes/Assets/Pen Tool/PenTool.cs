using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenTool : MonoBehaviour
{
    [Header("Pen Canvas")]
    [SerializeField] private PenCanvas penCanvas;

    [Header("Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform lineParent;
    private LineController currentLine;

    [Header("Colors")]
    [SerializeField] private Color activeColor;
    [SerializeField] private Color normalColor;

    [Header("Loop Toggle")]
    [SerializeField] Image loopToggle;
    [SerializeField] Sprite loopSprite;
    [SerializeField] Sprite unloopSprite;


 
    private void Start() {
        penCanvas.OnPenCanvasLeftClickEvent += AddDot;
        penCanvas.OnPenCanvasRightClickEvent += EndCurrentLine;
    }

    public void ToggleLoop() {
        if (currentLine != null) {

            currentLine.ToggleLoop();
            loopToggle.sprite = (currentLine.isLooped()) ? unloopSprite : loopSprite;
        }
    }

    private void AddDot() {
        if (currentLine == null) {
            LineController lineController = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
            SetCurrentLine(lineController);
        }

        DotController dot = Instantiate(dotPrefab, GetMousePosition(), Quaternion.identity, dotParent).GetComponent<DotController>();
        dot.OnDragEvent += MoveDot;
        dot.OnRightClickEvent += RemoveDot;
        dot.OnLeftClickEvent += SetCurrentLine;

        currentLine.AddDot(dot);
    }

    private void RemoveDot(DotController dot) {
        dot.line.SplitPointsAtIndex(dot.index, out List<DotController> before, out List<DotController> after);

        Destroy(dot.line.gameObject);
        Destroy(dot.gameObject);

        LineController beforeLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
        for (int i = 0; i < before.Count; i++) {
            beforeLine.AddDot(before[i]);
        }

        LineController afterLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
        for (int i = 0; i < after.Count; i++) {
            afterLine.AddDot(after[i]);
        }
    }

    private void EndCurrentLine() {
        if (currentLine != null) {
            currentLine.SetColor(normalColor);
            loopToggle.enabled = false;
            currentLine = null;
        }
    }

    private void SetCurrentLine(LineController newLine) {
        EndCurrentLine();

        currentLine = newLine;
        currentLine.SetColor(activeColor);

        loopToggle.enabled = true;
        loopToggle.sprite = (currentLine.isLooped()) ? unloopSprite : loopSprite;
    }

    private void MoveDot(DotController dot) {
        dot.transform.position = GetMousePosition();
    }

    private Vector3 GetMousePosition() {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.z = 0;

        return worldMousePosition;
    }
}
