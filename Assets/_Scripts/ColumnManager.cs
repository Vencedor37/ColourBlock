using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class ColumnManager : MonoBehaviour {
  public Camera mainCamera;
  public Rect gameArea;
  public Transform foreground;
  public GameObject type;
  public bool showColumns;
  private float screenWidth;
  private float screenHeight;
  public int numberColumns;
  public Color columnColor;
  public Shader lineShader;
  public float columnLineWidth;
  public float gameAreaHeight;
  public float gameAreaWidth;
  public Texture texture;
  private List<ColumnController> columns;
  private List<LineRenderer> lineRenderers;
  private LineRenderer borderRenderer;
  private LineRenderer[] borderRenderers;
  private float columnWidth;
  public float columnBorderOffset = 0.025f;

	// Use this for initialization
	void Start () {
    screenHeight = 2f * mainCamera.orthographicSize;
    screenWidth = screenHeight * mainCamera.aspect;
    gameAreaHeight = screenHeight * .85f;
    gameAreaWidth = screenWidth * .85f;

    gameArea = new Rect(0 - screenWidth/2 + screenWidth * .075f, 0 - screenHeight/2 + screenHeight * .075f, gameAreaWidth, gameAreaHeight);
    Vector3 scale = new Vector3(gameAreaWidth, gameAreaHeight, 0);
    Vector3 point = gameArea.position;


    point = mainCamera.ScreenToWorldPoint(point);
    foreground.localScale = scale;

    InitialiseColumns();
    InitialiseLineRenderer();
    DrawBorders();
	}

	// Update is called once per frame
	void Update () {
    DrawColumns();
	}


  public bool getShowColumns()
  {
    return showColumns;
  }

  void DrawColumns()
  {
    float yMin = 0 - gameAreaHeight/2;
    float yMax = 0 + gameAreaHeight/2;
    if (showColumns) {
      for (int i = 1; i < columns.Count; i++) {
        columns[i].DrawLeft(yMin, yMax);
      }
    }
  }

  public void CheckInputColumn(Vector3 point)
  {
    for (int i = 0; i < columns.Count; i ++) {
      if (columns[i].IsPointInColumn(point)) {
        Debug.Log("clicked in column: " + i);
      }
    }
  }

  public ColumnController CheckPointColumn(Vector3 point)
  {
    for (int i = 0; i < columns.Count; i ++) {
      if (columns[i].IsPointInColumn(point)) {
        return columns[i];
      }
    }
    return null;
  }

  void InitialiseColumns()
  {
    columns = new List<ColumnController>();
    float xPos = 0 - gameAreaWidth / 2;
    columnWidth = gameAreaWidth / numberColumns;
    for (int i = 0; i < numberColumns; i++) {
      GameObject newObject = (GameObject)Instantiate(type, new Vector3(0, 0, 0), Quaternion.identity);
      newObject.transform.SetParent(GetComponent<Transform>(), false);
      ColumnController column = newObject.GetComponent<ColumnController>();
      column.setLeftX(xPos);
      column.setRightX(xPos + columnWidth);
      column.SetColumnManager(this);
      columns.Add(column);
      xPos += columnWidth;
    }
  }

  void InitialiseLineRenderer()
  {
    borderRenderers = gameObject.GetComponentsInChildren<LineRenderer>();
    for (int i = 0; i < borderRenderers.Length; i++) {
      LineRenderer borderRenderer = borderRenderers[i];
      borderRenderer.SetColors(columnColor, columnColor);
      borderRenderer.material = new Material(lineShader);
      borderRenderer.SetVertexCount(2);
      borderRenderer.useWorldSpace = true;
      borderRenderer.SetWidth(columnLineWidth, columnLineWidth);
      borderRenderer.sortingLayerName = "Background";
    }
  }

  void DrawBorders()
  {
    for (int i = 0; i < borderRenderers.Length; i++) {
      if (i == 0) {
        Vector3 point1 = new Vector3(gameArea.xMin - columnBorderOffset, gameArea.yMin, 1);
        Vector3 point2 = new Vector3(gameArea.xMin - columnBorderOffset, gameArea.yMax, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 1) {
        Vector3 point1 = new Vector3(gameArea.xMin, gameArea.yMax, 1);
        Vector3 point2 = new Vector3(gameArea.xMax, gameArea.yMax, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 2) {
        Vector3 point1 = new Vector3(gameArea.xMax - columnBorderOffset, gameArea.yMax, 1);
        Vector3 point2 = new Vector3(gameArea.xMax - columnBorderOffset, gameArea.yMin, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 3) {
        Vector3 point1 = new Vector3(gameArea.xMax, gameArea.yMin, 1);
        Vector3 point2 = new Vector3(gameArea.xMin, gameArea.yMin, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
    }
  }

  public float getColumnWidth()
  {
    return columnWidth;
  }

  public List<ColumnController> getColumns()
  {
    return columns;
  }

  public float getGameAreaTop()
  {
    return gameArea.yMax;
  }


}
