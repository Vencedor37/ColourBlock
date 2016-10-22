using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class ColumnManager : MonoBehaviour {
  public Camera mainCamera;
  public Bounds gameArea;
  public Transform foreground;
  public GameObject type;
  public bool showColumns;
  private float screenWidth;
  private float screenHeight;
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
    gameAreaWidth = screenWidth * .85f;
    columnWidth = gameAreaWidth / Grid.w;
    gameAreaHeight = Grid.h * columnWidth;

    gameArea = foreground.GetComponent<SpriteRenderer>().bounds;

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
    if (showColumns) {
      for (int i = 1; i < columns.Count; i++) {
        columns[i].DrawLeft(gameArea.min.y, gameArea.max.y);
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

  public int CheckPointColumnIndex(Vector3 point)
  {
    for (int i = 0; i < columns.Count; i ++) {
      if (columns[i].IsPointInColumn(point)) {
        return i;
      }
    }
    return -1;
  }

  void InitialiseColumns()
  {
    columns = new List<ColumnController>();
    float xPos = 0 - gameAreaWidth / 2;
    for (int i = 0; i < Grid.w; i++) {
      GameObject newObject = (GameObject)Instantiate(type, new Vector3(0, 0, 0), Quaternion.identity);
      newObject.transform.SetParent(GetComponent<Transform>(), false);
      ColumnController column = newObject.GetComponent<ColumnController>();
      column.setIndex(i);
      column.setLeftX(i);
      column.setRightX(i + 1);
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
        Vector3 point1 = new Vector3(gameArea.min.x, gameArea.min.y, 1);
        Vector3 point2 = new Vector3(gameArea.min.x, gameArea.max.y, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 1) {
        Vector3 point1 = new Vector3(gameArea.min.x, gameArea.max.y, 1);
        Vector3 point2 = new Vector3(gameArea.max.x, gameArea.max.y, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 2) {
        Vector3 point1 = new Vector3(gameArea.max.x, gameArea.max.y, 1);
        Vector3 point2 = new Vector3(gameArea.max.x, gameArea.min.y, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
      if (i == 3) {
        Vector3 point1 = new Vector3(gameArea.max.x, gameArea.min.y, 1);
        Vector3 point2 = new Vector3(gameArea.min.x, gameArea.min.y, 1);
        borderRenderers[i].SetPositions(new Vector3[]{point1, point2});
      }
    }
  }

  public float getColumnWidth()
  {
    return 1;
  }

  public List<ColumnController> getColumns()
  {
    return columns;
  }

  public int getGameAreaTop()
  {
    return (int)gameArea.max.y;
  }


}
