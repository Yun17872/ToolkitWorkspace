using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygon : MonoBehaviour
{   
    public int sides = 5;
    public float radius = 3;
    public LineRenderer polygonRenderer;

    public int extraSteps = 2;
    public bool isLooped;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
         if(isLooped){
        DrawLooped();
    }else{
     DrawOverLapped();
    }
    }
    void DrawLooped(){

    polygonRenderer.positionCount= sides;
    float TAU = 2*Mathf.PI;

    for (int currentPoint = 0;currentPoint<sides;currentPoint++){
    float currentRadian =((float)currentPoint/sides)*TAU;
    float x = Mathf.Cos(currentRadian)*radius;
    float y = Mathf.Sin(currentRadian)*radius;
    polygonRenderer.SetPosition(currentPoint,new Vector3(x,y,0));


    }
    polygonRenderer.loop = true;
    }

    void DrawOverLapped(){
    DrawLooped();
    polygonRenderer.loop = false;
    polygonRenderer.positionCount += extraSteps;

    int positionCount = polygonRenderer.positionCount;
    for(int i = 0;i<extraSteps;i ++){
        polygonRenderer.SetPosition((positionCount - extraSteps + i),polygonRenderer.GetPosition(i));
    }
    }
}
