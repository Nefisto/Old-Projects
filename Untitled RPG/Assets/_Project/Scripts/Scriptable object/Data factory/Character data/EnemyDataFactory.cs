using UnityEngine;

[CreateAssetMenu(fileName = "Enemy data", menuName = EditorConstants.MenuAssets.ENEMY_MENU + "Enemy data", order = 0)]
public class EnemyDataFactory : ScriptableObjectFactory<EnemyData>
{
    public override EnemyData GetInstance() => base.GetInstance();
}