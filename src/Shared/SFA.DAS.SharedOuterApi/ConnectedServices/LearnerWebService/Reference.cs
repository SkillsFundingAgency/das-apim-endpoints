namespace LearnerServiceClient
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/exceptions")]
    public partial class MIAPAPIException
    {
        
        private string errorCodeField;
        
        private string errorActorField;
        
        private string descriptionField;
        
        private string furtherDetailsField;
        
        private string errorTimestampField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string ErrorActor
        {
            get
            {
                return this.errorActorField;
            }
            set
            {
                this.errorActorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string FurtherDetails
        {
            get
            {
                return this.furtherDetailsField;
            }
            set
            {
                this.furtherDetailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string ErrorTimestamp
        {
            get
            {
                return this.errorTimestampField;
            }
            set
            {
                this.errorTimestampField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class Learner
    {
        
        private string createdDateField;
        
        private string lastUpdatedDateField;
        
        private string uLNField;
        
        private string masterSubstitutedField;
        
        private string titleField;
        
        private string givenNameField;
        
        private string middleOtherNameField;
        
        private string familyNameField;
        
        private string preferredGivenNameField;
        
        private string previousFamilyNameField;
        
        private string familyNameAtAge16Field;
        
        private string schoolAtAge16Field;
        
        private string lastKnownAddressLine1Field;
        
        private string lastKnownAddressLine2Field;
        
        private string lastKnownAddressTownField;
        
        private string lastKnownAddressCountyOrCityField;
        
        private string lastKnownPostCodeField;
        
        private string dateOfAddressCaptureField;
        
        private string dateOfBirthField;
        
        private string placeOfBirthField;
        
        private string genderField;
        
        private string emailAddressField;
        
        private string nationalityField;
        
        private string scottishCandidateNumberField;
        
        private string verificationTypeField;
        
        private string otherVerificationDescriptionField;
        
        private string tierLevelField;
        
        private string abilityToShareField;
        
        private string learnerStatusField;
        
        private string[] linkedULNsField;
        
        private string notesField;
        
        private int versionNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string CreatedDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string LastUpdatedDate
        {
            get
            {
                return this.lastUpdatedDateField;
            }
            set
            {
                this.lastUpdatedDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string MasterSubstituted
        {
            get
            {
                return this.masterSubstitutedField;
            }
            set
            {
                this.masterSubstitutedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string MiddleOtherName
        {
            get
            {
                return this.middleOtherNameField;
            }
            set
            {
                this.middleOtherNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string PreferredGivenName
        {
            get
            {
                return this.preferredGivenNameField;
            }
            set
            {
                this.preferredGivenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string PreviousFamilyName
        {
            get
            {
                return this.previousFamilyNameField;
            }
            set
            {
                this.previousFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string FamilyNameAtAge16
        {
            get
            {
                return this.familyNameAtAge16Field;
            }
            set
            {
                this.familyNameAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string SchoolAtAge16
        {
            get
            {
                return this.schoolAtAge16Field;
            }
            set
            {
                this.schoolAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string LastKnownAddressLine1
        {
            get
            {
                return this.lastKnownAddressLine1Field;
            }
            set
            {
                this.lastKnownAddressLine1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string LastKnownAddressLine2
        {
            get
            {
                return this.lastKnownAddressLine2Field;
            }
            set
            {
                this.lastKnownAddressLine2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string LastKnownAddressTown
        {
            get
            {
                return this.lastKnownAddressTownField;
            }
            set
            {
                this.lastKnownAddressTownField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string LastKnownAddressCountyOrCity
        {
            get
            {
                return this.lastKnownAddressCountyOrCityField;
            }
            set
            {
                this.lastKnownAddressCountyOrCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public string LastKnownPostCode
        {
            get
            {
                return this.lastKnownPostCodeField;
            }
            set
            {
                this.lastKnownPostCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string DateOfAddressCapture
        {
            get
            {
                return this.dateOfAddressCaptureField;
            }
            set
            {
                this.dateOfAddressCaptureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=18)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=19)]
        public string PlaceOfBirth
        {
            get
            {
                return this.placeOfBirthField;
            }
            set
            {
                this.placeOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=20)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=21)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=22)]
        public string Nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=23)]
        public string ScottishCandidateNumber
        {
            get
            {
                return this.scottishCandidateNumberField;
            }
            set
            {
                this.scottishCandidateNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=24)]
        public string VerificationType
        {
            get
            {
                return this.verificationTypeField;
            }
            set
            {
                this.verificationTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=25)]
        public string OtherVerificationDescription
        {
            get
            {
                return this.otherVerificationDescriptionField;
            }
            set
            {
                this.otherVerificationDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=26)]
        public string TierLevel
        {
            get
            {
                return this.tierLevelField;
            }
            set
            {
                this.tierLevelField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=27)]
        public string AbilityToShare
        {
            get
            {
                return this.abilityToShareField;
            }
            set
            {
                this.abilityToShareField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=28)]
        public string LearnerStatus
        {
            get
            {
                return this.learnerStatusField;
            }
            set
            {
                this.learnerStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=29)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ULN", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public string[] LinkedULNs
        {
            get
            {
                return this.linkedULNsField;
            }
            set
            {
                this.linkedULNsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=30)]
        public string Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=31)]
        public int VersionNumber
        {
            get
            {
                return this.versionNumberField;
            }
            set
            {
                this.versionNumberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindLearnerResp))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public abstract partial class LearnerServiceWrappedResponse
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public partial class FindLearnerResp : LearnerServiceWrappedResponse
    {
        
        private string responseCodeField;
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        private string lastKnownPostCodeField;
        
        private string previousFamilyNameField;
        
        private string schoolAtAge16Field;
        
        private string placeOfBirthField;
        
        private string emailAddressField;
        
        private Learner[] learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string LastKnownPostCode
        {
            get
            {
                return this.lastKnownPostCodeField;
            }
            set
            {
                this.lastKnownPostCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string PreviousFamilyName
        {
            get
            {
                return this.previousFamilyNameField;
            }
            set
            {
                this.previousFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string SchoolAtAge16
        {
            get
            {
                return this.schoolAtAge16Field;
            }
            set
            {
                this.schoolAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string PlaceOfBirth
        {
            get
            {
                return this.placeOfBirthField;
            }
            set
            {
                this.placeOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Learner", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public Learner[] Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseFindLearnerServiceRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerByDemographicsRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerByULNRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public abstract partial class BaseLearnerServiceRequestPart
    {
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerByDemographicsRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerByULNRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public abstract partial class BaseFindLearnerServiceRqst : BaseLearnerServiceRequestPart
    {
        
        private string findTypeField;
        
        private string organisationRefField;
        
        private string uKPRNField;
        
        private string orgPasswordField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string FindType
        {
            get
            {
                return this.findTypeField;
            }
            set
            {
                this.findTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string OrganisationRef
        {
            get
            {
                return this.organisationRefField;
            }
            set
            {
                this.organisationRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=2)]
        public string UKPRN
        {
            get
            {
                return this.uKPRNField;
            }
            set
            {
                this.uKPRNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string OrgPassword
        {
            get
            {
                return this.orgPasswordField;
            }
            set
            {
                this.orgPasswordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public partial class LearnerByDemographicsRqst : BaseFindLearnerServiceRqst
    {
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        private string lastKnownPostCodeField;
        
        private string previousFamilyNameField;
        
        private string schoolAtAge16Field;
        
        private string placeOfBirthField;
        
        private string emailAddressField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string LastKnownPostCode
        {
            get
            {
                return this.lastKnownPostCodeField;
            }
            set
            {
                this.lastKnownPostCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=5)]
        public string PreviousFamilyName
        {
            get
            {
                return this.previousFamilyNameField;
            }
            set
            {
                this.previousFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=6)]
        public string SchoolAtAge16
        {
            get
            {
                return this.schoolAtAge16Field;
            }
            set
            {
                this.schoolAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=7)]
        public string PlaceOfBirth
        {
            get
            {
                return this.placeOfBirthField;
            }
            set
            {
                this.placeOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=8)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg")]
    public partial class LearnerByULNRqst : BaseFindLearnerServiceRqst
    {
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LearnerServiceClient.LearnerPortType")]
    public interface LearnerPortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/learnerByULNResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/learnerByULNMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.findLearnerByULNResponse learnerByULN(LearnerServiceClient.learnerByULNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/learnerByULNResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.findLearnerByULNResponse> learnerByULNAsync(LearnerServiceClient.learnerByULNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/learnerByDemographicsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/learnerByDemographicsMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.findLearnerByDemographicsResponse learnerByDemographics(LearnerServiceClient.learnerByDemographicsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/learnerByDemographicsResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.findLearnerByDemographicsResponse> learnerByDemographicsAsync(LearnerServiceClient.learnerByDemographicsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/registerSingleLearnerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/registerSingleLearnerMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.registerSingleLearnerResponse registerSingleLearner(LearnerServiceClient.registerSingleLearnerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/registerSingleLearnerResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.registerSingleLearnerResponse> registerSingleLearnerAsync(LearnerServiceClient.registerSingleLearnerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/updateSingleLearnerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/updateSingleLearnerMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.updateLearnerResponse updateSingleLearner(LearnerServiceClient.updateLearnerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/updateSingleLearnerResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.updateLearnerResponse> updateSingleLearnerAsync(LearnerServiceClient.updateLearnerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/submitBatchRegistrationResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/submitBatchRegistrationMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.submitBatchResponse submitBatchRegistration(LearnerServiceClient.submitBatchRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/submitBatchRegistrationResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.submitBatchResponse> submitBatchRegistrationAsync(LearnerServiceClient.submitBatchRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/getBatchRegistrationOutputResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/getBatchRegistrationOutputMIAPAPIExceptionFaul" +
            "t", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.batchOutputResponse getBatchRegistrationOutput(LearnerServiceClient.batchOutputRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/getBatchRegistrationOutputResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.batchOutputResponse> getBatchRegistrationOutputAsync(LearnerServiceClient.batchOutputRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/submitVerifyBatchResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/submitVerifyBatchMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.submitVerifyBatchResponse submitVerifyBatch(LearnerServiceClient.submitVerifyBatchRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/submitVerifyBatchResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.submitVerifyBatchResponse> submitVerifyBatchAsync(LearnerServiceClient.submitVerifyBatchRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/getVerifyBatchOutputResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/getVerifyBatchOutputMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.verifyBatchOutputResponse getVerifyBatchOutput(LearnerServiceClient.verifyBatchOutputRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/getVerifyBatchOutputResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.verifyBatchOutputResponse> getVerifyBatchOutputAsync(LearnerServiceClient.verifyBatchOutputRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/retrieveULNsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/retrieveULNsMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.retrieveULNsResponse retrieveULNs(LearnerServiceClient.retrieveULNsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="http://tempuri.org/LearnerPortType/retrieveULNsResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.retrieveULNsResponse> retrieveULNsAsync(LearnerServiceClient.retrieveULNsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/LearnerPortType/verifyLearner", ReplyAction="http://tempuri.org/LearnerPortType/verifyLearnerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LearnerServiceClient.MIAPAPIException), Action="http://tempuri.org/LearnerPortType/verifyLearnerMIAPAPIExceptionFault", Name="MIAPAPIException", Namespace="http://api.lrs.miap.gov.uk/exceptions")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(LearnerServiceWrappedResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BaseLearnerServiceRequestPart))]
        LearnerServiceClient.verifyLearnerResponse verifyLearner(LearnerServiceClient.verifyLearnerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/LearnerPortType/verifyLearner", ReplyAction="http://tempuri.org/LearnerPortType/verifyLearnerResponse")]
        System.Threading.Tasks.Task<LearnerServiceClient.verifyLearnerResponse> verifyLearnerAsync(LearnerServiceClient.verifyLearnerRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class learnerByULNRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg", Order=0)]
        public LearnerServiceClient.LearnerByULNRqst FindLearnerByULN;
        
        public learnerByULNRequest()
        {
        }
        
        public learnerByULNRequest(LearnerServiceClient.LearnerByULNRqst FindLearnerByULN)
        {
            this.FindLearnerByULN = FindLearnerByULN;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class findLearnerByULNResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg", Order=0)]
        public LearnerServiceClient.FindLearnerResp FindLearnerResponse;
        
        public findLearnerByULNResponse()
        {
        }
        
        public findLearnerByULNResponse(LearnerServiceClient.FindLearnerResp FindLearnerResponse)
        {
            this.FindLearnerResponse = FindLearnerResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class learnerByDemographicsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg", Order=0)]
        public LearnerServiceClient.LearnerByDemographicsRqst FindLearnerByDemographics;
        
        public learnerByDemographicsRequest()
        {
        }
        
        public learnerByDemographicsRequest(LearnerServiceClient.LearnerByDemographicsRqst FindLearnerByDemographics)
        {
            this.FindLearnerByDemographics = FindLearnerByDemographics;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class findLearnerByDemographicsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/findmsg", Order=0)]
        public LearnerServiceClient.FindLearnerResp FindLearnerResponse;
        
        public findLearnerByDemographicsResponse()
        {
        }
        
        public findLearnerByDemographicsResponse(LearnerServiceClient.FindLearnerResp FindLearnerResponse)
        {
            this.FindLearnerResponse = FindLearnerResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public partial class RegisterSingleLearnerRqst : BaseLearnerServiceRqst
    {
        
        private LearnerToRegister learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public LearnerToRegister Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerToUpdate))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class LearnerToRegister : BaseLearnerServiceRequestPart2
    {
        
        private string titleField;
        
        private string givenNameField;
        
        private string middleOtherNameField;
        
        private string familyNameField;
        
        private string preferredGivenNameField;
        
        private string previousFamilyNameField;
        
        private string familyNameAtAge16Field;
        
        private string schoolAtAge16Field;
        
        private string lastKnownAddressLine1Field;
        
        private string lastKnownAddressLine2Field;
        
        private string lastKnownAddressTownField;
        
        private string lastKnownAddressCountyOrCityField;
        
        private string lastKnownPostCodeField;
        
        private string dateOfAddressCaptureField;
        
        private string dateOfBirthField;
        
        private string placeOfBirthField;
        
        private string genderField;
        
        private string emailAddressField;
        
        private string nationalityField;
        
        private string scottishCandidateNumberField;
        
        private string verificationTypeField;
        
        private string otherVerificationDescriptionField;
        
        private string abilityToShareField;
        
        private string notesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MiddleOtherName
        {
            get
            {
                return this.middleOtherNameField;
            }
            set
            {
                this.middleOtherNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string PreferredGivenName
        {
            get
            {
                return this.preferredGivenNameField;
            }
            set
            {
                this.preferredGivenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string PreviousFamilyName
        {
            get
            {
                return this.previousFamilyNameField;
            }
            set
            {
                this.previousFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string FamilyNameAtAge16
        {
            get
            {
                return this.familyNameAtAge16Field;
            }
            set
            {
                this.familyNameAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string SchoolAtAge16
        {
            get
            {
                return this.schoolAtAge16Field;
            }
            set
            {
                this.schoolAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string LastKnownAddressLine1
        {
            get
            {
                return this.lastKnownAddressLine1Field;
            }
            set
            {
                this.lastKnownAddressLine1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string LastKnownAddressLine2
        {
            get
            {
                return this.lastKnownAddressLine2Field;
            }
            set
            {
                this.lastKnownAddressLine2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string LastKnownAddressTown
        {
            get
            {
                return this.lastKnownAddressTownField;
            }
            set
            {
                this.lastKnownAddressTownField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string LastKnownAddressCountyOrCity
        {
            get
            {
                return this.lastKnownAddressCountyOrCityField;
            }
            set
            {
                this.lastKnownAddressCountyOrCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string LastKnownPostCode
        {
            get
            {
                return this.lastKnownPostCodeField;
            }
            set
            {
                this.lastKnownPostCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string DateOfAddressCapture
        {
            get
            {
                return this.dateOfAddressCaptureField;
            }
            set
            {
                this.dateOfAddressCaptureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string PlaceOfBirth
        {
            get
            {
                return this.placeOfBirthField;
            }
            set
            {
                this.placeOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=18)]
        public string Nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=19)]
        public string ScottishCandidateNumber
        {
            get
            {
                return this.scottishCandidateNumberField;
            }
            set
            {
                this.scottishCandidateNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=20)]
        public string VerificationType
        {
            get
            {
                return this.verificationTypeField;
            }
            set
            {
                this.verificationTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=21)]
        public string OtherVerificationDescription
        {
            get
            {
                return this.otherVerificationDescriptionField;
            }
            set
            {
                this.otherVerificationDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=22)]
        public string AbilityToShare
        {
            get
            {
                return this.abilityToShareField;
            }
            set
            {
                this.abilityToShareField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=23)]
        public string Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MIAPLearnerToVerify))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MIAPBatchLearnerToVerify))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchLearner))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OutputBatchLearner))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerToRegister))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LearnerToUpdate))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRequestPart", Namespace="http://api.lrs.miap.gov.uk/learner")]
    public abstract partial class BaseLearnerServiceRequestPart2
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class MIAPLearnerToVerify : BaseLearnerServiceRequestPart2
    {
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=3)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=4)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class MIAPBatchLearnerToVerify : BaseLearnerServiceRequestPart2
    {
        
        private string mISIdentifierField;
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MISIdentifier
        {
            get
            {
                return this.mISIdentifierField;
            }
            set
            {
                this.mISIdentifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=4)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=5)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OutputBatchLearner))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class BatchLearner : BaseLearnerServiceRequestPart2
    {
        
        private string uLNField;
        
        private string mISIdentifierField;
        
        private string titleField;
        
        private string givenNameField;
        
        private string preferredGivenNameField;
        
        private string middleOtherNameField;
        
        private string familyNameField;
        
        private string previousFamilyNameField;
        
        private string familyNameAtAge16Field;
        
        private string schoolAtAge16Field;
        
        private string lastKnownAddressLine1Field;
        
        private string lastKnownAddressLine2Field;
        
        private string lastKnownAddressTownField;
        
        private string lastKnownAddressCountyOrCityField;
        
        private string lastKnownPostCodeField;
        
        private string dateOfAddressCaptureField;
        
        private string dateOfBirthField;
        
        private string placeOfBirthField;
        
        private string emailAddressField;
        
        private string genderField;
        
        private string nationalityField;
        
        private string scottishCandidateNumberField;
        
        private string abilityToShareField;
        
        private string verificationTypeField;
        
        private string otherVerificationDescriptionField;
        
        private string notesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string MISIdentifier
        {
            get
            {
                return this.mISIdentifierField;
            }
            set
            {
                this.mISIdentifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string PreferredGivenName
        {
            get
            {
                return this.preferredGivenNameField;
            }
            set
            {
                this.preferredGivenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string MiddleOtherName
        {
            get
            {
                return this.middleOtherNameField;
            }
            set
            {
                this.middleOtherNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string PreviousFamilyName
        {
            get
            {
                return this.previousFamilyNameField;
            }
            set
            {
                this.previousFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string FamilyNameAtAge16
        {
            get
            {
                return this.familyNameAtAge16Field;
            }
            set
            {
                this.familyNameAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string SchoolAtAge16
        {
            get
            {
                return this.schoolAtAge16Field;
            }
            set
            {
                this.schoolAtAge16Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string LastKnownAddressLine1
        {
            get
            {
                return this.lastKnownAddressLine1Field;
            }
            set
            {
                this.lastKnownAddressLine1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string LastKnownAddressLine2
        {
            get
            {
                return this.lastKnownAddressLine2Field;
            }
            set
            {
                this.lastKnownAddressLine2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string LastKnownAddressTown
        {
            get
            {
                return this.lastKnownAddressTownField;
            }
            set
            {
                this.lastKnownAddressTownField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string LastKnownAddressCountyOrCity
        {
            get
            {
                return this.lastKnownAddressCountyOrCityField;
            }
            set
            {
                this.lastKnownAddressCountyOrCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string LastKnownPostCode
        {
            get
            {
                return this.lastKnownPostCodeField;
            }
            set
            {
                this.lastKnownPostCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string DateOfAddressCapture
        {
            get
            {
                return this.dateOfAddressCaptureField;
            }
            set
            {
                this.dateOfAddressCaptureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=17)]
        public string PlaceOfBirth
        {
            get
            {
                return this.placeOfBirthField;
            }
            set
            {
                this.placeOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=18)]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=19)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=20)]
        public string Nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=21)]
        public string ScottishCandidateNumber
        {
            get
            {
                return this.scottishCandidateNumberField;
            }
            set
            {
                this.scottishCandidateNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=22)]
        public string AbilityToShare
        {
            get
            {
                return this.abilityToShareField;
            }
            set
            {
                this.abilityToShareField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=23)]
        public string VerificationType
        {
            get
            {
                return this.verificationTypeField;
            }
            set
            {
                this.verificationTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=24)]
        public string OtherVerificationDescription
        {
            get
            {
                return this.otherVerificationDescriptionField;
            }
            set
            {
                this.otherVerificationDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=25)]
        public string Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class OutputBatchLearner : BatchLearner
    {
        
        private string returnCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class LearnerToUpdate : LearnerToRegister
    {
        
        private string uLNField;
        
        private int versionNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public int VersionNumber
        {
            get
            {
                return this.versionNumberField;
            }
            set
            {
                this.versionNumberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateLearnerRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegisterSingleLearnerRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public abstract partial class BaseLearnerServiceRqst : BaseLearnerServiceRequestPart1
    {
        
        private string organisationRefField;
        
        private string uKPRNField;
        
        private string orgPasswordField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=0)]
        public string OrganisationRef
        {
            get
            {
                return this.organisationRefField;
            }
            set
            {
                this.organisationRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string UKPRN
        {
            get
            {
                return this.uKPRNField;
            }
            set
            {
                this.uKPRNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string OrgPassword
        {
            get
            {
                return this.orgPasswordField;
            }
            set
            {
                this.orgPasswordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseLearnerServiceRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateLearnerRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegisterSingleLearnerRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRequestPart", Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public abstract partial class BaseLearnerServiceRequestPart1
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public partial class UpdateLearnerRqst : BaseLearnerServiceRqst
    {
        
        private LearnerToUpdate learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public LearnerToUpdate Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public partial class RegisterSingleLearnerResp : LearnerServiceWrappedResponse1
    {
        
        private string responseCodeField;
        
        private string uLNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateLearnerResp))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegisterSingleLearnerResp))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="LearnerServiceWrappedResponse", Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public abstract partial class LearnerServiceWrappedResponse1
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg")]
    public partial class UpdateLearnerResp : LearnerServiceWrappedResponse1
    {
        
        private string responseCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class registerSingleLearnerRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg", Order=0)]
        public LearnerServiceClient.RegisterSingleLearnerRqst RegisterLearner;
        
        public registerSingleLearnerRequest()
        {
        }
        
        public registerSingleLearnerRequest(LearnerServiceClient.RegisterSingleLearnerRqst RegisterLearner)
        {
            this.RegisterLearner = RegisterLearner;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class registerSingleLearnerResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg", Order=0)]
        public LearnerServiceClient.RegisterSingleLearnerResp RegisterLearnerResponse;
        
        public registerSingleLearnerResponse()
        {
        }
        
        public registerSingleLearnerResponse(LearnerServiceClient.RegisterSingleLearnerResp RegisterLearnerResponse)
        {
            this.RegisterLearnerResponse = RegisterLearnerResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class updateLearnerRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg", Order=0)]
        public LearnerServiceClient.UpdateLearnerRqst UpdateLearner;
        
        public updateLearnerRequest()
        {
        }
        
        public updateLearnerRequest(LearnerServiceClient.UpdateLearnerRqst UpdateLearner)
        {
            this.UpdateLearner = UpdateLearner;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class updateLearnerResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/regmsg", Order=0)]
        public LearnerServiceClient.UpdateLearnerResp UpdateLearnerResponse;
        
        public updateLearnerResponse()
        {
        }
        
        public updateLearnerResponse(LearnerServiceClient.UpdateLearnerResp UpdateLearnerResponse)
        {
            this.UpdateLearnerResponse = UpdateLearnerResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public partial class BatchRegistrationRqst : BaseLearnerServiceRqst1
    {
        
        private string jobTypeField;
        
        private int learnerRecordCountField;
        
        private BatchLearner[] learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string JobType
        {
            get
            {
                return this.jobTypeField;
            }
            set
            {
                this.jobTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public int LearnerRecordCount
        {
            get
            {
                return this.learnerRecordCountField;
            }
            set
            {
                this.learnerRecordCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Learner", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public BatchLearner[] Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchOutputRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchRegistrationRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRqst", Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public abstract partial class BaseLearnerServiceRqst1 : BaseLearnerServiceRequestPart3
    {
        
        private string organisationRefField;
        
        private string uKPRNField;
        
        private string orgPasswordField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=0)]
        public string OrganisationRef
        {
            get
            {
                return this.organisationRefField;
            }
            set
            {
                this.organisationRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string UKPRN
        {
            get
            {
                return this.uKPRNField;
            }
            set
            {
                this.uKPRNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string OrgPassword
        {
            get
            {
                return this.orgPasswordField;
            }
            set
            {
                this.orgPasswordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseLearnerServiceRqst1))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchOutputRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchRegistrationRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRequestPart", Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public abstract partial class BaseLearnerServiceRequestPart3
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public partial class BatchOutputRqst : BaseLearnerServiceRqst1
    {
        
        private long jobIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long JobID
        {
            get
            {
                return this.jobIDField;
            }
            set
            {
                this.jobIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public partial class BatchRegistrationResp : LearnerServiceWrappedResponse2
    {
        
        private string responseCodeField;
        
        private long jobIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public long JobID
        {
            get
            {
                return this.jobIDField;
            }
            set
            {
                this.jobIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchOutputResp))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BatchRegistrationResp))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="LearnerServiceWrappedResponse", Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public abstract partial class LearnerServiceWrappedResponse2
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg")]
    public partial class BatchOutputResp : LearnerServiceWrappedResponse2
    {
        
        private string responseCodeField;
        
        private string jobStatusField;
        
        private int newLearnersCountField;
        
        private bool newLearnersCountFieldSpecified;
        
        private int existsUpdatedLearnersCountField;
        
        private bool existsUpdatedLearnersCountFieldSpecified;
        
        private int possibleMatchLearnersCountField;
        
        private bool possibleMatchLearnersCountFieldSpecified;
        
        private int learnersNotFoundCountField;
        
        private bool learnersNotFoundCountFieldSpecified;
        
        private int rejectedLearnersCountField;
        
        private bool rejectedLearnersCountFieldSpecified;
        
        private string jobStartedDateTimeField;
        
        private string jobFinishedDateTimeField;
        
        private OutputBatchLearner[] learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string JobStatus
        {
            get
            {
                return this.jobStatusField;
            }
            set
            {
                this.jobStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public int NewLearnersCount
        {
            get
            {
                return this.newLearnersCountField;
            }
            set
            {
                this.newLearnersCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NewLearnersCountSpecified
        {
            get
            {
                return this.newLearnersCountFieldSpecified;
            }
            set
            {
                this.newLearnersCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public int ExistsUpdatedLearnersCount
        {
            get
            {
                return this.existsUpdatedLearnersCountField;
            }
            set
            {
                this.existsUpdatedLearnersCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExistsUpdatedLearnersCountSpecified
        {
            get
            {
                return this.existsUpdatedLearnersCountFieldSpecified;
            }
            set
            {
                this.existsUpdatedLearnersCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public int PossibleMatchLearnersCount
        {
            get
            {
                return this.possibleMatchLearnersCountField;
            }
            set
            {
                this.possibleMatchLearnersCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PossibleMatchLearnersCountSpecified
        {
            get
            {
                return this.possibleMatchLearnersCountFieldSpecified;
            }
            set
            {
                this.possibleMatchLearnersCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public int LearnersNotFoundCount
        {
            get
            {
                return this.learnersNotFoundCountField;
            }
            set
            {
                this.learnersNotFoundCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LearnersNotFoundCountSpecified
        {
            get
            {
                return this.learnersNotFoundCountFieldSpecified;
            }
            set
            {
                this.learnersNotFoundCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public int RejectedLearnersCount
        {
            get
            {
                return this.rejectedLearnersCountField;
            }
            set
            {
                this.rejectedLearnersCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RejectedLearnersCountSpecified
        {
            get
            {
                return this.rejectedLearnersCountFieldSpecified;
            }
            set
            {
                this.rejectedLearnersCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string JobStartedDateTime
        {
            get
            {
                return this.jobStartedDateTimeField;
            }
            set
            {
                this.jobStartedDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string JobFinishedDateTime
        {
            get
            {
                return this.jobFinishedDateTimeField;
            }
            set
            {
                this.jobFinishedDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Learner", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public OutputBatchLearner[] Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class submitBatchRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg", Order=0)]
        public LearnerServiceClient.BatchRegistrationRqst SubmitBatch;
        
        public submitBatchRequest()
        {
        }
        
        public submitBatchRequest(LearnerServiceClient.BatchRegistrationRqst SubmitBatch)
        {
            this.SubmitBatch = SubmitBatch;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class submitBatchResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg", Order=0)]
        public LearnerServiceClient.BatchRegistrationResp SubmitBatchResponse;
        
        public submitBatchResponse()
        {
        }
        
        public submitBatchResponse(LearnerServiceClient.BatchRegistrationResp SubmitBatchResponse)
        {
            this.SubmitBatchResponse = SubmitBatchResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class batchOutputRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg", Order=0)]
        public LearnerServiceClient.BatchOutputRqst GetSubmitBatchOutput;
        
        public batchOutputRequest()
        {
        }
        
        public batchOutputRequest(LearnerServiceClient.BatchOutputRqst GetSubmitBatchOutput)
        {
            this.GetSubmitBatchOutput = GetSubmitBatchOutput;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class batchOutputResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/batchmsg", Order=0)]
        public LearnerServiceClient.BatchOutputResp GetSubmitBatchOutputResponse;
        
        public batchOutputResponse()
        {
        }
        
        public batchOutputResponse(LearnerServiceClient.BatchOutputResp GetSubmitBatchOutputResponse)
        {
            this.GetSubmitBatchOutputResponse = GetSubmitBatchOutputResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public partial class VerifyBatchRqst : BaseLearnerServiceRqst2
    {
        
        private int learnerRecordCountField;
        
        private string orgEmailField;
        
        private MIAPBatchLearnerToVerify[] learnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int LearnerRecordCount
        {
            get
            {
                return this.learnerRecordCountField;
            }
            set
            {
                this.learnerRecordCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string OrgEmail
        {
            get
            {
                return this.orgEmailField;
            }
            set
            {
                this.orgEmailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Learner", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public MIAPBatchLearnerToVerify[] Learner
        {
            get
            {
                return this.learnerField;
            }
            set
            {
                this.learnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchOutputRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRqst", Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public abstract partial class BaseLearnerServiceRqst2 : BaseLearnerServiceRequestPart4
    {
        
        private string organisationRefField;
        
        private string uKPRNField;
        
        private string orgPasswordField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=0)]
        public string OrganisationRef
        {
            get
            {
                return this.organisationRefField;
            }
            set
            {
                this.organisationRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string UKPRN
        {
            get
            {
                return this.uKPRNField;
            }
            set
            {
                this.uKPRNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string OrgPassword
        {
            get
            {
                return this.orgPasswordField;
            }
            set
            {
                this.orgPasswordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseLearnerServiceRqst2))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchOutputRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRequestPart", Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public abstract partial class BaseLearnerServiceRequestPart4
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public partial class VerifyBatchOutputRqst : BaseLearnerServiceRqst2
    {
        
        private long jobIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long JobID
        {
            get
            {
                return this.jobIDField;
            }
            set
            {
                this.jobIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public partial class VerifyBatchResp : LearnerServiceWrappedResponse3
    {
        
        private string responseCodeField;
        
        private long jobIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public long JobID
        {
            get
            {
                return this.jobIDField;
            }
            set
            {
                this.jobIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchOutputResp))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyBatchResp))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="LearnerServiceWrappedResponse", Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public abstract partial class LearnerServiceWrappedResponse3
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg")]
    public partial class VerifyBatchOutputResp : LearnerServiceWrappedResponse3
    {
        
        private string responseCodeField;
        
        private string jobStatusField;
        
        private string jobStartedDateTimeField;
        
        private string jobFinishedDateTimeField;
        
        private MIAPVerifiedBatchLearner[] verifiedLearnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string JobStatus
        {
            get
            {
                return this.jobStatusField;
            }
            set
            {
                this.jobStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string JobStartedDateTime
        {
            get
            {
                return this.jobStartedDateTimeField;
            }
            set
            {
                this.jobStartedDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string JobFinishedDateTime
        {
            get
            {
                return this.jobFinishedDateTimeField;
            }
            set
            {
                this.jobFinishedDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("VerifiedLearner", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public MIAPVerifiedBatchLearner[] VerifiedLearner
        {
            get
            {
                return this.verifiedLearnerField;
            }
            set
            {
                this.verifiedLearnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class MIAPVerifiedBatchLearner
    {
        
        private string mISIdentifierField;
        
        private string searchedULNField;
        
        private string searchedFamilyNameField;
        
        private string searchedGivenNameField;
        
        private string searchedDateOfBirthField;
        
        private string searchedGenderField;
        
        private string responseCodeField;
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        private string[] failureFlagField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string MISIdentifier
        {
            get
            {
                return this.mISIdentifierField;
            }
            set
            {
                this.mISIdentifierField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string SearchedULN
        {
            get
            {
                return this.searchedULNField;
            }
            set
            {
                this.searchedULNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string SearchedFamilyName
        {
            get
            {
                return this.searchedFamilyNameField;
            }
            set
            {
                this.searchedFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string SearchedGivenName
        {
            get
            {
                return this.searchedGivenNameField;
            }
            set
            {
                this.searchedGivenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string SearchedDateOfBirth
        {
            get
            {
                return this.searchedDateOfBirthField;
            }
            set
            {
                this.searchedDateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string SearchedGender
        {
            get
            {
                return this.searchedGenderField;
            }
            set
            {
                this.searchedGenderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FailureFlag", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string[] FailureFlag
        {
            get
            {
                return this.failureFlagField;
            }
            set
            {
                this.failureFlagField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class submitVerifyBatchRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg", Order=0)]
        public LearnerServiceClient.VerifyBatchRqst SubmitVerifyBatch;
        
        public submitVerifyBatchRequest()
        {
        }
        
        public submitVerifyBatchRequest(LearnerServiceClient.VerifyBatchRqst SubmitVerifyBatch)
        {
            this.SubmitVerifyBatch = SubmitVerifyBatch;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class submitVerifyBatchResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg", Order=0)]
        public LearnerServiceClient.VerifyBatchResp SubmitVerifyBatchResponse;
        
        public submitVerifyBatchResponse()
        {
        }
        
        public submitVerifyBatchResponse(LearnerServiceClient.VerifyBatchResp SubmitVerifyBatchResponse)
        {
            this.SubmitVerifyBatchResponse = SubmitVerifyBatchResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class verifyBatchOutputRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg", Order=0)]
        public LearnerServiceClient.VerifyBatchOutputRqst GetVerifyBatchOutput;
        
        public verifyBatchOutputRequest()
        {
        }
        
        public verifyBatchOutputRequest(LearnerServiceClient.VerifyBatchOutputRqst GetVerifyBatchOutput)
        {
            this.GetVerifyBatchOutput = GetVerifyBatchOutput;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class verifyBatchOutputResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/vldmsg", Order=0)]
        public LearnerServiceClient.VerifyBatchOutputResp GetVerifyBatchOutputResponse;
        
        public verifyBatchOutputResponse()
        {
        }
        
        public verifyBatchOutputResponse(LearnerServiceClient.VerifyBatchOutputResp GetVerifyBatchOutputResponse)
        {
            this.GetVerifyBatchOutputResponse = GetVerifyBatchOutputResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public partial class RetrieveULNsRqst : BaseLearnerServiceRqst3
    {
        
        private string[] uLNsToRetrieveField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ULN", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public string[] ULNsToRetrieve
        {
            get
            {
                return this.uLNsToRetrieveField;
            }
            set
            {
                this.uLNsToRetrieveField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyLearnerRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveULNsRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRqst", Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public abstract partial class BaseLearnerServiceRqst3 : BaseLearnerServiceRequestPart5
    {
        
        private string organisationRefField;
        
        private string uKPRNField;
        
        private string orgPasswordField;
        
        private string userNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=0)]
        public string OrganisationRef
        {
            get
            {
                return this.organisationRefField;
            }
            set
            {
                this.organisationRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=1)]
        public string UKPRN
        {
            get
            {
                return this.uKPRNField;
            }
            set
            {
                this.uKPRNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string OrgPassword
        {
            get
            {
                return this.orgPasswordField;
            }
            set
            {
                this.orgPasswordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseLearnerServiceRqst3))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyLearnerRqst))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveULNsRqst))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="BaseLearnerServiceRequestPart", Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public abstract partial class BaseLearnerServiceRequestPart5
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public partial class VerifyLearnerRqst : BaseLearnerServiceRqst3
    {
        
        private MIAPLearnerToVerify learnerToVerifyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public MIAPLearnerToVerify LearnerToVerify
        {
            get
            {
                return this.learnerToVerifyField;
            }
            set
            {
                this.learnerToVerifyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public partial class RetrieveULNsResp : LearnerServiceWrappedResponse4
    {
        
        private MIAPRetrievedULN[] retrievedULNsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("VerifiedULN", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public MIAPRetrievedULN[] RetrievedULNs
        {
            get
            {
                return this.retrievedULNsField;
            }
            set
            {
                this.retrievedULNsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class MIAPRetrievedULN
    {
        
        private string uLNField;
        
        private string masterULNField;
        
        private string responseCodeField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string MasterULN
        {
            get
            {
                return this.masterULNField;
            }
            set
            {
                this.masterULNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VerifyLearnerResp))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveULNsResp))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="LearnerServiceWrappedResponse", Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public abstract partial class LearnerServiceWrappedResponse4
    {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg")]
    public partial class VerifyLearnerResp : LearnerServiceWrappedResponse4
    {
        
        private MIAPVerifiedLearner verifiedLearnerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public MIAPVerifiedLearner VerifiedLearner
        {
            get
            {
                return this.verifiedLearnerField;
            }
            set
            {
                this.verifiedLearnerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.lrs.miap.gov.uk/learner")]
    public partial class MIAPVerifiedLearner
    {
        
        private string searchedULNField;
        
        private string searchedFamilyNameField;
        
        private string searchedGivenNameField;
        
        private string searchedDateOfBirthField;
        
        private string searchedGenderField;
        
        private string responseCodeField;
        
        private string uLNField;
        
        private string familyNameField;
        
        private string givenNameField;
        
        private string dateOfBirthField;
        
        private string genderField;
        
        private string[] failureFlagField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SearchedULN
        {
            get
            {
                return this.searchedULNField;
            }
            set
            {
                this.searchedULNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string SearchedFamilyName
        {
            get
            {
                return this.searchedFamilyNameField;
            }
            set
            {
                this.searchedFamilyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string SearchedGivenName
        {
            get
            {
                return this.searchedGivenNameField;
            }
            set
            {
                this.searchedGivenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string SearchedDateOfBirth
        {
            get
            {
                return this.searchedDateOfBirthField;
            }
            set
            {
                this.searchedDateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string SearchedGender
        {
            get
            {
                return this.searchedGenderField;
            }
            set
            {
                this.searchedGenderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string ULN
        {
            get
            {
                return this.uLNField;
            }
            set
            {
                this.uLNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FailureFlag", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string[] FailureFlag
        {
            get
            {
                return this.failureFlagField;
            }
            set
            {
                this.failureFlagField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class retrieveULNsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg", Order=0)]
        public LearnerServiceClient.RetrieveULNsRqst RetrieveULNs;
        
        public retrieveULNsRequest()
        {
        }
        
        public retrieveULNsRequest(LearnerServiceClient.RetrieveULNsRqst RetrieveULNs)
        {
            this.RetrieveULNs = RetrieveULNs;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class retrieveULNsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg", Order=0)]
        public LearnerServiceClient.RetrieveULNsResp RetrieveULNsResponse;
        
        public retrieveULNsResponse()
        {
        }
        
        public retrieveULNsResponse(LearnerServiceClient.RetrieveULNsResp RetrieveULNsResponse)
        {
            this.RetrieveULNsResponse = RetrieveULNsResponse;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class verifyLearnerRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg", Order=0)]
        public LearnerServiceClient.VerifyLearnerRqst VerifyLearner;
        
        public verifyLearnerRequest()
        {
        }
        
        public verifyLearnerRequest(LearnerServiceClient.VerifyLearnerRqst VerifyLearner)
        {
            this.VerifyLearner = VerifyLearner;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class verifyLearnerResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://api.lrs.miap.gov.uk/reportmsg", Order=0)]
        public LearnerServiceClient.VerifyLearnerResp VerifyLearnerResponse;
        
        public verifyLearnerResponse()
        {
        }
        
        public verifyLearnerResponse(LearnerServiceClient.VerifyLearnerResp VerifyLearnerResponse)
        {
            this.VerifyLearnerResponse = VerifyLearnerResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface LearnerPortTypeChannel : LearnerServiceClient.LearnerPortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class LearnerPortTypeClient : System.ServiceModel.ClientBase<LearnerServiceClient.LearnerPortType>, LearnerServiceClient.LearnerPortType
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public LearnerPortTypeClient() : 
                base(LearnerPortTypeClient.GetDefaultBinding(), LearnerPortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_LearnerPortType.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public LearnerPortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(LearnerPortTypeClient.GetBindingForEndpoint(endpointConfiguration), LearnerPortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public LearnerPortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(LearnerPortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public LearnerPortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(LearnerPortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public LearnerPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public LearnerServiceClient.findLearnerByULNResponse learnerByULN(LearnerServiceClient.learnerByULNRequest request)
        {
            return base.Channel.learnerByULN(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.findLearnerByULNResponse> learnerByULNAsync(LearnerServiceClient.learnerByULNRequest request)
        {
            return base.Channel.learnerByULNAsync(request);
        }
        
        public LearnerServiceClient.findLearnerByDemographicsResponse learnerByDemographics(LearnerServiceClient.learnerByDemographicsRequest request)
        {
            return base.Channel.learnerByDemographics(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.findLearnerByDemographicsResponse> learnerByDemographicsAsync(LearnerServiceClient.learnerByDemographicsRequest request)
        {
            return base.Channel.learnerByDemographicsAsync(request);
        }
        
        public LearnerServiceClient.registerSingleLearnerResponse registerSingleLearner(LearnerServiceClient.registerSingleLearnerRequest request)
        {
            return base.Channel.registerSingleLearner(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.registerSingleLearnerResponse> registerSingleLearnerAsync(LearnerServiceClient.registerSingleLearnerRequest request)
        {
            return base.Channel.registerSingleLearnerAsync(request);
        }
        
        public LearnerServiceClient.updateLearnerResponse updateSingleLearner(LearnerServiceClient.updateLearnerRequest request)
        {
            return base.Channel.updateSingleLearner(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.updateLearnerResponse> updateSingleLearnerAsync(LearnerServiceClient.updateLearnerRequest request)
        {
            return base.Channel.updateSingleLearnerAsync(request);
        }
        
        public LearnerServiceClient.submitBatchResponse submitBatchRegistration(LearnerServiceClient.submitBatchRequest request)
        {
            return base.Channel.submitBatchRegistration(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.submitBatchResponse> submitBatchRegistrationAsync(LearnerServiceClient.submitBatchRequest request)
        {
            return base.Channel.submitBatchRegistrationAsync(request);
        }
        
        public LearnerServiceClient.batchOutputResponse getBatchRegistrationOutput(LearnerServiceClient.batchOutputRequest request)
        {
            return base.Channel.getBatchRegistrationOutput(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.batchOutputResponse> getBatchRegistrationOutputAsync(LearnerServiceClient.batchOutputRequest request)
        {
            return base.Channel.getBatchRegistrationOutputAsync(request);
        }
        
        public LearnerServiceClient.submitVerifyBatchResponse submitVerifyBatch(LearnerServiceClient.submitVerifyBatchRequest request)
        {
            return base.Channel.submitVerifyBatch(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.submitVerifyBatchResponse> submitVerifyBatchAsync(LearnerServiceClient.submitVerifyBatchRequest request)
        {
            return base.Channel.submitVerifyBatchAsync(request);
        }
        
        public LearnerServiceClient.verifyBatchOutputResponse getVerifyBatchOutput(LearnerServiceClient.verifyBatchOutputRequest request)
        {
            return base.Channel.getVerifyBatchOutput(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.verifyBatchOutputResponse> getVerifyBatchOutputAsync(LearnerServiceClient.verifyBatchOutputRequest request)
        {
            return base.Channel.getVerifyBatchOutputAsync(request);
        }
        
        public LearnerServiceClient.retrieveULNsResponse retrieveULNs(LearnerServiceClient.retrieveULNsRequest request)
        {
            return base.Channel.retrieveULNs(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.retrieveULNsResponse> retrieveULNsAsync(LearnerServiceClient.retrieveULNsRequest request)
        {
            return base.Channel.retrieveULNsAsync(request);
        }
        
        public LearnerServiceClient.verifyLearnerResponse verifyLearner(LearnerServiceClient.verifyLearnerRequest request)
        {
            return base.Channel.verifyLearner(request);
        }
        
        public System.Threading.Tasks.Task<LearnerServiceClient.verifyLearnerResponse> verifyLearnerAsync(LearnerServiceClient.verifyLearnerRequest request)
        {
            return base.Channel.verifyLearnerAsync(request);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_LearnerPortType))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                result.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Certificate;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_LearnerPortType))
            {
                return new System.ServiceModel.EndpointAddress("https://cmp-ws.dev.lrs.education.gov.uk/LearnerService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return LearnerPortTypeClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_LearnerPortType);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return LearnerPortTypeClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_LearnerPortType);
        }
        
        public enum EndpointConfiguration
        {
            BasicHttpBinding_LearnerPortType,
        }
    }
}
