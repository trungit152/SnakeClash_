using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UISnakeMove : MonoBehaviour
{
    private List<Vector3> positionHistory;
    private List<Quaternion> angleHistory;
    public int gap = 12;
    public int gapAngle = 8;
    public float movementSpeed;
    [SerializeField] private List<GameObject> bodyParts;
    void Start()
    {
        gap = 16;
        gapAngle = 8;
        positionHistory = new List<Vector3>();
        angleHistory = new List<Quaternion>();
        movementSpeed =10f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        MoveBody();
    }
    private void MoveBody()
    {
        positionHistory.Insert(0, gameObject.transform.position);
        angleHistory.Insert(0, gameObject.transform.rotation);
        if (positionHistory.Count > 150)
        {
            positionHistory.Remove(positionHistory[positionHistory.Count - 1]);
            angleHistory.Remove(angleHistory[angleHistory.Count - 1]);
        }
        int i = 0;
        foreach (var body in bodyParts)
        {
            if (body != null)
            {
                Vector3 point = positionHistory[Mathf.Clamp(i * gap, 0, positionHistory.Count - 1)];
                Quaternion angle = angleHistory[Mathf.Clamp(i * gapAngle, 0, angleHistory.Count - 1)];
                Vector3 moveDirection =FreezeYPos(point, body.transform.position.y) - body.transform.position;
                body.transform.rotation = angle;
                body.transform.position = FreezeYPos(point, body.transform.position.y);
                i++;
            }
        }
    }
    private Vector3 FreezeYPos(Vector3 pos, float y)
    {
        Vector3 newPos = new Vector3(pos.x, y, pos.z);
        return newPos;
    }
}
