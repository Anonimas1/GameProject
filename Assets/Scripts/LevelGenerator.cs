using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> wallModules;
    
    [SerializeField]
    private List<GameObject> modules;

    [Range(0, 1f)]
    public float fillPercentage = 0.5f;

    [Range(1, 10)]
    [SerializeField]
    private int smoothingIterations = 2;

    [Min(0)]
    [SerializeField]
    private int moduleSize = 10;
    
    [Range(0, 500)]
    [SerializeField]
    private int mapSize = 10;
    
    [Range(0, 500)]
    [SerializeField]
    private int wallifyStep = 1;

    [Range(2, 50)]
    [SerializeField]
    private int wallRange = 2;

    public List<GameObject> WallModules => wallModules;

    public List<GameObject> Modules => modules;

    public float FillPercentage => fillPercentage;

    public int ModuleSize => moduleSize;

    public int MapSize => mapSize;

    public int SmoothingIterations => smoothingIterations;

    public int WallifyStep => wallifyStep;
    public int WallRange => wallRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
