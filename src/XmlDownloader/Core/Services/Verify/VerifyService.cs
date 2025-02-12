﻿using XmlDownloader.Core.Builder;
using XmlDownloader.Core.Helpers;
using XmlDownloader.Core.Services.Authenticate;
using XmlDownloader.Core.Services.Common;
using XmlDownloader.Core.SoapClient;

namespace XmlDownloader.Core.Services.Verify
{
    public class VerifyService
    {
        private readonly SoapEnvelopeBuilder soapEnvelopeBuilder;

        public VerifyService(SoapEnvelopeBuilder envelopeBuilder)
        {
            soapEnvelopeBuilder = envelopeBuilder;
        }


        public async Task<VerifyResult> Verify(string requestUuid, AuthenticateResult token)
        {
            var rawRequest = soapEnvelopeBuilder.BuildVerify(requestUuid);


            var endpoint = Helper.GetVerifyEndPoint();


            var internalRequest = new InternalRequest
            {
                Url = endpoint.Uri,
                SoapAction = endpoint.SoapAction,
                RawRequest = rawRequest,
                HttpMethod = HttpMethod.Post,
                EndPointName = EndPointName.Verify,
                Token = token
            };


            var internalResponse = await InternalHttpClient.SendAsync(internalRequest);


            var result = Helper.GetVerifyResult(internalResponse?.RawResponse);
            result.InternalRequest = internalRequest;
            result.InternalResponse = internalResponse;

            return result;
        }
    }
}