
/******************************************************************************
 * Class Name : CsvReadWriteController.cs
 * Author     : Shailesh Vora
 *______________________________________________________________________________
 * Date         Author	     Change description
 *______________________________________________________________________________
 * 03/08/2022   svora        Alogirithm based csv read/write operation
 *******************************************************************************/

using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Services;
using kloudscript.Test.API.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kloudscript.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CsvReadWriteController : BaseController
    {
        #region Private
        private ILogger<CsvReadWriteController> logger;
        private string currentDirectory;
        private ICsvParserService CsvFileParser;
        private IDsAlgoService DsAlgoService;
        #endregion

        #region Constructor 
        public CsvReadWriteController(ILogger<CsvReadWriteController> _logger, ICsvParserService _csvParserService, IDsAlgoService _DsAlgoService)
        {
            logger = _logger;
            currentDirectory = Directory.GetCurrentDirectory();
            CsvFileParser = _csvParserService;
            DsAlgoService = _DsAlgoService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get Shape wise color information
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetColorShape")]
        public async Task<IActionResult> GetColorShapeData()
        {

            object? nullObject = null;
            string url = string.Empty;
            try
            {
                string absolutePath = currentDirectory + @"\Assets\ColorShapes.csv";
                string copyPath = currentDirectory + @"\Assets\ColorShapes_modified.csv";

                List<ColorShapeEntity> colorShapeEntitylst = await CsvFileParser.ReadCsvFile<ColorShapeEntity>(absolutePath);
                var finalResult = DsAlgoService.ArrangeColor(colorShapeEntitylst);
                colorShapeEntitylst = finalResult.Select(x => new ColorShapeEntity { Shape= x.Shape, Color= x.Color }).ToList();

                bool IsSuccess = await CsvFileParser.WriteCsvFile<ColorShapeEntity>(colorShapeEntitylst, copyPath);

                if (IsSuccess)
                {
                    return SetResponse(HttpStatusCode.OK, true, copyPath, CommongMsg.Success);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommongMsg.NoValueFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            }

        }

        /// <summary>
        /// Sort mentioned phone number with the best sorting method
        /// </summary>
        /// <returns></returns>
        [HttpGet("SortPhoneNo")]
        public async Task<IActionResult> PhoneNoSorting()
        {

            object? nullObject = null;
            string url = string.Empty;
            string copyPath = currentDirectory + @"\Assets\PhoneNumbers-8-digits_modified.csv";
            try
            {
                string absolutePath = currentDirectory + @"\Assets\PhoneNumbers-8-digits.csv";

                List<int> data = await CsvFileParser.ReadCsvFile<int>(absolutePath);

                DsAlgoService.SortPhoneNo(data, 0, data.Count - 1);

                bool IsSuccess = await CsvFileParser.WriteCsvFile<int>(data, copyPath);

                if (IsSuccess)
                {
                    return SetResponse(HttpStatusCode.OK, true, copyPath, CommongMsg.FileWriteMsg);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommongMsg.NoValueFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            }

        }
        /// <summary>
        /// Method returns the best insurance plan for patient
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPatientInsurance")]
        public async Task<IActionResult> GetBestInsurancePlan()
        {

            object? nullObject = null;
            string url = string.Empty;

            string insPlanUrl = currentDirectory + @"\Assets\InsurancePlans.csv";
            string patientMedsUrl = currentDirectory + @"\Assets\PatientMeds.csv";
            string patientBestPlanUrl = currentDirectory + @"\Assets\PatientBestInsurance.csv";
            try
            {

                List<InsurancePlanEntity> lstInsurance = await CsvFileParser.ReadCsvFile<InsurancePlanEntity>(insPlanUrl);
                List<PatientMedsEntity> lstPatientMeds = await CsvFileParser.ReadCsvFile<PatientMedsEntity>(patientMedsUrl);
                List<PatientBestInsPlanEntity> lstBestPlan = DsAlgoService.CalculateInsPlan(lstInsurance, lstPatientMeds);

                bool IsSuccess = await CsvFileParser.WriteCsvFile<PatientBestInsPlanEntity>(lstBestPlan, patientBestPlanUrl);

                if (IsSuccess)
                {
                    return SetResponse(HttpStatusCode.OK, true, patientBestPlanUrl, CommongMsg.FileWriteMsg);
                }
                else
                {
                    return SetResponse(HttpStatusCode.NotFound, false, nullObject, CommongMsg.NoValueFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return SetResponse(HttpStatusCode.InternalServerError, false, nullObject, CommongMsg.ExceptionMsg);
            }

        }
        #endregion 
    }
}
