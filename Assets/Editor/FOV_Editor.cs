using PlasticGui.WorkspaceWindow.PendingChanges;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemiesFOV))]
public class FOV_Editor : Editor
{

    private void OnSceneGUI()
    {
        EnemiesFOV fov = (EnemiesFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, fov.transform.up, fov.transform.forward, 360, fov.viewRadius);
        Handles.DrawWireArc(fov.transform.position, fov.transform.right, fov.transform.up, 360, fov.viewRadius);

        Vector3 viewAngle01 = Quaternion.AngleAxis(fov.viewAngle / 2, fov.transform.up) * fov.transform.forward;
        Vector3 viewAngle02 = Quaternion.AngleAxis(fov.viewAngle / 2, -fov.transform.up) * fov.transform.forward;
        Vector3 viewAngle03 = Quaternion.AngleAxis(fov.viewAngle / 2, fov.transform.right) * fov.transform.forward;
        Vector3 viewAngle04 = Quaternion.AngleAxis(fov.viewAngle / 2, -fov.transform.right) * fov.transform.forward;

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle03 * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle04 * fov.viewRadius);

    }

}
