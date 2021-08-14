using System;
using System.Collections.Generic;
using System.Text;

namespace TrackZero.Abstract
{
    internal interface IEntityReference
    {
        public string Type { get; set; }
        public object Id { get; }
    }
}
