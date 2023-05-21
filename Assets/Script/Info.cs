using UnityEngine;

[System.Serializable]
public class Info {
    public int index;
    public float[] position;
    public int camera;
}

[System.Serializable]
public class ScreenPointInfo {
    public int index;
    public Vector3 position;
    public int camera;
    public bool active;
}

[System.Serializable]
public class shootBool {
    public bool[] shoot;
}