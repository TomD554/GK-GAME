using System;
using System.IO;
using System.Linq;
using Unity.Services.Leaderboards.Editor.Authoring.Deployment;
using Unity.Services.Leaderboards.Editor.Authoring.Model;
using Unity.Services.Leaderboards.Editor.Authoring.Shared.Analytics;
using Unity.Services.Leaderboards.Editor.Authoring.Shared.UI.DeploymentConfigInspectorFooter;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Unity.Services.Leaderboards.Editor.Authoring.UI
{
    [CustomEditor(typeof(LeaderboardConfigAsset))]
    class LeaderboardConfigAssetInspector : UnityEditor.Editor
    {
        const int k_MaxLines = 75;
        const string k_Uxml = "Packages/com.unity.services.leaderboards/Editor/Authoring/UI/Assets/LeaderboardConfigAssetInspector.uxml";

        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(k_Uxml);

            visualTree.CloneTree(myInspector);
            ShowResourceBody(myInspector);
            var leaderboard = target as LeaderboardConfigAsset;

            var deploymentConfigInspectorFooter = myInspector.Q<DeploymentConfigInspectorFooter>();
            deploymentConfigInspectorFooter.BindGUI(
                AssetDatabase.GetAssetPath(target),
                LeaderboardAuthoringServices.Instance.GetService<ICommonAnalytics>(),
                "leaderboards");
            // cannot null-coalesce, because its a unity object
            if (leaderboard != null && leaderboard.Model?.Id != null)
            {
                deploymentConfigInspectorFooter.DashboardLinkUrlGetter = () => LeaderboardAuthoringServices.Instance
                    .GetService<ILeaderboardsDashboardUrlResolver>()
                    .Leaderboard(leaderboard.Model?.Id);
            }

            return myInspector;
        }

        void ShowResourceBody(VisualElement myInspector)
        {
            var body = myInspector.Q<TextField>();
            if (targets.Length == 1)
            {
                body.visible = true;
                body.value = ReadResourceBody(targets[0]);
            }
            else
            {
                body.visible = false;
            }
        }

        static string ReadResourceBody(Object resource)
        {
            var path = AssetDatabase.GetAssetPath(resource);
            var lines = File.ReadLines(path).Take(k_MaxLines).ToList();
            if (lines.Count == k_MaxLines)
            {
                lines.Add("...");
            }
            return string.Join(Environment.NewLine, lines);
        }
    }
}
