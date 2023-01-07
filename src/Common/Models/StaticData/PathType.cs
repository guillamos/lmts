using System;
using LMTS.Common.Abstract;
using System.Collections.Generic;

namespace LMTS.Common.Models.StaticData
{
    public class PathType: IStringIdentifiedObject
    {
        public PathType(string key, decimal width, IEnumerable<PathLane> lanes)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Lanes = lanes ?? throw new ArgumentNullException(nameof(lanes));
            Width = width;
        }

        public string Key { get; set; }

        public IEnumerable<PathLane> Lanes { get; set; }

        public decimal Width { get; set; }
    }
}
