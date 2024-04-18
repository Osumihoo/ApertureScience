using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class Response
    {
        public List<ResponseLogin> responseLogin { get; set; }
        public List<User> users { get; set; }
        public List<Conflicts> conflicts { get; set; }
        public ResponseItemCode responseItemCode { get; set; }
        public ResponseIdReleaseH responseIdReleaseH { get; set; }
        public ResponseIdEntryH responseIdEntryH { get; set; }
        public ResponseSupplyCode responseSupplyCode { get; set; }
        public ResponseSAPActiveClass responseSAPActiveClass { get; set; }
        public ResponseFixedAssetName responseFixedAssetName { get; set; }
        public ResponseFixedAssetId responseFixedAssetId { get; set; }
        public SAPAssetDocumentLineCollection assetDocumentLineCollection { get; set; }
    }
}
