using UnityEngine;
using System.Collections;

public class ColumnController : MonoBehaviour {
  private LineRenderer lineRenderer;
  private ColumnManager columnManager;
  private float leftX;
  private float rightX;
  private bool initialised = false;
  private int index;

	// Use this for initialization
	void Start () {
    InitialiseLineRenderer();

	}

	// Update is called once per frame
	void Update () {

	}

  public void setLeftX(float value)
  {
    leftX = value;
  }

  public void setRightX(float value)
  {
    rightX = value;
  }

  public float getLeftX()
  {
    return leftX;
  }

  public float getRightX()
  {
    return rightX;
  }

  public float getCentreX()
  {
    return (leftX + rightX)/2;
  }

  public void SetColumnManager(ColumnManager value)
  {
    columnManager = value;
  }

  void InitialiseLineRenderer()
  {
    if (!initialised) {
      lineRenderer = gameObject.AddComponent<LineRenderer>();
      lineRenderer.SetColors(columnManager.columnColor, columnManager.columnColor);
      lineRenderer.material = new Material(columnManager.lineShader);
      lineRenderer.SetVertexCount(2);
      lineRenderer.useWorldSpace = true;
      lineRenderer.SetWidth(columnManager.columnLineWidth, columnManager.columnLineWidth);
      lineRenderer.sortingLayerName = "Background";
      initialised = true;
    }
  }

  public void DrawLeft(float bottomY, float topY)
  {
    Vector3 start = new Vector3(leftX - 0.5f, bottomY, 1);
    Vector3 end = new Vector3(leftX - 0.5f, topY, 1);
    Vector3[] points = new Vector3[]{start, end};
    lineRenderer.SetPositions(points);
  }

  public void DrawRight(float bottomY, float topY)
  {
    Vector3 start = new Vector3(rightX, bottomY, 1);
    Vector3 end = new Vector3(rightX, topY, 1);
    Vector3[] points = new Vector3[]{start, end};
    lineRenderer.SetPositions(points);
  }

  public bool IsPointInColumn(Vector3 point)
  {
    point = columnManager.mainCamera.ScreenToWorldPoint(point);
    return (point.x > leftX - 0.5f && point.x < rightX - 0.5f);
  }

  public void setIndex(int value)
  {
    index = value;
  }

  public int getIndex()
  {
    return index;
  }

}
