using UnityEngine;

namespace ET
{
    public class B2S_DebuggerComponentSystems
    {
        public class B2S_DebuggerComponentAwakeSystem : AwakeSystem<B2S_DebuggerComponent>
        {
            public override void Awake(B2S_DebuggerComponent self)
            {
                GameObject gameObject = new GameObject($"B2S_DebuggerComponent----{self.GetParent<Unit>().Id}");
                gameObject.transform.SetParent(self.GetParent<Unit>().GetComponent<GameObjectComponent>()
                    .GameObject.transform);
                gameObject.transform.localPosition = Vector3.zero;
                self.GoSupportor = gameObject;
            }
        }

        public class B2S_DebuggerComponentFixedUpdate : FixedUpdateSystem<B2S_DebuggerComponent>
        {
            public override void FixedUpdate(B2S_DebuggerComponent self)
            {
                foreach (var tobeRemovedProcessor in self.TobeRemovedProcessors)
                {
                    UnityEngine.Object.Destroy(self.AllLinerRendersDic[tobeRemovedProcessor]);
                    self.AllLinerRendersDic.Remove(tobeRemovedProcessor);
                    self.AllVexs.Remove(tobeRemovedProcessor);
                }

                self.TobeRemovedProcessors.Clear();

                foreach (var debuggerProcessor in self.AllLinerRendersDic)
                {
                    if (debuggerProcessor.Key.IsDisposed)
                    {
                        self.TobeRemovedProcessors.Add(debuggerProcessor.Key);
                    }
                    else
                    {
                        self.RefreshBox2dDebugInfo(debuggerProcessor.Key);
                    }
                }
            }
        }
    }
}