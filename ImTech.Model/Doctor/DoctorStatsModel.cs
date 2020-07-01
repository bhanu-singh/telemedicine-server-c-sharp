using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model.Doctor
{
    public class DoctorStatsModel
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
