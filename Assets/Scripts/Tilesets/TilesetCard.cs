using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tileset Card", menuName = "Tileset Card")]
public class TilesetCard : ScriptableObject
{
    public List<EnemyCard> enemyCards = new List<EnemyCard>();
}

[Serializable]
public class EnemyCard
{
    public GameObject entity;
    public int count = 1;
    public int weighting = 1;
    public int minDifficulty = 1;
    public int maxDifficulty = 0;
}

