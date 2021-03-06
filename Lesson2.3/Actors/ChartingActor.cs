﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;

namespace Lesson.Actors
{
    class ChartingActor : ReceiveActor
    {
        #region Messages

        public class InitializeChart
        {
            public InitializeChart(Dictionary<string, Series> initialSeries)
            {
                InitialSeries = initialSeries;
            }

            public Dictionary<string, Series> InitialSeries { get; }
        }

        public class AddSeries
        {
            public AddSeries(Series series)
            {
                Series = series;
            }

            public Series Series { get; }
        }

        public class RemoveSeries
        {
            public RemoveSeries(string series)
            {
                Series = series;
            }

            public string Series { get; }
        }

        #endregion

        private readonly Chart _chart;
        private Dictionary<string, Series> _seriesIndex;

        public const int MaxPoints = 250;
        private int _xPosCounter = 0;

        public ChartingActor(Chart chart) : this(chart, new Dictionary<string, Series>())
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;

            Receive<InitializeChart>(ic => HandleInitialize(ic));
            Receive<AddSeries>(addSeries => HandleAddSeries(addSeries));
            Receive<RemoveSeries>(msg => HandleRemoveSeries(msg));
            Receive<Metric>(metric => HandleMetrics(metric));
        }

        #region Individual Message Type Handlers

        private void HandleAddSeries(AddSeries series)
        {
            var seriesName = series.Series.Name;
            if (!string.IsNullOrEmpty(seriesName) && !_seriesIndex.ContainsKey(seriesName))
            {
                _seriesIndex.Add(seriesName, series.Series);
                _chart.Series.Add(series.Series);

                SetChartBoundaries();
            }
        }

        private void HandleRemoveSeries(RemoveSeries msg)
        {
            if (!string.IsNullOrEmpty(msg.Series) && _seriesIndex.ContainsKey(msg.Series))
            {
                var seriesToRemove = _seriesIndex[msg.Series];
                _seriesIndex.Remove(msg.Series);
                _chart.Series.Remove(seriesToRemove);

                SetChartBoundaries();
            }
        }

        private void HandleInitialize(InitializeChart ic)
        {
            if (ic.InitialSeries != null)
            {
                //swap the two series out
                _seriesIndex = ic.InitialSeries;
            }

            //delete any existing series
            _chart.Series.Clear();

            var area = _chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            //attempt to render the initial chart
            if (_seriesIndex.Any())
            {
                foreach (var series in _seriesIndex)
                {
                    //force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    _chart.Series.Add(series.Value);
                }
            }

            SetChartBoundaries();
        }

        private void HandleMetrics(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && _seriesIndex.ContainsKey(metric.Series))
            {
                var series = _seriesIndex[metric.Series];
                series.Points.AddXY(_xPosCounter++, metric.CounterValue);

                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }

                SetChartBoundaries();
            }
        }

        #endregion

        private void SetChartBoundaries()
        {
            var allPoints = _seriesIndex.Values.SelectMany(series => series.Points).ToList();
            List<double> yValues = allPoints.SelectMany(point => point.YValues).ToList();

            double maxAxisX = _xPosCounter;
            double minAxisX = maxAxisX - MaxPoints;
            double maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1d;
            double minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0d;

            if (allPoints.Count > 2)
            {
                var area = _chart.ChartAreas[0];
                area.AxisX.Minimum = minAxisX;
                area.AxisX.Maximum = maxAxisX;
                area.AxisY.Minimum = minAxisY;
                area.AxisY.Maximum = maxAxisY;
            }
        }
    }
}
