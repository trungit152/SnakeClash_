using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class demo_1
{
   public int id;
}

public class demo : MonoBehaviour
{
    public List<demo_1> demo_1s = new List<demo_1>();
    // Start is called before the first frame update
    void Start()
    {
        demo_1s = demo_1s.OrderBy(o => o.id).ToList(); // min den max
        demo_1s = demo_1s.OrderByDescending(o => o.id).ToList(); //max den min
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
