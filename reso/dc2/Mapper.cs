using Business.Entity;
using Newtonsoft.Json;
using ResoWebApi.Model;
using ResoWebApi.Model.GoodReciept;
using System.Collections.Generic;

namespace ResoWebApi.Mappers
{
    public class Mapper
    {
        public static List<GoodReceiptImportEntity> GetGoodsReceiptsImportEntity(List<GoodReceiptImportModel> goodsReceipts)
        {
            var GoodReceiptImportEntityList = new List<GoodReceiptImportEntity>();

            foreach (var item in goodsReceipts)
            {
                var goodsReceiptsJson = JsonConvert.SerializeObject(item, Formatting.None);
                GoodReceiptImportEntityList.Add(JsonConvert.DeserializeObject<GoodReceiptImportEntity>(goodsReceiptsJson));
            }

            return GoodReceiptImportEntityList;
        }

        public static BilledRevenueEntity GetBilledRevenueRequestEntity(BilledRevenueImport billedRevenue)
        {
            var billedRevenueJson = JsonConvert.SerializeObject(billedRevenue, Formatting.None);
            return JsonConvert.DeserializeObject<BilledRevenueEntity>(billedRevenueJson);
        }

        public static MaterialProcurementEntity GetMaterialProcurementEntity(MaterialProcurement materialProcurement)
        {
            var materialProcurementJson = JsonConvert.SerializeObject(materialProcurement, Formatting.None);
            return JsonConvert.DeserializeObject<MaterialProcurementEntity>(materialProcurementJson);
        }

        public static PaymentPayformEntity GetPaymentPayformEntity(PaymentPayform paymentPayform)
        {
            var paymentPayformJson = JsonConvert.SerializeObject(paymentPayform, Formatting.None);
            return JsonConvert.DeserializeObject<PaymentPayformEntity>(paymentPayformJson);
        }
    }
}
