using Business.Interface;
using Microsoft.AspNetCore.Mvc;
using ResoWebApi.Common.ServiceResponse;
using ResoWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResoWebApi.Mappers;
using ResoWebApi.Model.GoodReciept;
using ResoWebApi.Common;

namespace ResoWebApi.Controllers
{
    [ApiController]
    [Route("v1/api/ineight")]
    public class InEightController : BaseController
    {
        public readonly IValidateMaterialRequest _validateMaterialRequest;
        public InEightController(IValidateMaterialRequest validateMaterialRequest)
        {
            _validateMaterialRequest = validateMaterialRequest;
        }

        [HttpPost]
        [Route("validatematerialprocurement")]
        public async Task<IActionResult> ValidateMaterialProcurement([FromBody] MaterialProcurement materialProcurementRequest)
        {
            if (materialProcurementRequest.Contract != default(Contract))
            {
                var materialProcurementEntity = Mapper.GetMaterialProcurementEntity(materialProcurementRequest);
                var response = await _validateMaterialRequest.ValidateMaterialProcurement(materialProcurementEntity);
                if(response.Status == ResponseStatus.Success)
                {
                    return CreateResponse(() => response, ServiceResultStatuses.Success);
                }
                else
                {
                    return CreateResponse(() => response, ServiceResultStatuses.Failed);
                }
            }
            else
            {
                return CreateResponse(() => "Material Procurement data must be required", ServiceResultStatuses.Failed);
            }
        }

        [HttpPost]
        [Route("billedrevenue")]
        public async Task<IActionResult> UpdateBilledRevenue([FromBody] BilledRevenueImport billedRevenue)
        {
            if (billedRevenue != default(BilledRevenueImport))
            {
                var billedRevenueEntity = Mapper.GetBilledRevenueRequestEntity(billedRevenue);
                var billedRevenuere = await _validateMaterialRequest.UpdateBilledRevenue(billedRevenueEntity);
                return CreateResponse(() => billedRevenuere, ServiceResultStatuses.Success);
            }
            else
            {
                return CreateResponse(() => "Billed Revenue data must be required", ServiceResultStatuses.Failed);
            }
        }

        [HttpPost]
        [Route("importgoodreceipt")]
        public async Task<IActionResult> CreateGoodReciept([FromBody] List<GoodReceiptImportModel> goodsReceiptsRequest)
        {
            if(goodsReceiptsRequest.Count > 0)
            {
                var goodRecieptEntity = Mapper.GetGoodsReceiptsImportEntity(goodsReceiptsRequest);
                var goodReceipt = await _validateMaterialRequest.ImportGoodReceipt(goodRecieptEntity);
                return CreateResponse(() => goodReceipt, ServiceResultStatuses.Success);
            }
            else
            {
                return CreateResponse(() => "Good Reciept data must be required", ServiceResultStatuses.Failed);
            }
        }

        [HttpPost]
        [Route("createxerobill")]
        public async Task<IActionResult> CreateXeroBill([FromBody] PaymentPayform paymentPayformRequest)
        {
            if (paymentPayformRequest.PayRequest != default(PayRequest))
            {
                var PaymentPayformEntity = Mapper.GetPaymentPayformEntity(paymentPayformRequest);
                var response = await _validateMaterialRequest.ValidatePaymentPayform(PaymentPayformEntity);
                if (response.Status == ResponseStatus.Success)
                {
                    return CreateResponse(() => response, ServiceResultStatuses.Success);
                }
                else
                {
                    return CreateResponse(() => response, ServiceResultStatuses.Failed);
                }
            }
            else
            {
                return CreateResponse(() => "Payment Payform data must be required", ServiceResultStatuses.Failed);
            }
        }
    }
}
