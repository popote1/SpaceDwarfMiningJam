
using UnityEngine;

public static class Metrics {
    public const int GROUNDMOVECOST = 10;
    public const int WALLMOVECOST = 99999;
    public const int STRAINGHTMOVECOST = 10;
    public const int DIAGONALEMOVECOST = 10;
    public const float ACTORMOVEUPDATETIME = 0.5f;
    public const float BURININGTICKDELAY = 1;
    public const int BURININGTICKDAMAGE = 1;

    public enum RESSOURCETYPE {
        None,
        Gaz,
        Petrole,
        Mass,
        Everything
    }

    public static Vector3 BezierPoint(Vector3 pos1, Vector3 pos2, Vector3 key , float t) {
        Vector3 ac = Vector3.Lerp(pos1, key, t);
        Vector3 cb = Vector3.Lerp(key, pos2, t);
        return  Vector3.Lerp(ac, cb, t);
    }
}

