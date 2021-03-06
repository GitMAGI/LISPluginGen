﻿using System;

namespace Seminabit.Sanita.OrderEntry.LIS.IBLL.DTO
{
    public class LabelDTO
    {
        public long? labeidid { get; set; }
        public string labebarcode { get; set; }
        public string labeidcont { get; set; }
        public string labedesc { get; set; }
        public string labeidlab { get; set; }
        public long? labeesam { get; set; }
        public DateTime? labeesamacce { get; set; }
        public long? labepaziid { get; set; }
        public int? labereri { get; set; }
        public string labrerinome { get; set; }
        public string labeacceid { get; set; }
        public string labemateid { get; set; }
        public string labeelenanal { get; set; }
        public string labesectid { get; set; }
        public string labesectnome { get; set; }
        public DateTime? labedaorprel { get; set; }
        public string laberich { get; set; }
    }
}
