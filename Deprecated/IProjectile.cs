using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{

    public int GetAmmo();
    public void GetScoreKeeper(ScoreKeeper sk);

    public float GetFireRate();
}
