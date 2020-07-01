using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.Mapper
{
    public class DoctorStatsViewModel
    {
        public int CountTxtConsultationInitiatedOrInProgress { get; set; }
        public int CountCallConsultationInitiatedOrInProgress { get; set; }
        public int CountVideoConsultationInitiatedOrInProgress { get; set; }
        public int CountTxtConsultationClosed { get; set; }
        public int CountCallConsultationClosed { get; set; }
        public int CountVideoConsultationClosed { get; set; }
        public int CountTxtConsultationCancelled { get; set; }
        public int CountCallConsultationCancelled { get; set; }
        public int CountVideoConsultationCancelled { get; set; }
        public long LastMonthRevenue { get; set; }
        public long TotalRevenue { get; set; }
    }
}