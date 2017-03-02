using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TradeApi_WebDemo
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        //Obtained from the server earlier, APIKey MUST be stored securely and in App.Config
        private string APPId;
        private string APIKey;

        public CustomDelegatingHandler()
        {
         
        }

        public CustomDelegatingHandler(string appID, string appKey)
        {
            APPId = appID;
            APIKey = appKey;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;

            string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Checking if the request contains body, usually will be null wiht HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            //Creating the raw signature string
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);



            var secretKeyByteArray = Convert.FromBase64String(APIKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                request.Headers.Authorization = new AuthenticationHeaderValue("amx", string.Format("{0}:{1}:{2}:{3}", APPId, requestSignatureBase64String, nonce, requestTimeStamp));
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }


        /*
                     def send_msg(msg, env= 'prod'):
    key = 'YOUR_API_KEY_GENERATED_IN_API_MODULE'
    secret = 'YOUR_SECRET_KEY_GENERATED_IN_API_MODULE'

    dt = datetime.datetime.now()
    nonce = str(int((time.mktime(dt.timetuple()) + dt.microsecond / 1000000.0) * 1000000))
    signature = hmac.new(secret, nonce, digestmod = hashlib.sha256).hexdigest()

    if env == 'prod':
      req = urllib2.Request("https://api.blinktrade.com/tapi/v1/message")
    else:
      req = urllib2.Request("https://api.testnet.blinktrade.com/tapi/v1/message")
    req.add_header('Content-Type', 'application/json')  # You must POST a JSON message
    req.add_header('APIKey', key)  # Your APIKey
    req.add_header('Nonce', nonce) # The nonce must be an integer, always greater than the previous one. 
    req.add_header('Signature', signature)  # Use the API Secret  to sign the nonce using HMAC_SHA256 algo
         * -*/

        private string HexDecode(string hex)
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        private static byte[] HexDecode2(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }

        private string UrlEncode(string v)
        {
            throw new NotImplementedException();
        }
    }
}
