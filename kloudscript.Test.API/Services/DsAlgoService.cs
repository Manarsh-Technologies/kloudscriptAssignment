using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Utility;

namespace kloudscript.Test.API.Services
{
    public interface IDsAlgoService
    {
        void SortPhoneNo(List<int> lstPhoneNos, int low, int high);        
        List<ColorShortResult> ArrangeColor(List<ColorShapeEntity> lstColorShape);
        List<PatientBestInsPlanEntity> CalculateInsPlan(List<InsurancePlanEntity> lstInsPlan, List<PatientMedsEntity> lstPatient);
    }
    public class DsAlgoService: IDsAlgoService
    {
        public void SortPhoneNo(List<int> lstPhoneNos, int low, int high)
        {
            if (low < high)
            {
                int pi = Mid(lstPhoneNos, low, high);
                SortPhoneNo(lstPhoneNos, low, pi - 1);
                SortPhoneNo(lstPhoneNos, pi + 1, high);
            }
        }
        private int Mid(List<int> lstPhoneNos, int low, int high)
        {
            int pivot = lstPhoneNos[high];
            int i = (low - 1);

            for (int j = low; j <= high - 1; j++)
            {

                if (lstPhoneNos[j] < pivot)
                {
                    i++;
                    SwapNos(lstPhoneNos, i, j);
                }
            }
            SwapNos(lstPhoneNos, i + 1, high);
            return (i + 1);
        }
        private void SwapNos(List<int> lstPhoneNos, int i, int j)
        {
            int temp = lstPhoneNos[i];
            lstPhoneNos[i] = lstPhoneNos[j];
            lstPhoneNos[j] = temp;
        }
        public List<ColorShortResult> ArrangeColor(List<ColorShapeEntity> lstColorShape)
        {
            List<ColorShapeEntity> lstFinalColorShape = new List<ColorShapeEntity>();

            var cList = lstColorShape.Select(s =>
            {
                return s.ConvertTo<ColorShortResult>();
            });

            cList = cList.OrderBy(s => s.Color)
                .GroupBy(g => g.Color)
                .Select((s, i) =>
                {
                    return new { Color = s.Key, Shapes = s.Select(a => a) };
                })
                .SelectMany(s =>
                {
                    return s.Shapes.Select((p, j) =>
                    {
                        p.Rank = j + 1;
                        return p;
                    });
                }).ToList();

            return cList.OrderBy(o => o.Rank).ThenBy(o => o.Shape).ToList();
        }
        public List<PatientBestInsPlanEntity> CalculateInsPlan(List<InsurancePlanEntity> lstInsPlan, List<PatientMedsEntity> lstPatient)
        {
            List<PatientBestInsPlanEntity> lstPatientBestInsPlan = new List<PatientBestInsPlanEntity>();
            foreach (var patientItem in lstPatient)
            {
                patientItem.FixedCost = (patientItem.Cost / patientItem.DaysSupply) * 365;
            }
            foreach (var groupItem in lstPatient.GroupBy(d => d.PatientId).Select(grp => grp.ToList()))
            {
                PatientBestInsPlanEntity bestInsPlanEntity = new PatientBestInsPlanEntity();
                double total = 0;
                groupItem.ForEach(g => total += g.FixedCost);
                double min = Double.MaxValue;
                bestInsPlanEntity.PatientId = groupItem[0].PatientId;
                foreach (var insPlanItem in lstInsPlan)
                {
                    double variableCost = 0;
                    if (total < insPlanItem.Deductible)
                    {
                        variableCost = total;
                    }
                    else if (total > insPlanItem.Deductible && total < insPlanItem.MaximumOutOfPocketExpenses)
                    {
                        variableCost = (100 - insPlanItem.Coinsurance) * (total / 100);
                    }
                    else
                    {
                        variableCost = 0;
                    }
                    variableCost += insPlanItem.Premium;
                    if (variableCost < min)
                    {
                        min = variableCost;
                        bestInsPlanEntity.Cost = min;
                        bestInsPlanEntity.RecommendedInsurancePlan = insPlanItem.InsurancePlanName;
                    }
                }

                lstPatientBestInsPlan.Add(bestInsPlanEntity);
            }
            return lstPatientBestInsPlan;
        }
    }
}
