using System;
using System.Collections.Generic;
using System.Text;

namespace TrackZero.Abstract
{
    public interface IEntityReference
    {
        public string Type { get; set; }
        public object Id { get; }

        public void Validate();
    }
}
