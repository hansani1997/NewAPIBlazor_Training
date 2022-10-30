﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Core.Domain.Entity.Base
{
    public class BaseEntity
    {
        public int IsActive { get; set; }
        public int IsApproved { get; set; }
        public bool IsRecordLocked { get; set; }
        public bool IsPersisted { get; set; }
        public bool IsDirty { get; set; }
        public DateTime CreatedDate { get ; set; }= DateTime.Now;
        public DateTime UpdatedDate { get ; set; }=DateTime.Now;
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
        public IDictionary<string, object> AddtionalData { get; set; }
        public bool IsDefault { get; set; }

        public BaseEntity()
        {
            AddtionalData = new Dictionary<string, object>();

        }
    }
}
