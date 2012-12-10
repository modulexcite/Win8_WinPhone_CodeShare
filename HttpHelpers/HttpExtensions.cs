﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Win8WinPhone.CodeShare.Extensions
{
    public static class HttpExtensions
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(ar =>
            {
                Stream requestStream = request.EndGetRequestStream(ar);
                taskComplete.TrySetResult(requestStream);
            }, request);
            return taskComplete.Task;
        }

        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(asyncResponse =>
            {
                try
                {
                    HttpWebRequest responseRequest = (HttpWebRequest)asyncResponse.AsyncState;
                    HttpWebResponse someResponse = (HttpWebResponse)responseRequest.EndGetResponse(asyncResponse);
                    taskComplete.TrySetResult(someResponse);
                }
                catch (WebException webExc)
                {
                    HttpWebResponse failedResponse = (HttpWebResponse)webExc.Response;
                    taskComplete.TrySetResult(failedResponse);
                }
            }, request);
            return taskComplete.Task;
        }
    }

    public static class HttpMethod
    {
        public static string Head { get{return "HEAD";} }
        public static string Post { get{return "POST";} }
        public static string Put { get{return "PUT";} }
        public static string Get { get{return "GET";} }
        public static string Delete { get{return "DELETE";} }
        public static string Trace { get{return "TRACE";} }
        public static string Options { get{return "OPTIONS";} }
        public static string Connect { get{return "CONNECT";} }
        public static string Patch { get{return "PATCH";} }
    }

}
