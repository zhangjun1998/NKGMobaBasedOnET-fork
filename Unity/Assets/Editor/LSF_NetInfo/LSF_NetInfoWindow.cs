//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年7月25日 13:09:27
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ET
{
    public class LSF_NetInfoWindow : OdinEditorWindow
    {
        [LabelText("C2GPing值")] public float C2GPing;

        [LabelText("C2MPing值")] public float C2MPing;

        [LabelText("客户端当前FixedUpdate间隔")] public long ClientFixedUpdateInternal;

        [LabelText("客户端当前FixedUpdate帧数")] public float ClientFixedUpdateFrame;

        [LabelText("客户端当前帧")]
        [ProgressBar(nameof(GetCompareMinFrame), nameof(GetCompareMaxFrame), Segmented = true, Height = 30,
            DrawValueLabel = true)]
        public uint ClientCurrentFrame;

        [LabelText("服务端当前帧")]
        [ProgressBar(nameof(GetCompareMinFrame), nameof(GetCompareMaxFrame), Segmented = true, Height = 30,
            DrawValueLabel = true)]
        public uint ServerCurrentFrame;

        private uint GetCompareMinFrame()
        {
            return (uint) Mathf.Clamp(ServerCurrentFrame - 100, 0.0f, Single.NaN);
        }

        private uint GetCompareMaxFrame()
        {
            return ServerCurrentFrame + 100;
        }

        [Command("ETEditor_LSF_NetInfoWindow", "监测状态帧同步网络情况", Category = "ETEditor")]
        private static void OpenWindow()
        {
            var window = GetWindow<LSF_NetInfoWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("监测状态帧同步网络情况");
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                ShowNotification(new GUIContent("请运行游戏并进入战斗以查看状态帧同步状况"));
                return;
            }


            LSF_Component lsfComponent =
                Game.Scene.GetComponent<PlayerComponent>()?.BelongToRoom?.GetComponent<LSF_Component>();
            if (lsfComponent != null)
            {
                if (lsfComponent.FixedUpdate != null)
                {
                    this.ClientFixedUpdateFrame = 1000.0f / lsfComponent.FixedUpdate.TargetElapsedTime.Milliseconds;
                    this.ClientFixedUpdateInternal = lsfComponent.FixedUpdate.TargetElapsedTime.Milliseconds;
                    this.ClientCurrentFrame = lsfComponent.CurrentFrame;
                    this.ServerCurrentFrame = lsfComponent.ServerCurrentFrame;
                }
            }

            PingComponent pingComponent =
                Game.Scene.GetComponent<PlayerComponent>()?.GateSession?.GetComponent<PingComponent>();
            if (pingComponent != null)
            {
                C2GPing = pingComponent.C2GPingValue;
                C2MPing = pingComponent.C2MPingValue;
            }

            Repaint();
        }
    }
}