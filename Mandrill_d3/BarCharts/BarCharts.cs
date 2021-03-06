﻿using System.Collections.Generic;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;
using System.Drawing;

namespace D3jsLib.BarChart
{
    public class BarChartModel : ChartModel
    {
        public string BarFill { get; set; }
        public string BarHover { get; set; }
        public string YAxisLabel { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string TickMarksX { get; set; }
        public string Data { get; set; }
        public bool xTextRotation { get; set; }
    }

    public class BarData
    {
        public List<DataPoint1> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class BarStyle : ChartStyle
    {
        public Color BarColor { get; set; }
        public Color BarHoverColor { get; set; }
        public string YAxisLabel { get; set; }
        public int TickMarksX { get; set; }
        public bool xTextRotation { get; set; }
    }

    public class d3BarChart : Chart
    {
        public string CssStyleString { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public BarData Data;
        public BarStyle Style;

        public d3BarChart(BarData data, BarStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            BarChartModel model = new BarChartModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.TickMarksX = this.Style.TickMarksX.ToString();
            model.BarFill = ChartsUtilities.ColorToHexString(this.Style.BarColor);
            model.BarHover = ChartsUtilities.ColorToHexString(this.Style.BarHoverColor);
            model.DivId = "div" + counter.ToString();
            model.xTextRotation = this.Style.xTextRotation;
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Height / 100d);

            if (this.Data.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.Data.Domain.A.ToString();
                model.DomainB = this.Data.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.Data.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempBar" + counter.ToString();
            BarChartModel model = this.ChartModel as BarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.BarCharts.BarChartScript.html", templateName);
            return colString;
        }
    }
}
