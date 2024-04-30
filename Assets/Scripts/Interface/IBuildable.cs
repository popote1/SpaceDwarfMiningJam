
using UnityEngine;

interface IBuildable {
    public abstract bool CanBeBuild(Cell cell);
    public abstract GameObject SelecteBuild(Cell cell);
    public abstract void OnBuild(Cell cell);
}


