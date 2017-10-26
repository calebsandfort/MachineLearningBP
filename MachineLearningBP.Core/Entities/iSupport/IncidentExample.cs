using MachineLearningBP.CollectiveIntelligence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities.iSupport
{
    [Table("IncidentExamples")]
    public class IncidentExample : ExampleMinimum<IncidentStatLine, Double, IncidentExampleGenerationInfo>
    {
        public override IncidentStatLine StatLine { get; set; }
        public override int StatLineId { get; set; }

        public override bool PythonIndexOnly
        {
            get { return true; }
        }

        public override void SetData(IncidentExampleGenerationInfo info)
        {
            //Customer - ID - Nominal
            //Company - ID - Nominal
            //Source - int - Nominal
            //Template - ID - Nominal
            //Month - number - Ordinal ?
            //Day of week - number - Ordinal ?
            //Most used Priority - int - Ordinal
            //Most used Category - All levels - ID - Nominal
            //Most used Assignee - ID - Nominal
            //Most used Status - ID - Nominal

            //List<Double> numericData = new List<double>();
            //numericData.Add(info.StatLine.Runtime);
            //numericData.Add(info.StatLine.TheaterCount);
            //this.NumericData = numericData;

            List<Double> ordinalData = new List<double>();
            ordinalData.Add((int)info.StatLine.Month);
            ordinalData.Add((int)info.StatLine.DayOfWeek);
            ordinalData.Add((int)info.StatLine.Priority);
            this.OrdinalData = ordinalData;

            List<String> nominalData = new List<String>();
            nominalData.Add(info.StatLine.Customer);
            nominalData.Add(info.StatLine.Company);
            nominalData.Add(info.StatLine.Source);
            nominalData.Add(info.StatLine.Template);
            nominalData.Add(info.StatLine.Category);
            nominalData.Add(info.StatLine.Assignee);
            nominalData.Add(info.StatLine.Status);
            this.NominalData = nominalData;

            //List<bool> asymmBinaryData = new List<bool>();
            //asymmBinaryData.Add(info.StatLine.Brand.IsMegaBrand());
            //asymmBinaryData.Add(info.StatLine.Brand.IsAnimationBrand());
            //this.AsymmBinaryData = asymmBinaryData;
        }

        public override void SetResult(IncidentExampleGenerationInfo info)
        {
            this.Result = (Double)info.StatLine.TotalTimeWorked;
        }
    }
}
