//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FunctionalTests.ProductivityApi.TemplateModels.CsMonsterModel
{
    using System;
    
    public partial class ContactDetailsMm
    {
        public ContactDetailsMm()
        {
            this.HomePhone = new Another.Place.PhoneMm();
            this.WorkPhone = new Another.Place.PhoneMm();
            this.MobilePhone = new Another.Place.PhoneMm();
        }
    
        public string Email { get; set; }
    
        public Another.Place.PhoneMm HomePhone { get; set; }
        public Another.Place.PhoneMm WorkPhone { get; set; }
        public Another.Place.PhoneMm MobilePhone { get; set; }
    }
}
