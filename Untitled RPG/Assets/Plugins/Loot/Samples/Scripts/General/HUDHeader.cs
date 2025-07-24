namespace Sample
{
    public struct HUDHeader
    {
        public string description;

        public HUDHeader (string description = null)
            => this.description = description;

        public HUDHeader (CaseData caseData)
            => description = caseData.description;
    }
}