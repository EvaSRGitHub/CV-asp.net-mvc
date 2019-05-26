using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.Models
{
    public class Certificate:BaseModel<int>
    {
        [Required]
        public string CertificateTitle { get; set; }

        [Required]
        public string IssuingAuthority { get; set; }

        public string AdditionalInfo { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [Required]
        public int ResumeId { get; set; }

        public Resume Resume { get; set; }

    }
}
