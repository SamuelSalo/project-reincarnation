using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
public class IsometricTileCollider : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    public float size;

    // Start is called before the first frame update
    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!polyCollider) polyCollider = GetComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[] { new Vector2(0, size / 2), new Vector2(size, 0), new Vector2(0, -size / 2), new Vector2(-size, 0) };
        polyCollider.points = points;
    }
}
