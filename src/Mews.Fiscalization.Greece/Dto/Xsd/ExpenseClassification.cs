﻿using System;
using System.Xml.Serialization;

namespace Mews.Fiscalization.Greece.Dto.Xsd
{
    [Serializable]
    [XmlType(Namespace = InvoicesDoc.Namespace)]
    public class ExpenseClassification
    {
        [XmlElement(ElementName = "id")]
        public byte SerialNumber { get; set; }

        [XmlIgnore]
        public bool SerialNumberSpecified { get; set; }

        [XmlElement(ElementName = "classificationType")]
        public ExpenseClassificationType ClassificationType { get; set; }

        [XmlElement(ElementName = "classificationCategory")]
        public ExpenseClassificationCategory ClassificationCategory { get; set; }

        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }
    }
}