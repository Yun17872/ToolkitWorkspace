using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 
public class Nav_pro : MonoBehaviour
{
    private NavMeshAgent agent;
 
    public MeshRenderer meshRenderer;//箭头3D对象Quad
    private List<Transform> points = new List<Transform>();//路径点
    private List<MeshRenderer> lines = new List<MeshRenderer>();//显示的路径
    private Vector3[] path;
    public float xscale = 1f;//缩放比例
    public float yscale = 1f;
 
 
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //箭头宽度缩放值
        xscale = meshRenderer.transform.localScale.x;
        //箭头长度缩放值
        yscale = meshRenderer.transform.localScale.y;
        meshRenderer.gameObject.SetActive(false);
    }
 
    [System.Obsolete]
    private void Update()
    {
 
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100))
            {
                HidePath();
                Vector3 point = hit.point;
                agent.destination = hit.point;
                path = agent.path.corners;
                //线段整体y轴加高，使轨迹悬浮
                for (int i = 0; i < path.Length; i++)
                {
                    path[i] = path[i] + new Vector3(0, 0.01f, 0);
                }
                //绘制路径
                DrawPath();
            }
        }
    }
    //画路径
    public void DrawPath()
    {
        if (path == null || path.Length <= 1)
            return;
        for (int i = 0; i < path.Length - 1; i++)
        {
            DrawLine(path[i], path[i + 1], i);
        }
    }
 
 
    //隐藏路径
    public void HidePath()
    {
        for (int i = 0; i < lines.Count; i++)
            lines[i].gameObject.SetActive(false);
    }
 
     //画路径
    private void DrawLine(Vector3 start, Vector3 end, int index)
    {
        MeshRenderer mr;
        if (index >= lines.Count)
        {
            mr = Instantiate(meshRenderer);
            lines.Add(mr);
        }
        else
        {
            mr = lines[index];
        }
 
        var tran = mr.transform;
        var length = Vector3.Distance(start, end);
        tran.localScale = new Vector3(xscale, length, 1);
        tran.position = (start + end) / 2;
        //指向
        tran.LookAt(start);
        //旋转偏移
        tran.Rotate(90, 0, 0);
        mr.material.mainTextureScale = new Vector2(1, length * yscale);
        mr.gameObject.SetActive(true);
    }
 
}
