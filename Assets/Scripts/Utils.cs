using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Utils
{

    public enum Axis {
        Horizontal,
        Vertical
    }
    public static List<RaycastHit2D> LinecastAlongLine(
        Vector2 start,
        Vector2 end,
        Vector2 direction,
        float length,
        int rayCount,
        LayerMask layers,
        Axis axis,
        Color color,
        float duration = 0
    )
    {
        List<RaycastHit2D> none = new List<RaycastHit2D>();

        if ( length == 0 ) {
            return none;
        }

        foreach (int rayIndex in Enumerable.Range(0, rayCount + 1)) {
            var lineStart = axis == Axis.Horizontal
                ? new Vector2(
                    start.x,
                    start.y + (( end.y - start.y ) / rayCount) * rayIndex
                )
                : new Vector2(
                    start.x + (( end.x - start.x ) / rayCount) * rayIndex,
                    start.y
                );
            var lineEnd = lineStart + direction * length;

            Debug.DrawLine(start, end, Color.white, duration);
            Debug.DrawLine(lineStart, lineEnd, color, duration); 
            RaycastHit2D hit = Physics2D.Linecast(lineStart, lineEnd, layers.value);

            if (hit.collider != null) {
                return new List<RaycastHit2D>() { hit };
            }
        }

        return none;
    }

    public static void DrawCrossOnPoint(Vector2 point, float size, Color color, float duration = 0) {
        Debug.DrawLine(
            new Vector2(point.x - size, point.y - size),
            new Vector2(point.x + size, point.y + size),
            color,
            duration
        );
        Debug.DrawLine(
            new Vector2(point.x - size, point.y + size),
            new Vector2(point.x + size, point.y - size),
            color,
            duration
        );
    }
}