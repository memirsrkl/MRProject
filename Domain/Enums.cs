using Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Enums
    {
        public enum ReservationStatus
        {
            [EnumsAttributes("Bekliyor", "rgba(255,193,7,1)")]
            Pending,
            [EnumsAttributes("Onaylandı", "rgba(40,167,69,1)")]
            Approved,
            [EnumsAttributes("Reddedildi", "rgba(220,53,69,1)")]
            Rejected,
        }
        public enum ParticipantType
        {
            [EnumsAttributes("Katılımcı", "rgba(40,167,69,1)")]
            Participant,
            [EnumsAttributes("Oluşturucu", "rgba(255,193,7,1)")]
            Creator,
        }
    }
}
