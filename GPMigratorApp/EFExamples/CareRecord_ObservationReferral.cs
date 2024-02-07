//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotNetGPSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class CareRecord_ObservationReferral
    {
        public string ObservationGuid { get; set; }
        public string PatientGuid { get; set; }
        public string OrganisationGuid { get; set; }
        public string ReferralTargetOrganisationGuid { get; set; }
        public string ReferralUrgency { get; set; }
        public string ReferralServiceType { get; set; }
        public string ReferralMode { get; set; }
        public string ReferralReceivedDate { get; set; }
        public string ReferralReceivedTime { get; set; }
        public string ReferralEndDate { get; set; }
        public string ReferralSourceId { get; set; }
        public string ReferralSourceOrganisationGuid { get; set; }
        public string ReferralUBRN { get; set; }
        public string ReferralReasonCodeId { get; set; }
        public string ReferringCareProfessionalStaffGroupCodeId { get; set; }
        public string ReferralEpisodeRTTMeasurementTypeId { get; set; }
        public string ReferralEpisodeClosureDate { get; set; }
        public string ReferralEpisodeDischargeLetterIssuedDate { get; set; }
        public string ReferralClosureReasonCodeId { get; set; }
        public string ProcessingId { get; set; }
    
        public virtual Admin_Organisation Admin_Organisation { get; set; }
        public virtual Admin_Organisation Admin_Organisation1 { get; set; }
        public virtual Admin_Organisation Admin_Organisation2 { get; set; }
        public virtual Admin_Patient Admin_Patient { get; set; }
        public virtual Coding_ClinicalCode Coding_ClinicalCode { get; set; }
        public virtual Coding_ClinicalCode Coding_ClinicalCode1 { get; set; }
        public virtual Coding_ClinicalCode Coding_ClinicalCode2 { get; set; }
    }
}