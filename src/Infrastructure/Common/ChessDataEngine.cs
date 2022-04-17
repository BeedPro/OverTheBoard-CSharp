using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Common
{
    public class ChessDataEngine
    {
        private readonly List<ChessChartData> _data;
        private ChartType Type;
        List<ChessChartData> Datas = new List<ChessChartData>();
        public ChessDataEngine(List<ChessChartData> data)
        {
            _data = data;
        }

        public ChessDataEngine Build()
        {
            if (_data.Count == 0)
            {
                Datas.Add(new ChessChartData() { StartDate = DateTime.Now, DeltaRate = 0 });
                return this;
            }

            var max = _data.Max(e => e.StartDate);
            var min = _data.Min(e => e.StartDate);
            var dateDef = max - min;
            if (dateDef.Days > 90)
            {
                Type = ChartType.Month;
                BuildData(dateDef, max, 30);
            }
            else if (dateDef.Days > 21)
            {
                Type = ChartType.Week;
                BuildData(dateDef, max, 7);
            }
            else
            {
                Type = ChartType.Day;
                Datas = _data.GroupBy(e=>e.StartDate.Date).Select(s=> new ChessChartData(){StartDate = s.Key, DeltaRate = s.Sum(e=>e.DeltaRate)}).ToList();
            }

            return this;
        }

        private void BuildData(TimeSpan dateDef, DateTime max, int period)
        {
            var times = dateDef.Days / period;
            var dateTo = max;
            var dateFrom = dateTo.AddDays(-7);
            for (int i = 0; i < times; i++)
            {
                var total = _data.Where(e => e.StartDate > dateFrom && e.StartDate <= dateTo).Sum(e => e.DeltaRate);
                Datas.Add(new ChessChartData() {StartDate = dateTo, DeltaRate = total});
                dateTo = dateFrom;
                dateFrom = dateTo.AddDays(period * -1);
            }
        }

        public override string ToString()
        {
            if (Datas.Count == 0)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            sb.AppendLine("[");
            sb.Append($"['{Type}',  'Rating']");

            foreach (var data in Datas.OrderBy(e=>e.StartDate))
            {
                sb.AppendLine(",");
                sb.Append($"['{data.StartDate:dd-MM-yy}', {data.DeltaRate}]");
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        private enum ChartType
        {
            Day,
            Week,
            Month
        }

       
    }
}
