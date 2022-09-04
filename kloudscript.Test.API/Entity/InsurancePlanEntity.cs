namespace kloudscript.Test.API.Entity
{
    public class InsurancePlanEntity
    {
        public string InsurancePlanName { get; set; }
        public int Premium { get; set; }
        public int MaximumOutOfPocketExpenses { get; set; }
        public int Coinsurance { get; set; }
        public int Deductible { get; set; }
    }
}
