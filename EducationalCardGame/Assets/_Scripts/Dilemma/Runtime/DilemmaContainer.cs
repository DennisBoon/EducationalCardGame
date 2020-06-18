using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DilemmaContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DilemmaNodeData> DilemmaNodeData = new List<DilemmaNodeData>();
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
}
