using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3
{
    public class Utils
    {

        public static async Task<String> callGet(
            string token,
            IHttpContextAccessor httpContextAccessor,
            string destination, 
            StringContent content=null)
        {

            Uri uri = new Uri(httpContextAccessor.HttpContext?.Request?.GetDisplayUrl());
            string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(host +"/api/"+ destination))
                {
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                        throw new UnauthorizedAccessException();
                    else if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        throw new Exception(response.StatusCode.ToString());

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static async Task<String> callPost(
            string token,
            IHttpContextAccessor httpContextAccessor,
            string destination,
            StringContent content = null)
        {

            Uri uri = new Uri(httpContextAccessor.HttpContext?.Request?.GetDisplayUrl());
            string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync(host + "/api/" + destination, content))
                {
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                        throw new UnauthorizedAccessException();
                    else if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        throw new Exception();

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static async Task<String> callPut(
            string token,
            IHttpContextAccessor httpContextAccessor,
            string destination,
            StringContent content = null)
        {

            Uri uri = new Uri(httpContextAccessor.HttpContext?.Request?.GetDisplayUrl());
            string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PutAsync(host + "/api/" + destination, content))
                {
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                        throw new UnauthorizedAccessException();
                    else if (!response.StatusCode.Equals(HttpStatusCode.OK))
                        throw new Exception();

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }


    }
}
