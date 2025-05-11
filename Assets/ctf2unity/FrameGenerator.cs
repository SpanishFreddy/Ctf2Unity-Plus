#if UNITY_EDITOR
using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.Events.Conditions;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ctf2Unity
{
    public class FrameGenerator
    {
        private readonly Ctf2UnityProcessor processor;
        private Frame frame;
        private GameObject runtime;
        private Transform objectInfosObject;
        public Scene scene;
        public string assetPath;
        public FrameHandler frameInf;

        public FrameGenerator(Ctf2UnityProcessor processor)
        {
            this.processor = processor;
        }

        public void Generate(Frame frame)
        {
            this.frame = frame;
            scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            RenderSettings.skybox = null;

            runtime = new GameObject("Runtime");
            objectInfosObject = new GameObject("Object Infos").transform;

            frameInf = runtime.AddComponent<FrameHandler>();
            frameInf.Create(frame);


            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Objects"));
            if (frame.Objects != null && frame.Objects.Count != 0)
            {
                int order = 0;
                var currentLayer = frame.Objects[0].Layer;
                for (int a = 0; a < frame.Objects.Count; a++)
                {
                    var obj = frame.Objects[a];

                    if (currentLayer != obj.Layer)
                    {
                        currentLayer = obj.Layer;
                        order = 0;
                    }

                    GetOrCreateObjectInfo(obj).CreateInstance(new Vector2Int(obj.X, obj.Y), obj.Layer, order);
                    order++;
                }
            }

            if (frame.Events != null && frame.Events.Items != null)
            for (int a = 0; a < frame.Events.Items.Count; a++)
            {
                var item = frame.Events.Items[a];
                ProcessEvent(item);
            }

            assetPath = "Assets\\Frames\\" + frame.Name + ".unity";
            EditorSceneManager.SaveScene(scene, assetPath);
        }

        private void ProcessEvent(EventGroup eg)
        {
            var group = new EventGroupInfo();
            frameInf.events.Add(group);

            var currentConds = new ConditionsGroup();
            group.conditionGroups.Add(currentConds);
            for (int a = 0; a < eg.Conditions.Count; a++)
            {
                var cond = eg.Conditions[a];
                var c = ConditionBase.CreateCondition(cond.ObjectType, cond.Num);

                if (c == null)
                    continue;
                if (c is FakeConditionBase)
                {
                    currentConds = new ConditionsGroup();
                    group.conditionGroups.Add(currentConds);
                    continue;
                }

                if (c is CommonObjectConditionBase com)
                {
                    com.objectInfo = (CommonObjectInfoBase)frameInf.GetObjectInfoByHandle(cond.ObjectInfo);
                }
                for (int b = 0; b < cond.Items.Count; b++)
                {
                    var par = cond.Items[b];
                    c.parameters.Add(EventParameterBase.CreateParam(par.Code, (ParameterCommon)par.Loader)); //adding it even if its null
                }

                currentConds.AddCondition(c);
            }

            for (int a = 0; a < eg.Actions.Count; a++)
            {
                var act = eg.Actions[a];
                var newAct = EventActionBase.CreateAction(act.ObjectType, act.Num);
                if (newAct == null)
                    continue;
                if (newAct is CommonObjectActionBase com)
                {
                    com.objectInfo = (CommonObjectInfoBase)frameInf.GetObjectInfoByHandle(act.ObjectInfo);
                }
                for (int b = 0; b < act.Items.Count; b++)
                {
                    var par = act.Items[b];
                    newAct.parameters.Add(EventParameterBase.CreateParam(par.Code, (ParameterCommon)par.Loader)); //adding it even if its null
                }

                group.actions.Add(newAct);
            }
        }

        private ObjectInfoHolder GetOrCreateObjectInfo(ObjectInstanceReader obj)
        {
            for (int a = 0; a < frameInf.objectInfos.Count; a++)
            {
                var inf = frameInf.objectInfos[a];
                if (obj.ObjectInfoHandle == inf.handle)
                    return inf;
            }

            var frameItem = processor.Frameitems.FromHandle(obj.ObjectInfoHandle);
            var res = ObjectInfoHolder.Create(frameItem, objectInfosObject, processor);
            frameInf.objectInfos.Add(res);
            return res;
        }
    }
}
#endif