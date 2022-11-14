//=================================================
//
//    Created by jianzhou.yao
//
//=================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace ET
{
    public class PlayableNodeView : GraphNodeView<UnityEngine.Playables.Playable>
    {
        private Label _lblInputCount;
        private Label _lblOutputCount;
        private Label _lblPlayState;
        private Label _lblTime;
        private Label _lblValid;
        private Label _lblDone;
        private Label _lblDuration;

        private ProgressBar _weight;

        // private Label _lblDepth;

        private Label _lblSpeed;

        private List<Port> _inputs = new List<Port>();
        private List<Port> _outputs = new List<Port>();

        public List<Port> Inputs
        {
            get => _inputs;
        }

        public List<Port> Outputs
        {
            get => _outputs;
        }

        public override Port GetPort(Direction direction, int index)
        {
            List<Port> ports = direction == Direction.Input ? _inputs : _outputs;
            return ports[index];
        }

        public PlayableNodeView(UnityEngine.Playables.Playable data) : base(data)
        {
            titleContainer.style.backgroundColor = GetColor();
            style.color = Color.black;

            // _lblDepth = new Label();
            // mainContainer.Add(_lblDepth);

            _lblPlayState = new Label();
            mainContainer.Add(_lblPlayState);
            _lblTime = new Label();
            mainContainer.Add(_lblTime);
            _lblInputCount = new Label();
            mainContainer.Add(_lblInputCount);
            _lblOutputCount = new Label();
            mainContainer.Add(_lblOutputCount);
            _lblSpeed = new Label();
            mainContainer.Add(_lblSpeed);

            _lblValid = new Label();
            mainContainer.Add(_lblValid);
            _lblDone = new Label();
            mainContainer.Add(_lblDone);

            _lblDuration = new Label();
            //mainContainer.Add(_lblDuration);

            _weight = new ProgressBar();
            _weight.name = "weight：";
            _weight.Q<VisualElement>(null, ProgressBar.backgroundUssClassName).style.height = 10;
            mainContainer.Add(_weight);

            for (int i = 0; i < Data.GetInputCount(); i++)
            {
                var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                    typeof(Port));
                port.portName = "input";
                inputContainer.Add(port);
                _inputs.Add(port);
            }

            for (int i = 0; i < Data.GetOutputCount(); i++)
            {
                var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                    typeof(Port));
                port.portName = "output";
                outputContainer.Add(port);
                _outputs.Add(port);
            }

            CreateChildren();

            UpdateView();
        }

        private Color GetColor()
        {
            Type type = Data.GetPlayableType();
            return GetColor(type);
        }

        protected virtual void CreateChildren()
        {
        }

        public override void UpdateView()
        {
            this.title = _data.GetPlayableType().Name + $" Index:{this.SiblingIndex}";

            _lblPlayState.text = $"PlayState:{Data.GetPlayState()}";
            //_lblTime.text = $"Time:{Data.GetTime():F4}";
            //_lblInputCount.text = $"InputCount:{Data.GetInputCount()}";
            //_lblOutputCount.text = $"OutputCount:{Data.GetOutputCount()}";
            _lblSpeed.text = $"Speed:{Data.GetSpeed()}";
            _lblDone.text = $"IsDone:{Data.IsDone()}";
            _lblValid.text = $"IsValid:{Data.IsValid()}";
            //_lblDuration.text = $"Duration:{(float)Data.GetDuration()}";

            try
            {
                if (this.ParentNode is PlayableNodeView parentNode)
                {
                    _weight.title = $"{parentNode.Data.GetInputWeight(this.SiblingIndex):F4}";
                    _weight.value = parentNode.Data.GetInputWeight(this.SiblingIndex) * 100;
                }
            }
            catch (Exception e)
            {
                Log.Info($"{e.Message}");
            }

            //this.title = this.Data.GetPlayableType().Name +$"SiblingIndex:{this.SiblingIndex}";
            // _lblDepth.text = $"Depth:{Depth}";
        }
    }
}