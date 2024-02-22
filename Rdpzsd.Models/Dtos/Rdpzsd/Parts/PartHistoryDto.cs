using Rdpzsd.Models.Models.Rdpzsd.Base;
using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.Rdpzsd.Parts
{
    public class PartHistoryDto<TPart, TPartInfo, THistory, THistoryInfo>
        where TPart : Part<TPartInfo>
        where TPartInfo : PartInfo
        where THistory : Part<THistoryInfo>
        where THistoryInfo : PartInfo
    {
        public TPart Actual { get; set; }
        public List<THistory> Histories { get; set; }

        public PartHistoryDto(TPart actual, List<THistory> histories)
        {
            Actual = actual;
            Histories = histories;
        }
    }
}
