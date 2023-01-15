using System;
using LMTS.Common.Abstract;
using System.Collections.Generic;

namespace LMTS.Common.Models.StaticData
{
    public class PathType: IStringIdentifiedObject
    {
        public PathType(string key, decimal width, IEnumerable<PathLaneSettings> lanes)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Lanes = lanes ?? throw new ArgumentNullException(nameof(lanes));
            Width = width;
        }

        public string Key { get; set; }

        public IEnumerable<PathLaneSettings> Lanes { get; set; }

        public decimal Width { get; set; }
    }
}
