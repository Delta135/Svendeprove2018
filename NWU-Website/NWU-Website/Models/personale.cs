//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NWU_Website.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Personale
    {
        public int personaleID { get; set; }
        public string fornavn { get; set; }
        public string efternavn { get; set; }
        
        public string brugernavn { get; set; }
        [DataType(DataType.Password)]
        public string adgangskode { get; set; }
        public int rolleID { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}
