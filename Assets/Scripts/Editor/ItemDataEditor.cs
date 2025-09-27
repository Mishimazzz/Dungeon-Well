using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemData itemData = (ItemData)target;

        // 想要的字段
        itemData.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", itemData.prefab, typeof(GameObject), false);
        itemData.probability = EditorGUILayout.FloatField("Probability", itemData.probability);
        itemData.rareLevel = (Rare)EditorGUILayout.EnumPopup("Rare Level", itemData.rareLevel);
        itemData.itemLevel = (Level)EditorGUILayout.EnumPopup("Item Level", itemData.itemLevel);
        itemData.isSeed = (Seed)EditorGUILayout.EnumPopup("Is Seed", itemData.isSeed);

        // 只有 isSeed == Yes 时，才显示额外字段
        if (itemData.isSeed == Seed.Yes)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Seed Options", EditorStyles.boldLabel);

            itemData.harvestPrefab = (GameObject)EditorGUILayout.ObjectField("Harvest Prefab", itemData.harvestPrefab, typeof(GameObject), false);
            itemData.firstPhase = EditorGUILayout.FloatField("FirstPhase", itemData.firstPhase);
            itemData.secondPhase = EditorGUILayout.FloatField("SecondPhase", itemData.secondPhase);
            itemData.thirdPhase = EditorGUILayout.FloatField("ThirdPhase", itemData.thirdPhase);
            itemData.seedManagerScript = (SeedManager)EditorGUILayout.ObjectField(
                "Seed Manager Script",
                itemData.seedManagerScript,
                typeof(SeedManager),
                true   // 允许场景对象（false 就只能拖 asset）
            );
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(itemData);
        }
    }
}
