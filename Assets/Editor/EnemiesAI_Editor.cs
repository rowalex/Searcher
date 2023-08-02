using PlasticGui.WorkspaceWindow.PendingChanges;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemiesAI))]
public class EnemiesAI_Editor : Editor
{

    private void OnSceneGUI()
    {
        EnemiesAI AI = (EnemiesAI)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(AI.transform.position, AI.transform.up, AI.transform.forward, 360, AI.chargeDistance);
        Handles.DrawWireArc(AI.transform.position, AI.transform.right, AI.transform.up, 360, AI.chargeDistance);

        Handles.color = Color.yellow;
        Handles.DrawWireArc(AI.transform.position, AI.transform.up, AI.transform.forward, 360, AI.checkDistance);
        Handles.DrawWireArc(AI.transform.position, AI.transform.right, AI.transform.up, 360, AI.checkDistance);

    }

}
