using System;
using System.Collections.Generic;
using System.Linq;
using UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.VFX.UI
{
    class VFXNodeUI : Node
    {
        public VFXNodeUI()
        {
            clipChildren = false;
            inputContainer.clipChildren = false;
            mainContainer.clipChildren = false;
            leftContainer.clipChildren = false;
            rightContainer.clipChildren = false;
            outputContainer.clipChildren = false;
        }
        public override NodeAnchor InstantiateNodeAnchor(NodeAnchorPresenter presenter)
        {
            if (presenter.direction == Direction.Input)
            {
                return VFXEditableDataAnchor.Create<VFXDataEdgePresenter>(presenter as VFXDataAnchorPresenter);
            }
            else
            {
                return VFXDataAnchor.Create<VFXDataEdgePresenter>(presenter as VFXDataAnchorPresenter);
            }
        }
    }
    class VFXParameterUI : VFXNodeUI
    {
        private TextField m_ExposedName;
        private Toggle m_Exposed;

        public void OnNameChanged()
        {
            var presenter = GetPresenter<VFXParameterPresenter>();

            presenter.exposedName = m_ExposedName.text;
        }


        private void ToggleExposed()
        {
            var presenter = GetPresenter<VFXParameterPresenter>();
            presenter.exposed = !presenter.exposed;
        }

        public VFXParameterUI()
        {
            m_Exposed = new Toggle(ToggleExposed);
            m_ExposedName = new TextField();

            m_ExposedName.onTextChanged += OnNameChanged;
            m_ExposedName.AddToClassList("value");

            VisualElement exposedLabel = new VisualElement();
            exposedLabel.text = "exposed";
            exposedLabel.AddToClassList("label");
            VisualElement exposedNameLabel = new VisualElement();
            exposedNameLabel.text = "name";
            exposedNameLabel.AddToClassList("label");

            VisualContainer exposedContainer = new VisualContainer();
            VisualContainer exposedNameContainer = new VisualContainer();

            exposedContainer.AddChild(exposedLabel);
            exposedContainer.AddChild(m_Exposed);

            exposedContainer.name = "exposedContainer";
            exposedNameContainer.name = "exposedNameContainer";

            exposedNameContainer.AddChild(exposedNameLabel);
            exposedNameContainer.AddChild(m_ExposedName);

            inputContainer.AddChild(exposedNameContainer);
            inputContainer.AddChild(exposedContainer);
        }

        public override void OnDataChanged()
        {
            base.OnDataChanged();
            var presenter = GetPresenter<VFXParameterPresenter>();
            if (presenter == null || presenter.parameter == null)
                return;

            presenter.node.position = presenter.position.position;
            presenter.node.collapsed = !presenter.expanded;
            presenter.parameter.exposed = presenter.exposed;
            presenter.parameter.exposedName = presenter.exposedName;

            m_ExposedName.height = 24.0f;
            m_Exposed.height = 24.0f;
            m_ExposedName.text = presenter.exposedName == null ? "" : presenter.exposedName;
            m_Exposed.on = presenter.exposed;
       }
    }
}